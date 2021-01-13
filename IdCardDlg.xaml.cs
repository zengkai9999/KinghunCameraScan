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
using System.Windows.Threading;
using System.IO;
using System.Windows.Interop;

namespace CameraScan
{
    /// <summary>
    /// IdCardDlg.xaml 的交互逻辑
    /// </summary>
    public partial class IdCardDlg : Window
    {

        [DllImport("shell32.dll")]
        public extern static Int32 ShellExecute(IntPtr hwnd, string lpOperation, string lpFile, string lpParameters, string lpDirectory, int nShowCmd);

        [DllImport("DevCapture.dll", CallingConvention = CallingConvention.Cdecl)]
        public extern static void SetCutType(int cutType);

        [DllImport("DevCapture.dll",  CallingConvention = CallingConvention.Cdecl)]
        public extern static void SetFormatType(int format);

        [DllImport("DevCapture.dll",  CallingConvention = CallingConvention.Cdecl)]
        public extern static int CombineIdCardImge(byte[] desPath, byte[] srcPath1, byte[] srcPath2, int isDelete);

        [DllImport("DevCapture.dll", CallingConvention = CallingConvention.Cdecl)]
        public extern static int ReadIdCard(byte[] folderPath);

        [DllImport("DevCapture.dll", CallingConvention = CallingConvention.Cdecl)]
        public extern static IntPtr GetIdCardInfo(int index);


        [DllImport("CmRecongize.dll")]
        public static extern string ReadCard(string listFilePath, int count, int type, string filePath);
        [DllImport("CmRecongize.dll")]
        public static extern string ReadBarCode(string listFilePath, int count, string filePath);
        [DllImport("CmRecongize.dll")]
        public static extern string ReadBarCode2(byte[] byt, int width, int height, int bpp);

        public static string ReadIDCard(string[] listFilePath, int cardType, string filePath)
        {
            return ReadCard(listFilePath[0], listFilePath.Length, cardType, filePath);
        }


        string PathA;
        string PathB;
        string CombinePath;
        int StillScanCount = 0;
        List<string[]> cardInfols = new List<string[]>();

        public IdCardDlg()
        {
            InitializeComponent();      
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            global.pIdCardDlgHaveRun = false;
            MainWindow mMainWindow = (MainWindow)this.Owner;
            mMainWindow.Focus();
            mMainWindow.IdCardTransfEvent -= StillIdImageCapture;  //注销委托
            global.pIdCardDlgHaveRun = false;
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
            MainWindow mMainWindow = (MainWindow)this.Owner;
            mMainWindow.IdCardTransfEvent += StillIdImageCapture;
        }

        private void ExitBt_Click(object sender, RoutedEventArgs e)
        {
            global.pIdCardDlgHaveRun = false;
            this.Close();
        }

