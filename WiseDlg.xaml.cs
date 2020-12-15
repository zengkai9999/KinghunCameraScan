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

        [DllImport("DevCapture.dll", CallingConvention = CallingConvention.Cdecl)]
        public extern static void ClearCutPoint();

        WiseCaptureCALLBACK WiseCallBackFunc = null;  //静态拍照回调函数
        bool isTimerStart = false;
        //int pScanCount = 0;
        private List<string> pdfImgPathList = new List<string>();
        private int listIndex = 0;

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

            NumTextBox.Text = Convert.ToString(pdfImgPathList.Count);
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
                    NumTextBox.Text = Convert.ToString(pdfImgPathList.Count);
                    pdfImgPathList.Add(imgPath);
                }
         
                string TipStr = "拍照已完成，请重新放纸";
                if (global.pLangusge == 1) TipStr = "拍照已完成，請重新放紙";
                if (global.pLangusge == 2) TipStr = "Photo is complete. Please put the paper back";
                if (global.pLangusge == 3) TipStr = "La foto ha sido tomada.";
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
                string TipStr_2 = "拍摄中...";
                if (global.pLangusge == 1) TipStr_2 = "拍摄中...";
                if (global.pLangusge == 2) TipStr_2 = "Taking photos...";
                if (global.pLangusge == 3) TipStr_2 = "Tomando fotos...";
                StatusLabel.Content = TipStr_2;
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

                        string TipStr = "拍摄中...";
                        if (global.pLangusge == 1) TipStr = "拍摄中...";
                        if (global.pLangusge == 2) TipStr = "Taking photos...";
                        if (global.pLangusge == 3) TipStr = "Tomando fotos...";
                        StatusLabel.Content = TipStr;

                        if (global.isJoinMainCam == 1 && global.isOpenCameraB)  //如果与主画面合并拍照
                        {
                            mMainWindow.FuncStillMainAssistImageJoin(global.FileFormat, tmpNameMode, true);
                            NumTextBox.Text = Convert.ToString(pdfImgPathList.Count);
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
                        NumTextBox.Text = Convert.ToString(pdfImgPathList.Count);
                        return;
                    }

                    if (global.isAutoCutMore == 1)
                    {
                        ClearCutPoint();
                        for (int i = 1; i <= 10; i++)
                        {
                            SrcPath = mMainWindow.FuncCaptureFromPreview(global.FileFormat, tmpNameMode, true, i);
                            if(File.Exists(SrcPath))
                            {
                                if (global.isTakeSlaveCamImg == 1)
                                    mMainWindow.FuncSlaveCaptureFromPreview(global.FileFormat, tmpNameMode, true);
                                pdfImgPathList.Insert(listIndex, SrcPath);
                                listIndex++;
                                PageCurrentLabel.Content = Convert.ToString(listIndex);
                                PdfImg.Source = mMainWindow.CreateImageSourceThumbnia(SrcPath, 200, 150);
                                NumTextBox.Text = Convert.ToString(pdfImgPathList.Count);
                            }
                        }
                    }
                    else
                    {
                        SrcPath = mMainWindow.FuncCaptureFromPreview(global.FileFormat, tmpNameMode, true, 0);
                        if (global.isTakeSlaveCamImg == 1)
                            mMainWindow.FuncSlaveCaptureFromPreview(global.FileFormat, tmpNameMode, true);
                        //if (File.Exists(SrcPath))
                        //    pdfImgPathList.Add(SrcPath);
                        pdfImgPathList.Insert(listIndex, SrcPath);
                        listIndex++;
                        PageCurrentLabel.Content = Convert.ToString(listIndex);
                        PdfImg.Source = mMainWindow.CreateImageSourceThumbnia(SrcPath, 200, 150);
                        NumTextBox.Text = Convert.ToString(pdfImgPathList.Count);
                    }


                    //string TipStr = "拍照已完成，请重新放纸";
                    //if (global.pLangusge == 1) TipStr = "拍照已完成，請重新放紙";
                    //if (global.pLangusge == 2) TipStr = "Photo is complete. Please put the paper back";
                    //if (global.pLangusge == 3) TipStr = "La foto ha sido tomada.";
                    //StatusLabel.Content = TipStr;
                }

               
            }
            else if (SteadyFlag == 2)
            {
                string TipStr = "拍照已完成，请重新放纸";
                if (global.pLangusge == 1) TipStr = "拍照已完成，請重新放紙";
                if (global.pLangusge == 2) TipStr = "Photo is complete. Please put the paper back";
                if (global.pLangusge == 3) TipStr = "La foto ha sido tomada.";
                StatusLabel.Content = TipStr;
            }
                
        }


        private void StartBt_Click(object sender, RoutedEventArgs e)
        {
            if (isTimerStart == false)
            {
                pdfImgPathList.Clear();
                NumTextBox.Text = Convert.ToString(pdfImgPathList.Count);
                listIndex = 0;
                PageCurrentLabel.Content = Convert.ToString(listIndex);

                isTimerStart = true;
                string TipStr = "停止";
                if (global.pLangusge == 1) TipStr = "停止";
                if (global.pLangusge == 2) TipStr = "Stop";
                if (global.pLangusge == 3) TipStr = "Stop";
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
                listIndex = 0;
            }
        }

       
        private void PdfSetCheck_Checked(object sender, RoutedEventArgs e)
        {
            
        }

        private void PdfSetCheck_Unchecked(object sender, RoutedEventArgs e)
        {

        }

        private void PageUp_Click(object sender, RoutedEventArgs e)
        {
            if (listIndex <= 1)
                return;

            listIndex--;
            PageCurrentLabel.Content = Convert.ToString(listIndex);
            MainWindow mMainWindow = (MainWindow)this.Owner;
            PdfImg.Source = mMainWindow.CreateImageSourceThumbnia(pdfImgPathList[listIndex - 1], 200, 150);
        }

        private void PageDown_Click(object sender, RoutedEventArgs e)
        {
            if (listIndex >= pdfImgPathList.Count)
                return;

            listIndex++;
            PageCurrentLabel.Content = Convert.ToString(listIndex);
            MainWindow mMainWindow = (MainWindow)this.Owner;
            PdfImg.Source = mMainWindow.CreateImageSourceThumbnia(pdfImgPathList[listIndex - 1], 200, 150);
        }

        private void PageDelete_Click(object sender, RoutedEventArgs e)
        {
            if (listIndex <= 0)
                return;
            // 索引减1
            listIndex--;
            PageCurrentLabel.Content = Convert.ToString(listIndex);
            // 显示前一张
            MainWindow mMainWindow = (MainWindow)this.Owner;
            if (listIndex > 0)
                PdfImg.Source = mMainWindow.CreateImageSourceThumbnia(pdfImgPathList[listIndex - 1], 200, 150);
            else
                PdfImg.Source = mMainWindow.CreateImageSourceThumbnia(null, 200, 150);
            // 删除文件
            if (File.Exists(pdfImgPathList[listIndex]))
                File.Delete(pdfImgPathList[listIndex]);
            //if (global.isSavePdfSoure == 1)
            //{
                //foreach (PreviewPhoto item in mMainWindow.PreviewImgList.Items)
                //{
                //    if(item.ImagePath == pdfImgPathList[listIndex])
                //    {
                //        mMainWindow.PreviewImgList.Items.Remove(item);
                //    }
                //}
                int index = 0;
                for (int i = 0; i < mMainWindow.PreviewImgList.Items.Count; i++)
                {
                    var obj = mMainWindow.PreviewImgList.Items[i] as PreviewPhoto;
                    if (obj.ImagePath == pdfImgPathList[listIndex])
                    {
                        index = i;
                        break;
                    }
                }
                mMainWindow.PreviewImgList.Items.RemoveAt(index);
                //mMainWindow.PreviewImgList.ScrollIntoView(mMainWindow.PreviewImgList.Items[mMainWindow.PreviewImgList.Items.Count - 1]); //设置总显示最后一项
            //}
            // 从列表中移除
            pdfImgPathList.RemoveAt(listIndex);
            // 更新总页数
            NumTextBox.Text = Convert.ToString(pdfImgPathList.Count);
        }
    }
}
