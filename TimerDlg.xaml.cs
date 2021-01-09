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
using System.Windows.Threading;
using System.Runtime.InteropServices;
using System.IO;

namespace CameraScan
{
    /// <summary>
    /// TimerDlg.xaml 的交互逻辑
    /// </summary>
    public partial class TimerDlg : Window
    {

        [DllImport("DevCapture.dll", CallingConvention = CallingConvention.Cdecl)]
        public extern static void SetFormatType(int format);
        [DllImport("DevCapture.dll", CallingConvention = CallingConvention.Cdecl)]
        public extern static int pdf_start(byte[] pdfpath);
        [DllImport("DevCapture.dll", CallingConvention = CallingConvention.Cdecl)]
        public extern static int pdf_addPage(byte[] pdfpath);
        [DllImport("DevCapture.dll", CallingConvention = CallingConvention.Cdecl)]
        public extern static int pdf_end();


        bool isTimerStart = false;
        DispatcherTimer PhotoTimer;
        int pScanCount = 0;
        private List<string> pdfImgPathList = new List<string>(); 

        public TimerDlg()
        {
            InitializeComponent();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            global.pTimerDlgHaveRun = false;
            MainWindow mMainWindow = (MainWindow)this.Owner;
            mMainWindow.TimeDlgTransfEvent -= StillIdImageCapture;
            mMainWindow.Focus();
            global.pTimerDlgHaveRun = false;
            PhotoTimer.Stop();  //停止定时器
            if (PdfSetCheck.IsChecked == true)
                mergepdf();
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
            IntervalTextBox.Text = Convert.ToString(global.pTimerInterval);
            NumTextBox.Text = Convert.ToString(pScanCount);
            PhotoTimer = new DispatcherTimer();
            PhotoTimer.Tick += new EventHandler(PhotoTimer_Tick);
            PhotoTimer.Interval = new TimeSpan(0, 0, 0, 0, 1000);

            MainWindow mMainWindow = (MainWindow)this.Owner;
            mMainWindow.TimeDlgTransfEvent += StillIdImageCapture;
            pdfImgPathList.Clear();
        }



        //静态拍照委托传递参数
        void StillIdImageCapture(string imgPath)
        {
            SetFormatType(global.FileFormat);  //还原参数设置
            if (File.Exists(imgPath))
            {
                bool bFind = false;
                for (int n = 0; n < pdfImgPathList.Count; n++)
                {
                    string cmpStr = pdfImgPathList[n];
                    if (cmpStr == imgPath)
                    {
                        bFind = true;
                        break;
                    }
                }
                if (!bFind)
                {
                    pScanCount++;
                    NumTextBox.Text = Convert.ToString(pScanCount);
                    pdfImgPathList.Add(imgPath);
                }
            }
        }

       

        private void ExitBt_Click(object sender, RoutedEventArgs e)
        {
            global.pTimerDlgHaveRun = false;
            this.Close();
        }

        private void StartBt_Click(object sender, RoutedEventArgs e)
        {
            global.pTimerInterval = Convert.ToInt32(IntervalTextBox.Text);
            if (global.pTimerInterval == 0)
            {
                string TipStr = "时间间隔必须大于0";
                if (global.pLangusge == 1) TipStr = "時間間隔必須大於0";
                if (global.pLangusge == 2) TipStr = "The time interval must be greater than 0.";
                if (global.pLangusge == 3) TipStr = "El intervalo debe ser superior a 0.";
                if (global.pLangusge == 4) TipStr = "時間間隔は0より大きい必要があります。";
                if (global.pLangusge == 5) TipStr = "Das Zeitintervall muss größer sein als 0.";
                System.Windows.Forms.MessageBox.Show(TipStr);
                return;
            }
            if (isTimerStart == false)
            {
                isTimerStart = true;
                string TipStr = "停止";
                if (global.pLangusge == 1) TipStr = "停止";
                if (global.pLangusge == 2) TipStr = "Stop";
                if (global.pLangusge == 3) TipStr = "Stop";
                if (global.pLangusge == 4) TipStr = "停止";
                if (global.pLangusge == 5) TipStr = "Hör auf";
                StartBt.Content = TipStr;
                StartBt.Image = new BitmapImage(new Uri(@"/Images/Stop.png", UriKind.Relative));
                if (global.isOpenCameraA)
                {
                    PhotoTimer.Interval = new TimeSpan(0, 0, 0, 0, global.pTimerInterval * 1000);
                    PhotoTimer.Start();
                }               
            }
            else 
            {
                isTimerStart = false;
                string TipStr = "开始";
                if (global.pLangusge == 1) TipStr = "開始";
                if (global.pLangusge == 2) TipStr = "Start";
                if (global.pLangusge == 3) TipStr = "Empezar";
                if (global.pLangusge == 4) TipStr = "開始";
                if (global.pLangusge == 5) TipStr = "Start";
                StartBt.Content = TipStr;
                StartBt.Image = new BitmapImage(new Uri(@"/Images/Start.png", UriKind.Relative));
                if (global.isOpenCameraA)
                {
                    PhotoTimer.Stop();  //停止定时器
                }

                if (PdfSetCheck.IsChecked == true)
                    mergepdf();
               
            }           
        }


