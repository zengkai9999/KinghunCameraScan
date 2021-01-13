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
using System.Threading;
using System.IO;
using System.Windows.Threading;
using System.Windows.Interop;

namespace CameraScan
{
    /// <summary>
    /// JoinImgDlg.xaml 的交互逻辑
    /// </summary>
    public partial class JoinImgDlg : Window
    {

        [DllImport("shell32.dll")]
        public extern static Int32 ShellExecute(IntPtr hwnd, string lpOperation, string lpFile, string lpParameters, string lpDirectory, int nShowCmd);

        [DllImport("DevCapture.dll",  CallingConvention = CallingConvention.Cdecl)]
        public extern static void SetCutType(int cutType);

        [DllImport("DevCapture.dll",  CallingConvention = CallingConvention.Cdecl)]
        public extern static void SetFormatType(int format);

        [DllImport("DevCapture.dll",  CallingConvention = CallingConvention.Cdecl)]
        public extern static int AddImagePath(byte[] path);

        [DllImport("DevCapture.dll",  CallingConvention = CallingConvention.Cdecl)]
        public extern static int MergeImages(byte[] despath , int dir ,int space);

        bool isSet = false;
        int ScanCount = 0;
        int ScanCountB = 0;
        string SrcPath;
        string CombinePath="";
        private List<string> pImgPathList = new List<string>(); 

        public JoinImgDlg()
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

            if (global.JoinPages == 2)
                PagesCbBox.SelectedIndex = 0;
            else if (global.JoinPages == 3)
                PagesCbBox.SelectedIndex = 1;
            else if (global.JoinPages == 4)
                PagesCbBox.SelectedIndex = 2;

            DirCbBox.SelectedIndex = global.JoinDir;

            if (global.isSaveJoinSoure == 0) JoinSetCheck.IsChecked = false;
            else JoinSetCheck.IsChecked = true;

            MainWindow mMainWindow = (MainWindow)this.Owner;
            mMainWindow.JoinImgTransfEvent += StillIdImageCapture;
        }


        private void ExitBt_Click(object sender, RoutedEventArgs e)
        {
            global.pJoinImgDlgHaveRun = false;
            this.Close();
        }


