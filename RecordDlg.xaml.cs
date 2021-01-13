using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Runtime.InteropServices;
using System.Windows.Threading;

namespace CameraScan
{
    /// <summary>
    /// RecordDlg.xaml 的交互逻辑
    /// </summary>
    public partial class RecordDlg : Window
    {

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void PMYRECORDCALLBACK(int videopts);

        [DllImport("DevCapture.dll", CallingConvention = CallingConvention.Cdecl)]
        public extern static int Start_REC(byte[] videopath, int fps, int isAddSound, int audioIndex);

        [DllImport("DevCapture.dll", CallingConvention = CallingConvention.Cdecl)]
        public extern static int Stop_REC();

        [DllImport("DevCapture.dll", CallingConvention = CallingConvention.Cdecl)]
        public extern static void SetRECType(int type);

        [DllImport("DevCapture.dll", CallingConvention = CallingConvention.Cdecl)]
        public extern static void SetRECCallBack(PMYRECORDCALLBACK cb);


        bool isSet = false;
        PMYRECORDCALLBACK CallBackFunc = null;    //录像回调函数

        public RecordDlg()
        {
            InitializeComponent();
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            isSet = true;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ResourceDictionary langRd = null;
            try
            {
                if (global.pLangusge == 0)
                    langRd = System.Windows.Application.LoadComponent(new Uri(@"zh-cn.xaml", UriKind.Relative)) as ResourceDictionary;
                if (global.pLangusge == 1)
                    langRd = System.Windows.Application.LoadComponent(new Uri(@"zh-tw.xaml", UriKind.Relative)) as ResourceDictionary;
                if (global.pLangusge == 2)
                    langRd = System.Windows.Application.LoadComponent(new Uri(@"en-us.xaml", UriKind.Relative)) as ResourceDictionary;
                if (global.pLangusge == 3)
                    langRd = System.Windows.Application.LoadComponent(new Uri(@"zh-spain.xaml", UriKind.Relative)) as ResourceDictionary;
                if (global.pLangusge == 4)
                    langRd = System.Windows.Application.LoadComponent(new Uri(@"zh-Japan.xaml", UriKind.Relative)) as ResourceDictionary;
                if (global.pLangusge == 5)
                    langRd = System.Windows.Application.LoadComponent(new Uri(@"zh-Italian.xaml", UriKind.Relative)) as ResourceDictionary;
                if (global.pLangusge == 6)
                    langRd = System.Windows.Application.LoadComponent(new Uri(@"zh-French.xaml", UriKind.Relative)) as ResourceDictionary;
                if (global.pLangusge == 7)
                    langRd = System.Windows.Application.LoadComponent(new Uri(@"zh-Germany.xaml", UriKind.Relative)) as ResourceDictionary;
            }
            catch (Exception e2)
            {
                //MessageBox.Show(e2.Message);
            }
            if (langRd != null)
            {
                if (this.Resources.MergedDictionaries.Count > 0)
                {
                    this.Resources.MergedDictionaries.Clear();
                }
                this.Resources.MergedDictionaries.Add(langRd);
            }

            RecordTypeBox.SelectedIndex = global.pRecordType;
            RecordFormatBox.SelectedIndex = global.pRecordFormat;
            RecordFpsBox.SelectedIndex = global.pRecordFps;
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            global.pRecordDlgHaveRun = false;
        }


        int fps = 10;
        bool is_move = false;
        private System.Windows.Point StartPosition; // 本次移动开始时的坐标点位置
        private System.Windows.Point EndPosition;   // 本次移动结束时的坐标点位置