        //静态拍照委托传递参数
        void StillIdImageCapture(string imgPath)
        {
            SetFormatType(global.FileFormat);  //还原参数设置
            SetCutType(global.CutType);        //还原参数设置

            MainWindow mMainWindow = (MainWindow)this.Owner;
            string ocrImgPath = imgPath;
            if (File.Exists(ocrImgPath))
            {
                //System.Diagnostics.Process.Start(exePath, ocrImgPath);
                string excelPath = global.ImagesFolder + "\\IdCard";
                string text4 = ReadCard(ocrImgPath, 1, 2, excelPath);
                excelPath += ".csv";
                if (text4.Length <= 0)
                {
                    string TipStr = "      未识别到身份证信息！\r\n请尽量保持身份证水平放置以及\r\n识别的范围为身份证大小范围！";
                    if (global.pLangusge == 1) TipStr = "      未識別到身份證信息！\r\n請盡量保持身份證水平放置以及\r\n識別的範圍為身份證大小範圍！";
                    if (global.pLangusge == 2) TipStr = "    ID card information is not recognized. \r\n please keep the ID card level as far as possible \r\n and the scope of identification is the size of the ID card.";
                    if (global.pLangusge == 3) TipStr = "      La tarjeta de identificación no puede ser reconocida. \r\n Por favor, mantener la tarjeta \r\n de identificación horizontal y dentro del área de captura.";
                    if (global.pLangusge == 4) TipStr = "      IDカードが認識できませんでした。\r\n IDカードを読み取る箇所は水平に保ってください。";
                    if (global.pLangusge == 5) TipStr = "Il documento d'identità non può essere riconosciuto.\r\n Prova a mantenere il documento d'identità in posizione \r\n orizzontale e all'interno della zona di acquisizione.";
                    if (global.pLangusge == 6) TipStr = "La carte d'identité ne peut pas être reconnue, \r\nveuillez essayer de garder la carte d'identité à l'horizontale et dans la plage capturée.";
                    System.Windows.MessageBox.Show(TipStr);
                    return;
                }
                if (File.Exists(excelPath))
                {
                    //global.OpenFileAndPreview(excelPath);
                    //CSVHelper.OpenCSV(excelPath);
                    cardInfols.Clear();
                    int iRes = CSVHelper.ReadCSV(excelPath, cardInfols);
                    if (iRes != 0)
                    {
                        string TipStr = "      未识别到身份证信息！\r\n请先关闭已打开的身份证数据表格！";
                        if (global.pLangusge == 1) TipStr = "      未識別到身份證信息！\r\n請先關閉已打開的身份證數據表格！";
                        if (global.pLangusge == 2) TipStr = "   ID card information is not recognized. \r\n please turn off the open ID data table first!";
                        if (global.pLangusge == 3) TipStr = "   La tarjeta de identificación no puede ser reconocida. \r\n Por favor, cerrar el formulario abierto. ";
                        if (global.pLangusge == 4) TipStr = "   IDカードが認識できませんでした。\r\n 開いているIDカードのデータフォームを閉じてください。 ";
                        if (global.pLangusge == 5) TipStr = "Il documento d'identità non può essere riconosciuto, \r\n si prega di chiudere il modulo dei dati del documento d'identità prima dell’acquisizione.";
                        if (global.pLangusge == 6) TipStr = "La carte d'identité ne peut pas être reconnue, \r\n veuillez d'abord fermer le formulaire de données de la carte d'identité.";
                        System.Windows.MessageBox.Show(TipStr);
                        return;
                    }
                    string[] infos = cardInfols[cardInfols.Count - 1];
                    if (infos.Length > 5)
                    {
                        NameTextBox.Text = infos[1];
                        SexTextBox.Text = infos[2];
                        NationTextBox.Text = infos[3];
                        BornTextBox.Text = infos[4].Substring(0, infos[4].Length - 1);
                        AddressTextBox.Text = infos[5];
                        NumTextBox.Text = infos[6].Substring(0, infos[6].Length - 1);

                        string headImgPath = infos[7];
                        if (File.Exists(headImgPath))
                        {
                            CardHeadImg.Source = mMainWindow.CreateImageSourceThumbnia(headImgPath, 85, 100);
                        }
                    }
                    return;
                }
            }

        
            if (StillScanCount == 1)   //正面照
            {
                PathA = imgPath;
                if (File.Exists(PathA))
                    CardImgA.Source = mMainWindow.CreateImageSourceThumbnia(PathA, 240, 180);
                CardImgB.Source = null;
            }

            if (StillScanCount == 2)   //反面照
            {
                StillScanCount = 0;

                PathB = imgPath;
                if (File.Exists(PathB))
                    CardImgB.Source = mMainWindow.CreateImageSourceThumbnia(PathB, 240, 180);

                DispatcherTimer timer = new DispatcherTimer();
                timer.Interval = new TimeSpan(0, 0, 0, 0, 5);
                timer.Tick += delegate
                {
                    CombineIdCardPhoto(PathA, PathB);
                    timer.Stop();
                };
                timer.Start();
            }



            
        }

