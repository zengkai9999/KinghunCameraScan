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
    /// MorePdfDlg.xaml 的交互逻辑
    /// </summary>
    public partial class MorePdfDlg : Window
    {
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

        private List<string> pdfImgPathList = new List<string>();
        private int listIndex = 0;

        public MorePdfDlg()
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

            if (global.isSavePdfSoure == 0) PdfSetCheck.IsChecked = false;
            else PdfSetCheck.IsChecked = true;

            MainWindow mMainWindow = (MainWindow)this.Owner;
            mMainWindow.MorePdfTransfEvent += StillIdImageCapture;
        }


        private void PdfSetCheck_Checked(object sender, RoutedEventArgs e)
        {
            global.isSavePdfSoure = 1;
        }

        private void PdfSetCheck_Unchecked(object sender, RoutedEventArgs e)
        {
            global.isSavePdfSoure = 0;
        }


        //静态拍照委托传递参数
        void StillIdImageCapture(string imgPath)
        {
            SetFormatType(global.FileFormat);  //还原参数设置
            MainWindow mMainWindow = (MainWindow)this.Owner;
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
                    pdfImgPathList.Add(imgPath);
                    PdfImg.Source = mMainWindow.CreateImageSourceThumbnia(imgPath, 200, 150);
                    PageCountLabel.Content = Convert.ToString(pdfImgPathList.Count);
                }      
            }

        }


        private void StartPdfBt_Click(object sender, RoutedEventArgs e)
        {
            if (global.isOpenCameraA)
            {
                PdfSetCheck.IsEnabled = false; //不使能控件
                MainWindow mMainWindow = (MainWindow)this.Owner;
                if (global.Is16MDevice)   //静态拍照
                {
                    global.pMorePdfFormScanDo = true;
                    SetFormatType(0); //JPG
                    bool isShowToList = true;
                    if (global.isSavePdfSoure == 0) isShowToList = false;
                    if (global.isJoinMainCam == 1 && global.isOpenCameraB)  //如果与主画面合并拍照
                    {
                        //mMainWindow.FuncStillMainAssistImageJoin(0, global.NameMode, isShowToList);
                        mMainWindow.FuncStillMainAssistImageJoin(0, 0, isShowToList);
                        return;
                    }
                    //mMainWindow.FuncCaptureFromStill(0, global.NameMode, isShowToList);
                    mMainWindow.FuncCaptureFromStill(0, 0, isShowToList);
                }
                else    //非静态拍照
                {
                    SetFormatType(0); //JPG
                    bool isShowToList = true;
                    if (global.isSavePdfSoure == 0) isShowToList = false;

                    if (global.isJoinMainCam == 1 && global.isOpenCameraB)  //如果与主画面合并拍照
                    {
                        //string xSrcPath = mMainWindow.FuncMainAssistImageJoin(0, global.NameMode, isShowToList);
                        string xSrcPath = mMainWindow.FuncMainAssistImageJoin(0, 0, isShowToList);
                        SetFormatType(global.FileFormat);
                        if (File.Exists(xSrcPath))
                        {
                            //pdfImgPathList.Add(xSrcPath);
                            pdfImgPathList.Insert(listIndex, xSrcPath);
                            listIndex++;
                            PageCurrentLabel.Content = Convert.ToString(listIndex);
                            PdfImg.Source = mMainWindow.CreateImageSourceThumbnia(xSrcPath, 200, 150);
                            PageCountLabel.Content = Convert.ToString(pdfImgPathList.Count);
                        }                       
                        return;
                    }

                    if (global.isAutoCutMore == 1)
                    {
                        ClearCutPoint();
                        for (int i = 1; i <= 10; i++)
                        {
                            string SrcPath = mMainWindow.FuncCaptureFromPreview(0, 0, isShowToList, i);
                            SetFormatType(global.FileFormat);
                            if (File.Exists(SrcPath))
                            {
                                //pdfImgPathList.Add(SrcPath);
                                pdfImgPathList.Insert(listIndex, SrcPath);
                                listIndex++;
                                PageCurrentLabel.Content = Convert.ToString(listIndex);
                                PdfImg.Source = mMainWindow.CreateImageSourceThumbnia(SrcPath, 200, 150);
                                PageCountLabel.Content = Convert.ToString(pdfImgPathList.Count);
                            }
                        }
                    }
                    else
                    {
                        string SrcPath = mMainWindow.FuncCaptureFromPreview(0, 0, isShowToList, 0);
                        SetFormatType(global.FileFormat);
                        if (File.Exists(SrcPath))
                        {
                            //pdfImgPathList.Add(SrcPath);
                            pdfImgPathList.Insert(listIndex, SrcPath);
                            listIndex++;
                            PageCurrentLabel.Content = Convert.ToString(listIndex);
                            PdfImg.Source = mMainWindow.CreateImageSourceThumbnia(SrcPath, 200, 150);
                            PageCountLabel.Content = Convert.ToString(pdfImgPathList.Count);
                        }
                    }
                                          
                }
            }
            
        }


        //合并PDF
        private void EndPdfBt_Click(object sender, RoutedEventArgs e)
        {
            PdfSetCheck.IsEnabled = true;  //使能控件
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
                if (global.NameMode == 2)
                {
                    global.CalculateSuffix();
                    string CountStr = global.SuffixSupplyZero(global.SuffixCount, global.SuffixLength);
                    pdfpath = global.ImagesFolder + "\\" + global.PrefixNmae + "_" + CountStr + ".pdf";
                    global.PreviousSuffixCount = global.SuffixCount;
                    global.SuffixCount = global.SuffixCount + global.IncreaseStep;
                    if (global.pSetDlgHaveRun)
                    {
                        CountStr = global.SuffixSupplyZero(global.SuffixCount, global.SuffixLength);
                        mMainWindow.mSettingDlg.SuffixTextBox.Text = CountStr;
                    }
                }

               //add at 2019.04.02
                if (global.NameMode == 5)
                {
                    mMainWindow.mFixedNameDlg = new FixedNameDlg();
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
                string prePath = pdfImgPathList[pdfImgPathList.Count-1];
                mPhoto.SourceImage = mMainWindow.CreateImageSourceThumbnia(prePath, 120, 90);
                mPhoto.LogoImage = new BitmapImage(new Uri(@"Images\pdfb.png", UriKind.Relative));
                mPhoto.ImageName = name;
                mPhoto.ImagePath = pdfpath;
                mMainWindow.PreviewImgList.Items.Add(mPhoto);
                mMainWindow.PreviewImgList.ScrollIntoView(mMainWindow.PreviewImgList.Items[mMainWindow.PreviewImgList.Items.Count - 1]); //设置总显示最后一项
            }
            else
            {
                ////if (File.Exists(pdfpath))
                ////    File.Delete(pdfpath);
                //string TipStr = "多页PDF生成失败，合并源图像不存在或已删除！";
                //if (global.pLangusge == 1) TipStr = "多頁PDF生成失敗，合並源圖像不存在或已刪除！";
                //if (global.pLangusge == 2) TipStr = "Multi-page PDF generation failed.\r\n merged source image does not exist or has been deleted!";
                //if (global.pLangusge == 3) TipStr = "La generación de múltiples páginas de PDF fracasó y la fusión de la imagen fuente no existe o fue borrada!";
                //System.Windows.MessageBox.Show(TipStr);
            }

            if (global.isSavePdfSoure == 0)
            {
                for (int i = 0; i < pdfImgPathList.Count; i++)
                {
                    if (File.Exists(pdfImgPathList[i]))
                    {
                        File.Delete(pdfImgPathList[i]);
                    }
                }
            }
            pdfImgPathList.Clear();
            listIndex = 0;
        }


        //退出
        private void ExitBt_Click(object sender, RoutedEventArgs e)
        {
            global.pMorePdfDlgHaveRun = false;
            this.Close();
        }


        private void Window_Closed(object sender, EventArgs e)
        {
            global.pMorePdfDlgHaveRun = false;
            MainWindow mMainWindow = (MainWindow)this.Owner;
            mMainWindow.Focus();
            mMainWindow.MorePdfTransfEvent -= StillIdImageCapture;
            global.pMorePdfDlgHaveRun = false;
            EndPdfBt_Click(null,null);
            global.pMorePdfFormScanDo = false;
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
            if(listIndex > 0)
                PdfImg.Source = mMainWindow.CreateImageSourceThumbnia(pdfImgPathList[listIndex - 1], 200, 150);
            else
                PdfImg.Source = mMainWindow.CreateImageSourceThumbnia(null, 200, 150);
            // 删除文件
            if (File.Exists(pdfImgPathList[listIndex]))
                File.Delete(pdfImgPathList[listIndex]);
            if (global.isSavePdfSoure == 1)
            {
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
            }
            // 从列表中移除
            pdfImgPathList.RemoveAt(listIndex);
            // 更新总页数
            PageCountLabel.Content = Convert.ToString(pdfImgPathList.Count);
        }
    }
}