        private void StackPanel_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed && is_move == true)
            {
                EndPosition = e.GetPosition(RecStackPanel);
                var X = EndPosition.X - StartPosition.X;
                var Y = EndPosition.Y - StartPosition.Y;
                this.Left = this.Left + X;
                this.Top = this.Top + Y;
                if (this.Left < 0)
                {
                    this.Left = 0;
                    return;
                }
                if (this.Top < 0)
                {
                    this.Top = 0;
                    return;
                }

                if (this.Left + this.ActualWidth > SystemParameters.WorkArea.Width)
                {
                    this.Left = SystemParameters.WorkArea.Width - this.ActualWidth;
                    return;
                }

                if (this.Top + this.ActualHeight > SystemParameters.WorkArea.Height)
                {
                    this.Top = SystemParameters.WorkArea.Height - this.ActualHeight;
                    return;
                }
              
            }            
        }

        private void StackPanel_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            is_move = true;
            StartPosition = e.GetPosition(RecStackPanel); 
        }

        private void StackPanel_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            is_move = false;
        }



        private void RecordTypeBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            global.pRecordType = RecordTypeBox.SelectedIndex;
        }

        private void RecordFormatBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            global.pRecordFormat = RecordFormatBox.SelectedIndex;
        }

        private void RecordFpsBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            global.pRecordFps = RecordFpsBox.SelectedIndex;
        }



        private void ExitBt_Click(object sender, RoutedEventArgs e)
        {
            if (isRecord)
            {
                isRecord = false;
                Stop_REC();  //停止录像
                CallBackFunc = null;
                SetRECCallBack(CallBackFunc);
                System.Threading.Thread.Sleep(100);
                count = 0;
                RecordBt.Image = new BitmapImage(new Uri(@"/Images/Start.png", UriKind.Relative));
                
            }        
            this.Close();
        }

        bool isRecord = false;
        private int count = 0;
        private void RecordBt_Click(object sender, RoutedEventArgs e)
        {
            if (isRecord == false)
            {
                isRecord = true;
                count = 0;
                string videoName = DateTime.Now.ToString("yyyyMMddhhmmssfff");
                string videopath = global.ImagesFolder + "\\" + videoName + ".avi";
                if(global.pRecordFormat==1)
                    videopath = global.ImagesFolder + "\\" + videoName + ".wmv";
                if (global.pRecordFormat == 2)
                    videopath = global.ImagesFolder + "\\" + videoName + ".mp4";
                byte[] pBuf = Encoding.GetEncoding(global.pEncodType).GetBytes(videopath);

                if (global.pRecordType==0)
                    SetRECType(0);   //摄像头录制
                else
                    SetRECType(1);   //屏幕录制

               
                if(global.pRecordFps==0)
                    fps = 5;
                if (global.pRecordFps == 1)
                    fps = 10;
                if (global.pRecordFps == 2)
                    fps = 15;
                if (global.pRecordFps == 3)
                    fps = 25;

                int iRest = Start_REC(pBuf, fps, 1, 0);  //默认添加声音录制
                if (iRest != 0)
                {
                    //MessageBox.Show(Convert.ToString(iRest));
                    isRecord = false;
                    string TipStr = "启动录像失败！";
                    if (global.pLangusge == 1) TipStr = "啟動錄像失敗！";
                    if (global.pLangusge == 2) TipStr = "Failed to start video recording!";
                    if (global.pLangusge == 3) TipStr = "No se ha podido iniciar la grabación de vídeo!";
                    if (global.pLangusge == 4) TipStr = "録画が開始できません。";
                    if (global.pLangusge == 5) TipStr = "Impossibile avviare la registrazione video.";
                    if (global.pLangusge == 6) TipStr = "Échec du démarrage de l'enregistrement vidéo.";
                    System.Windows.MessageBox.Show(TipStr);
                }
                else 
                {
                    if (CallBackFunc == null)
                    {
                        CallBackFunc = new PMYRECORDCALLBACK(RecordCallBackFunc);
                        SetRECCallBack(CallBackFunc);
                    }

                    RecordBt.Image = new BitmapImage(new Uri(@"/Images/Stop.png", UriKind.Relative));
                    RecordTypeBox.IsEnabled = false;
                    RecordFormatBox.IsEnabled = false;
                    RecordFpsBox.IsEnabled = false;
                }
            }
            else 
            {
                isRecord = false;       
                Stop_REC();  //停止录像
                CallBackFunc = null;
                SetRECCallBack(CallBackFunc);
                System.Threading.Thread.Sleep(100);
                count = 0;
                RecordBt.Image = new BitmapImage(new Uri(@"/Images/Start.png", UriKind.Relative));
                RecordTypeBox.IsEnabled = true;
                RecordFormatBox.IsEnabled = true;
                RecordFpsBox.IsEnabled = true;
            }
        }


        bool isChange = false;
        private delegate void RecordUiDelegate(int show);

        private void RecordCallBackFunc(int videoFps)
        {
            count = videoFps / fps;         //AVI、MP4格式
            if (global.pRecordFormat == 1)  //WMV格式，
                count = videoFps/1000;
            this.Dispatcher.BeginInvoke(new RecordUiDelegate(UiUpdate), count);
            return ;
        }


        private void UiUpdate(int timeCount)
        {
            if (isRecord == false)
                return;
            int hour = timeCount / 60 / 60;
            int minute = (timeCount - hour * 60 * 60) / 60;
            int second = (timeCount - hour * 60 * 60 - minute * 60);
            TimeLabel.Content = global.SuffixSupplyZero(hour, 2) + ":" + global.SuffixSupplyZero(minute, 2) + ":" + global.SuffixSupplyZero(second, 2);
            //TimeLabel.Content = timeCount.ToString();
            if (isChange)
            {
                isChange = false;
                RecordBt.Image = new BitmapImage(new Uri(@"/Images/Recording.png", UriKind.Relative));
            }
            else
            {
                isChange = true;
                RecordBt.Image = new BitmapImage(new Uri(@"/Images/Stop.png", UriKind.Relative));
            }
        }


        
    }
}