        //定时器处理事件
        private void PhotoTimer_Tick(object sender, EventArgs e)
        {
            if (global.isOpenCameraA)
            {
                MainWindow mMainWindow = (MainWindow)this.Owner;
                int tmpNameMode = global.NameMode;
                if (tmpNameMode == 5)
                    tmpNameMode = 0;
                if (global.Is16MDevice)
                {
                    if (global.IsTimerScanCanDo)
                    {
                        global.IsTimerScanCanDo = false;
                        if (global.isJoinMainCam == 1 && global.isOpenCameraB)  //如果与主画面合并拍照
                        {
                            mMainWindow.FuncStillMainAssistImageJoin(global.FileFormat, tmpNameMode, true);
                            pScanCount++;
                            NumTextBox.Text = Convert.ToString(pScanCount);
                            return;
                        }
                        mMainWindow.FuncCaptureFromStill(global.FileFormat, tmpNameMode, true);
                        pScanCount++;
                        NumTextBox.Text = Convert.ToString(pScanCount);
                    }             
                }
                else 
                {
                    string SrcPath = "";
                    if (global.isJoinMainCam == 1 && global.isOpenCameraB)  //如果与主画面合并拍照
                    {
                        SrcPath = mMainWindow.FuncMainAssistImageJoin(global.FileFormat, tmpNameMode, true);
                        if (File.Exists(SrcPath))
                            pdfImgPathList.Add(SrcPath);    
                        pScanCount++;
                        NumTextBox.Text = Convert.ToString(pScanCount);
                        return;
                    }

                    SrcPath = mMainWindow.FuncCaptureFromPreview(global.FileFormat, tmpNameMode, true);
                    if (global.isTakeSlaveCamImg == 1)
                        mMainWindow.FuncSlaveCaptureFromPreview(global.FileFormat, tmpNameMode, true);
                    if (File.Exists(SrcPath))
                        pdfImgPathList.Add(SrcPath);  
                    pScanCount++;
                    NumTextBox.Text = Convert.ToString(pScanCount);
                }
            }
        }



        private void mergepdf()
        {
            if (pdfImgPathList.Count > 0)
            {
                MainWindow mMainWindow = (MainWindow)this.Owner;
                string pdfName = DateTime.Now.ToString("yyyyMMddhhmmssfff");
                string pdfpath = global.ImagesFolder + "\\" + pdfName + ".pdf";
                if (global.NameMode == 1)
                {
                    string dateStr = DateTime.Now.ToString("yyyy-MM-dd");
                    pdfpath = global.ImagesFolder + "\\" + dateStr;
                    if (!Directory.Exists(pdfpath))
                        Directory.CreateDirectory(pdfpath);
                    string timeStr = DateTime.Now.ToString("yyyyMMddhhmmssfff");
                    pdfpath = pdfpath + "\\" + timeStr + ".pdf";
                }
                //if (global.NameMode == 2)
                //{
                //    string CountStr = global.SuffixSupplyZero(global.SuffixCount, global.SuffixLength);
                //    pdfpath = global.ImagesFolder + "\\" + global.PrefixNmae + "_" + CountStr + ".pdf";
                //    global.SuffixCount++;
                //}

                if (global.NameMode == 5)
                {
                    mMainWindow.mFixedNameDlg = new FixedNameDlg();
                    //mFixedNameDlg.Owner = this;
                    mMainWindow.mFixedNameDlg.ShowDialog();
                    if (global.pFixedNameStr == "")
                    {
                        mMainWindow.mFixedNameDlg.Close();
                        return;
                    }
                    pdfpath = global.ImagesFolder + "\\" + global.pFixedNameStr + ".pdf";
                }

                byte[] pdfBuf = Encoding.GetEncoding(global.pEncodType).GetBytes(pdfpath);
                pdf_start(pdfBuf);
                for (int i = 0; i < pdfImgPathList.Count; i++)
                {
                    if (File.Exists(pdfImgPathList[i]))
                    {
                        byte[] pPathBuf = Encoding.GetEncoding(global.pEncodType).GetBytes(pdfImgPathList[i]);
                        pdf_addPage(pPathBuf);
                    }
                }
                pdf_end();

                int pos = pdfpath.LastIndexOf("\\");
                string name = pdfpath.Substring(pos + 1, pdfpath.Length - pos - 1);
                PreviewPhoto mPhoto = new PreviewPhoto();
                string prePath = pdfImgPathList[pdfImgPathList.Count - 1];
                mPhoto.SourceImage = mMainWindow.CreateImageSourceThumbnia(prePath, 120, 90);
                mPhoto.LogoImage = new BitmapImage(new Uri(@"Images\pdfb.png", UriKind.Relative));
                mPhoto.ImageName = name;
                mPhoto.ImagePath = pdfpath;
                mMainWindow.PreviewImgList.Items.Add(mPhoto);
                mMainWindow.PreviewImgList.ScrollIntoView(mMainWindow.PreviewImgList.Items[mMainWindow.PreviewImgList.Items.Count - 1]); //设置总显示最后一项

                pdfImgPathList.Clear();
            }
        }



        //是否是数字
        public static bool isNumberic(string _string)
        {
            if (string.IsNullOrEmpty(_string))
                return false;
            foreach (char c in _string)
            {
                if (!char.IsDigit(c))
                    //if(c<'0' c="">'9')
                    return false;
            }
            return true;
        }


        //只允许输入数字
        private void IntervalTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if ((e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9) ||
               (e.Key >= Key.D0 && e.Key <= Key.D9) ||
               e.Key == Key.Back ||
               e.Key == Key.Left || e.Key == Key.Right)
            {
                if (e.KeyboardDevice.Modifiers != ModifierKeys.None)
                {
                    e.Handled = true;
                }
            }
            else
            {
                e.Handled = true;
            }
        }


        private void IntervalTextBox_TextInput(object sender, TextCompositionEventArgs e)
        {
           
        }

       
        private void NumTextBox_TextInput(object sender, TextCompositionEventArgs e)
        {
            
        }



    }
}