        private void PagesCbBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (isSet)
            {
                if (PagesCbBox.SelectedIndex == 0)
                    global.JoinPages = 2;
                else if (PagesCbBox.SelectedIndex == 1)
                    global.JoinPages = 3;
                else if (PagesCbBox.SelectedIndex == 2)
                    global.JoinPages = 4;
            }
        }

        private void DirCbBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (isSet)
            {
                if (DirCbBox.SelectedIndex == 0)
                    global.JoinDir = 0;  //垂直
                else if (DirCbBox.SelectedIndex == 1)
                    global.JoinDir = 1;  //水平
            }
        }


        private void JoinSetCheck_Checked(object sender, RoutedEventArgs e)
        {
            global.isSaveJoinSoure = 1;
        }

        private void JoinSetCheck_Unchecked(object sender, RoutedEventArgs e)
        {
            global.isSaveJoinSoure = 0;
        }


        //静态拍照委托传递参数
        void StillIdImageCapture(string imgPath)
        {
            SetFormatType(global.FileFormat);  //还原参数设置
            MainWindow mMainWindow = (MainWindow)this.Owner;
            pImgPathList.Add(imgPath);
            SrcPath = imgPath;
            if (File.Exists(SrcPath))
                CardImgA.Source = mMainWindow.CreateImageSourceThumbnia(SrcPath, 240, 180);
            CardImgB.Source = null;
            ScanCountB++;
            PageCountLabel.Content = Convert.ToString(ScanCountB);
            byte[] pSrcBuf = Encoding.GetEncoding(global.pEncodType).GetBytes(SrcPath);
            AddImagePath(pSrcBuf);
            if (global.JoinPages == ScanCountB)
            {
                //System.Windows.MessageBox.Show("正在合并图片");
                string TipStr = "正在合并图片...";
                if (global.pLangusge == 1) TipStr = "正在合並圖片...";
                if (global.pLangusge == 2) TipStr = "Merging pictures...";
                if (global.pLangusge == 3) TipStr = "Combinando imágenes...";
                if (global.pLangusge == 4) TipStr = "写真統合...";
                if (global.pLangusge == 5) TipStr = "Unione di immagini...";
                if (global.pLangusge == 6) TipStr = "Fusionner des images...";
                this.Title = TipStr;
                ScanBt.IsEnabled = false;
                DispatcherTimer timer = new DispatcherTimer();
                timer.Interval = new TimeSpan(0, 0, 0, 0, 5);
                timer.Tick += delegate
                {
                    timer.Stop();                  
                    ScanCountB = 0;
                    CombineImages();
                    ScanBt.IsEnabled = true;
                    string TitleStr = "图片合并";
                    if (global.pLangusge == 1) TitleStr = "圖像合並";
                    if (global.pLangusge == 2) TitleStr = "MergeImage";
                    if (global.pLangusge == 3) TitleStr = "Combinando imágenes";
                    if (global.pLangusge == 4) TitleStr = "写真統合";
                    if (global.pLangusge == 5) TitleStr = "Unione di immagini";
                    if (global.pLangusge == 6) TitleStr = "Fusionner des images";
                    this.Title = TitleStr;
                };
                timer.Start();
            }

        }


        //合并拍摄
        private void ScanBt_Click(object sender, RoutedEventArgs e)
        {
            if (global.isOpenCameraA)
            { 
                MainWindow mMainWindow = (MainWindow)this.Owner;
                if (global.Is16MDevice)   //静态拍照
                {
                    global.pJoinImgFormScanDo = true;
                    SetFormatType(0); //JPG
                    Thread.Sleep(20);
                    bool isShowToList = true;
                    if (global.isSaveJoinSoure == 0) isShowToList = false;
                    if (global.isJoinMainCam == 1 && global.isOpenCameraB)  //如果与主画面合并拍照
                    {
                        mMainWindow.FuncStillMainAssistImageJoin(0, 0, isShowToList);
                    }
                    else
                    {
                        mMainWindow.FuncCaptureFromStill(0, 0, isShowToList); 
                    }                
                }
                else    //非静态拍照
                {
                    SetFormatType(0); //JPG
                    bool isShowToList = true;
                    if (global.isSaveJoinSoure == 0) isShowToList = false;
                    if (global.isJoinMainCam == 1 && global.isOpenCameraB)  //如果与主画面合并拍照
                    {
                        SrcPath = mMainWindow.FuncMainAssistImageJoin(0, 0, isShowToList);
                    }
                    else 
                    {
                        SrcPath = mMainWindow.FuncCaptureFromPreview(0, 0, isShowToList, 0);
                    }               
                    pImgPathList.Add(SrcPath);
                    SetFormatType(global.FileFormat);
                    SetCutType(global.CutType);
                    if (File.Exists(SrcPath))
                        CardImgA.Source = mMainWindow.CreateImageSourceThumbnia(SrcPath, 240, 180);
                    CardImgB.Source = null;
                    ScanCount++;
                    PageCountLabel.Content = Convert.ToString(ScanCount);
                    byte[] pSrcBuf = Encoding.GetEncoding(global.pEncodType).GetBytes(SrcPath);
                    AddImagePath(pSrcBuf);

                    if (global.JoinPages == ScanCount)
                    {
                        string TipStr = "正在合并图片...";
                        if (global.pLangusge == 1) TipStr = "正在合並圖片...";
                        if (global.pLangusge == 2) TipStr = "Merging pictures...";
                        if (global.pLangusge == 3) TipStr = "Combinando imágenes...";
                        if (global.pLangusge == 4) TipStr = "写真統合...";
                        if (global.pLangusge == 5) TipStr = "Unione di immagini...";
                        if (global.pLangusge == 6) TipStr = "Fusionner des images...";
                        this.Title = TipStr;
                        ScanBt.IsEnabled = false;
                        DispatcherTimer timer = new DispatcherTimer();
                        timer.Interval = new TimeSpan(0, 0, 0, 0, 5);
                        timer.Tick += delegate
                        {
                            timer.Stop();
                            ScanCount = 0;
                            CombineImages();                            
                            ScanBt.IsEnabled = true;
                            string TitleStr = "图片合并";
                            if (global.pLangusge == 1) TitleStr = "圖像合並";
                            if (global.pLangusge == 2) TitleStr = "MergeImage";
                            if (global.pLangusge == 3) TitleStr = "Combinando imágenes";
                            if (global.pLangusge == 4) TitleStr = "写真統合";
                            if (global.pLangusge == 5) TitleStr = "Unione di immagini";
                            if (global.pLangusge == 6) TitleStr = "Fusionner des images";
                            this.Title = TitleStr;
                        };
                        timer.Start();
                    }
                }

            }
        }


        private void CombineImages()
        {
            string fFormatStr = ".jpg";
            if (global.FileFormat == 0) fFormatStr = ".jpg";
            if (global.FileFormat == 1) fFormatStr = ".bmp";
            if (global.FileFormat == 2) fFormatStr = ".png";
            if (global.FileFormat == 3) fFormatStr = ".tif";
            if (global.FileFormat == 4) fFormatStr = ".pdf";
            string ImgName = DateTime.Now.ToString("yyyyMMddhhmmssfff");
            string imgpath = global.ImagesFolder + "\\" + ImgName + fFormatStr;

            if (global.NameMode == 1)
            {
                string dateStr = DateTime.Now.ToString("yyyy-MM-dd");
                imgpath = global.ImagesFolder + "\\" + dateStr;
                if (!Directory.Exists(imgpath))
                    Directory.CreateDirectory(imgpath);
                string timeStr = DateTime.Now.ToString("yyyyMMddhhmmssfff");
                imgpath = imgpath + "\\" + timeStr + fFormatStr;
            }
            if (global.NameMode == 2)
            {
                global.CalculateSuffix();
                string CountStr = global.SuffixSupplyZero(global.SuffixCount, global.SuffixLength);
                imgpath = global.ImagesFolder + "\\" + global.PrefixNmae + "_" + CountStr + fFormatStr;
                global.PreviousSuffixCount = global.SuffixCount;
                global.SuffixCount = global.SuffixCount + global.IncreaseStep;
                if (global.pSetDlgHaveRun)
                {
                    CountStr = global.SuffixSupplyZero(global.SuffixCount, global.SuffixLength);
                    MainWindow mMainWindow = (MainWindow)this.Owner;
                    mMainWindow.mSettingDlg.SuffixTextBox.Text = CountStr;
                }
            }

            //add at 2019.04.02
            if (global.NameMode == 5)
            {
                MainWindow mMainWindow = (MainWindow)this.Owner;
                mMainWindow.mFixedNameDlg = new FixedNameDlg();
                mMainWindow.mFixedNameDlg.ShowDialog();
                if (global.pFixedNameStr == "")
                {
                    mMainWindow.mFixedNameDlg.Close();
                    return;
                }
                imgpath = global.ImagesFolder + "\\" + global.pFixedNameStr + fFormatStr;
            }

            byte[] pDesBuf = Encoding.GetEncoding(global.pEncodType).GetBytes(imgpath);
            int iRest = MergeImages(pDesBuf, global.JoinDir,15);
            if (iRest == 0)
            {
                MainWindow mMainWindow = (MainWindow)this.Owner;
                string prePath = imgpath;
                if (global.FileFormat == 4)
                {
                    prePath = System.Windows.Forms.Application.StartupPath + "\\tpdf.jpg";
                    int pp1 = imgpath.LastIndexOf("\\");
                    int pp2 = imgpath.LastIndexOf(".");
                    if (pp2 > pp1)
                    {
                        string tmpname = imgpath.Substring(pp1 + 1, pp2 - pp1 - 1);
                        prePath = System.Windows.Forms.Application.StartupPath + "\\" + tmpname + ".jpg";
                        //prePath = "C:\\" + tmpname + ".jpg";
                    }
                }
                if (File.Exists(prePath))
                {
                    //this.Title = prePath;
                    CardImgB.Source = mMainWindow.CreateImageSourceThumbnia(prePath, 165, 200);                 
                }
                    
                if (File.Exists(imgpath))
                {
                    mMainWindow.AddPreviewImageToList(imgpath, 210, 297);
                    CombinePath = imgpath;
                    if (global.FileFormat == 4)
                         File.Delete(prePath);
                }
                else
                {
                    string TipStr = "图像合并失败";
                    if (global.pLangusge == 1) TipStr = "圖像合並失敗";
                    if (global.pLangusge == 2) TipStr = "Image merging failure";
                    if (global.pLangusge == 3) TipStr = "Fallo al combinar imágenes";
                    if (global.pLangusge == 4) TipStr = "画像統合エラー";
                    if (global.pLangusge == 5) TipStr = "Impossibile scattare la foto";
                    if (global.pLangusge == 6) TipStr = "Échec de la prise de photo";
                    System.Windows.MessageBox.Show(TipStr);
                }
            }

            if (global.isSaveJoinSoure == 0)
            {
                for (int i = 0; i < pImgPathList.Count; i++)
                {
                    if (File.Exists(pImgPathList[i]))
                    {
                        File.Delete(pImgPathList[i]);
                    }
                }
            }
            pImgPathList.Clear();

        }


        private void CardImgB_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                if (File.Exists(CombinePath))
                {
                    global.OpenFileAndPreview(CombinePath);
                }
            }
        }

        private void CardImgA_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                if (File.Exists(SrcPath))
                {
                    global.OpenFileAndPreview(SrcPath);
                }
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            global.pJoinImgDlgHaveRun = false ;
            MainWindow mMainWindow = (MainWindow)this.Owner;
            mMainWindow.Focus();
            mMainWindow.JoinImgTransfEvent -= StillIdImageCapture;
            global.pJoinImgDlgHaveRun = false;
            ScanBt.IsEnabled = true;
            global.pMorePdfFormScanDo = false;
            pImgPathList.Clear();
        }

    }
}
