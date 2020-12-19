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
using System.IO;

namespace CameraScan
{
    /// <summary>
    /// WiseDlg.xaml 的交互逻辑
    /// </summary>
    public partial class WiseDlg : Window
    {
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void WiseCaptureCALLBACK(int SteadyFlag);

        [DllImport("DevCapture.dll", CallingConvention = CallingConvention.Cdecl)]
        public extern static int WiseCapture(WiseCaptureCALLBACK WiseCb, int OnOff);
        [DllImport("DevCapture.dll", CallingConvention = CallingConvention.Cdecl)]
        public extern static void SetFormatType(int format);
        [DllImport("DevCapture.dll", CallingConvention = CallingConvention.Cdecl)]
        public extern static int pdf_start(byte[] pdfpath);
        [DllImport("DevCapture.dll", CallingConvention = CallingConvention.Cdecl)]
        public extern static int pdf_addPage(byte[] pdfpath);
        [DllImport("DevCapture.dll", CallingConvention = CallingConvention.Cdecl)]
        public extern static int pdf_end();

        WiseCaptureCALLBACK WiseCallBackFunc = null;  //静态拍照回调函数
        bool isTimerStart = false;
        int pScanCount = 0;
        private List<string> pdfImgPathList = new List<string>(); 

        private delegate void WiseCaptureDelegate(int flag);
        private delegate void UpdateStatusTextDelegate(string str);

        public WiseDlg()
        {
            InitializeComponent();
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

            NumTextBox.Text = Convert.ToString(pScanCount);
            MainWindow mMainWindow = (MainWindow)this.Owner;
            mMainWindow.WiseDlgTransfEvent += StillIdImageCapture;
            pdfImgPathList.Clear();
        }


        private void Window_Closed(object sender, EventArgs e)
        {
             
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            global.pWiseShootDlgHaveRun = false;
            if (isTimerStart == true)
            {
                isTimerStart = false;
                WiseCapture(WiseCallBackFunc, 0);
            }

            if (PdfSetCheck.IsChecked == true)
                mergepdf();

            MainWindow mMainWindow = (MainWindow)this.Owner;
            mMainWindow.WiseDlgTransfEvent += StillIdImageCapture;
            mMainWindow.Focus();
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
         
                string TipStr = "拍照已完成，请重新放纸";
                if (global.pLangusge == 1) TipStr = "拍照已完成，請重新放紙";
                if (global.pLangusge == 2) TipStr = "Photo is complete. Please put the paper back";
                if (global.pLangusge == 3) TipStr = "La foto ha sido tomada.";
                if (global.pLangusge == 4) TipStr = "写真を撮りました。紙をもう一度置いてください。";
                this.Dispatcher.BeginInvoke(new UpdateStatusTextDelegate(UpdateStatusText), TipStr);    
            }
        }


        void UpdateStatusText(string str)
        {
            StatusLabel.Content = str;
        }

        private void ExitBt_Click(object sender, RoutedEventArgs e)
        {       
            this.Close();
        }

        private void WiseCaptureCallBackFunc(int SteadyFlag)
        {
            this.Dispatcher.BeginInvoke(new WiseCaptureDelegate(Wisedelegate), SteadyFlag);          
        }