        private void ScanBt_Click(object sender, RoutedEventArgs e)
        {
            if (global.isOpenCameraA)
            {
                if (ScanBt.Content.ToString() == "正面拍摄")
                {
                    MainWindow mMainWindow = (MainWindow)this.Owner;
                    if (global.Is16MDevice)
                    {
                        global.pIdCardFormScanDo = true;
                        StillScanCount = 1;
                        SetCutType(1);
                        SetFormatType(0); //JPG
                        Thread.Sleep(20);
                        mMainWindow.FuncCaptureFromStill(0, global.NameMode, true);                                             
                    }
                    else 
                    {
                        SetCutType(1);
                        SetFormatType(0); //JPG
                        Thread.Sleep(20);
                        PathA = mMainWindow.FuncCaptureFromPreview(0, global.NameMode, true, 0);
                        SetFormatType(global.FileFormat);
                        SetCutType(global.CutType);
                        if (File.Exists(PathA))
                            CardImgA.Source = mMainWindow.CreateImageSourceThumbnia(PathA, 240, 180);
                        CardImgB.Source = null;
                    }
                    ScanBt.Content = "反面拍摄";
                }
                else
                {
                    MainWindow mMainWindow = (MainWindow)this.Owner;
                    if (global.Is16MDevice)
                    {
                        global.pIdCardFormScanDo = true;
                        StillScanCount = 2;
                        SetCutType(1);
                        SetFormatType(0); //JPG
                        Thread.Sleep(20);
                        mMainWindow.FuncCaptureFromStill(0, global.NameMode, true);
                       
                    }
                    else
                    {
                        SetCutType(1);
                        SetFormatType(0); //JPG
                        Thread.Sleep(20);
                        PathB = mMainWindow.FuncCaptureFromPreview(0, global.NameMode, true, 0);
                        SetFormatType(global.FileFormat);
                        SetCutType(global.CutType);
                        if (File.Exists(PathB))
                            CardImgB.Source = mMainWindow.CreateImageSourceThumbnia(PathB, 240, 180);

                        DispatcherTimer timer = new DispatcherTimer();
                        timer.Interval = new TimeSpan(0, 0, 0, 0, 5);
                        timer.Tick += delegate
                        {
                            CombineIdCardPhoto(PathA, PathB);
                            timer.Stop();
                        };
                        timer.Start();
                    }
                    ScanBt.Content = "正面拍摄";
                }
            }
        }


        private void CombineIdCardPhoto(string srcPath1, string srcPath2)
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

            byte[] pDesBuf = Encoding.GetEncoding(global.pEncodType).GetBytes(imgpath);
            byte[] pSrcBuf1 = Encoding.GetEncoding(global.pEncodType).GetBytes(srcPath1);
            byte[] pSrcBuf2 = Encoding.GetEncoding(global.pEncodType).GetBytes(srcPath2);
            int iRest=CombineIdCardImge(pDesBuf, pSrcBuf1, pSrcBuf2,0);
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
                    CardImg.Source = mMainWindow.CreateImageSourceThumbnia(prePath, 165, 200);
                }
                    
                if (File.Exists(imgpath))
                {
                    mMainWindow.AddPreviewImageToList(imgpath, 210, 297);
                    CombinePath = imgpath;
                    if (global.FileFormat == 4)
                        File.Delete(prePath);
                }            
            }

        }



        private void CardImg_MouseDown(object sender, MouseButtonEventArgs e)
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
                if (File.Exists(PathA))
                {
                    global.OpenFileAndPreview(PathA);
                }
            }
        }

        private void CardImgB_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                if (File.Exists(PathB))
                {
                    //IntPtr hwnd = ((HwndSource)PresentationSource.FromVisual(this)).Handle;
                    //ShellExecute(hwnd, "open", PathB, null, null, 4);  //打开文件
                    global.OpenFileAndPreview(PathB);
                }
            }
        }

        //****************************************************************************//

        //身份证OCR识别
        private void CardOcrBt_Click(object sender, RoutedEventArgs e)
        {
            string headImgpath = System.Windows.Forms.Application.StartupPath + "\\headPic.bmp";
            if (File.Exists(headImgpath))
            {
                File.Delete(headImgpath);
            }

            if (global.isOpenCameraA)
            {

                NameTextBox.Text = "";
                SexTextBox.Text = "";
                NationTextBox.Text = "";
                BornTextBox.Text = "";
                AddressTextBox.Text = "";
                NumTextBox.Text = "";
                CardHeadImg.Source = null;

                MainWindow mMainWindow = (MainWindow)this.Owner;
                if (global.Is16MDevice)
                {
                    global.pIdCardFormScanDo = true;
                    SetCutType(1);
                    SetFormatType(0); //JPG
                    Thread.Sleep(20);
                    mMainWindow.FuncCaptureFromStill(0, 0, true);
                }
                else
                {
                    SetFormatType(0); //JPG
                    Thread.Sleep(20);
                    string ocrImgPath = mMainWindow.FuncCaptureFromPreview(0, global.NameMode, true, 0);
                    SetFormatType(global.FileFormat);
                    if (File.Exists(ocrImgPath))
                    {
                        //System.Diagnostics.Process.Start(exePath, ocrImgPath);
                        string excelPath = global.ImagesFolder + "\\IdCard";
                        string text4 = ReadCard(ocrImgPath, 1, 2, excelPath);
                        excelPath += ".csv";
                        if (text4.Length <= 0)
                        {
                            string TipStr = "      未识别到身份证信息！\r\n请尽量保持身份证水平放置以及\r\n识别的范围为身份证大小范围！";
                            if (global.pLangusge == 1) TipStr = "      未識別到身份證信息！\r\n請盡量保持身份證水平放置以及\r\n識別的範圍為身份證大小範圍！";
                            if (global.pLangusge == 2) TipStr = "    ID card information is not recognized. \r\n please keep the ID card level as far as possible \r\n and the scope of identification is the size of the ID card.";
                            if (global.pLangusge == 3) TipStr = "      La tarjeta de identificación no puede ser reconocida. \r\n Por favor, mantener la tarjeta \r\n de identificación horizontal y dentro del área de captura.";
                            if (global.pLangusge == 4) TipStr = "      IDカードが認識できませんでした。\r\n IDカードを読み取る箇所は水平に保ってください。";
                            if (global.pLangusge == 5) TipStr = "Il documento d'identità non può essere riconosciuto.\r\n Prova a mantenere il documento d'identità in posizione \r\n orizzontale e all'interno della zona di acquisizione.";
                            if (global.pLangusge == 6) TipStr = "La carte d'identité ne peut pas être reconnue, \r\nveuillez essayer de garder la carte d'identité à l'horizontale et dans la plage capturée.";
                            System.Windows.MessageBox.Show(TipStr);
                            return;
                        }
                        if (File.Exists(excelPath))
                        {
                            //global.OpenFileAndPreview(excelPath);
                            //CSVHelper.OpenCSV(excelPath);
                            cardInfols.Clear();
                            int iRes=CSVHelper.ReadCSV(excelPath, cardInfols);
                            if (iRes != 0)
                            {
                                string TipStr = "      未识别到身份证信息！\r\n请先关闭已打开的身份证数据表格！";
                                if (global.pLangusge == 1) TipStr = "      未識別到身份證信息！\r\n請先關閉已打開的身份證數據表格！";
                                if (global.pLangusge == 2) TipStr = "   ID card information is not recognized. \r\n please turn off the open ID data table first!";
                                if (global.pLangusge == 3) TipStr = "   La tarjeta de identificación no puede ser reconocida. \r\n Por favor, cerrar el formulario abierto. ";
                                if (global.pLangusge == 4) TipStr = "   IDカードが認識できませんでした。\r\n 開いているIDカードのデータフォームを閉じてください。 ";
                                if (global.pLangusge == 5) TipStr = "Il documento d'identità non può essere riconosciuto, \r\n si prega di chiudere il modulo dei dati del documento d'identità prima dell’acquisizione.";
                                if (global.pLangusge == 6) TipStr = "La carte d'identité ne peut pas être reconnue, \r\n veuillez d'abord fermer le formulaire de données de la carte d'identité.";
                                System.Windows.MessageBox.Show(TipStr);
                                return;
                            }                           
                            string[] infos = cardInfols[cardInfols.Count - 1];
                            if (infos.Length > 5)
                            {
                                NameTextBox.Text = infos[1];
                                SexTextBox.Text = infos[2];
                                NationTextBox.Text = infos[3];
                                BornTextBox.Text = infos[4].Substring(0, infos[4].Length - 1);
                                AddressTextBox.Text = infos[5];
                                NumTextBox.Text = infos[6].Substring(0, infos[6].Length - 1);

                                string headImgPath = infos[7];
                                if (File.Exists(headImgPath))
                                {
                                    CardHeadImg.Source = mMainWindow.CreateImageSourceThumbnia(headImgPath, 85, 100);
                                }
                            }                                              
                            return;
                        }
                    }
    
                }
            }
        }


        private void SaveInfoBt_Click(object sender, RoutedEventArgs e)
        {
            if (global.isOpenCameraA)
            {
                if (NameTextBox.Text == "" && SexTextBox.Text == "" && NationTextBox.Text == "" &&
                    BornTextBox.Text == "" && AddressTextBox.Text == "" && NumTextBox.Text == "")
                {
                    string TipStr = "保存的身份证信息不能为空！";
                    if (global.pLangusge == 1) TipStr = "保存的身份證信息不能為空！";
                    if (global.pLangusge == 2) TipStr = "The ID information can not be empty!";
                    if (global.pLangusge == 3) TipStr = "La información de la tarjeta ID guardada no puede estar vacía.";
                    if (global.pLangusge == 4) TipStr = "保存するIDカード情報は空にできません。";
                    if (global.pLangusge == 5) TipStr = "Le informazioni del documento d'identità salvate non possono essere vuote.";
                    if (global.pLangusge == 6) TipStr = "Les informations de la carte d'identité enregistrées ne peuvent pas être vides.";
                    System.Windows.MessageBox.Show(TipStr);
                    return;
                }
                if (cardInfols.Count > 0)
                {
                    string[] infos = cardInfols[cardInfols.Count - 1];
                    infos[1] = NameTextBox.Text;
                    infos[2] = SexTextBox.Text;
                    infos[3] = NationTextBox.Text;
                    infos[4] = BornTextBox.Text + "'";
                    infos[5] = AddressTextBox.Text;
                    infos[6] = NumTextBox.Text + "'";
                    cardInfols.RemoveAt(cardInfols.Count - 1);
                    cardInfols.Add(infos);
                    string excelPath = global.ImagesFolder + "\\IdCard.csv";
                    if (File.Exists(excelPath))
                    {
                        int iRes=CSVHelper.WriteCSV(excelPath, cardInfols);
                        if (iRes != 0)
                        {
                            string TipStr = "保存修改的信息前，请先关闭已打开的身份证数据表格！";
                            if (global.pLangusge == 1) TipStr = "保存修改的信息前，請先關閉已打開的身份證數據表格！";
                            if (global.pLangusge == 2) TipStr = "Please close the open ID data table before saving the modified information.";
                            if (global.pLangusge == 3) TipStr = "Por favor, cierre el formulario de datos de tarjeta de identificación abierto antes de guardar la información modificada.";
                            if (global.pLangusge == 4) TipStr = "修正した情報を保存する前に開いているIDカードのデータフォームを閉じてください。";
                            if (global.pLangusge == 5) TipStr = "Chiudere il modulo dati del documento d'identità aperto prima di salvare le informazioni modificate.";
                            if (global.pLangusge == 6) TipStr = "Veuillez fermer le formulaire de données de la carte d'identité ouvert avant d'enregistrer les informations modifiées.";
                            System.Windows.MessageBox.Show(TipStr);
                            return;
                        }
                        string TipStr2 = "修改成功";
                        if (global.pLangusge == 1) TipStr2 = "修改成功";
                        if (global.pLangusge == 2) TipStr2 = "Amend success";
                        if (global.pLangusge == 3) TipStr2 = "Datos modificados correctamente.";
                        if (global.pLangusge == 4) TipStr2 = "修正が完了しました。";
                        if (global.pLangusge == 5) TipStr2 = "Modifica riuscita.";
                        if (global.pLangusge == 6) TipStr2 = "La modification a réussi.";
                        System.Windows.MessageBox.Show(TipStr2);
                    }
                }
                else 
                {
                    string TipStr = "保存修改的信息前，请先关闭已打开的身份证数据表格！";
                    if (global.pLangusge == 1) TipStr = "保存修改的信息前，請先關閉已打開的身份證數據表格！";
                    if (global.pLangusge == 2) TipStr = "Please close the open ID data table before saving the modified information.";
                    if (global.pLangusge == 3) TipStr = "Por favor, cierre el formulario de datos de tarjeta de identificación abierto antes de guardar la información modificada.";
                    if (global.pLangusge == 4) TipStr = "修正した情報を保存する前に開いているIDカードのデータフォームを閉じてください。";
                    if (global.pLangusge == 5) TipStr = "Chiudere il modulo dati del documento d'identità aperto prima di salvare le informazioni modificate.";
                    if (global.pLangusge == 6) TipStr = "Veuillez fermer le formulaire de données de la carte d'identité ouvert avant d'enregistrer les informations modifiées.";
                    System.Windows.MessageBox.Show(TipStr);
                    return;
                }
            }
        }

        private void OpenExcelBt_Click(object sender, RoutedEventArgs e)
        {
            string excelPath = global.ImagesFolder + "\\IdCard.csv";        
            try
            {
                if (File.Exists(excelPath))
                    global.OpenFileAndPreview(excelPath);
            }
            catch (Exception ex)
            {

            }              
        }

        private void ReadCardBt_Click(object sender, RoutedEventArgs e)
        {
            NameTextBox.Text = "";
            SexTextBox.Text = "";
            NationTextBox.Text = "";
            BornTextBox.Text = "";
            AddressTextBox.Text = "";
            NumTextBox.Text = "";
            CardHeadImg.Source = null;

            byte[] pBuf = Encoding.GetEncoding(global.pEncodType).GetBytes(global.ImagesFolder);
            int iRest = ReadIdCard(pBuf);
            if (iRest == -1)
            {
                string TipStr = "读卡失败！请检查读卡器连接是否正常！";
                if (global.pLangusge == 1) TipStr = "讀卡失敗！請檢查讀卡器連接是否正常！";
                if (global.pLangusge == 2) TipStr = "Failed to read card! Please check whether the card reader connection is normal.";
                if (global.pLangusge == 3) TipStr = "La lectura de la tarjeta ha fallado. Por favor, compruebe si el lector está conectado correctamente.";
                if (global.pLangusge == 4) TipStr = "カードの読み取りができませんでした。カードリーダーが正しく接続されているかご確認ください。";
                if (global.pLangusge == 5) TipStr = "Lettura della carta non riuscita, controllare se il lettore è collegato correttamente!";
                if (global.pLangusge == 6) TipStr = "La lecture de la carte a échoué, veuillez vérifier si le lecteur est correctement connecté!";
                System.Windows.MessageBox.Show(TipStr);
                return;
            }
            if (iRest == -2)
            {
                string TipStr = "读卡失败！";
                if (global.pLangusge == 1) TipStr = "讀卡失敗！";
                if (global.pLangusge == 2) TipStr = "Failed to read card!";
                if (global.pLangusge == 3) TipStr = "Fallo de lectura de la tarjeta!";
                if (global.pLangusge == 4) TipStr = "カードの読み取り不可!";
                if (global.pLangusge == 5) TipStr = "Errore di lettura della carta!";
                if (global.pLangusge == 6) TipStr = "Échec de lecture de la carte!";
                System.Windows.MessageBox.Show(TipStr);
                return;
            }

            if (iRest == 0)
            {
                IntPtr info = GetIdCardInfo(0);
                string infoStr = Marshal.PtrToStringAnsi(info);
                NameTextBox.Text = infoStr;
                info = GetIdCardInfo(1);
                infoStr = Marshal.PtrToStringAnsi(info);
                SexTextBox.Text = infoStr;
                info = GetIdCardInfo(2);
                infoStr = Marshal.PtrToStringAnsi(info);
                NationTextBox.Text = infoStr;
                info = GetIdCardInfo(3);
                infoStr = Marshal.PtrToStringAnsi(info);
                infoStr = infoStr.Substring(0, 4) + "-" + infoStr.Substring(4, 2) + "-" + infoStr.Substring(6, 2);
                BornTextBox.Text = infoStr;
                info = GetIdCardInfo(4);
                infoStr = Marshal.PtrToStringAnsi(info);
                AddressTextBox.Text = infoStr;
                info = GetIdCardInfo(5);
                infoStr = Marshal.PtrToStringAnsi(info);
                NumTextBox.Text = infoStr;

                info = GetIdCardInfo(9);
                string headImgPath= Marshal.PtrToStringAnsi(info);
                if (File.Exists(headImgPath))
                {
                    MainWindow mMainWindow = (MainWindow)this.Owner;
                    CardHeadImg.Source = mMainWindow.CreateImageSourceThumbnia(headImgPath, 85, 100);
                }
                   
                try
                {
                    int iRes;
                    string excelPath = global.ImagesFolder + "\\IdCard.csv"; 
                    cardInfols.Clear();
                    //int iRes=CSVHelper.ReadCSV(excelPath, cardInfols);
                    //if (iRes != 0)
                    //{
                    //    string TipStr = "写入身份证数据失败!请关闭已打开的身份证数据表格！";
                    //    if (global.pLangusge == 1) TipStr = "寫入身份證數據失敗!請關閉已打開的身份證數據表格！";
                    //    if (global.pLangusge == 2) TipStr = "Failed to write ID data! Please close the open ID data table!";
                    //    if (global.pLangusge == 3) TipStr = "No se ha podido guardar la tarjeta de identificación. Por favor, cierre el formulario de tarjeta de identificación abierto.";
                    //    if (global.pLangusge == 4) TipStr = "IDカードへの書き込みができませんでした。開いているIDカードのデータフォームテーブルを閉じてください。";
                    //    System.Windows.MessageBox.Show(TipStr);
                    //    return;
                    //}
                    string[] CardInfo = new string[11];
                    CardInfo[0] = "";
                    CardInfo[1] = NameTextBox.Text;
                    CardInfo[2] = SexTextBox.Text;
                    CardInfo[3] = NationTextBox.Text;
                    CardInfo[4] = BornTextBox.Text + "'";
                    CardInfo[5] = AddressTextBox.Text;
                    CardInfo[6] = NumTextBox.Text + "'";
                    infoStr = Marshal.PtrToStringAnsi(GetIdCardInfo(6));
                    CardInfo[7] = infoStr;
                    infoStr = Marshal.PtrToStringAnsi(GetIdCardInfo(7)) + "-" + Marshal.PtrToStringAnsi(GetIdCardInfo(8));
                    CardInfo[8] = infoStr;
                    CardInfo[9] = headImgPath;
                    string timeStr = DateTime.Now.ToString("yyyy-M-dd H:mm");
                    CardInfo[10] = timeStr;
                    cardInfols.Add(CardInfo);
                    iRes = CSVHelper.WriteCSV(excelPath, cardInfols);
                    if (iRes != 0)
                    {
                        string TipStr = "写入身份证数据失败!请关闭已打开的身份证数据表格！";
                        if (global.pLangusge == 1) TipStr = "寫入身份證數據失敗!請關閉已打開的身份證數據表格！";
                        if (global.pLangusge == 2) TipStr = "Failed to write ID data! Please close the open ID data table!";
                        if (global.pLangusge == 3) TipStr = "No se ha podido guardar la tarjeta de identificación. Por favor, cierre el formulario de tarjeta de identificación abierto.";
                        if (global.pLangusge == 4) TipStr = "IDカードへの書き込みができませんでした。開いているIDカードのデータフォームテーブルを閉じてください。";
                        if (global.pLangusge == 5) TipStr = "Impossibile scrivere i dati del documento d'identità! Chiudere la tabella dei dati del documento d'identità!";
                        if (global.pLangusge == 6) TipStr = "Échec de l'écriture des données de la carte d'identité! Veuillez fermer le tableau de données de la carte d'identité ouvert!";
                        System.Windows.MessageBox.Show(TipStr);
                        return;
                    }
                }
                catch (Exception ex)
                {

                }
            }
        }
             
    }
}