        int statusCount = 0;
        private void Wisedelegate(int SteadyFlag)
        {
            if (SteadyFlag == 0)
            {
                string TipStr = "检测中...";
                if (statusCount==1)
                    TipStr = "检测中.....";
                else
                    TipStr = "检测中...";

                if (global.pLangusge == 1)
                {
                    if (statusCount == 1)
                        TipStr = "檢測中.....";
                    else
                        TipStr = "檢測中...";
                }
                if (global.pLangusge == 2)
                {
                    if (statusCount == 1)
                        TipStr = "Checking.....";
                    else
                        TipStr = "Checking...";
                }
                if (global.pLangusge == 3)
                {
                    if (statusCount == 1)
                        TipStr = "Probando.....";
                    else
                        TipStr = "Probando...";
                }

                StatusLabel.Content = TipStr;

                statusCount++;
                if (statusCount > 1)
                    statusCount = 0;
         
            }
            else if (SteadyFlag == 1)
            {
                //StatusLabel.Content = "已稳定,开始拍照...";

                MainWindow mMainWindow = (MainWindow)this.Owner;
                int tmpNameMode = global.NameMode;
                if (tmpNameMode == 5)
                    tmpNameMode = 0;
                if (global.Is16MDevice)
                {
                    if (global.IsWiseScanCanDo)
                    {
                        global.IsWiseScanCanDo = false;

                        string TipStr = "正在拍照...";
                        if (global.pLangusge == 1) TipStr = "正在拍照...";
                        if (global.pLangusge == 2) TipStr = "Taking photos...";
                        if (global.pLangusge == 3) TipStr = "Tomando fotos...";
                        if (global.pLangusge == 4) TipStr = "写真を撮っています...";
                        StatusLabel.Content = TipStr;

                        if (global.isJoinMainCam == 1 && global.isOpenCameraB)  //如果与主画面合并拍照
                        {
                            mMainWindow.FuncStillMainAssistImageJoin(global.FileFormat, tmpNameMode, true);
                            pScanCount++;
                            NumTextBox.Text = Convert.ToString(pScanCount);
                            return;
                        }
                        mMainWindow.FuncCaptureFromStill(global.FileFormat, tmpNameMode, true);                     
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

                    string TipStr = "拍照已完成，请重新放纸";
                    if (global.pLangusge == 1) TipStr = "拍照已完成，請重新放紙";
                    if (global.pLangusge == 2) TipStr = "Photo is complete. Please put the paper back";
                    if (global.pLangusge == 3) TipStr = "La foto ha sido tomada.";
                    if (global.pLangusge == 4) TipStr = "写真を撮りました。紙をもう一度置いてください。";
                    StatusLabel.Content = TipStr;
                }

               
            }
            else if (SteadyFlag == 2)
            {
                //string TipStr = "拍照已完成，请重新放纸";
                //if (global.pLangusge == 1) TipStr = "拍照已完成，請重新放紙";
                //if (global.pLangusge == 2) TipStr = "Photo is complete. Please put the paper back";
                //if (global.pLangusge == 3) TipStr = "La foto ha sido tomada.";
                //StatusLabel.Content = TipStr;
            }
                
        }


        private void StartBt_Click(object sender, RoutedEventArgs e)
        {
            if (isTimerStart == false)
            {
                isTimerStart = true;
                string TipStr = "停止";
                if (global.pLangusge == 1) TipStr = "停止";
                if (global.pLangusge == 2) TipStr = "Stop";
                if (global.pLangusge == 3) TipStr = "Stop";
                if (global.pLangusge == 4) TipStr = "停止";
                StartBt.Content = TipStr;
                StartBt.Image = new BitmapImage(new Uri(@"/Images/Stop.png", UriKind.Relative));
                if (WiseCallBackFunc == null)
                {
                    WiseCallBackFunc = new WiseCaptureCALLBACK(WiseCaptureCallBackFunc);
                }
               
                WiseCapture(WiseCallBackFunc,1);
            }
            else
            {
                isTimerStart = false;
                string TipStr = "开始";
                if (global.pLangusge == 1) TipStr = "開始";
                if (global.pLangusge == 2) TipStr = "Start";
                if (global.pLangusge == 3) TipStr = "Empezar";
                if (global.pLangusge == 4) TipStr = "開始";
                StartBt.Content = TipStr;
                StartBt.Image = new BitmapImage(new Uri(@"/Images/Start.png", UriKind.Relative));
                if (WiseCallBackFunc == null)
                {
                    WiseCallBackFunc = new WiseCaptureCALLBACK(WiseCaptureCallBackFunc);
                }
                WiseCallBackFunc = null;   
                WiseCapture(WiseCallBackFunc, 0);

                if (PdfSetCheck.IsChecked==true)
                    mergepdf();
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
                        return ;
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

       
        private void PdfSetCheck_Checked(object sender, RoutedEventArgs e)
        {
            
        }

        private void PdfSetCheck_Unchecked(object sender, RoutedEventArgs e)
        {

        }

       

        

        
    }
}
