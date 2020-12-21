
//#define  VERIFY
//#define  ADDRESOLUTION

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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Runtime.InteropServices;
using System.Windows.Threading;
using System.Windows.Interop;
using System.IO;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using System.Drawing.Printing;
//using Spire.Pdf;
using System.Diagnostics;

namespace CameraScan
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    /// 
   
    public partial class MainWindow : Window
    {

        
        #region import dll
     
        [DllImport("shell32.dll")]
        public extern static Int32 ShellExecute(IntPtr hwnd, string lpOperation, string lpFile, string lpParameters, string lpDirectory, int nShowCmd);

        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        public extern static bool DeleteObject(IntPtr hObject);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate int PFCALLBACK(IntPtr buf, int ww, int hh);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate int STILLPHOTOCALLBACK(IntPtr Imagepath,int FileType, int isAddList);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate int VIDEOPARAMCALLBACK();

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate int HIDDEVCALLBACK( int signal);

        [DllImport("DevCapture.dll",  CallingConvention = CallingConvention.Cdecl)]
        public extern static int GetDeviceCount();

        [DllImport("DevCapture.dll",  CallingConvention = CallingConvention.Cdecl)]
        public extern static int GetDeviceName(int index, byte[] nbuf);

        [DllImport("DevCapture.dll", CallingConvention = CallingConvention.Cdecl)]
        public extern static int GetResolutionCount(int index);

        [DllImport("DevCapture.dll",  CallingConvention = CallingConvention.Cdecl)]
        public extern static int GetStillResolutionCount(int index);

        [DllImport("DevCapture.dll",  CallingConvention = CallingConvention.Cdecl)]
        public extern static int GetResolution(int R_index, ref int width, ref int height);

        [DllImport("DevCapture.dll",  CallingConvention = CallingConvention.Cdecl)]
        public extern static int GetStillResolution(int R_index, ref int width, ref int height);

        [DllImport("DevCapture.dll", CallingConvention = CallingConvention.Cdecl)]
        public extern static int SetResolution(int stillIndex, int previewIndex);

        [DllImport("DevCapture.dll",  CallingConvention = CallingConvention.Cdecl)]
        public extern static int OpenDevice(int index, int width, int height, IntPtr mhwnd, Boolean isDisplay);

        [DllImport("DevCapture.dll",  CallingConvention = CallingConvention.Cdecl)]
        public extern static int CloseDevice();

        [DllImport("DevCapture.dll", CallingConvention = CallingConvention.Cdecl)]
        public extern static int Is500InSert800ValDevice();

        [DllImport("DevCapture.dll", CallingConvention = CallingConvention.Cdecl)]
        public extern static int Is1300InSert1500ValDevice();

        [DllImport("DevCapture.dll", CallingConvention = CallingConvention.Cdecl)]
        public extern static int Is1300InSert1600ValDevice();

        [DllImport("DevCapture.dll", CallingConvention = CallingConvention.Cdecl)]
        public extern static void StillCaptureSuccess(byte[] path, int formattype);

        [DllImport("DevCapture.dll", CallingConvention = CallingConvention.Cdecl)]
        public extern static void SetCallBackFunction(PFCALLBACK mCB);

        [DllImport("DevCapture.dll",  CallingConvention = CallingConvention.Cdecl)]
        public extern static void SetStillPhotoCallBack(STILLPHOTOCALLBACK mCB);

        [DllImport("DevCapture.dll", CallingConvention = CallingConvention.Cdecl)]
        public extern static IntPtr CaptureFromPreview(byte[] Imagepath, int type);

        [DllImport("DevCapture.dll",  CallingConvention = CallingConvention.Cdecl)]
        public extern static int CaptureFromStill(byte[] Imagepath,int type);

        [DllImport("DevCapture.dll",  CallingConvention = CallingConvention.Cdecl)]
        public extern static void SetCutType(int cutType);

        [DllImport("DevCapture.dll",  CallingConvention = CallingConvention.Cdecl)]
        public extern static void SetFormatType(int format);

        [DllImport("DevCapture.dll",  CallingConvention = CallingConvention.Cdecl)]
        public extern static void SetColorType(int type);

        [DllImport("DevCapture.dll",  CallingConvention = CallingConvention.Cdecl)]
        public extern static void SetRotateAngle(int angle);

        [DllImport("DevCapture.dll",  CallingConvention = CallingConvention.Cdecl)]
        public extern static void SetDelBlackEdge(int flag);

        [DllImport("DevCapture.dll",  CallingConvention = CallingConvention.Cdecl)]
        public extern static void SetDelBgColor(int flag);

        [DllImport("DevCapture.dll", EntryPoint = "SetDelShade", CallingConvention = CallingConvention.Cdecl)]
        public extern static void SetDelShade(int flag);

        [DllImport("DevCapture.dll", EntryPoint = "SetDelGrayBg", CallingConvention = CallingConvention.Cdecl)]
        public extern static void SetDelGrayBg(int flag);

        [DllImport("DevCapture.dll",  CallingConvention = CallingConvention.Cdecl)]
        public extern static void SetManualCutRect(double left,double top,double right, double bottom);

        [DllImport("DevCapture.dll",  CallingConvention = CallingConvention.Cdecl)]
        public extern static int GetVideoProcParms(int ParamID, ref int min, ref int max, ref int def, ref int current);

        [DllImport("DevCapture.dll", CallingConvention = CallingConvention.Cdecl)]
        public extern static int GetVideoProcParmsEx(int ParamID, ref int min, ref int max, ref int def, ref int current, ref int flag);

        [DllImport("DevCapture.dll",  CallingConvention = CallingConvention.Cdecl)]
        public extern static int SetVideoProcParms(int ParamID, int val);

        [DllImport("DevCapture.dll", CallingConvention = CallingConvention.Cdecl)]
        public extern static int SetVideoProcParmsEx(int ParamID, int val,int flag);

        [DllImport("DevCapture.dll",  CallingConvention = CallingConvention.Cdecl)]
        public extern static int GetCameraCotrolParms(int ParamID, ref int min, ref int max, ref int def, ref int current, ref int flag);

        [DllImport("DevCapture.dll",  CallingConvention = CallingConvention.Cdecl)]
        public extern static int SetCameraCotrolParms(int ParamID, int val, int flag);

        [DllImport("DevCapture.dll",  CallingConvention = CallingConvention.Cdecl)]
        public extern static int ShowCameraSettingWindow();

        [DllImport("DevCapture.dll",  CallingConvention = CallingConvention.Cdecl)]
        public extern static void SetVideoParamsCallBack(VIDEOPARAMCALLBACK mCB);

        [DllImport("DevCapture.dll", CallingConvention = CallingConvention.Cdecl)]
        public extern static int ManualFocus();

        [DllImport("DevCapture.dll", CallingConvention = CallingConvention.Cdecl)]
        public extern static int GetXDPI();
        [DllImport("DevCapture.dll", CallingConvention = CallingConvention.Cdecl)]
        public extern static int GetYDPI();

        [DllImport("DevCapture.dll", CallingConvention = CallingConvention.Cdecl)]
        public extern static int pdf_start(byte[] pdfpath);
        [DllImport("DevCapture.dll", CallingConvention = CallingConvention.Cdecl)]
        public extern static int pdf_addPage(byte[] pdfpath);
        [DllImport("DevCapture.dll", CallingConvention = CallingConvention.Cdecl)]
        public extern static int pdf_end();
        [DllImport("DevCapture.dll", CallingConvention = CallingConvention.Cdecl)]
        public extern static int GetSN(byte[] snbuf);

        [DllImport("DevCapture.dll", CallingConvention = CallingConvention.Cdecl)]
        public extern static int GetDeviceLisences(byte[] dbuf, int len);

        [DllImport("DevCapture.dll", CallingConvention = CallingConvention.Cdecl)]
        public extern static IntPtr MainAssistJoinImages(byte[] Imagepath, int isBarcode,byte[] slaveBuf, int slWidth,int slHeight,
                                                         int jx, int jy, int jw, int jh ,int JoinType);
        [DllImport("DevCapture.dll", CallingConvention = CallingConvention.Cdecl)]
        public extern static int StillMainAssistJoinImages(byte[] Imagepath, int isBarcode, byte[] slaveBuf, int slWidth, int slHeight,
                                                         int jx, int jy, int jw, int jh, int JoinType);

        [DllImport("DevCapture.dll", CallingConvention = CallingConvention.Cdecl)]
        public extern static void SetDpi(int dpiType,int dpiVal);

        [DllImport("DevCapture.dll", CallingConvention = CallingConvention.Cdecl)]
        public extern static int GetRoiWidthHeight(ref int RWidth,ref int RHeight);

        [DllImport("DevCapture.dll", CallingConvention = CallingConvention.Cdecl)]
        public extern static int OpenHidDevice(HIDDEVCALLBACK cb);

        [DllImport("DevCapture.dll", CallingConvention = CallingConvention.Cdecl)]
        public extern static int CloseHidDevice();

        [DllImport("DevCaptureB.dll", CallingConvention = CallingConvention.Cdecl)]
        public extern static int SetResolutionB(int stillIndex, int previewIndex);

        [DllImport("DevCaptureB.dll", CallingConvention = CallingConvention.Cdecl)]
        public extern static int OpenDeviceB(int index, int width, int height, IntPtr mhwnd, Boolean isDisplay);

        [DllImport("DevCaptureB.dll", CallingConvention = CallingConvention.Cdecl)]
        public extern static int CloseDeviceB();

        [DllImport("DevCaptureB.dll", CallingConvention = CallingConvention.Cdecl)]
        public extern static void SetCallBackFunctionB(PFCALLBACK mCB);

        [DllImport("DevCaptureB.dll", CallingConvention = CallingConvention.Cdecl)]
        public extern static IntPtr CaptureFromPreviewB(byte[] Imagepath, int type);

        [DllImport("DevCaptureB.dll", CallingConvention = CallingConvention.Cdecl)]
        public extern static void SetCutTypeB(int cutType);

        [DllImport("DevCaptureB.dll", CallingConvention = CallingConvention.Cdecl)]
        public extern static void SetFormatTypeB(int format);

        [DllImport("DevCaptureB.dll", CallingConvention = CallingConvention.Cdecl)]
        public extern static void SetColorTypeB(int type);

        [DllImport("DevCaptureB.dll", CallingConvention = CallingConvention.Cdecl)]
        public extern static int GetResolutionCountB(int index);

        [DllImport("DevCaptureB.dll", CallingConvention = CallingConvention.Cdecl)]
        public extern static int GetResolutionB(int R_index, ref int width, ref int height);

        [DllImport("DevCaptureB.dll", CallingConvention = CallingConvention.Cdecl)]
        public extern static void SetJpgQualityB(int quality);

        [DllImport("DevCaptureB.dll", CallingConvention = CallingConvention.Cdecl)]
        public extern static void SetManualCutRectB(double left, double top, double right, double bottom);

        [DllImport("DevCapture.dll", EntryPoint = "SetJpgQuality", CallingConvention = CallingConvention.Cdecl)]
        public extern static void SetJpgQuality(int quality);

        #endregion


        PrintDocument printDocument; //打印
        System.Windows.Forms.PrintDialog printDialog = new System.Windows.Forms.PrintDialog();
        PageSetupDialog pageSetupDialog = new PageSetupDialog();
        PrintPreviewDialog printPreviewDialog = new PrintPreviewDialog();
       // PdfDocument pdfDocument = new PdfDocument();
        
        private delegate void UpdateUiDelegate(byte[] buf);
        private delegate void UpdateUiDelegate2(IntPtr buf, int w, int h);
        private delegate void AddImgToListDelegate(string path, int fType, int isAddList);
        private delegate void VideoParamsChangeDelegate();
        private delegate void WaitDlgDelegate(int show);
        private delegate void HidSignalDelegate(int signal);

        public delegate void JoinImgTransfDelegate(string path);   //用委托向子窗体传值
        public event JoinImgTransfDelegate JoinImgTransfEvent;

        public delegate void MorePdfTransfDelegate(string path);   //用委托向子窗体传值
        public event MorePdfTransfDelegate MorePdfTransfEvent;

        public delegate void WiseDlgTransfDelegate(string path);   //用委托向子窗体传值
        public event WiseDlgTransfDelegate WiseDlgTransfEvent;

        public delegate void TimeDlgTransfDelegate(string path);   //用委托向子窗体传值
        public event TimeDlgTransfDelegate TimeDlgTransfEvent;

        public delegate void IdCardTransfDelegate(string path);   //用委托向子窗体传值
        public event IdCardTransfDelegate IdCardTransfEvent;

        public delegate void TimePhotoTransfDelegate(string path);   //用委托向子窗体传值
        public event TimePhotoTransfDelegate TimePhotoTransfEvent;  

        private bool isTriggerComboBoxEvent = true;
        private bool isRunWaitDialogThread = false;

        PFCALLBACK CallBackFunc = null;     //摄像头回调函数
        PFCALLBACK CallSlaveBackFunc = null;     //摄像头回调函数
        STILLPHOTOCALLBACK StillImageCallBackFunc = null;  //静态拍照回调函数
        VIDEOPARAMCALLBACK VideoParamsCallBackFunc = null;  //视频参数改变回调函数
        HIDDEVCALLBACK HidSignalCallBackFunc = null;
        //System.Drawing.Bitmap CamBitmap;
        Int32Rect CamRect;
        //Int32Rect TmpCamRect;
        Int32Rect CamSlaveRect;
        WriteableBitmap mWBitmap = null;
        WriteableBitmap mTmpWBitmap = null;
        WriteableBitmap mWSlaveBitmap = null;
        DispatcherTimer StartCameraTimer;
        DispatcherTimer CheckCameraTimer;     
        //byte[] CamBuf = null;
        //byte[] TmpCamBuf = null;
        byte[] CamSlaveBuf = null;
        WaitDialog mWaitDialog = new WaitDialog();

        public Setting mSettingDlg;
        MarkDlg mMarkDlg;
        MorePdfDlg mMorePdfDlg;
        JoinImgDlg mJoinImgDlg;
        IdCardDlg mIdCardDlg;
        RecordDlg mRecordDlg;
        TimerDlg mTimerDlg;
        ReNameDlg mReNameDlg;
        AssistSetDlg mAssistSetDlg;
        WiseDlg mWiseDlg;
        public QRDlg mQRDlg;
        public FixedNameDlg mFixedNameDlg;
       
        private const int VideoBright = 0;
        private const int VideoContrast = 1;
        private const int VideoHun = 2;
        private const int VideoSaturation = 3;
        private const int VideoSharpness = 4;
        private const int VideoGamma = 5;
        private const int VideoBalance = 7;  //白平衡
        private const int VideoBacklightCs = 8;
        private const int VideoGain = 9;

        private const int VideoExposure = 4;

        bool pIsRotate = false;
        int FrameCount = 0;
        int RotateFlag = 0 ;
        int TmpCameraCount = 0;
        bool pStillImgShowToList = true;  //静态拍照的图像是否显示到列表
        bool isGetSlaveCamImage = false;

        bool isFullScreen = false;

        bool isChangeDevice = false;

        public MainWindow()
        {
            InitializeComponent();
        }

        //按键信号回调函数
        int GetHidSignalCallBackPro(int signal)
        {
            this.Dispatcher.BeginInvoke(new HidSignalDelegate(HidSignalDelegateFunction), signal);
            return 0;
        }
        private void HidSignalDelegateFunction(int signal)
        {
            if (signal == 0x51)
            {
                BestSizeBt_Click(null, null);
            }
            if (signal == 0x52)
            {
                ZoomOutBt_Click(null, null);
            }
            if (signal == 0x53)
            {
                ZoomInBt_Click(null, null);
            }
            if (signal == 0x54)
            {
                if (global.Is16MDevice)
                {
                    if (global.pTimerDlgHaveRun)  //如果定时器窗体已经运行
                        return ;
                    FuncCaptureFromStill(global.FileFormat, global.NameMode, true);
                }
                else
                {
                    FuncCaptureFromPreview(global.FileFormat, global.NameMode, true);
                }
            }
            if (signal == 0x55)
            {
                TrueSizeBt_Click(null, null);
            }
            if (signal == 0x56)
            {
                RotateLBt_Click(null, null);
            }
            if (signal == 0x57)
            {
                RotateRBt_Click(null, null);
            }
        }

        private void InitBtnSize()
        {
            if (global.pLangusge == 3)
            {
                WiseCaptureBt.Width = 100;
                TimeCaptureBt.Width = 100;

                BarCodeBt.Width = 96;
                MorePdfBt.Width = 76;
                JoinImgBt.Width = 96;
                IdCardBt.Width = 86;
            }
            else
            {
                WiseCaptureBt.Width = 76;
                TimeCaptureBt.Width = 76;

                BarCodeBt.Width = 66;
                MorePdfBt.Width = 66;
                JoinImgBt.Width = 66;
                IdCardBt.Width = 66;
            }
        }

        //############界面初始化#################
        private void InitUIControl()
        {
            try
            {
                if (!Directory.Exists(global.ImagesFolder))
                    Directory.CreateDirectory(global.ImagesFolder);
            }
            catch (Exception e)
            {
                global.ImagesFolder = System.Windows.Forms.Application.StartupPath + "\\GpyImages";
                Directory.CreateDirectory(System.Windows.Forms.Application.StartupPath + "\\GpyImages");
            }
           
            ImgFolderTexBox.Text = global.ImagesFolder;
           

            FileFormatCbBox.SelectedIndex = global.FileFormat;


            if (global.CutType == 0) 
            {
                RdBtNoCut.IsChecked = true;
                MyMark.Visibility = Visibility.Hidden;
            }
            else if (global.CutType == 1) 
            {
                RdBtAutoCut.IsChecked = true;
                MyMark.Visibility = Visibility.Hidden;
            }
            else if (global.CutType == 2)
            {
                RdBtManulCut.IsChecked = true;
                MyMark.Visibility = Visibility.Visible;
            } 

            if (global.ColorType == 0) RdBtColor.IsChecked = true;
            else if (global.ColorType == 1) RdBtGray.IsChecked = true;
            else if (global.ColorType == 2) RdBtBlackWhite.IsChecked = true;

            if (global.isDelBlackEdge == 0) CkBox_DelBlackEdge.IsChecked = false;
            else CkBox_DelBlackEdge.IsChecked = true;

            if (global.isDelBgColor == 0) CkBox_DelBgColor.IsChecked = false;
            else CkBox_DelBgColor.IsChecked = true;

            if (global.isDelShade == 0) CkBox_DelShade.IsChecked = false;
            else CkBox_DelShade.IsChecked = true;

            if (global.isDelGrayBg == 0) CkBox_DelGray.IsChecked = false;
            else CkBox_DelGray.IsChecked = true;

            global.SetWaterMarkParameters();

            SetJpgQuality(global.JpgQuality);  //设置JPEG图片质量

            InitBtnSize();
        }

        //############获取视频参数#################
        void GetVideoParameter()
        {           
            //if (global.isOpenCameraA)
            //{
                int iRest = -1;
                int min, max, def, current, flag;
                min = 0; max = 0; def = 0; current = 0; flag = 0;
                //亮度
                iRest = GetVideoProcParms(VideoBright, ref min, ref max, ref def, ref current);
                if (iRest == 0 )
                {
                    SliderBright.Minimum = min;
                    SliderBright.Maximum = max;
                    if (global.pVideoParamObj.Bright < min || global.pVideoParamObj.Bright > max)
                        global.pVideoParamObj.Bright = current;
                    SliderBright.Value = global.pVideoParamObj.Bright;
                    LableBright.Content = Convert.ToString(SliderBright.Value);
                    SetVideoProcParms(VideoBright, global.pVideoParamObj.Bright);
                }

                //对比度
                iRest = GetVideoProcParms(VideoContrast, ref min, ref max, ref def, ref current);
                if (iRest == 0)
                {
                    SliderContrast.Minimum = min;
                    SliderContrast.Maximum = max;
                    if (global.pVideoParamObj.Contrast < min || global.pVideoParamObj.Contrast > max)
                        global.pVideoParamObj.Contrast = current;
                    SliderContrast.Value = global.pVideoParamObj.Contrast;
                    LableContrast.Content = Convert.ToString(SliderContrast.Value);
                    SetVideoProcParms(VideoContrast, global.pVideoParamObj.Contrast);
                }

                //曝光度
                iRest = GetCameraCotrolParms(VideoExposure, ref min, ref max, ref def, ref current,ref flag);
                if (iRest == 0)
                {
                    SliderExp.Minimum = min;
                    SliderExp.Maximum = max;
                    if (global.pVideoParamObj.Exposure < min || global.pVideoParamObj.Exposure > max)
                        global.pVideoParamObj.Exposure = current;
                    SliderExp.Value = global.pVideoParamObj.Exposure;
                    LableExp.Content = Convert.ToString(SliderExp.Value);
                    if (global.pVideoParamObj.isAutoExp==0)
                        CheckBoxExp.IsChecked=false;
                    if (global.pVideoParamObj.isAutoExp ==1)
                        CheckBoxExp.IsChecked = true;
                    SetCameraCotrolParms(VideoExposure, global.pVideoParamObj.Exposure, global.pVideoParamObj.isAutoExp);
                }

                //色调
                iRest = GetVideoProcParms(VideoHun, ref min, ref max, ref def, ref current);
                if (iRest == 0)
                {
                    if (global.pVideoParamObj.Hun < min || global.pVideoParamObj.Hun > max)
                        global.pVideoParamObj.Hun = current;
                    SetVideoProcParms(VideoHun, global.pVideoParamObj.Hun);
                }
                //饱和度
                iRest = GetVideoProcParms(VideoSaturation, ref min, ref max, ref def, ref current);
                if (iRest == 0)
                {
                    if (global.pVideoParamObj.Saturation < min || global.pVideoParamObj.Saturation > max)
                        global.pVideoParamObj.Saturation = current;
                    SetVideoProcParms(VideoSaturation, global.pVideoParamObj.Saturation);
                }
                //清晰度（锐度）
                iRest = GetVideoProcParms(VideoSharpness, ref min, ref max, ref def, ref current);
                if (iRest == 0)
                {
                    if (global.pVideoParamObj.Sharpness < min || global.pVideoParamObj.Sharpness > max)
                        global.pVideoParamObj.Sharpness = current;
                    SetVideoProcParms(VideoSharpness, global.pVideoParamObj.Sharpness);
                }
                //伽玛
                iRest = GetVideoProcParms(VideoGamma, ref min, ref max, ref def, ref current);
                if (iRest == 0)
                {
                    if (global.pVideoParamObj.Gamma < min || global.pVideoParamObj.Gamma > max)
                        global.pVideoParamObj.Gamma = current;
                    SetVideoProcParms(VideoGamma, global.pVideoParamObj.Gamma);
                }
                //逆光对比
                iRest = GetVideoProcParms(VideoBacklightCs, ref min, ref max, ref def, ref current);
                if (iRest == 0)
                {
                    if (global.pVideoParamObj.BacklightCs < min || global.pVideoParamObj.BacklightCs > max)
                        global.pVideoParamObj.BacklightCs = current;
                    SetVideoProcParms(VideoBacklightCs, global.pVideoParamObj.BacklightCs);
                }
                //增益
                iRest = GetVideoProcParms(VideoGain, ref min, ref max, ref def, ref current);
                if (iRest == 0)
                {
                    if (global.pVideoParamObj.Gain < min || global.pVideoParamObj.Gain > max)
                        global.pVideoParamObj.Gain = current;
                    SetVideoProcParms(VideoGain, global.pVideoParamObj.Gain);
                }


                //白平衡
                iRest = GetVideoProcParms(VideoBalance, ref min, ref max, ref def, ref current);
                if (iRest == 0)
                {
                    if (global.pVideoParamObj.Balance < min || global.pVideoParamObj.Balance > max)
                        global.pVideoParamObj.Balance = current;
                    SetVideoProcParmsEx(VideoBalance, global.pVideoParamObj.Balance, global.pVideoParamObj.isAutoBalance);
                }

            //}
        }

        //>>>>>>>>>>>>>>>>>>>>>>视频参数改变回调函数<<<<<<<<<<<<<<<<<<<<<<<<
        public int VideoParamsChangeCallBackFunc()
        {
            this.Dispatcher.BeginInvoke(new VideoParamsChangeDelegate(VideoParamsChangeEvent), null);
            return 0;
        }
        private void VideoParamsChangeEvent()
        {
            if (global.isOpenCameraA == false)
                return;

            int iRest = -1;
            int min, max, def, current, flag;
            min = 0; max = 0; def = 0; current = 0; flag = 0;
            //亮度
            iRest = GetVideoProcParms(VideoBright, ref min, ref max, ref def, ref current);
            if (iRest == 0)
            {
                global.pVideoParamObj.Bright = current;
                SliderBright.Value = current;
                LableBright.Content = Convert.ToString(current);
            }

            //对比度
            iRest = GetVideoProcParms(VideoContrast, ref min, ref max, ref def, ref current);
            if (iRest == 0)
            {
                global.pVideoParamObj.Contrast = current;
                SliderContrast.Value = current;
                LableContrast.Content = Convert.ToString(current);
            }

            //曝光度
            iRest = GetCameraCotrolParms(VideoExposure, ref min, ref max, ref def, ref current, ref flag);
            if (iRest == 0)
            {
                global.pVideoParamObj.Exposure= current;
                SliderExp.Value = current;
                LableExp.Content = Convert.ToString(SliderExp.Value);
                if (flag == 0 || flag == 2)
                {
                    global.pVideoParamObj.isAutoExp = 0;
                    CheckBoxExp.IsChecked = false;
                }
                if (flag == 1)
                {
                    global.pVideoParamObj.isAutoExp = 1;
                    CheckBoxExp.IsChecked = true;
                }         
            }

            //色调
            iRest = GetVideoProcParms(VideoHun, ref min, ref max, ref def, ref current);
            if (iRest == 0)
            {
                global.pVideoParamObj.Hun = current;
            }
            //饱和度
            iRest = GetVideoProcParms(VideoSaturation, ref min, ref max, ref def, ref current);
            if (iRest == 0)
            {
                global.pVideoParamObj.Saturation = current;
            }
            //清晰度（锐度）
            iRest = GetVideoProcParms(VideoSharpness, ref min, ref max, ref def, ref current);
            if (iRest == 0)
            {
                global.pVideoParamObj.Sharpness = current;
            }
            //伽玛
            iRest = GetVideoProcParms(VideoGamma, ref min, ref max, ref def, ref current);
            if (iRest == 0)
            {
                global.pVideoParamObj.Gamma = current;
            }
            //逆光对比
            iRest = GetVideoProcParms(VideoBacklightCs, ref min, ref max, ref def, ref current);
            if (iRest == 0)
            {
                global.pVideoParamObj.BacklightCs = current;
            }
            //增益
            iRest = GetVideoProcParms(VideoGain, ref min, ref max, ref def, ref current);
            if (iRest == 0)
            {
                global.pVideoParamObj.Gain = current;
            }


            //白平衡
            iRest = GetVideoProcParmsEx(VideoBalance, ref min, ref max, ref def, ref current, ref flag);
            if (iRest == 0)
            {
                global.pVideoParamObj.Balance = current;
                //SetVideoProcParmsEx(VideoBalance, 5000, 3);
                if (flag == 0 || flag == 2)
                {
                    global.pVideoParamObj.isAutoBalance = 3;
                }
                if (flag == 1)
                {
                    global.pVideoParamObj.isAutoBalance = 1;
                }         
            }

        }


        /****************
         * 窗体初始化
         * *************/
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (this.Height > SystemParameters.WorkArea.Size.Height)
            {
                this.Height = SystemParameters.WorkArea.Size.Height;
                this.Top = 0;
            }

            global.pSlaveCamera.PreWidth = 640;  //副头默认30万开图
            global.pSlaveCamera.PreHeight = 480; //副头默认30万开图

            try
            {
                global.WriteMessage("Window_Loaded->001");
                global.ImagesFolder = System.Windows.Forms.Application.StartupPath + "\\GpyImages";
                global.ConfigIniPath = System.Windows.Forms.Application.StartupPath + "\\KHCAMERA.ini";
                global.WriteMessage("Window_Loaded->002");
                global.ReadConfigPramas(); //读取设置参数
                global.WriteMessage("Window_Loaded->003");
                if (!File.Exists(global.ConfigIniPath))
                {
                    global.ImagesFolder = "C:\\GpyImages";
                    global.ConfigIniPath = "C:\\KHCAMERA.ini";
                    global.WriteMessage("Window_Loaded->004");
                    global.ReadConfigPramas(); //读取设置参数
                    global.WriteMessage("Window_Loaded->005");
                }
            }
            catch(Exception)
            {
                if (File.Exists(global.ConfigIniPath))
                {
                    File.Delete(global.ConfigIniPath);
                }
                global.WriteMessage("Window_Loaded->Exception->001");
                global.ReadConfigPramas(); //重新建立参数配置文件
                global.WriteMessage("Window_Loaded->Exception->002");
            }
            global.WriteMessage("Window_Loaded->006");
            ChangeLanguage(global.pLangusge);
            InitUIControl();
            global.WriteMessage("Window_Loaded->007");
            MyMark.DoubleClickMaskDelegate += ManuualCutDoubleClick;  //手动裁切蒙版的双击事件

            printDocument = new PrintDocument();
            printDocument.PrintPage += new PrintPageEventHandler(this.printDocument_PrintPage);
            global.WriteMessage("Window_Loaded->008");

            StartCameraTimer = new DispatcherTimer();
            StartCameraTimer.Tick += new EventHandler(StartCameraTimer_Tick);
            StartCameraTimer.Interval = new TimeSpan(0, 0, 0, 0, 5);
            StartCameraTimer.Start();
            global.WriteMessage("Window_Loaded->009");

          
      
        }

        /****************
         * 窗体关闭
         * *************/
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            global.WriteConfigPramas();  //保存设置参数

            mWaitDialog.Close();  //必须关闭等待对话框

            CloseHidDevice();  //关闭HID设备

            if (CheckCameraTimer!=null)
                CheckCameraTimer.Stop();  //停止定时器

            RotateFlag = 10000;

            if (global.isOpenCameraA == true)
            {
                FrameCount = 0;
                global.isOpenCameraA = false;
                CloseDevice();
            }
            if (global.isOpenCameraB == true)
            {
                global.isOpenCameraB = false;
                CloseDeviceB();
            }
        }

        /****************
         * 窗体大小改变事件
         * *************/
        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {

            if (global.isOpenCameraA)
            {
                FN_SuitableSize(global.RotateCount);
                FN_DisplayCenter(global.RotateCount);
                FN_SetImagePosition();
            }

            if (global.isOpenCameraB)
            {
                if (global.pSlaveImgShowStartX <= 0)
                    global.pSlaveImgShowStartX = 0;
                if (global.pSlaveImgShowStartY <= 0)
                    global.pSlaveImgShowStartY = 0;
                if (global.pSlaveImgShowStartX + global.pSlaveImgShowWidth >= CamPreivewBorder.ActualWidth)
                    global.pSlaveImgShowStartX = CamPreivewBorder.ActualWidth - global.pSlaveImgShowWidth;
                if (global.pSlaveImgShowStartY + global.pSlaveImgShowHeight >= CamPreivewBorder.ActualHeight)
                    global.pSlaveImgShowStartY = CamPreivewBorder.ActualHeight - global.pSlaveImgShowHeight;
                FN_SetSlaveImagePosition();
            }

            MyMark.SetValue(Canvas.LeftProperty, (double)0);
            MyMark.SetValue(Canvas.TopProperty, (double)0);
            MyMark.SetValue(Canvas.WidthProperty, SystemParameters.WorkArea.Width);
            MyMark.SetValue(Canvas.HeightProperty, SystemParameters.WorkArea.Height);

            if (global.isOpenCameraA)
            {
                Rect pRect = MyMark.GetIndicatorRegion();
                MyMark.UpdateMarkAndRegion(pRect);
            }             
        }



        //>>>>>>>>>>>>>>>>>>>>>摄像头回调函数<<<<<<<<<<<<<<<<<<<<<<<<<<<
        public int CameraProFunc(IntPtr buf, int width, int height)
        {
            FrameCount++;
            if (FrameCount > 2) FrameCount = 3;
            //if (CamBuf == null)
            //{
            //    CamBuf = new byte[width * height * 3];
            //}           
            //Marshal.Copy(buf, CamBuf, 0, width * height * 3);
            //this.Dispatcher.BeginInvoke(new UpdateUiDelegate(CameraPaint), CamBuf);

            int w = 0;
            int h = 0;
            if (global.RotateCount == 0 || global.RotateCount == 2)
            {
                w = width; h = height;
            }
            if (global.RotateCount == 1 || global.RotateCount == 3)
            {
                w = height; h = width;
            }
            if ((FrameCount > 0) && (global.pHostCamera.PreWidth == w) && (global.pHostCamera.PreHeight == h))
                this.Dispatcher.BeginInvoke(new UpdateUiDelegate2(CameraPaint2), buf, width, height);

            return 0;
        }

        //##################刷新显示#######################
        private void CameraPaint(byte[] buf)
        {
            //if (RotateFlag != 0)
            //{
            //    GC.Collect();
            //    RotateFlag--;
            //    return;
            //}
            //mWBitmap.WritePixels(CamRect, CamBuf, mWBitmap.BackBufferStride, 0);
            //this.CamVideoPreivew.Source = mWBitmap;
        }

        //##################刷新显示2#######################
        private void CameraPaint2(IntPtr buf, int width, int height)
        {
            //global.WriteMessage("CameraPaint->001");
            if (RotateFlag != 0)
            {
                GC.Collect();
                RotateFlag--;
                return;
            }
            //global.WriteMessage("CameraPaint->002");
            int w = 0;
            int h = 0;
            if (global.RotateCount == 0 || global.RotateCount == 2)
            {
                w = width; h = height;
            }
            if (global.RotateCount == 1 || global.RotateCount == 3)
            {
                w = height; h = width;
            }
            if (global.isOpenCameraA &&  (global.pHostCamera.PreWidth == w) && (global.pHostCamera.PreHeight == h))
                mWBitmap.WritePixels(CamRect, buf, width * height * 3, mWBitmap.BackBufferStride);
            //global.WriteMessage("CameraPaint->003");
        }


        //>>>>>>>>>>>>>>>>>>>>>>静态拍照回调函数<<<<<<<<<<<<<<<<<<<<<<<<
        public int GetStillImageCallBackFunc(IntPtr path, int fType, int isAddList)
        {
            string imgpath = Marshal.PtrToStringAnsi(path);

            if (isAddList == 200)
            {
                byte[] pBuf = Encoding.GetEncoding(global.pEncodType).GetBytes(imgpath);
                StillCaptureSuccess(pBuf, fType);
            }
            else
                this.Dispatcher.BeginInvoke(new AddImgToListDelegate(AddStillImageToListBox), imgpath, fType, isAddList);

            return 0;
        }

        private void AddStillImageToListBox(string imgpath,int fType, int isAddList)
        {
            isRunWaitDialogThread = false; //隐藏等待对话框

            if (pStillImgShowToList)
            {
                int pos = imgpath.LastIndexOf("\\");
                string name = imgpath.Substring(pos + 1, imgpath.Length - pos - 1);
                PreviewPhoto mPhoto = new PreviewPhoto();
                string prePath = imgpath;
                if (fType == 4)
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
                // 1600万插值到1800或2000，显示buffer.jpg的预览图
                if(isAddList == 300 && fType != 4)
                    prePath = System.Windows.Forms.Application.StartupPath + "\\buffer.jpg";

                mPhoto.SourceImage = CreateImageSourceThumbnia(prePath, 120, 90);
                if (File.Exists(prePath) && fType == 4)
                    File.Delete(prePath);

                if (fType == 4)
                    mPhoto.LogoImage = new BitmapImage(new Uri(@"Images\pdfb.png", UriKind.Relative));

                mPhoto.ImageName = name;
                mPhoto.ImagePath = imgpath;
                PreviewImgList.Items.Add(mPhoto);
                PreviewImgList.ScrollIntoView(PreviewImgList.Items[PreviewImgList.Items.Count - 1]); //设置总显示最后一项
                //global.pImagePathList.Add(imgpath);
            }


            //如果是全屏模式
            if (isFullScreen==true)
            {
                string prePath = "";
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
                else
                {
                    if (PreviewImgList.Items.Count > 0)
                    {
                        PreviewPhoto mlistItem = (PreviewPhoto)PreviewImgList.Items[PreviewImgList.Items.Count - 1];
                        prePath = mlistItem.ImagePath;
                    }
                }
                if (File.Exists(prePath))
                {
                    PhotoPreivew.Source = CreateImageSourceThumbnia(prePath, 120, 90);
                    if (global.FileFormat == 4)
                        File.Delete(prePath);
                }
                    

            }



            //副头拍照
            if (global.isTakeSlaveCamImg == 1 && global.isJoinMainCam == 0 && isGetSlaveCamImage == true)
                FuncSlaveCaptureFromPreview(global.FileFormat, global.NameMode, true);

            global.IsTimerScanCanDo = true;  //静态拍照完成后，定时拍照才能触发
            global.IsWiseScanCanDo = true;   //静态拍照完成后，智能连拍才能触发
            if (global.pWiseShootDlgHaveRun)
            {
                WiseDlgTransfEvent(imgpath);
            }
            if (global.pTimerDlgHaveRun)
            {
                TimeDlgTransfEvent(imgpath);
            }

            if (global.pMorePdfFormScanDo)
            {
                global.pMorePdfFormScanDo = false;
                MorePdfTransfEvent(imgpath); //委托向子窗体传值              
            }

            if (global.pJoinImgFormScanDo)
            {
                global.pJoinImgFormScanDo = false;
                JoinImgTransfEvent(imgpath); //委托向子窗体传值              
            }

            if (global.pIdCardFormScanDo)
            {
                global.pIdCardFormScanDo = false;
                IdCardTransfEvent(imgpath); //委托向子窗体传值              
            }


            SetFormatType(global.FileFormat);  //还原参数设置
            SetDelBlackEdge(global.isDelBlackEdge);
            SetDelBgColor(global.isDelBgColor);
        }


        //############适合大小函数#################
        void FN_SuitableSize(int dir)
        {
            double ImgWidth = global.pHostCamera.PreWidth;
            double ImgHeight = global.pHostCamera.PreHeight;
            if (dir == 0 || dir == 2)
            {
               ImgWidth = global.pHostCamera.PreWidth;
               ImgHeight = global.pHostCamera.PreHeight;
            }
            if (dir == 1 || dir == 3)
            {
                ImgHeight = global.pHostCamera.PreWidth;
                ImgWidth = global.pHostCamera.PreHeight;
            }
            double ImageCtlWidth = CamPreivewBorder.ActualWidth - 2;  //边框占2个像素
            double ImageCtlHeight = CamPreivewBorder.ActualHeight - 2;  //边框占2个像素
            double multip1, multip2;
            if ((ImgWidth < ImageCtlWidth) && (ImgHeight < ImageCtlHeight))
            {
                global.pImgShowScale = 100.0;
            }
            else
            {
                multip1 = (double)ImgWidth / ImgHeight;
                multip2 = ImageCtlWidth / ImageCtlHeight;
                if (multip1 < multip2)
                {
                    global.pImgShowScale = ImageCtlHeight * 100 / ImgHeight;
                }
                else
                {
                    global.pImgShowScale = ImageCtlWidth * 100 / ImgWidth;
                }
            }

            global.pImgShowFitScale = global.pImgShowScale;
        }

        //############居中显示#################
        void FN_DisplayCenter(int dir)
        {
            double ImgWidth = global.pHostCamera.PreWidth;
            double ImgHeight = global.pHostCamera.PreHeight;
            if (dir == 0 || dir == 2)
            {
                ImgWidth = global.pHostCamera.PreWidth;
                ImgHeight = global.pHostCamera.PreHeight;
            }
            if (dir == 1 || dir == 3)
            {
                ImgHeight = global.pHostCamera.PreWidth;
                ImgWidth = global.pHostCamera.PreHeight;
            }
            double center_x, center_y;
            double ImageCtlWidth = CamPreivewBorder.ActualWidth - 2;  //边框占2个像素
            double ImageCtlHeight = CamPreivewBorder.ActualHeight - 2;  //边框占2个像素
            center_x = ImageCtlWidth / 2;
            center_y = ImageCtlHeight / 2;
            global.pImgShowWidth = ImgWidth * global.pImgShowScale / 100;
            global.pImgShowHeight = ImgHeight * global.pImgShowScale / 100;
            global.pImgShowStartX = (ImageCtlWidth - global.pImgShowWidth) / 2;
            global.pImgShowStartY = (ImageCtlHeight - global.pImgShowHeight) / 2;
        }

        //############Image位置显示#################
        void FN_SetImagePosition()
        {
            if (global.isOpenCameraA)
            {
                CamVideoPreivew.SetValue(Canvas.LeftProperty, global.pImgShowStartX);
                CamVideoPreivew.SetValue(Canvas.TopProperty, global.pImgShowStartY);  
                CamVideoPreivew.SetValue(Canvas.WidthProperty, global.pImgShowWidth);
                CamVideoPreivew.SetValue(Canvas.HeightProperty, global.pImgShowHeight);
            }
        }

        //##################添加图片到预览列表#######################
        public void AddPreviewImageToList(string imgpath, int ww, int hh)
        {
            int pos = imgpath.LastIndexOf("\\");
            string name = imgpath.Substring(pos + 1, imgpath.Length - pos - 1);
            PreviewPhoto mPhoto = new PreviewPhoto();
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
            mPhoto.SourceImage = CreateImageSourceThumbnia(prePath, ww, hh);
            if (File.Exists(prePath) && global.FileFormat == 4)
                File.Delete(prePath);
            
            if(global.FileFormat==4)
                mPhoto.LogoImage = new BitmapImage(new Uri(@"Images\pdfb.png", UriKind.Relative));
            mPhoto.ImageName = name;
            mPhoto.ImagePath = imgpath;
            PreviewImgList.Items.Add(mPhoto);
            PreviewImgList.ScrollIntoView(PreviewImgList.Items[PreviewImgList.Items.Count - 1]); //设置总显示最后一项
            //global.pImagePathList.Add(imgpath);
        }


        //##################创建缩略图函数#######################
        public  ImageSource CreateImageSourceThumbnia(string fileName, double width, double height)
        {
            BitmapSource bitmapSource = null;
            if (fileName.Substring(fileName.Length - 3, 3) == "pdf")
            {
                bitmapSource = new BitmapImage(new Uri(@"Images\PdfLogo.png", UriKind.Relative));
            }
            else 
            {
                if (!File.Exists(fileName))
                    return null;
                System.Drawing.Image sourceImage = System.Drawing.Image.FromFile(fileName);
                double rw = width / sourceImage.Width; 
                double rh = height / sourceImage.Height;
                var aspect = (float)Math.Min(rw, rh);
                int w = sourceImage.Width, h = sourceImage.Height;
                if (aspect < 1)
                {
                    w = (int)Math.Round(sourceImage.Width * aspect); h = (int)Math.Round(sourceImage.Height * aspect);
                }
                Bitmap sourceBmp = new Bitmap(sourceImage, w, h);
                IntPtr hBitmap = sourceBmp.GetHbitmap();
                bitmapSource = Imaging.CreateBitmapSourceFromHBitmap(hBitmap, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());

                bitmapSource.Freeze();
                DeleteObject(hBitmap);
                sourceImage.Dispose();
                sourceBmp.Dispose();
            }
            return bitmapSource;
        }

        //############打开摄像头函数#################
        void toOpenCamera(int index)
        {
            isTriggerComboBoxEvent = false;
            global.WriteMessage("toOpenCamera->001");
            global.pLHostPreWidths.Clear();
            global.pLHostPreHeights.Clear();
            global.pLHostStillWidths.Clear();
            global.pLHostStillHeights.Clear();
            PreResCbBox.Items.Clear();
            StillResCbBox.Items.Clear();

            if (global.isOpenCameraA == true)
            {
                CloseDevice();
                global.isOpenCameraA = false;                
            }
            global.WriteMessage("toOpenCamera->002");
            int iRest = -1;
            //获取设备分辨率
            if (global.pCameraNamesList.Count > 0)
            {               
                global.CameraIndex = index; 
               
                int xWidth = 0;
                int xHeight = 0;
                int tmpval = 0;
                int pos = 0;
                int previewPos = 0;

                //***************************获取预览分辨率**********************************
                int resCount = GetResolutionCount(global.CameraIndex);
                if (resCount > 0)
                {
                    for (int i = 0; i < resCount; i++)
                    {
                        iRest = GetResolution(i, ref xWidth, ref xHeight);//获取预览分辨率
                        if (iRest == 0)
                        {
                            global.pLHostPreWidths.Add(xWidth);
                            global.pLHostPreHeights.Add(xHeight);
                            string resStr = Convert.ToString(xWidth) + "*" + Convert.ToString(xHeight);
                            PreResCbBox.Items.Add(resStr);
                        }
                    }
                    for (int i = 0; i < global.pLHostPreWidths.Count; i++)
                    {
                        if (global.pLHostPreWidths[i] > tmpval)
                        {
                            tmpval = global.pLHostPreWidths[i];
                            previewPos = i;
                        }
                    }                   
                    global.pHostCamera.PreWidth = global.pLHostPreWidths[pos];
                    global.pHostCamera.PreHeight = global.pLHostPreHeights[pos];

                }
                global.WriteMessage("toOpenCamera->003");
                //*******************************获取预览分辨率*******************************

                //****************************获取静态PIN分辨率********************************

                resCount = GetStillResolutionCount(global.CameraIndex); //获取静态PIN分辨率数目
                if (resCount <= 0) //如果不支持静态拍照
                {
                    CaptureBt.Visibility = Visibility.Visible;
                    PreResGroupBox.Visibility = Visibility.Visible;
                    StillResGroupBox.Visibility = Visibility.Collapsed;  //隐藏不占用空间
                    CaptureStillBt.Visibility = Visibility.Collapsed;    //隐藏不占用空间
                   
                    string btStr = "拍摄";
                    if (global.pLangusge == 1) btStr = "拍攝";
                    if (global.pLangusge == 2) btStr = "Capture";
                    if (global.pLangusge == 3) btStr = "Capturar";
                    if (global.pLangusge == 4) btStr = "撮影";
                    CaptureBt.Content = btStr;

#if VERIFY  //验证是否本公司高拍仪设备

                        if (global.CameraIndex == 0)
                        {
                            bool isMyGpyA = false;
                            bool isMyGpyB = false;
                            byte[] snbuf = new byte[256];
                            iRest = GetSN(snbuf);
                            if (iRest == 0)
                            {
                                int len = 0;
                                for (int j = 0; j < 256; j++)
                                {
                                    if (snbuf[j] == 0)
                                    {
                                        len = j;
                                        break;
                                    }
                                }
                                string snStr = Encoding.GetEncoding(global.pEncodType).GetString(snbuf, 0, len);
                                if (snStr.IndexOf("WHSN0001") >= 0)
                                {
                                    isMyGpyA = true;
                                }
                            }

                            byte[] dbuf = new byte[16];
                            iRest = GetDeviceLisences(dbuf, 16);
                            if (iRest == 0)
                            {

                                string dlStr = Encoding.GetEncoding(global.pEncodType).GetString(dbuf);
                                if (dlStr.IndexOf("WHABCDE012345678") >= 0)
                                {
                                    isMyGpyB = true;
                                }
                            }

                            if (isMyGpyA == false && isMyGpyB == false)
                            {
                                global.pCameraNamesList.Clear();
                                DevNameCbBox.Items.Clear();
                                PreResCbBox.Items.Clear();
                                string TipStr = "非授权设备！";
                                if (global.pLangusge == 1) TipStr = "非授權設備！";
                                if (global.pLangusge == 2) TipStr = "Unauthorized device!";
                                if (global.pLangusge == 3) TipStr = "Equipo no autorizado!";
                                System.Windows.MessageBox.Show(TipStr);
                                return;
                            }
                        }
                           
#endif
                }
                else
                {
                    for (int i = 0; i < resCount; i++)
                    {
                        iRest = GetStillResolution(i, ref xWidth, ref xHeight);//获取预览分辨率
                        if (iRest == 0)
                        {
                            global.pLHostStillWidths.Add(xWidth);
                            global.pLHostStillHeights.Add(xHeight);
                        }
                    }
                    for (int i = 0; i < global.pLHostStillWidths.Count; i++)
                    {
                        if (global.pLHostStillWidths[i] > tmpval)
                        {
                            tmpval = global.pLHostStillWidths[i];
                            pos = i;
                        }
                    }

                    //如果静态PIN的最大分辨率为1600万以上，则捕获跟拍摄同时使用，否则只有捕获模式
                    if (global.pLHostStillWidths[pos] * global.pLHostStillHeights[pos] >= 4608 * 3456)
                    {
                        global.Is16MDevice = true;
                        CaptureBt.Visibility = Visibility.Collapsed;
                        PreResGroupBox.Visibility = Visibility.Collapsed;
                        CaptureStillBt.Visibility = Visibility.Visible;
                        StillResGroupBox.Visibility = Visibility.Visible; 
                        for (int i = 0; i < global.pLHostStillWidths.Count; i++)
                        {
                            string resStr = Convert.ToString(global.pLHostStillWidths[i]) + "*" + Convert.ToString(global.pLHostStillHeights[i]);
                            StillResCbBox.Items.Add(resStr);
                        }

                        //以记忆的分辨率打开
                        for (int i = 0; i < StillResCbBox.Items.Count; i++)
                        {
                            if (global.OpenStillResolutionStr == StillResCbBox.Items[i].ToString())
                            {
                                pos = i;
                                break;
                            }
                        }                 
                        StillResCbBox.SelectedIndex = pos;
                        global.pHostCamera.StillWidth = global.pLHostStillWidths[pos];
                        global.pHostCamera.StillHeight = global.pLHostStillHeights[pos];
                        if (global.pHostCamera.StillWidth == 4896 && global.pHostCamera.StillHeight == 3672)
                        {
                            global.pHostCamera.StillWidth = 4608;
                            global.pHostCamera.StillHeight = 3456;
                        }
                        SetResolution(StillResCbBox.SelectedIndex, PreResCbBox.SelectedIndex);  //此处设置静态图片分辨率
                        string btStr = "捕获";
                        if (global.pLangusge == 1) btStr = "捕獲";
                        if (global.pLangusge == 2) btStr = "Snap";
                        if (global.pLangusge == 3) btStr = "Capturar";
                        if (global.pLangusge == 4) btStr = "撮影";
                        CaptureBt.Content = btStr;
                    }
                    else
                    {
                        global.Is16MDevice = false;
                        CaptureBt.Visibility = Visibility.Visible;
                        PreResGroupBox.Visibility = Visibility.Visible;
                        StillResGroupBox.Visibility = Visibility.Collapsed;  //隐藏不占用空间
                        CaptureStillBt.Visibility = Visibility.Collapsed;    //隐藏不占用空间
                        string btStr = "拍摄";
                        if (global.pLangusge == 1) btStr = "拍攝";
                        if (global.pLangusge == 2) btStr = "Capture";
                        if (global.pLangusge == 3) btStr = "Capturar";
                        if (global.pLangusge == 4) btStr = "撮影";
                        CaptureBt.Content = btStr;

#if VERIFY  //验证是否本公司高拍仪设备

                            if (global.CameraIndex == 0)
                            {
                                bool isMyGpyA = false;
                                bool isMyGpyB = false;
                                byte[] snbuf = new byte[256];
                                iRest = GetSN(snbuf);
                                if (iRest == 0)
                                {
                                    int len = 0;
                                    for (int j = 0; j < 256; j++)
                                    {
                                        if (snbuf[j] == 0)
                                        {
                                            len = j;
                                            break;
                                        }
                                    }
                                    string snStr = Encoding.GetEncoding(global.pEncodType).GetString(snbuf, 0, len);
                                    if (snStr.IndexOf("WHSN0001") >= 0)
                                    {
                                        isMyGpyA = true;
                                    }
                                }

                                byte[] dbuf = new byte[16];
                                iRest = GetDeviceLisences(dbuf, 16);
                                if (iRest == 0)
                                {

                                    string dlStr = Encoding.GetEncoding(global.pEncodType).GetString(dbuf);
                                    if (dlStr.IndexOf("WHABCDE012345678") >= 0)
                                    {
                                        isMyGpyB = true;
                                    }
                                }

                                if (isMyGpyA == false && isMyGpyB == false)
                                {
                                    global.pCameraNamesList.Clear();
                                    DevNameCbBox.Items.Clear();
                                    PreResCbBox.Items.Clear();
                                    string TipStr = "非授权设备！";
                                    if (global.pLangusge == 1) TipStr = "非授權設備！";
                                    if (global.pLangusge == 2) TipStr = "Unauthorized device!";
                                    if (global.pLangusge == 3) TipStr = "Equipo no autorizado!";
                                    System.Windows.MessageBox.Show(TipStr);
                                    return;
                                }
                            }

#endif
                    }
                }
                //*******************************获取静态PIN分辨率********************************

                //以记忆的分辨率打开
                bool isFindRes = false;
                for (int i = 0; i < PreResCbBox.Items.Count; i++)
                {
                    if (global.OpenResolutionStr == PreResCbBox.Items[i].ToString())
                    {
                        isFindRes = true;
                        previewPos = i;
                        break;
                    }
                }

                if (isFindRes == false)
                {
                    tmpval = 0;
                    for (int i = 0; i < global.pLHostPreWidths.Count; i++)
                    {
                        if (global.pLHostPreWidths[i] > tmpval)
                        {
                            tmpval = global.pLHostPreWidths[i];
                            previewPos = i;
                        }
                    }
                    global.pHostCamera.PreWidth = global.pLHostPreWidths[previewPos];
                    global.pHostCamera.PreHeight = global.pLHostPreHeights[previewPos];
                }

                PreResCbBox.SelectedIndex = previewPos;  //以最大分辨率预览，此处触发ComboBox SelectionChanged事件来打开摄像头
                isTriggerComboBoxEvent = true;
            }

        }


        /****************
         * 定时器检测摄像头连接
         * *************/
        private void CheckCameraTimer_Tick(object sender, EventArgs e)
        {
            int devCount = GetDeviceCount();


            if (devCount == 0)
            {
                if (global.isOpenCameraB)
                {
                    global.isOpenCameraB = false;
                    CloseDeviceB();
                    this.CamVideoPreivew2.Source = null; //切断与显示控件的连接  
                    GC.Collect();
                }

                if (global.isOpenCameraA)
                {
                    global.isOpenCameraA = false;
                    CloseDevice();
                    this.CamVideoPreivew.Source = null; //切断与显示控件的连接
                    GC.Collect();
                }
            }
           

            if (devCount != TmpCameraCount)
            {              
                TmpCameraCount = devCount;
                if (global.isOpenCameraB)  
                {
                    global.isOpenCameraB = false;
                    CloseDeviceB();
                    this.CamVideoPreivew2.Source = null; //切断与显示控件的连接    
                    GC.Collect();
                }

                if (global.isOpenCameraA)
                {
                    global.isOpenCameraA = false;
                    CloseDevice();
                    this.CamVideoPreivew.Source = null; //切断与显示控件的连接
                    GC.Collect();
                }

                if (devCount > 0)
                {

                    CloseHidDevice();
                    if (HidSignalCallBackFunc == null)
                    {
                        HidSignalCallBackFunc = new HIDDEVCALLBACK(GetHidSignalCallBackPro);
                    }
                    int hidRest = OpenHidDevice(HidSignalCallBackFunc);
                    if (hidRest != 0)
                        CloseHidDevice();

                    global.pCameraNamesList.Clear();
                    DevNameCbBox.Items.Clear();
                    int iRest = -1;
                    //*************************获取设备名称***************************

                    byte[] namebuf = new byte[256];
                    for (int i = 0; i < devCount; i++)
                    {
                        iRest = GetDeviceName(i, namebuf);
                        if (iRest != 0)
                        {
                            //string TipStr = "获取设备信息异常！请检测设备是否正常！";
                            //if (global.pLangusge == 1) TipStr = "獲取設備信息異常！請檢測設備是否正常！";
                            //if (global.pLangusge == 2) TipStr = "Get device info error! Please check whether the device is normal!";
                            //if (global.pLangusge == 3) TipStr = "Para obtener información sobre el dispositivo, compruebe si el dispositivo funciona!";
                            //System.Windows.MessageBox.Show(TipStr);
                            //return;
                            break;
                        }

                        int len = 0;
                        for (int j = 0; j < 256; j++)
                        {
                            if (namebuf[j] == 0)
                            {
                                len = j;
                                break;
                            }
                        }
                        string devName = Encoding.GetEncoding(global.pEncodType).GetString(namebuf, 0, len);
                        global.pCameraNamesList.Add(devName);
                    }

                    if (global.pCameraNamesList.Count <= 0)
                        return;

                    for (int i = 0; i < global.pCameraNamesList.Count; i++)
                    {
                        DevNameCbBox.Items.Add(global.pCameraNamesList[i]);
                    }

                    //*************************获取设备名称***************************

                    int index = 0;  //程序第一次运行默认打开主头
                    DevNameCbBox.SelectedIndex = index;
                    //toOpenCamera(index); //开启设备
                }
                else 
                {
                    if (global.isOpenCameraB)
                    {
                        global.isOpenCameraB = false;
                        CloseDeviceB();
                        this.CamVideoPreivew2.Source = null; //切断与显示控件的连接  
                        GC.Collect();
                    }

                    if (global.isOpenCameraA)
                    {
                        global.isOpenCameraA = false;
                        CloseDevice();
                        this.CamVideoPreivew.Source = null; //切断与显示控件的连接
                        GC.Collect();
                    }

                    global.pCameraNamesList.Clear();
                    DevNameCbBox.Items.Clear();
                    PreResCbBox.Items.Clear();
                    StillResCbBox.Items.Clear();
                }
            }
        }


        /****************
         * 程序第一次运行开启设备
         * *************/
        private void StartCameraTimer_Tick(object sender, EventArgs e)
        {
            StartCameraTimer.Stop();
            global.WriteMessage("StartCameraTimer_Tick->001");
            CloseHidDevice();
            global.WriteMessage("StartCameraTimer_Tick->002");
            if (HidSignalCallBackFunc == null)
            {
                HidSignalCallBackFunc = new HIDDEVCALLBACK(GetHidSignalCallBackPro);
            }
            global.WriteMessage("StartCameraTimer_Tick->003");
            int hidRest = OpenHidDevice(HidSignalCallBackFunc);
            if (hidRest != 0)
                CloseHidDevice();
            global.WriteMessage("StartCameraTimer_Tick->004");
            global.pCameraNamesList.Clear();
            DevNameCbBox.Items.Clear();
            int iRest = -1;
            int devCount = GetDeviceCount();
            TmpCameraCount = devCount;
            global.WriteMessage("StartCameraTimer_Tick->005");
            //*************************获取设备名称***************************
            if (devCount > 0)    
            {
                global.WriteMessage("StartCameraTimer_Tick->006");
                byte[] namebuf=new byte[256];
                for (int i = 0; i < devCount; i++)
                {
                    iRest = GetDeviceName(i, namebuf);
                    if (iRest != 0)
                    {
                        //System.Windows.MessageBox.Show("获取设备信息异常！请检测设备是否正常！");
                        //return;
                        continue;
                    }

                    int len = 0;
                    for (int j = 0; j < 256; j++)
                    {
                        if (namebuf[j] == 0)
                        {
                            len = j;
                            break;
                        }
                    }
                    string devName = Encoding.GetEncoding(global.pEncodType).GetString(namebuf,0,len);
                    global.pCameraNamesList.Add(devName);
                }
                global.WriteMessage("StartCameraTimer_Tick->007");
                for (int i = 0; i < global.pCameraNamesList.Count; i++)
                {
                    DevNameCbBox.Items.Add(global.pCameraNamesList[i]);
                }
                
            }
            //*************************获取设备名称***************************

            global.WriteMessage("StartCameraTimer_Tick->008");
            int index = 0;  //程序第一次运行默认打开主头
            DevNameCbBox.SelectedIndex = index;
            //toOpenCamera(index); //开启设备
            global.WriteMessage("StartCameraTimer_Tick->009");

            CheckCameraTimer = new DispatcherTimer();
            CheckCameraTimer.Tick += new EventHandler(CheckCameraTimer_Tick);
            CheckCameraTimer.Interval = new TimeSpan(0, 0, 0, 0, 1000);
            CheckCameraTimer.Start();
            global.WriteMessage("StartCameraTimer_Tick->0010");
        }


        /*****************
         * 切换摄像头
         ****************/
        private void DevNameCbBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DevNameCbBox.SelectedIndex == 1)
            {
                if (global.isOpenCameraB)
                {
                    DevNameCbBox.SelectedIndex = 0;
                    return;
                }
            }
            
            //if (CamBuf != null)
            isChangeDevice = true;
            toOpenCamera(DevNameCbBox.SelectedIndex);
           
        }

        /*****************
         * 切换预览分辨率
         ****************/
        private void PreResCbBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if (PreResCbBox.Items.Count <= 0)
                return;
            global.WriteMessage("PreResCbBox_SelectionChanged->001");
            global.isOpenCameraA = false;
            CloseDevice();
            FrameCount = 0;
            this.CamVideoPreivew.Source = null; //切断与显示控件的连接
            GC.Collect();
            global.WriteMessage("PreResCbBox_SelectionChanged->002");

            string resStr = PreResCbBox.SelectedItem.ToString();
            global.OpenResolutionStr = resStr;  //保存选择的分辨率
            int pos = resStr.LastIndexOf('*');
            int openWidth = Convert.ToInt32(resStr.Substring(0, pos));
            int openHeight = Convert.ToInt32(resStr.Substring(pos + 1, resStr.Length - pos - 1));
            global.pHostCamera.PreWidth = openWidth;
            global.pHostCamera.PreHeight = openHeight;
            if (openWidth == 3672 && openHeight == 2856)
            {
                global.pHostCamera.PreWidth = 3264;
                global.pHostCamera.PreHeight = 2448;
            }

            if (openWidth == 4000 && openHeight == 3000)
            {
                global.pHostCamera.PreWidth = 3264;
                global.pHostCamera.PreHeight = 2448;
            }

            if (openWidth == 3264 && openHeight == 2448 && 0 == Is500InSert800ValDevice())
            {
                global.pHostCamera.PreWidth = 2592;
                global.pHostCamera.PreHeight = 1944;
            }
            if (openWidth == 4208 && openHeight == 3120 && 0 != Is1300InSert1500ValDevice() && 0 != Is1300InSert1600ValDevice())
            {
                global.pHostCamera.PreWidth = 3264;
                global.pHostCamera.PreHeight = 2448;
            }

            if (openWidth == 4608 && openHeight == 3456)
            {
                global.pHostCamera.PreWidth = 4208;
                global.pHostCamera.PreHeight = 3120;
            }

            if (openWidth == 4416 && openHeight == 3312)
            {

                global.pHostCamera.PreWidth = 4208;
                global.pHostCamera.PreHeight = 3120;

                //判断是不是1100万机器，如果有3840*2880的分辨率，则是1100万的机器
                for (int i = 0; i < global.pLHostPreWidths.Count; i++)
                {
                    if (global.pLHostPreWidths[i] == 3840 && global.pLHostPreHeights[i] == 2880)
                    {
                        global.pHostCamera.PreWidth = 3840;
                        global.pHostCamera.PreHeight = 2880;
                        break;
                    }
                }                 
            }

            if (StillResCbBox.Items.Count > 0)
            {
                resStr = StillResCbBox.SelectedItem.ToString();
                pos = resStr.LastIndexOf('*');
                global.pHostCamera.StillWidth = Convert.ToInt32(resStr.Substring(0, pos));
                global.pHostCamera.StillHeight = Convert.ToInt32(resStr.Substring(pos + 1, resStr.Length - pos - 1));
                if (global.pHostCamera.StillWidth == 4896 && global.pHostCamera.StillHeight == 3672)
                {
                    global.pHostCamera.StillWidth = 4608;
                    global.pHostCamera.StillHeight = 3456;
                }
            }
            global.WriteMessage("PreResCbBox_SelectionChanged->003");
            SetResolution(StillResCbBox.SelectedIndex, PreResCbBox.SelectedIndex);  //先设置一下分辨率

            //if (CamBuf != null)
            //{
            //    int size = global.pHostCamera.PreWidth * global.pHostCamera.PreHeight * 3;
            //    CamBuf = (byte[])global.Redim(CamBuf, size);  //重定义数组大小
            //    for (int i = 0; i < size; i++)
            //    {
            //        CamBuf[i] = 0;
            //    }
            //}


            if (CallBackFunc == null)
            {
                CallBackFunc = new PFCALLBACK(CameraProFunc);
                SetCallBackFunction(CallBackFunc);
            }

            if (StillImageCallBackFunc == null)
            {
                StillImageCallBackFunc = new STILLPHOTOCALLBACK(GetStillImageCallBackFunc);
                SetStillPhotoCallBack(StillImageCallBackFunc);
            }

            if (VideoParamsCallBackFunc == null)
            {
                VideoParamsCallBackFunc = new VIDEOPARAMCALLBACK(VideoParamsChangeCallBackFunc);
                SetVideoParamsCallBack(VideoParamsCallBackFunc);
            }
            global.WriteMessage("PreResCbBox_SelectionChanged->004");

            //if (global.RotateCount == 0 || global.RotateCount == 2)
            //{
            //    this.CamVideoPreivew.Source = null;
            //    mWBitmap = new WriteableBitmap(global.pHostCamera.PreWidth, global.pHostCamera.PreHeight, 72, 72, PixelFormats.Bgr24, null);
            //    CamRect = new Int32Rect(0, 0, global.pHostCamera.PreWidth, global.pHostCamera.PreHeight);
            //}
            //if (global.RotateCount == 1 || global.RotateCount == 3)
            //{
            //    this.CamVideoPreivew.Source = null;
            //    mWBitmap = new WriteableBitmap(global.pHostCamera.PreHeight, global.pHostCamera.PreWidth, 72, 72, PixelFormats.Bgr24, null);
            //    CamRect = new Int32Rect(0, 0, global.pHostCamera.PreHeight, global.pHostCamera.PreWidth);
            //}
            //this.CamVideoPreivew.Source = mWBitmap;

            IntPtr hwnd = ((HwndSource)PresentationSource.FromVisual(this.CamVideoPreivew)).Handle;
            global.WriteMessage(Convert.ToString(openWidth) + "," + Convert.ToString(openHeight));
            int iRest = OpenDevice(global.CameraIndex, openWidth, openHeight, hwnd, false);
            global.WriteMessage("PreResCbBox_SelectionChanged->005");
            if (iRest != 0)
            {
                if (iRest == -5)
                {
                    string TipStr = "未发现合适的设备！";
                    if (global.pLangusge == 1) TipStr = "未發現合適的設備！";
                    if (global.pLangusge == 2) TipStr = "No suitable device!";
                    if (global.pLangusge == 3) TipStr = "Ningún dispositivo encontrado!";
                    if (global.pLangusge == 4) TipStr = "デバイスが見つかりません。";
                    System.Windows.MessageBox.Show(TipStr);
                    global.isOpenCameraA = false;
                    return;
                }

                CloseDevice();
                iRest = OpenDevice(global.CameraIndex, openWidth, openHeight, hwnd, false);
                if (iRest != 0)
                {
                    string TipStr = "打开设备失败！";
                    if (global.pLangusge == 1) TipStr = "打開設備失敗！";
                    if (global.pLangusge == 2) TipStr = "Open device error!";
                    if (global.pLangusge == 3) TipStr = "No se pudo abrir el dispositivo!";
                    if (global.pLangusge == 4) TipStr = "デバイスを開けません。";
                    System.Windows.MessageBox.Show(TipStr);
                    global.isOpenCameraA = false;
                }
                else 
                {                   
                    GetVideoParameter();  //获取视频参数
                    global.isOpenCameraA = true;
                    //FN_SuitableSize(global.RotateCount);
                    //FN_DisplayCenter(global.RotateCount);
                    //FN_SetImagePosition();
                    if (isChangeDevice == true)
                    {
                        isChangeDevice = false;
                        SetVideoDefaultParam();
                    }
                }
            }
            else
            {
                global.WriteMessage("PreResCbBox_SelectionChanged->006");
                GetVideoParameter();  //获取视频参数
                global.isOpenCameraA = true;
                //FN_SuitableSize(global.RotateCount);
                //FN_DisplayCenter(global.RotateCount);
                //FN_SetImagePosition();
                //global.pSlaveImgShowStartX = global.pImgShowStartX;
                //global.pSlaveImgShowStartY = global.pImgShowStartY;
                if (isChangeDevice == true)
                {
                    isChangeDevice = false;
                    SetVideoDefaultParam();
                }
                global.WriteMessage("PreResCbBox_SelectionChanged->007");
            }

            int RoiWidth = 0;
            int RoiHeight=0;
            int IsCutPreviwEdge = GetRoiWidthHeight(ref RoiWidth, ref RoiHeight);
            if (IsCutPreviwEdge==0)
            {
                global.pHostCamera.PreWidth = RoiWidth;
                global.pHostCamera.PreHeight = RoiHeight;

            }

            if (global.RotateCount == 0 || global.RotateCount == 2)
            {
                this.CamVideoPreivew.Source = null;
                mWBitmap = new WriteableBitmap(global.pHostCamera.PreWidth, global.pHostCamera.PreHeight, 72, 72, PixelFormats.Bgr24, null);
                CamRect = new Int32Rect(0, 0, global.pHostCamera.PreWidth, global.pHostCamera.PreHeight);
                GC.Collect();
            }
            if (global.RotateCount == 1 || global.RotateCount == 3)
            {
                this.CamVideoPreivew.Source = null;
                mWBitmap = new WriteableBitmap(global.pHostCamera.PreHeight, global.pHostCamera.PreWidth, 72, 72, PixelFormats.Bgr24, null);
                CamRect = new Int32Rect(0, 0, global.pHostCamera.PreHeight, global.pHostCamera.PreWidth);
                GC.Collect();
            }

            this.CamVideoPreivew.Source = mWBitmap;
            global.WriteMessage("PreResCbBox_SelectionChanged->008");
            GC.Collect();
            RotateFlag = 1; //主要为了清理内存

            if (iRest == 0)
            {
                FN_SuitableSize(global.RotateCount);
                FN_DisplayCenter(global.RotateCount);
                FN_SetImagePosition();
            }
            global.WriteMessage("PreResCbBox_SelectionChanged->009");

        }


        /*****************
         * 切换静态拍照分辨率
         ****************/
        private void StillResCbBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (StillResCbBox.Items.Count>0)
                global.OpenStillResolutionStr = StillResCbBox.SelectedItem.ToString();
            if (isTriggerComboBoxEvent == true && global.isOpenCameraA == true)
            {
                PreResCbBox_SelectionChanged(null,null);
            }
        }


        /*****************
         * 设置色彩模式
         ****************/
        private void RdBtColor_Checked(object sender, RoutedEventArgs e)
        {
            if (RdBtColor.IsChecked == true)
            {
                global.ColorType = 0;
                SetColorType(global.ColorType);
            }
        }

        private void RdBtGray_Checked(object sender, RoutedEventArgs e)
        {
            if (RdBtGray.IsChecked == true)
            {
                global.ColorType = 1;
                SetColorType(global.ColorType);
            }
        }

        private void RdBtBlackWhite_Checked(object sender, RoutedEventArgs e)
        {
            if (RdBtBlackWhite.IsChecked == true)
            {
                global.ColorType = 2;
                SetColorType(global.ColorType);
            }
        }


        /*****************
        * 设置裁切方式
        ****************/
        private void RdBtNoCut_Checked(object sender, RoutedEventArgs e)
        {
            if (RdBtNoCut.IsChecked == true)
            {
                global.CutType = 0;
                SetCutType(global.CutType);
                MyMark.Visibility = Visibility.Hidden;
            }
        }
        private void RdBtAutoCut_Checked(object sender, RoutedEventArgs e)
        {
            if (RdBtAutoCut.IsChecked==true)
            {
                global.CutType = 1;
                SetCutType(global.CutType);
                MyMark.Visibility = Visibility.Hidden;
            }
        }
        private void RdBtManulCut_Checked(object sender, RoutedEventArgs e)
        {
            if (RdBtManulCut.IsChecked == true)
            {          
                global.CutType = 2;
                SetCutType(global.CutType);
                MyMark.Visibility = Visibility.Visible;
            }
        }


        /*****************
        * 手动裁切模式双击拍照事件
        ****************/
        private void ManuualCutDoubleClick()
        {
            //System.Windows.MessageBox.Show("手动裁切模式双击拍照事件");
            if (global.isOpenCameraA && FrameCount>2)
            {
                if (global.Is16MDevice)
                {
                    if (global.pTimerDlgHaveRun)  //如果定时器窗体已经运行
                        return;
                   SetFormatType(global.FileFormat);
                   FuncCaptureFromStill(global.FileFormat, global.NameMode, true);
                }
                else
                {
                    SetFormatType(global.FileFormat);
                   FuncCaptureFromPreview(global.FileFormat, global.NameMode, true);
                }
            }
        }


        /*****************
        * 从预览中拍照
        ****************/
        //int regCount = 0;
        public string FuncCaptureFromPreview(int FileType,int pNameMode,bool isShowToList) 
        {
            SetDpi(global.DpiType,global.DpiVal);
            if (global.isOpenCameraA && FrameCount>2)
            {
                if (global.CutType == 2)
                {
                    Rect pRect = MyMark.GetIndicatorRegion();
                    if (pRect.Width > 1 && pRect.Height > 1)
                    {
                        double xImgCutLeft, xImgCutTop, xImgCutRight, xImgCutBottom;
                        xImgCutLeft = (pRect.Left - global.pImgShowStartX) * 100 / global.pImgShowScale;
                        xImgCutTop = (pRect.Top - global.pImgShowStartY) * 100 / global.pImgShowScale;
                        xImgCutRight = xImgCutLeft + pRect.Width * 100 / global.pImgShowScale;
                        xImgCutBottom = xImgCutTop + pRect.Height * 100 / global.pImgShowScale;
                        SetManualCutRect(xImgCutLeft, xImgCutTop, xImgCutRight, xImgCutBottom);
                    }
                    else
                    {
                        string TipStr = "请设置裁切区域！";
                        if (global.pLangusge == 1) TipStr = "請設置裁切區域！";
                        if (global.pLangusge == 2) TipStr = "Please set the cropping area!";
                        if (global.pLangusge == 3) TipStr = "Por favor, definir zona de corte!";
                        if (global.pLangusge == 4) TipStr = "裁断箇所を設定してください。";
                        System.Windows.MessageBox.Show(TipStr);
                        return "";
                    }
                }
                string fFormatStr = ".jpg";
                if (FileType == 0) fFormatStr = ".jpg";
                if (FileType == 1) fFormatStr = ".bmp";
                if (FileType == 2) fFormatStr = ".png";
                if (FileType == 3) fFormatStr = ".tif";
                if (FileType == 4) fFormatStr = ".pdf";
                string ImgName = DateTime.Now.ToString("yyyyMMddhhmmssfff");
                string imgpath = global.ImagesFolder + "\\" + ImgName + fFormatStr;

                int isBarcode = 0;
                if (pNameMode == 3)
                    isBarcode = 1;
                if (pNameMode == 4)
                    isBarcode = 2;

                //add at 2019.04.02
                if (pNameMode == 5)
                {
                   
                        mFixedNameDlg = new FixedNameDlg();
                        //mFixedNameDlg.Owner = this;
                        mFixedNameDlg.ShowDialog();
                        if (global.pFixedNameStr == "")
                        {
                            mFixedNameDlg.Close();
                            return "";
                        }
                        imgpath = global.ImagesFolder + "\\" + global.pFixedNameStr + fFormatStr;               
                }
                                
                if (pNameMode == 1)
                {
                    string dateStr = DateTime.Now.ToString("yyyy-MM-dd");
                    imgpath = global.ImagesFolder + "\\" + dateStr;
                    if (!Directory.Exists(imgpath))
                        Directory.CreateDirectory(imgpath);
                    string timeStr = DateTime.Now.ToString("yyyyMMddhhmmssfff");
                    imgpath = imgpath + "\\" + timeStr + fFormatStr;
                }
                if (pNameMode == 2)
                {
                    global.CalculateSuffix();
                    string CountStr = global.SuffixSupplyZero(global.SuffixCount, global.SuffixLength);
                    imgpath = global.ImagesFolder + "\\" + global.PrefixNmae + "_" + CountStr + fFormatStr;
                    global.PreviousSuffixCount = global.SuffixCount;
                    global.SuffixCount = global.SuffixCount + global.IncreaseStep;
                    if (global.pSetDlgHaveRun)
                    { 
                        CountStr = global.SuffixSupplyZero(global.SuffixCount, global.SuffixLength);
                        mSettingDlg.SuffixTextBox.Text = CountStr;
                    }
                }

                byte[] pBuf = Encoding.GetEncoding(global.pEncodType).GetBytes(imgpath);
                IntPtr namePtr = CaptureFromPreview(pBuf, isBarcode);
                imgpath = Marshal.PtrToStringAnsi(namePtr);

                //regCount++;
                // int pos = imgpath.LastIndexOf("\\");
                // int pos1 = imgpath.LastIndexOf(".");
                // string name = imgpath.Substring(pos + 1, pos1 - pos - 1);
                // mQRDlg.listBox1.Items.Add("识别结果:" + Convert.ToString(regCount) + "\n");
                // mQRDlg.listBox1.Items.Add(name + "\r");
                // mQRDlg.listBox1.ScrollIntoView(mQRDlg.listBox1.Items[mQRDlg.listBox1.Items.Count-1]);

                if (isShowToList)
                {
                    int pos = imgpath.LastIndexOf("\\");
                    string name = imgpath.Substring(pos + 1, imgpath.Length - pos - 1);
                    PreviewPhoto mPhoto = new PreviewPhoto();
                    string prePath = imgpath;
                    if (FileType == 4)
                    {
                        prePath = System.Windows.Forms.Application.StartupPath + "\\tpdf.jpg";
                        int pp1 = imgpath.LastIndexOf("\\");
                        int pp2 = imgpath.LastIndexOf(".");
                        if (pp2 > pp1)
                        {
                            string tmpname = imgpath.Substring(pp1 + 1, pp2-pp1-1);
                            prePath = System.Windows.Forms.Application.StartupPath + "\\" + tmpname + ".jpg";
                            //prePath = "C:\\" + tmpname + ".jpg";
                        }
                    }           
                    mPhoto.SourceImage = CreateImageSourceThumbnia(prePath, 120, 90);
                    if (File.Exists(prePath) && FileType == 4)
                        File.Delete(prePath);

                    if (FileType == 4)
                        mPhoto.LogoImage = new BitmapImage(new Uri(@"Images\pdfb.png", UriKind.Relative));
                    mPhoto.ImageName = name;
                    mPhoto.ImagePath = imgpath;
                    PreviewImgList.Items.Add(mPhoto);
                    PreviewImgList.ScrollIntoView(PreviewImgList.Items[PreviewImgList.Items.Count - 1]); //设置总显示最后一项
                    //global.pImagePathList.Add(imgpath);
                }

                global.PlaySound(); //播放声音
                //GC.Collect();
                return imgpath;
            }
            else
            {
                return "";
            }
        }

        private void CaptureBt_Click(object sender, RoutedEventArgs e)
        {
            isGetSlaveCamImage = true;
            SetFormatType(global.FileFormat);
            if (global.isJoinMainCam == 1 && global.isOpenCameraB)  //如果与主画面合并拍照
            {
                FuncMainAssistImageJoin(global.FileFormat, global.NameMode, true);
                return;
            }
            FuncCaptureFromPreview(global.FileFormat,global.NameMode,true);
            if(global.isTakeSlaveCamImg==1)
                FuncSlaveCaptureFromPreview(global.FileFormat, global.NameMode, true);
        }

        /*****************
        * 静态拍照
        ****************/
        int wdlgCount = 0;
        private void WaitDialogRunPro()
        {
            while (true)
            {
                wdlgCount++;

                if (wdlgCount == 1)
                {
                    this.Dispatcher.BeginInvoke(new WaitDlgDelegate(OnOffWaitDialog), 1);
                }
                if (isRunWaitDialogThread == false)
                {
                    wdlgCount = 0;
                    this.Dispatcher.BeginInvoke(new WaitDlgDelegate(OnOffWaitDialog), 2);
                    break;
                }
                if (wdlgCount > 120)
                {
                    wdlgCount = 0;
                    isRunWaitDialogThread = false;
                    this.Dispatcher.BeginInvoke(new WaitDlgDelegate(OnOffWaitDialog), 3);
                    break;
                }
                Thread.Sleep(100);
            }
        }

        private void OnOffWaitDialog(int showtype)
        {
            if (showtype == 1)
            {
                mWaitDialog.Left = this.Left + (this.Width - mWaitDialog.ActualWidth) / 2;
                mWaitDialog.Top = this.Top + (this.Height - mWaitDialog.ActualHeight) / 2;
                mWaitDialog.ShowDialog(); //弹出等待对话框               
            }
            else if (showtype==2)
            {
                mWaitDialog.Hide();
            }
            else if (showtype == 3)
            {
                mWaitDialog.Hide();
                string TipStr = "拍摄图片失败！";
                if (global.pLangusge == 1) TipStr = "拍攝圖片失敗！";
                if (global.pLangusge == 2) TipStr = "Shooting pictures failed!";
                if (global.pLangusge == 3) TipStr = "No se pudo capturar la imagen!";
                if (global.pLangusge == 4) TipStr = "撮影に失敗しました。";
                System.Windows.MessageBox.Show(TipStr);
            }
        }


        public string FuncCaptureFromStill(int FileType, int pNameMode, bool isShowToList)
        {
            SetDpi(global.DpiType, global.DpiVal);
            if (global.isOpenCameraA && FrameCount > 2)
            {
                pStillImgShowToList = isShowToList;
                if (global.CutType == 2)
                {
                    Rect pRect = MyMark.GetIndicatorRegion();
                    if (pRect.Width > 1 && pRect.Height > 1)
                    {
                        double xImgCutLeft, xImgCutTop, xImgCutRight, xImgCutBottom;
                        xImgCutLeft = (pRect.Left - global.pImgShowStartX) * 100 / global.pImgShowScale;
                        xImgCutTop = (pRect.Top - global.pImgShowStartY) * 100 / global.pImgShowScale;
                        xImgCutRight = xImgCutLeft + pRect.Width * 100 / global.pImgShowScale;
                        xImgCutBottom = xImgCutTop + pRect.Height * 100 / global.pImgShowScale;
                        SetManualCutRect(xImgCutLeft, xImgCutTop, xImgCutRight, xImgCutBottom);
                    }
                    else
                    {
                        string TipStr = "请设置裁切区域！";
                        if (global.pLangusge == 1) TipStr = "請設置裁切區域！";
                        if (global.pLangusge == 2) TipStr = "Please set the cropping area!";
                        if (global.pLangusge == 3) TipStr = "Por favor, definir zona de corte!";
                        if (global.pLangusge == 4) TipStr = "裁断箇所を設定してください。";
                        System.Windows.MessageBox.Show(TipStr);
                        return "";
                    }
                }

                string fFormatStr = ".jpg";
                if (FileType == 0) fFormatStr = ".jpg";
                if (FileType == 1) fFormatStr = ".bmp";
                if (FileType == 2) fFormatStr = ".png";
                if (FileType == 3) fFormatStr = ".tif";
                if (FileType == 4) fFormatStr = ".pdf";
                string ImgName = DateTime.Now.ToString("yyyyMMddhhmmssfff");
                string imgpath = global.ImagesFolder + "\\" + ImgName + fFormatStr;

                int isBarcode = 0;
                if (pNameMode == 3)
                    isBarcode = 1;
                if (pNameMode == 4)
                    isBarcode = 2;

                //add at 2019.04.02
                if (pNameMode == 5)
                {                   
                        mFixedNameDlg = new FixedNameDlg();
                        //mFixedNameDlg.Owner = this;                       
                        mFixedNameDlg.ShowDialog();
                        if (global.pFixedNameStr == "")
                        {
                            mFixedNameDlg.Close();
                            return "";
                        }
                        imgpath = global.ImagesFolder + "\\" + global.pFixedNameStr + fFormatStr;          
                }

                if (pNameMode == 1)
                {
                    string dateStr = DateTime.Now.ToString("yyyy-MM-dd");
                    imgpath = global.ImagesFolder + "\\" + dateStr;
                    if (!Directory.Exists(imgpath))
                        Directory.CreateDirectory(imgpath);
                    string timeStr = DateTime.Now.ToString("yyyyMMddhhmmssfff");
                    imgpath = imgpath + "\\" + timeStr + fFormatStr;
                }
                if (pNameMode == 2)
                {
                    global.CalculateSuffix();
                    string CountStr = global.SuffixSupplyZero(global.SuffixCount, global.SuffixLength);
                    imgpath = global.ImagesFolder + "\\" + global.PrefixNmae + "_" + CountStr + fFormatStr;
                    global.PreviousSuffixCount = global.SuffixCount;
                    global.SuffixCount = global.SuffixCount + global.IncreaseStep;
                    if (global.pSetDlgHaveRun)
                    {
                        CountStr = global.SuffixSupplyZero(global.SuffixCount, global.SuffixLength);
                        mSettingDlg.SuffixTextBox.Text = CountStr;
                    }
                }


                byte[] pBuf = Encoding.GetEncoding(global.pEncodType).GetBytes(imgpath);
                CaptureFromStill(pBuf, isBarcode);

                //运行弹出等待对话框线程
                isRunWaitDialogThread = true;
                Thread thread = new Thread(WaitDialogRunPro);
                thread.Start();

                global.PlaySound(); //播放声音

                return imgpath;
            }
            else
            {
                return "";
            }
        }

        private void CaptureStillBt_Click(object sender, RoutedEventArgs e)
        {
            if (global.pTimerDlgHaveRun)  //如果定时器窗体已经运行
                return;
           
            SetFormatType(global.FileFormat);
            if (global.isJoinMainCam == 1 && global.isOpenCameraB)  //如果与主画面合并拍照
            {
                FuncStillMainAssistImageJoin(global.FileFormat, global.NameMode, true);
                return;
            }
            isGetSlaveCamImage = true;
            FuncCaptureFromStill(global.FileFormat,global.NameMode,true);
        }


        /*****************
        * 列表双击事件
        ****************/
        private void PreviewImgList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (PreviewImgList.SelectedItems.Count > 0)
            {
                for (int i = 0; i < PreviewImgList.SelectedItems.Count; i++)
                {
                    PreviewPhoto mlistItem = (PreviewPhoto)PreviewImgList.SelectedItem;
                    string imgpath = mlistItem.ImagePath;
                    if (File.Exists(imgpath))
                    {
                        //IntPtr hwnd = ((HwndSource)PresentationSource.FromVisual(this)).Handle;
                        //ShellExecute(hwnd, "open", imgpath, null, null, 4);  //打开文件
                        global.OpenFileAndPreview(imgpath);
                    }
                }               
            }          
        }

        /*************************************
        * 列表操作事件
        **************************************/
        //private RoutedCommand Ls_DeleteCmd = new RoutedCommand("删除", typeof(MainWindow));
        private void ListBoxItem_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            //ListBoxItem data = new ListBoxItem();
            //data = (ListBoxItem)sender;
            //System.Windows.Controls.MenuItem Del_MenuItem = new System.Windows.Controls.MenuItem();
            //Del_MenuItem.Header = "删除";
            //Del_MenuItem.Command = Ls_DeleteCmd;
            ////Ls_DeleteCmd.InputGestures.Add(new KeyGesture(Key.Delete, ModifierKeys.Control));   //添加快捷键
            //Ls_DeleteCmd.InputGestures.Add(new KeyGesture(Key.Delete));  //添加快捷键
            //Del_MenuItem.CommandTarget = data;   //命令作用目标
            //CommandBinding Del_cb = new CommandBinding();
            //Del_cb.Command = Ls_DeleteCmd;
            //Del_cb.Executed +=  Delete_Executed;
            //data.CommandBindings.Add(Del_cb);

            //data.ContextMenu = new System.Windows.Controls.ContextMenu();
            //data.ContextMenu.Items.Add(Del_MenuItem);
            //data.ContextMenu.IsOpen = true;
        }

        //列表->打开
        private void MenuItemOpen_Click(object sender, RoutedEventArgs e)
        {
            if (PreviewImgList.SelectedItems.Count > 0)
            {
                PreviewPhoto mlistItem = (PreviewPhoto)PreviewImgList.SelectedItem;
                string imgpath = mlistItem.ImagePath;
                if (File.Exists(imgpath))
                {
                    global.OpenFileAndPreview(imgpath);
                }
            }          
        }

        //列表->删除
        private void MenuItemDelete_Click(object sender, RoutedEventArgs e)
        {
            while (PreviewImgList.SelectedItems.Count > 0)
            {
                PreviewPhoto mlistItem = (PreviewPhoto)PreviewImgList.SelectedItem;
                string imgpath = mlistItem.ImagePath;
                if (File.Exists(imgpath))
                {
                    try
                    {
                        File.Delete(imgpath);
                    }
                    catch (Exception ex)
                    {

                    }                 
                }
                PreviewImgList.Items.Remove(PreviewImgList.SelectedItem);  
            }                        
        }

        //列表->全选
        private void MenuItemSeleteAll_Click(object sender, RoutedEventArgs e)
        {
            if (PreviewImgList.Items.Count>0)
                PreviewImgList.SelectAll();
        }

        //列表->属性
        private void MenuItemPropertie_Click(object sender, RoutedEventArgs e)
        {
            if (PreviewImgList.SelectedItems.Count > 0)
            {
                PreviewPhoto mlistItem = (PreviewPhoto)PreviewImgList.SelectedItem;
                string imgpath = mlistItem.ImagePath;
                if (File.Exists(imgpath))
                {
                    FilePropertie.ShowFileProperties(imgpath);
                }
            }          
        }


        //列表->打印预览
        bool IsPrintPreview = false;
        private void MenuItemPrintPreview_Click(object sender, RoutedEventArgs e)
        {
            if (PreviewImgList.SelectedItems.Count > 0)
            {
                PreviewPhoto mlistItem = (PreviewPhoto)PreviewImgList.SelectedItem;
                string imgpath = mlistItem.ImagePath;
                if (File.Exists(imgpath))
                {          
                    string suffix = imgpath.Substring(imgpath.Length-3,3);
                    if (suffix != "pdf")
                    {                 
                        printDocument.DocumentName = imgpath;
                        printPreviewDialog.Document = printDocument;
                        try
                        {
                            IsPrintPreview = true;
                            printPreviewDialog.ShowDialog();
                        }
                        catch (Exception ex)
                        {

                        }
                    }              
                }        
            }
        }

        //列表->打印设置
        private void MenuItemPrintSet_Click(object sender, RoutedEventArgs e)
        {         
            if (PreviewImgList.SelectedItems.Count > 0)
            {
                PreviewPhoto mlistItem = (PreviewPhoto)PreviewImgList.SelectedItem;
                string imgpath = mlistItem.ImagePath;
                if (File.Exists(imgpath))
                {
                    string suffix = imgpath.Substring(imgpath.Length - 3, 3);
                    if (suffix != "pdf")
                    {
                        pageSetupDialog.Document = printDocument;
                        pageSetupDialog.AllowMargins = false;
                        pageSetupDialog.ShowDialog();
                    }
                    else 
                    {
                        //pdfDocument.LoadFromFile(imgpath);
                        //pageSetupDialog.Document = pdfDocument.PrintDocument;
                        //pageSetupDialog.AllowMargins = false;
                        //pageSetupDialog.ShowDialog();
                    }
                }
            }



            
        }

        //列表->打印
        private void MenuItemPrint_Click(object sender, RoutedEventArgs e)
        {
            if (PreviewImgList.SelectedItems.Count > 0)
            {
                PreviewPhoto mlistItem = (PreviewPhoto)PreviewImgList.SelectedItem;
                string imgpath = mlistItem.ImagePath;
                if (File.Exists(imgpath))
                {
                    string suffix= imgpath.Substring(imgpath.Length-3,3);
                    if (suffix != "pdf")
                    {
                        //global.WriteMessage("列表打印:" + imgpath);
                        PrintImage(imgpath);                     
                    }          
                    else 
                    {
                        //pdfDocument.LoadFromFile(imgpath);
                        //printDialog.Document = pdfDocument.PrintDocument;
                        //StandardPrintController PrintStandard = new StandardPrintController();
                        //printDocument.PrintController = PrintStandard;
                        //if (printDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                        //{
                        //    pdfDocument.PrintDocument.Print();
                        //}                                     
                    }
                }
            }          
        }

         //列表->复制
        private void MenuItemCopy_Click(object sender, RoutedEventArgs e)
        {
            if (PreviewImgList.SelectedItems.Count > 0)
            {
                System.Collections.Specialized.StringCollection strcoll = new System.Collections.Specialized.StringCollection();
                for (int i = 0; i < PreviewImgList.SelectedItems.Count; i++)
                {
                    PreviewPhoto mlistItem = (PreviewPhoto)PreviewImgList.SelectedItems[i];
                    string imgpath = mlistItem.ImagePath;
                    if (File.Exists(imgpath))
                    {
                        try
                        {
                            strcoll.Add(imgpath);
                            System.Windows.Clipboard.SetFileDropList(strcoll);
                        }
                        catch (Exception ex)
                        {
                            //System.Windows.MessageBox.Show("复制失败:" + imgpath);
                        }
                       
                    }                   
                } 
            }          
        }

         //列表->合并PDF
        private void MenuItemPDF_Click(object sender, RoutedEventArgs e)
        {
            if (PreviewImgList.SelectedItems.Count > 0)
            {
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

                byte[] pdfBuf = Encoding.GetEncoding(global.pEncodType).GetBytes(pdfpath);
                pdf_start(pdfBuf);

                int jCount = 0;
                for (int i = 0; i < PreviewImgList.SelectedItems.Count; i++)
                {                
                    PreviewPhoto mlistItem = (PreviewPhoto)PreviewImgList.SelectedItems[i];
                    string imgpath = mlistItem.ImagePath;                 
                    if (File.Exists(imgpath))
                    {
                        //if (imgpath.Substring(imgpath.Length - 3, 3) != "pdf")
                        //{
                            byte[] pPathBuf = Encoding.GetEncoding(global.pEncodType).GetBytes(imgpath);
                            pdf_addPage(pPathBuf);
                            jCount ++;
                        //}          
                    }  
                }

                pdf_end();

                if (jCount > 0)
                {
                    int pos = pdfpath.LastIndexOf("\\");
                    string name = pdfpath.Substring(pos + 1, pdfpath.Length - pos - 1);
                    PreviewPhoto mPhoto = new PreviewPhoto();
                    PreviewPhoto mlistItem = (PreviewPhoto)PreviewImgList.SelectedItems[PreviewImgList.SelectedItems.Count - 1];
                    string prePath = mlistItem.ImagePath;
                    mPhoto.SourceImage = CreateImageSourceThumbnia(prePath, 120, 90);
                    mPhoto.LogoImage = new BitmapImage(new Uri(@"Images\pdfb.png", UriKind.Relative));
                    mPhoto.ImageName = name;
                    mPhoto.ImagePath = pdfpath;
                    PreviewImgList.Items.Add(mPhoto);
                    PreviewImgList.ScrollIntoView(PreviewImgList.Items[PreviewImgList.Items.Count - 1]); //设置总显示最后一项
                }
                else 
                {
                    string TipStr = "请选择图片完成PDF的合并";
                    if (global.pLangusge == 1) TipStr = "請選擇圖片完成PDF的合並";
                    if (global.pLangusge == 2) TipStr = "Please select the picture to complete the merge of PDF.";
                    if (global.pLangusge == 3) TipStr = "Seleccione la imagen para completar la fusión PDF.";
                    if (global.pLangusge == 4) TipStr = "画像を選択してPDFのマージを完了してください。";
                    System.Windows.Forms.MessageBox.Show(TipStr);
                }
            }
        }


        //列表->按数字后缀合并PDF
        List<string> mpdfFilePathLists = new List<string>();
        List<int> mpdfFileSuffixLists = new List<int>();
        private void MenuItemIncrePDF_Click(object sender, RoutedEventArgs e)
        {
            if (PreviewImgList.SelectedItems.Count > 0)
            {
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

                //筛选带数字后缀的文件
                mpdfFilePathLists.Clear();
                mpdfFileSuffixLists.Clear();
                for (int i = 0; i < PreviewImgList.SelectedItems.Count; i++)
                {
                    PreviewPhoto mlistItem = (PreviewPhoto)PreviewImgList.SelectedItems[i];
                    string imgpath = mlistItem.ImagePath;
                    string nameStr = mlistItem.ImageName;
                    int pp=nameStr.LastIndexOf("_");
                    if (pp > 0 && File.Exists(imgpath))
                    {
                        int pp2 = nameStr.LastIndexOf(".");
                        mpdfFilePathLists.Add(imgpath);
                        string couStr = nameStr.Substring(pp+1, pp2-pp-1);
                        mpdfFileSuffixLists.Add(Convert.ToInt32(couStr));
                        mpdfFileSuffixLists.Sort();    //将数组中的元素从小到大排列
                        //mpdfFileSuffixLists.Reverse(); //将数组中的元素进行翻转
                    }                  
                }
                if (mpdfFilePathLists.Count <= 0)
                    return;

                byte[] pdfBuf = Encoding.GetEncoding(global.pEncodType).GetBytes(pdfpath);
                pdf_start(pdfBuf);

                int jCount = 0;
                for (int i = 0; i < mpdfFileSuffixLists.Count; i++)
                {
                    for (int k = 0; k < mpdfFilePathLists.Count; k++)
                    {
                        string imgpath = mpdfFilePathLists[k];
                        int pp = imgpath.LastIndexOf("_");
                        if (pp > 0)
                        {
                            int pp2 = imgpath.LastIndexOf(".");
                            string couStr = imgpath.Substring(pp + 1, pp2 - pp - 1);
                            int sufCount = Convert.ToInt32(couStr);
                            if (sufCount == mpdfFileSuffixLists[i])
                            {
                                byte[] pPathBuf = Encoding.GetEncoding(global.pEncodType).GetBytes(imgpath);
                                pdf_addPage(pPathBuf);
                                jCount++;
                                break;
                            }
                        }
                    }
                }

                pdf_end();

                if (jCount > 0)
                {
                    int pos = pdfpath.LastIndexOf("\\");
                    string name = pdfpath.Substring(pos + 1, pdfpath.Length - pos - 1);
                    PreviewPhoto mPhoto = new PreviewPhoto();
                    PreviewPhoto mlistItem = (PreviewPhoto)PreviewImgList.SelectedItems[PreviewImgList.SelectedItems.Count - 1];
                    string prePath = mlistItem.ImagePath;
                    mPhoto.SourceImage = CreateImageSourceThumbnia(prePath, 120, 90);
                    mPhoto.LogoImage = new BitmapImage(new Uri(@"Images\pdfb.png", UriKind.Relative));
                    mPhoto.ImageName = name;
                    mPhoto.ImagePath = pdfpath;
                    PreviewImgList.Items.Add(mPhoto);
                    PreviewImgList.ScrollIntoView(PreviewImgList.Items[PreviewImgList.Items.Count - 1]); //设置总显示最后一项
                }
                else
                {
                    string TipStr = "请选择图片完成PDF的合并";
                    if (global.pLangusge == 1) TipStr = "請選擇圖片完成PDF的合並";
                    if (global.pLangusge == 2) TipStr = "Please select the picture to complete the merge of PDF.";
                    if (global.pLangusge == 3) TipStr = "Seleccione la imagen para completar la fusión PDF.";
                    if (global.pLangusge == 4) TipStr = "画像を選択してPDFのマージを完了してください。";
                    System.Windows.Forms.MessageBox.Show(TipStr);
                }
            }
        }


         //列表->重命名
        private void MenuItemRename_Click(object sender, RoutedEventArgs e)
        {
            
            if (PreviewImgList.SelectedItems.Count > 0)
            {
                if (global.pReNameDlgHaveRun)
                    return;

                PreviewPhoto mlistItem = (PreviewPhoto)PreviewImgList.SelectedItem;
                string imgpath = mlistItem.ImagePath;
                mReNameDlg = new ReNameDlg();
                mReNameDlg.Left = this.Left + 20;
                mReNameDlg.Top = this.Top + (this.Height - 130) / 2;
                mReNameDlg.Owner = this;
                mReNameDlg.OldPath = imgpath;
                mReNameDlg.index = PreviewImgList.SelectedIndex;
                mReNameDlg.Show(); 
            }
        }
        
        
        //^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^列表操作事件^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^

        /*****************
          * 左旋转
          ****************/

        bool isBtDown = false;
        DateTime dt1 = DateTime.Now;
        DateTime dt2 = DateTime.Now;
        private void RotateLBt_Click(object sender, RoutedEventArgs e)
        {
            if (isBtDown == true)
            {
                isBtDown = false;
                dt1 = DateTime.Now;
            }
            else
            {
                isBtDown = true;
                dt2 = DateTime.Now;
            }
            TimeSpan ts = dt1.Subtract(dt2);
            double secods =Math.Abs( ts.TotalMilliseconds);
            if (secods <700)  //600毫秒内连续点击两次不做旋转操作
                return;

            if (global.isOpenCameraA == false)
                return;

            RotateFlag = 1;

            //if (global.RotateCount == 0 || global.RotateCount == 2)
            //{
            //    this.CamVideoPreivew.Source = null;
            //    mTmpWBitmap = new WriteableBitmap(global.pHostCamera.PreWidth, global.pHostCamera.PreHeight, 72, 72, PixelFormats.Bgr24, null);
            //    Int32Rect TmpCamRect = new Int32Rect(0, 0, global.pHostCamera.PreWidth, global.pHostCamera.PreHeight);
            //    mTmpWBitmap.WritePixels(TmpCamRect, CamBuf, mTmpWBitmap.BackBufferStride, 0);
            //    this.CamVideoPreivew.Source = mTmpWBitmap;
            //}
            //if (global.RotateCount == 1 || global.RotateCount == 3)
            //{
            //    this.CamVideoPreivew.Source = null;
            //    mTmpWBitmap = new WriteableBitmap(global.pHostCamera.PreHeight, global.pHostCamera.PreWidth, 72, 72, PixelFormats.Bgr24, null);
            //    Int32Rect TmpCamRect = new Int32Rect(0, 0, global.pHostCamera.PreHeight, global.pHostCamera.PreWidth);
            //    mTmpWBitmap.WritePixels(TmpCamRect, CamBuf, mTmpWBitmap.BackBufferStride, 0);
            //    this.CamVideoPreivew.Source = mTmpWBitmap;
            //}

            global.RotateCount--;
            if (global.RotateCount <0)
                global.RotateCount = 3;

            if (global.RotateCount == 0 || global.RotateCount == 2)
            {
                this.CamVideoPreivew.Source = null;
                GC.Collect();
                mWBitmap = new WriteableBitmap(global.pHostCamera.PreWidth, global.pHostCamera.PreHeight, 72, 72, PixelFormats.Bgr24, null);
                CamRect = new Int32Rect(0, 0, global.pHostCamera.PreWidth, global.pHostCamera.PreHeight);
                FN_DisplayCenter(global.RotateCount);
                FN_SetImagePosition();              
                this.CamVideoPreivew.Source = mWBitmap;
            }
            if (global.RotateCount == 1 || global.RotateCount == 3)
            {
                this.CamVideoPreivew.Source = null;
                GC.Collect();
                mWBitmap = new WriteableBitmap(global.pHostCamera.PreHeight, global.pHostCamera.PreWidth, 72, 72, PixelFormats.Bgr24, null);
                CamRect = new Int32Rect(0, 0, global.pHostCamera.PreHeight, global.pHostCamera.PreWidth);
                FN_DisplayCenter(global.RotateCount);
                FN_SetImagePosition();              
                this.CamVideoPreivew.Source = mWBitmap;
            }
            RotateFlag = 1;
            SetRotateAngle(global.RotateCount);
            //FN_DisplayCenter(global.RotateCount);
            //FN_SetImagePosition();
        }
        /*****************
          * 右旋转
          ****************/
        private void RotateRBt_Click(object sender, RoutedEventArgs e)
        {

            if (isBtDown == true)
            {
                isBtDown = false;
                dt1 = DateTime.Now;
            }
            else
            {
                isBtDown = true;
                dt2 = DateTime.Now;
            }
            TimeSpan ts = dt1.Subtract(dt2);
            double secods = Math.Abs(ts.TotalMilliseconds);
            if (secods < 700) //600毫秒内连续点击两次不做旋转操作
                return;


            if (global.isOpenCameraA == false)
                return;

            RotateFlag = 1;

            //if (global.RotateCount == 0 || global.RotateCount == 2)
            //{
            //    this.CamVideoPreivew.Source = null;
            //    mTmpWBitmap = new WriteableBitmap(global.pHostCamera.PreWidth, global.pHostCamera.PreHeight, 72, 72, PixelFormats.Bgr24, null);
            //    Int32Rect TmpCamRect = new Int32Rect(0, 0, global.pHostCamera.PreWidth, global.pHostCamera.PreHeight);
            //    mTmpWBitmap.WritePixels(TmpCamRect, CamBuf, mTmpWBitmap.BackBufferStride, 0);
            //    this.CamVideoPreivew.Source = mTmpWBitmap;
            //}
            //if (global.RotateCount == 1 || global.RotateCount == 3)
            //{
            //    this.CamVideoPreivew.Source = null;
            //    mTmpWBitmap = new WriteableBitmap(global.pHostCamera.PreHeight, global.pHostCamera.PreWidth, 72, 72, PixelFormats.Bgr24, null);
            //    Int32Rect TmpCamRect = new Int32Rect(0, 0, global.pHostCamera.PreHeight, global.pHostCamera.PreWidth);
            //    mTmpWBitmap.WritePixels(TmpCamRect, CamBuf, mTmpWBitmap.BackBufferStride, 0);
            //    this.CamVideoPreivew.Source = mTmpWBitmap;
            //}

            global.RotateCount++;
            if (global.RotateCount >3)
                global.RotateCount = 0;

            if (global.RotateCount == 0 || global.RotateCount == 2)
            {
                this.CamVideoPreivew.Source = null;
                GC.Collect();
                mWBitmap = new WriteableBitmap(global.pHostCamera.PreWidth, global.pHostCamera.PreHeight, 72, 72, PixelFormats.Bgr24, null);
                CamRect = new Int32Rect(0, 0, global.pHostCamera.PreWidth, global.pHostCamera.PreHeight);
                FN_DisplayCenter(global.RotateCount);
                FN_SetImagePosition();                
                this.CamVideoPreivew.Source = mWBitmap;
            }
            if (global.RotateCount == 1 || global.RotateCount == 3)
            {
                this.CamVideoPreivew.Source = null;
                GC.Collect();
                mWBitmap = new WriteableBitmap(global.pHostCamera.PreHeight, global.pHostCamera.PreWidth, 72, 72, PixelFormats.Bgr24, null);
                CamRect = new Int32Rect(0, 0, global.pHostCamera.PreHeight, global.pHostCamera.PreWidth);
                FN_DisplayCenter(global.RotateCount);
                FN_SetImagePosition();           
                this.CamVideoPreivew.Source = mWBitmap;
            }
            RotateFlag = 1;
            SetRotateAngle(global.RotateCount);
           // FN_DisplayCenter(global.RotateCount);
           // FN_SetImagePosition();
        }
        /*****************
          * 放大
          ****************/
        private void ZoomInBt_Click(object sender, RoutedEventArgs e)
        {
            if (global.isOpenCameraA == false)
                return;
            global.pImgShowScale += 2;
            if (global.pImgShowScale > 600)
            {
                global.pImgShowScale = 600;
            }
            FN_DisplayCenter(global.RotateCount);
            FN_SetImagePosition();

        }
        /*****************
          * 缩小
          ****************/
        private void ZoomOutBt_Click(object sender, RoutedEventArgs e)
        {
            if (global.isOpenCameraA == false)
                return;
            global.pImgShowScale -= 2;
            if (global.pImgShowScale < (global.pImgShowFitScale/2))
            {
                global.pImgShowScale = global.pImgShowFitScale / 2;
            }
            FN_DisplayCenter(global.RotateCount);
            FN_SetImagePosition();
            
        }
        /*****************
         * 实际大小
         ****************/
        private void TrueSizeBt_Click(object sender, RoutedEventArgs e)
        {
            if (global.isOpenCameraA == false)
                return;
            global.pImgShowScale = 100;
            FN_DisplayCenter(global.RotateCount);
            FN_SetImagePosition();          
        }
        /*****************
         * 适合大小
         ****************/
        private void BestSizeBt_Click(object sender, RoutedEventArgs e)
        {
            if (global.isOpenCameraA == false)
                return;
            FN_SuitableSize(global.RotateCount);
            FN_DisplayCenter(global.RotateCount);
            FN_SetImagePosition();   
        }

        /*****************
        * 对焦
        ****************/
        private void FocusBt_Click(object sender, RoutedEventArgs e)
        {
            ManualFocus();
        }

        private System.Windows.Point StartPosition; // 本次移动开始时的坐标点位置
        private System.Windows.Point EndPosition;   // 本次移动结束时的坐标点位置
        private bool isMouseDown = false;
        /*****************
         * 视频预览鼠标按下事件
         ****************/
        private void CamVideoPreivew_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            isMouseDown = true;
            StartPosition = e.GetPosition(CamVideoPreivew);           
        }

        /*****************
        * 视频预览鼠标弹起事件
        ****************/
        private void CamVideoPreivew_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            isMouseDown = false;
        }

        /*****************
        * 视频预览鼠标移动事件
        ****************/
        private void CamVideoPreivew_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed && isMouseDown==true)
            {
                if (global.isOpenCameraA == false)
                    return;
                EndPosition = e.GetPosition(CamVideoPreivew);            
                var X = EndPosition.X - StartPosition.X;
                var Y = EndPosition.Y - StartPosition.Y;
                global.pImgShowStartX += X;
                global.pImgShowStartY += Y;
                FN_SetImagePosition();
            }
        }

        /*****************
       * 视频预览鼠标滚动事件
       ****************/
        private void CamVideoPreivew_MouseWheel(object sender, MouseWheelEventArgs e)
        {          
            if (global.isOpenCameraA == false)
                return;
            double tmpScale = (double)e.Delta / 150;
            global.pImgShowScale += tmpScale;
            if (global.pImgShowScale > 600)
            {
                global.pImgShowScale = 600;
            }
            if (global.pImgShowScale < (global.pImgShowFitScale / 2))
            {
                global.pImgShowScale = global.pImgShowFitScale / 2;
            }
            FN_DisplayCenter(global.RotateCount);
            FN_SetImagePosition();
            
        }


        /*****************
          * 选择图片保存根目录
          ****************/
        private void SelectFolderBt_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog m_Dialog = new FolderBrowserDialog();
            DialogResult result = m_Dialog.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.Cancel)
            {
                return;
            }
            string m_Dir = m_Dialog.SelectedPath.Trim();
            global.ImagesFolder = m_Dir;
            ImgFolderTexBox.Text = global.ImagesFolder;

        }
        /*****************
          * 打开图片保存根目录
          ****************/
        private void OpenFolderBt_Click(object sender, RoutedEventArgs e)
        {
            if (Directory.Exists(global.ImagesFolder))
            {
                //IntPtr hwnd = ((HwndSource)PresentationSource.FromVisual(this)).Handle;
                //ShellExecute(hwnd, "open", global.ImagesFolder, null, null, 4);  //打开文件
                global.OpenFileAndPreview(global.ImagesFolder);
            }
        }


        /*****************
           * 设置图片文件格式
           ****************/
        private void FileFormatCbBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            global.FileFormat = FileFormatCbBox.SelectedIndex;
            SetFormatType(global.FileFormat);
        }


        /*****************
           * 视频参数设置
           ****************/
        private void SliderBright_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (global.isOpenCameraA==false)
                return;
            int val = (int)SliderBright.Value;
            SetVideoProcParms(VideoBright, val);
            LableBright.Content = Convert.ToString(val);
            global.pVideoParamObj.Bright = val;
        }

        private void SliderContrast_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (global.isOpenCameraA == false)
                return;
            int val =(int)SliderContrast.Value;
            SetVideoProcParms(VideoContrast, val);
            LableContrast.Content = Convert.ToString(val);
            global.pVideoParamObj.Contrast = val;
        }

        private void SliderExp_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (global.isOpenCameraA == false)
                return;
            CheckBoxExp.IsChecked = false;
            int val = (int)SliderExp.Value;
            int flag=0;  //手动曝光
            SetCameraCotrolParms(VideoExposure, val, flag);
            LableExp.Content = Convert.ToString(val);
            global.pVideoParamObj.Exposure = val;
        }

        private void CheckBoxExp_Checked(object sender, RoutedEventArgs e)
        {
            if (global.isOpenCameraA == false)
                return;
            int min, max, def, current, flag;
            min = 0; max = 0; def = 0; current = 0; flag = 0;
            int iRest = GetCameraCotrolParms(VideoExposure, ref min, ref max, ref def, ref current, ref flag);
            if (iRest == 0)
            {
                flag = 1;  //自动曝光
                global.pVideoParamObj.isAutoExp = flag;
                SetCameraCotrolParms(VideoExposure, current, flag);
                LableExp.Content = Convert.ToString(current);
            }
        }

        private void CheckBoxExp_Unchecked(object sender, RoutedEventArgs e)
        {
            if (global.isOpenCameraA == false)
                return;
            int min, max, def, current, flag;
            min = 0; max = 0; def = 0; current = 0; flag = 0;
            int iRest = GetCameraCotrolParms(VideoExposure, ref min, ref max, ref def, ref current, ref flag);
            if (iRest == 0)
            {
                flag = 0;  //手动曝光
                global.pVideoParamObj.isAutoExp = flag;
                SetCameraCotrolParms(VideoExposure, current, flag);
                LableExp.Content = Convert.ToString(current);
            }
        }

        //设置默认的视频参数
        void SetVideoDefaultParam()
        {

            int iRest = -1;
            int min, max, def, current, flag;
            min = 0; max = 0; def = 0; current = 0; flag = 0;
            //亮度
            iRest = GetVideoProcParms(VideoBright, ref min, ref max, ref def, ref current);
            if (iRest == 0)
            {
                SetVideoProcParms(VideoBright, def);
                global.pVideoParamObj.Bright = def;
                SliderBright.Value = def;
                LableBright.Content = Convert.ToString(def);
            }

            //对比度
            iRest = GetVideoProcParms(VideoContrast, ref min, ref max, ref def, ref current);
            if (iRest == 0)
            {
                SetVideoProcParms(VideoContrast, def);
                global.pVideoParamObj.Contrast = def;
                SliderContrast.Value = def;
                LableContrast.Content = Convert.ToString(def);
            }

            //曝光度
            iRest = GetCameraCotrolParms(VideoExposure, ref min, ref max, ref def, ref current, ref flag);
            if (iRest == 0)
            {
                flag = 1;  //默认自动曝光
                SetCameraCotrolParms(VideoExposure, def, flag);
                global.pVideoParamObj.Exposure = def;
                SliderExp.Value = def;
                LableExp.Content = Convert.ToString(def);
                global.pVideoParamObj.isAutoExp = 1;
                CheckBoxExp.IsChecked = true;
            }

            //色调
            iRest = GetVideoProcParms(VideoHun, ref min, ref max, ref def, ref current);
            if (iRest == 0)
            {
                global.pVideoParamObj.Hun = def;
                SetVideoProcParms(VideoHun, global.pVideoParamObj.Hun);
            }
            //饱和度
            iRest = GetVideoProcParms(VideoSaturation, ref min, ref max, ref def, ref current);
            if (iRest == 0)
            {
                global.pVideoParamObj.Saturation = def;
                SetVideoProcParms(VideoSaturation, global.pVideoParamObj.Saturation);
            }
            //清晰度（锐度）
            iRest = GetVideoProcParms(VideoSharpness, ref min, ref max, ref def, ref current);
            if (iRest == 0)
            {
                global.pVideoParamObj.Sharpness = def;
                SetVideoProcParms(VideoSharpness, global.pVideoParamObj.Sharpness);
            }
            //伽玛
            iRest = GetVideoProcParms(VideoGamma, ref min, ref max, ref def, ref current);
            if (iRest == 0)
            {
                global.pVideoParamObj.Gamma = def;
                SetVideoProcParms(VideoGamma, global.pVideoParamObj.Gamma);
            }
            //逆光对比
            iRest = GetVideoProcParms(VideoBacklightCs, ref min, ref max, ref def, ref current);
            if (iRest == 0)
            {
                global.pVideoParamObj.BacklightCs = def;
                SetVideoProcParms(VideoBacklightCs, global.pVideoParamObj.BacklightCs);
            }
            //增益
            iRest = GetVideoProcParms(VideoGain, ref min, ref max, ref def, ref current);
            if (iRest == 0)
            {
                global.pVideoParamObj.Gain = def;
                SetVideoProcParms(VideoGain, global.pVideoParamObj.Gain);
            }
    }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (global.isOpenCameraA == false)
                return;
            SetVideoDefaultParam();
            
        }
        //更多视频设置参数
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (global.isOpenCameraA == false)
                return;
            ShowCameraSettingWindow();
        }


        /*****************
           * 影像处理设置
           ****************/
        private void CkBox_DelBlackEdge_Checked(object sender, RoutedEventArgs e)
        {
            global.isDelBlackEdge = 1;          
            SetDelBlackEdge(global.isDelBlackEdge);
        }
        private void CkBox_DelBlackEdge_Unchecked(object sender, RoutedEventArgs e)
        {
            global.isDelBlackEdge = 0;
            SetDelBlackEdge(global.isDelBlackEdge);
        }

        private void CkBox_DelBgColor_Checked(object sender, RoutedEventArgs e)
        {
            global.isDelBgColor = 1;
            SetDelBgColor(global.isDelBgColor);
        }
        private void CkBox_DelBgColor_Unchecked(object sender, RoutedEventArgs e)
        {
            global.isDelBgColor = 0;
            SetDelBgColor(global.isDelBgColor);
        }

        private void CkBox_DelShade_Checked(object sender, RoutedEventArgs e)
        {
            global.isDelShade = 1;
            SetDelShade(global.isDelShade);
        }
        private void CkBox_DelShade_Unchecked(object sender, RoutedEventArgs e)
        {
            global.isDelShade = 0;
            SetDelShade(global.isDelShade);
        }

        private void CkBox_DelGray_Checked(object sender, RoutedEventArgs e)
        {
            global.isDelGrayBg = 1;
            SetDelGrayBg(global.isDelGrayBg);
        }
        private void CkBox_DelGray_Unchecked(object sender, RoutedEventArgs e)
        {
            global.isDelGrayBg = 0;
            SetDelGrayBg(global.isDelGrayBg);
        }



        /*****************
          * 定时拍照
          ****************/
        private void TimeCaptureBt_Click(object sender, RoutedEventArgs e)
        {
            if (global.pTimerDlgHaveRun)
                return;
            isGetSlaveCamImage = true;
            mTimerDlg = new TimerDlg();
            mTimerDlg.Left = this.Left + (this.Width - 250) / 2;
            mTimerDlg.Top = this.Top + (this.Height - 210) / 2;
            mTimerDlg.Owner = this;
            mTimerDlg.Show();
            global.pTimerDlgHaveRun = true;
        }


        /*****************
         * 智能拍照
         ****************/
        private void WiseCaptureBt_Click(object sender, RoutedEventArgs e)
        {
            if (global.pWiseShootDlgHaveRun)
               return;
            mWiseDlg = new WiseDlg();
            mWiseDlg.Left = this.Left + (this.Width - 250) / 2;
            mWiseDlg.Top = this.Top + (this.Height - 250) / 2;
            mWiseDlg.Owner = this;
            mWiseDlg.Show();
            global.pWiseShootDlgHaveRun = true;
        }


        /*****************
          * 设置
          ****************/
        private void SetBt_Click(object sender, RoutedEventArgs e)
        {
            if (global.pSetDlgHaveRun)
                return;
            mSettingDlg = new Setting();
            mSettingDlg.Left = this.Left + (this.Width - 400) / 2;
            mSettingDlg.Top = this.Top + (this.Height - 400) / 2;
            mSettingDlg.Owner = this;
            mSettingDlg.Show();
            global.pSetDlgHaveRun = true;
        }


        /*****************
          * 身份证拍摄
          ****************/
        private void IdCardBt_Click(object sender, RoutedEventArgs e)
        {
            if (global.pIdCardDlgHaveRun)
                return;
            mIdCardDlg = new IdCardDlg();
            mIdCardDlg.Left = this.Left + (this.Width - 460) - 10;
            mIdCardDlg.Top = this.Top + (this.Height - 270) - 10;   //加身份证双面拍摄 对话框高度为470
            mIdCardDlg.Owner = this;
            mIdCardDlg.Show();
            global.pIdCardDlgHaveRun = true;

            //mQRDlg = new QRDlg();
            //mQRDlg.Left = this.Left + (this.Width - 300) - 10;
            //mQRDlg.Top = this.Top + (this.Height - 300) - 10;   //加身份证双面拍摄 对话框高度为470
            //mQRDlg.Owner = this;
            //mQRDlg.Show();
        }

        /*****************
         * 单页PDF拍摄
         ****************/
        private void SinglePdfBt_Click(object sender, RoutedEventArgs e)
        {
            isGetSlaveCamImage = false;
            if (global.Is16MDevice) //静态拍照
            {
                SetFormatType(4);
                if (global.isJoinMainCam == 1 && global.isOpenCameraB)  //如果与主画面合并拍照
                {
                    FuncStillMainAssistImageJoin(4, global.NameMode, true);
                    return;
                }
                FuncCaptureFromStill(4, global.NameMode,true);
                //SetFormatType(global.FileFormat);
            }
            else   //非静态拍照
            {
                SetFormatType(4); //设置PDF文件格式

                if (global.isJoinMainCam == 1 && global.isOpenCameraB)  //如果与主画面合并拍照
                {         
                    FuncMainAssistImageJoin(4, global.NameMode, true);
                    SetFormatType(global.FileFormat);
                    return;
                }
                FuncCaptureFromPreview(4, global.NameMode,true);
                SetFormatType(global.FileFormat);
            }
        }


        /*****************
         * 多页PDF拍摄
         ****************/
        private void MorePdfBt_Click(object sender, RoutedEventArgs e)
        {
            if (global.pMorePdfDlgHaveRun)
                return;
            isGetSlaveCamImage = false;
            mMorePdfDlg = new MorePdfDlg();
            mMorePdfDlg.Left = this.Left + (this.Width - 450) - 10;
            //mMorePdfDlg.Left = this.Left ;
            mMorePdfDlg.Top = this.Top + (this.Height - 280) - 10;
            mMorePdfDlg.Owner = this;
            mMorePdfDlg.Show();
            global.pMorePdfDlgHaveRun = true;
        }

        /*****************
         *条码识别
         ****************/
        private void BarCodeBt_Click(object sender, RoutedEventArgs e)
        {
            isGetSlaveCamImage = false;
            if (global.Is16MDevice) //静态拍照
            {     
                SetFormatType(global.FileFormat);
                int IsBarCode = 3; //条码拍照
                if (global.isJoinMainCam == 1 && global.isOpenCameraB)  //如果与主画面合并拍照
                {
                    FuncStillMainAssistImageJoin(global.FileFormat, IsBarCode, true);
                    return;
                }
                FuncCaptureFromStill(global.FileFormat, IsBarCode,true);          
            }
            else   //非静态拍照
            {
                int IsBarCode = 3;   //条码拍照
                SetFormatType(global.FileFormat);
                if (global.isJoinMainCam == 1 && global.isOpenCameraB)  //如果与主画面合并拍照
                {                   
                    FuncMainAssistImageJoin(global.FileFormat, IsBarCode, true);
                    return;
                }  
                FuncCaptureFromPreview(global.FileFormat, IsBarCode,true);       
            }
        }


        /*****************
         *图像合并
         ****************/
        private void JoinImgBt_Click(object sender, RoutedEventArgs e)
        {
            if (global.pJoinImgDlgHaveRun)
                return;
            isGetSlaveCamImage = false;
            mJoinImgDlg = new JoinImgDlg();
            mJoinImgDlg.Left = this.Left + (this.Width - 445) - 10;
            mJoinImgDlg.Top = this.Top + (this.Height - 310) - 10;
            mJoinImgDlg.Owner = this;
            mJoinImgDlg.Show();
            global.pJoinImgDlgHaveRun = true;
        }


        /*****************
        *浮水印
        ****************/
        private void MarkBt_Click(object sender, RoutedEventArgs e)
        {
            if (global.pMarkDlgHaveRun)
                return;
            mMarkDlg = new MarkDlg();
            mMarkDlg.Left = this.Left + (this.Width - 432) / 2;
            mMarkDlg.Top = this.Top + (this.Height - 278) / 2;
            mMarkDlg.Owner = this;
            mMarkDlg.Show();
            global.pMarkDlgHaveRun = true;
        }


        /*****************
        *打印 默认打印JPG图片
        ****************/

        void PrintImage(string imgpath)
        {
            //global.WriteMessage("打印的图片路径" + imgpath);
            printDocument.DocumentName = imgpath;
            printDialog.Document = printDocument;
            StandardPrintController PrintStandard  = new StandardPrintController();
            printDocument.PrintController = PrintStandard;    
            if (printDialog.ShowDialog() ==  System.Windows.Forms.DialogResult.OK)
            {
                printDocument.Print();
               // global.WriteMessage("开始打印....");
            }
        }

        private void PrintBt_Click(object sender, RoutedEventArgs e)
        {
            if (!global.isOpenCameraA )
                return;

            isGetSlaveCamImage = false;
            SetFormatType(0);  //默认打印JPG图片
            if (global.isJoinMainCam == 1 && global.isOpenCameraB)  //如果与主画面合并拍照
            {
                string xprintPath=FuncMainAssistImageJoin(0, global.NameMode, true);
                SetFormatType(global.FileFormat);
                PrintImage(xprintPath);
                return;
            }
    
            string printPath=FuncCaptureFromPreview(0, global.NameMode, true);
            SetFormatType(global.FileFormat);
            PrintImage(printPath);
        }

        private void printDocument_PrintPage(object sender, PrintPageEventArgs e)
        {
            //System.Windows.MessageBox.Show("打印");

            string printPath=printDocument.DocumentName;

            //global.WriteMessage("printDocument_PrintPage:" + printPath);

            if(!File.Exists(printPath))
                return;

            Bitmap M_img = new Bitmap(printDocument.DocumentName);

           // if(M_img==null)
           //     global.WriteMessage("M_img is null");

            //int xx;
            //xx=printDocument.PrinterSettings.DefaultPageSettings.Bounds.Width;
            //xx = printDocument.PrinterSettings.DefaultPageSettings.Bounds.Height;
            //xx = printDocument.PrinterSettings.DefaultPageSettings.Bounds.X;
            //xx = printDocument.PrinterSettings.DefaultPageSettings.Bounds.Y;
            //int topMargin = printDocument.DefaultPageSettings.Margins.Top;//上边距
            //int leftMargin = printDocument.DefaultPageSettings.Margins.Left;//左边距
            //int rightMargin = printDocument.DefaultPageSettings.Margins.Right;//右边距
            //int bottomMargin = printDocument.DefaultPageSettings.Margins.Bottom;//下边距
            int printx  = (int)printDocument.PrinterSettings.DefaultPageSettings.PrintableArea.Left;
            int printy = (int)printDocument.PrinterSettings.DefaultPageSettings.PrintableArea.Top;
            int printw = (int)printDocument.PrinterSettings.DefaultPageSettings.PrintableArea.Right - printx;
            int printh = (int)printDocument.PrinterSettings.DefaultPageSettings.PrintableArea.Bottom - printy;
            bool dire = printDocument.PrinterSettings.DefaultPageSettings.Landscape; //获取打印方向
            if (dire)
            {
                printh = (int)printDocument.PrinterSettings.DefaultPageSettings.PrintableArea.Right - printx;
                printw = (int)printDocument.PrinterSettings.DefaultPageSettings.PrintableArea.Bottom - printy;

                // 横向的情况下，打印区域左边起始位置跟上边起始位置 互换位置
                int tval = printx;
                printx = printy;
                printy = tval;
            }


            //global.WriteMessage("printDocument_PrintPage->" + " printx:" + Convert.ToString(printx) + " printy:" + Convert.ToString(printy)
            //                      + " printw:" + Convert.ToString(printw) + " printh:" + Convert.ToString(printh));
                   
            float re_x, re_y,re_w,re_h;
            float nw, nh;
            re_x=0;
            re_y=0;
            re_w=printw;
            re_h=printh;
            
            if (global.PrintType == 0)   //自适应打印
            {
                if (M_img.Width < printw && M_img.Height < printh )
                {
                    re_w = M_img.Width;
                    re_h = M_img.Height;
                }
                else
                {
                    float bs1 = (float)M_img.Width / M_img.Height;
                    float bs2 = (float)printw / printh;
                    if(bs1<bs2)
                    {
                        re_h = printh;
                        re_w = (float)printh * M_img.Width / M_img.Height;
                    }
                    else
                    {
                        re_w = printw;
                        re_h = (float)printw * M_img.Height / M_img.Width;
                    }
                }           
                re_x = (float)(printw - re_w) / 2 ;
                re_y = (float)(printh - re_h) / 2 ;
                if (IsPrintPreview==true)
                {
                    re_x = (float)(printw - re_w) / 2 + printx;
                    re_y = (float)(printh - re_h) / 2 + printy;
                }

                //global.WriteMessage("printDocument_PrintPage->自适应：" + " re_x:" + Convert.ToString(re_x) + " re_y:" + Convert.ToString(re_y)
                //                      + " re_w:" + Convert.ToString(re_w) + " re_h:" + Convert.ToString(re_h));


                e.Graphics.DrawImage(M_img, re_x, re_y, re_w, re_h);
            }

            if (global.PrintType == 1)    //1比1打印
            {

                float xdpi = M_img.HorizontalResolution;
                float ydpi = M_img.VerticalResolution;
                nw = (float)M_img.Width * 100 / M_img.HorizontalResolution;   //打印机DPI可能为96  
                nh = (float)M_img.Height * 100 / M_img.VerticalResolution;     //打印机DPI可能为96 
                Bitmap nbm = new Bitmap((int)nw, (int)nh);
                Graphics g = Graphics.FromImage(nbm);
                g.DrawImage(M_img, 0, 0, nw, nh);
                g.Dispose();
                nbm.SetResolution(xdpi, ydpi);
                re_y = (printh - nbm.Height) / 2 ;
                re_x = (printw - nbm.Width) / 2 ;
                if (IsPrintPreview == true)
                {
                    re_x = (printw - nbm.Width) / 2 + printx;
                    re_y = (printh - nbm.Height) / 2 + printy;
                }

                //global.WriteMessage("printDocument_PrintPage->1：1：" + " re_x:" + Convert.ToString(re_x) + " re_y:" + Convert.ToString(re_y)
                //                   + " nbm.Width:" + Convert.ToString(nbm.Width) + " nbm.Height:" + Convert.ToString(nbm.Height));

                e.Graphics.DrawImage(nbm, new System.Drawing.Rectangle((int)re_x, (int)re_y, nbm.Width, nbm.Height), 0, 0, nbm.Width, nbm.Height, System.Drawing.GraphicsUnit.Pixel);            
                nbm.Dispose();

              
            }

            M_img.Dispose();
            IsPrintPreview = false;

        }


        /*****************
        *编辑
        ****************/
        private void EditBt_Click(object sender, RoutedEventArgs e)
        {
            string exePath = System.Windows.Forms.Application.StartupPath + "\\edit\\Browser.exe";
            if (File.Exists(exePath))
            {
                global.OpenFileAndPreview(exePath);
            }
            else 
            {
                string TipStr = "该版本无编辑功能模块！";
                if (global.pLangusge == 1) TipStr = "該版本無編輯功能模塊！";
                if (global.pLangusge == 2) TipStr = "This version has no edit function module!";
                if (global.pLangusge == 3) TipStr = "Esta versión no tiene ninguna función de edición!";
                if (global.pLangusge == 4) TipStr = "本バージョンは編集機能が備わっていません。";
                System.Windows.MessageBox.Show(TipStr);
            }
        }




        private void Window_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if ( e.Key == Key.Enter)
            {
                if (global.Is16MDevice)
                {
                     if (global.pTimerDlgHaveRun)  //如果定时器窗体已经运行
                        return;
                    FuncCaptureFromStill(global.FileFormat, global.NameMode, true);
                }
                else
                {
                    FuncCaptureFromPreview(global.FileFormat, global.NameMode, true);
                }
            }
        }



       /***************************************************************************
       ***********************************辅摄像头*********************************
       ****************************************************************************/

        public string FuncSlaveCaptureFromPreview(int FileType, int pNameMode, bool isShowToList)
        {
            if (global.isOpenCameraB)
            {
                SetJpgQualityB(global.JpgQuality);  //设置JPEG图片质量
           
                #if ADDRESOLUTION
                    if (global.isSelectAddRes == true)
                        SetCutTypeB(2);  //358*441的分辨率时
                    else
                        SetCutTypeB(0);  //副头拍照裁切方式为不裁切      
                #else
                    SetCutTypeB(0);  //副头拍照裁切方式为不裁切
                #endif

                SetColorTypeB(global.ColorType);  //拍照色彩模式
                SetFormatTypeB(FileType);  //文件格式
                string fFormatStr = ".jpg";
                if (FileType == 0) fFormatStr = ".jpg";
                if (FileType == 1) fFormatStr = ".bmp";
                if (FileType == 2) fFormatStr = ".png";
                if (FileType == 3) fFormatStr = ".tif";
                if (FileType == 4) fFormatStr = ".pdf";
                string ImgName = DateTime.Now.ToString("yyyyMMddhhmmssfff");
                string imgpath = global.ImagesFolder + "\\" + ImgName + fFormatStr;

                if (pNameMode == 1)
                {
                    string dateStr = DateTime.Now.ToString("yyyy-MM-dd");
                    imgpath = global.ImagesFolder + "\\" + dateStr;
                    if (!Directory.Exists(imgpath))
                        Directory.CreateDirectory(imgpath);
                    string timeStr = DateTime.Now.ToString("yyyyMMddhhmmssfff");
                    imgpath = imgpath + "\\" + timeStr + fFormatStr;
                }
                if (pNameMode == 2)
                {
                    global.CalculateSuffix();
                    string CountStr = global.SuffixSupplyZero(global.SuffixCount, global.SuffixLength);
                    imgpath = global.ImagesFolder + "\\" + global.PrefixNmae + "_" + CountStr + fFormatStr;
                    global.PreviousSuffixCount = global.SuffixCount;
                    global.SuffixCount = global.SuffixCount + global.IncreaseStep;
                    if (global.pSetDlgHaveRun)
                    {
                        CountStr = global.SuffixSupplyZero(global.SuffixCount, global.SuffixLength);
                        mSettingDlg.SuffixTextBox.Text = CountStr;
                    }
                }


                #if ADDRESOLUTION
                    if (global.isSelectAddRes == true)
                    {
                        int left=(global.pSlaveCamera.PreWidth-358)/2;
                        int top=(global.pSlaveCamera.PreHeight-441)/2;
                        int right=left+358;
                        int bottom=top+441;
                        SetManualCutRectB(left, top, right, bottom);
                    }     
                #endif
              
                byte[] pBuf = Encoding.GetEncoding(global.pEncodType).GetBytes(imgpath);
                IntPtr namePtr = CaptureFromPreviewB(pBuf, 0);  //副头拍照没有条码命名
                imgpath = Marshal.PtrToStringAnsi(namePtr);

               

                if (isShowToList)
                {
                    int pos = imgpath.LastIndexOf("\\");
                    string name = imgpath.Substring(pos + 1, imgpath.Length - pos - 1);
                    PreviewPhoto mPhoto = new PreviewPhoto();
                    string prePath = imgpath;
                    if (FileType == 4)
                        prePath = System.Windows.Forms.Application.StartupPath + "\\tpdf2.jpg";
                    mPhoto.SourceImage = CreateImageSourceThumbnia(prePath, 120, 90);
                    if (FileType == 4)
                        mPhoto.LogoImage = new BitmapImage(new Uri(@"Images\pdfb.png", UriKind.Relative));
                    mPhoto.ImageName = name;
                    mPhoto.ImagePath = imgpath;
                    PreviewImgList.Items.Add(mPhoto);
                    PreviewImgList.ScrollIntoView(PreviewImgList.Items[PreviewImgList.Items.Count - 1]); //设置总显示最后一项
                    //global.pImagePathList.Add(imgpath);
                }

                return imgpath;
            }
            else
            {
                return "";
            }
        }

        /*回调*/
        public int CameraSlaveProFunc(IntPtr buf, int width, int height)
        {
            if (CamSlaveBuf == null)
            {
                CamSlaveBuf = new byte[width * height * 3];
            }
            Marshal.Copy(buf, CamSlaveBuf, 0, width * height * 3);
            this.Dispatcher.BeginInvoke(new UpdateUiDelegate(CameraSlavePaint), CamSlaveBuf);
            return 0;
        }

        /*预览绘图*/
        private void CameraSlavePaint(byte[] buf)
        {
            mWSlaveBitmap.WritePixels(CamSlaveRect, CamSlaveBuf, mWSlaveBitmap.BackBufferStride, 0);
        }

        /*开启副摄像头函数*/
        public void toOpenSlaveCamera()
        {
            if (global.pCameraNamesList.Count < 2)
            {
                string TipStr = "未检测到副摄像头！";
                if (global.pLangusge == 1) TipStr = "未檢測到副攝像頭！";
                if (global.pLangusge == 2) TipStr = "Not found the assist camera!";
                if (global.pLangusge == 3) TipStr = "No se detectó cámara secundaria!";
                if (global.pLangusge == 4) TipStr = "サブカメラが検出できません。";
                System.Windows.MessageBox.Show(TipStr);
                return;
            }

            if (DevNameCbBox.SelectedIndex == 1 && global.isOpenCameraA)
            {
                string TipStr = "副摄像头已被开启！";
                if (global.pLangusge == 1) TipStr = "副攝像頭已被開啟！";
                if (global.pLangusge == 2) TipStr = "The assist camera has been opened";
                if (global.pLangusge == 3) TipStr = "La cámara secundaria está conectada!";
                if (global.pLangusge == 4) TipStr = "サブカメラがオンの状態です。";
                System.Windows.MessageBox.Show(TipStr);
                return;
            }

            int CamIndex = 1; //开启副摄像头
            global.pLSlavePreWidths.Clear();
            global.pLSlavePreHeights.Clear();
            if (global.isOpenCameraB == true)
            {
                CloseDeviceB();
                global.isOpenCameraB = false;
            }

            int iRest = -1;
            int xWidth = 0;
            int xHeight = 0;
            int pos = 0;
            int resCount = GetResolutionCountB(CamIndex);
            if (resCount > 0)
            {
                for (int i = 0; i < resCount; i++)
                {
                    iRest = GetResolutionB(i, ref xWidth, ref xHeight);//获取预览分辨率
                    if (iRest == 0)
                    {
                        global.pLSlavePreWidths.Add(xWidth);
                        global.pLSlavePreHeights.Add(xHeight);
                    }
                }

                #if ADDRESOLUTION
                    global.pLSlavePreWidths.Add(358);
                    global.pLSlavePreHeights.Add(441);
                #endif

                for (int i = 0; i < global.pLSlavePreWidths.Count; i++)
                {
                    if (global.pLSlavePreWidths[i] == global.pSlaveCamera.PreWidth && global.pLSlavePreHeights[i] == global.pSlaveCamera.PreHeight)  
                    {
                        pos = i;
                        break;
                    }
                }
                global.pSlaveCamera.PreWidth = global.pLSlavePreWidths[pos];
                global.pSlaveCamera.PreHeight = global.pLSlavePreHeights[pos];
            }

            this.CamVideoPreivew2.Source = null; //切断与显示控件的连接
            CloseDeviceB();
            SetResolutionB(0, pos);  //先设置一下分辨率
            GC.Collect();

            if (CamSlaveBuf != null)
            {
                int size = global.pSlaveCamera.PreWidth * global.pSlaveCamera.PreHeight * 3;
                CamSlaveBuf = (byte[])global.Redim(CamSlaveBuf, size);  //重定义数组大小
                for (int i = 0; i < size; i++)
                {
                    CamSlaveBuf[i] = 0;
                }
            }

            if (CallSlaveBackFunc == null)
            {
                CallSlaveBackFunc = new PFCALLBACK(CameraSlaveProFunc);
                SetCallBackFunctionB(CallSlaveBackFunc);
            }

            this.CamVideoPreivew2.Source = null;
            mWSlaveBitmap = new WriteableBitmap(global.pSlaveCamera.PreWidth, global.pSlaveCamera.PreHeight, 72, 72, PixelFormats.Bgr24, null);
            CamSlaveRect = new Int32Rect(0, 0, global.pSlaveCamera.PreWidth, global.pSlaveCamera.PreHeight);
            this.CamVideoPreivew2.Source = mWSlaveBitmap;
            GC.Collect();

            IntPtr hwnd = ((HwndSource)PresentationSource.FromVisual(this.CamVideoPreivew2)).Handle;
            iRest = OpenDeviceB(CamIndex, global.pSlaveCamera.PreWidth, global.pSlaveCamera.PreHeight, hwnd, false);
            if (iRest != 0)
            {
                string TipStr = "开启副摄像头失败！";
                if (global.pLangusge == 1) TipStr = "開啟副攝像頭失敗！";
                if (global.pLangusge == 2) TipStr = "Open the assist camera error!";
                if (global.pLangusge == 3) TipStr = "Fallo al abrir cámara secundaria!";
                if (global.pLangusge == 4) TipStr = "サブカメラを開けません。";
                System.Windows.MessageBox.Show(TipStr);
                global.isOpenCameraB = false;
            }
            else
            {
                global.isOpenCameraB = true;
                FN_SetSlaveImagePosition();
            }            
        }

        public void FN_SetSlaveImagePosition()
        {
            if (global.isOpenCameraB)
            {
                if (global.SlaveShowSize == 0)
                {
                    global.pSlaveImgShowWidth = 120;
                    global.pSlaveImgShowHeight = 90;
                }
                else if (global.SlaveShowSize == 1)
                {
                    global.pSlaveImgShowWidth = 192;
                    global.pSlaveImgShowHeight = 144;
                }
                else if (global.SlaveShowSize == 2)
                {
                    global.pSlaveImgShowWidth = 240;
                    global.pSlaveImgShowHeight = 180;
                }
                else if (global.SlaveShowSize == 3)
                {
                    global.pSlaveImgShowWidth = 360;
                    global.pSlaveImgShowHeight = 270;
                }
                else if (global.SlaveShowSize == 4)
                {
                    global.pSlaveImgShowWidth = 480;
                    global.pSlaveImgShowHeight = 360;
                }
                else 
                {
                    global.pSlaveImgShowWidth = 240;
                    global.pSlaveImgShowHeight = 180;
                }

                if (global.pSlaveImgShowStartX + global.pSlaveImgShowWidth >= CamPreivewBorder.ActualWidth)
                    global.pSlaveImgShowStartX = CamPreivewBorder.ActualWidth - global.pSlaveImgShowWidth;
                if (global.pSlaveImgShowStartY + global.pSlaveImgShowHeight >= CamPreivewBorder.ActualHeight)
                    global.pSlaveImgShowStartY = CamPreivewBorder.ActualHeight - global.pSlaveImgShowHeight;

                CamVideoPreivew2.SetValue(Canvas.LeftProperty, global.pSlaveImgShowStartX);
                CamVideoPreivew2.SetValue(Canvas.TopProperty, global.pSlaveImgShowStartY);
                CamVideoPreivew2.SetValue(Canvas.WidthProperty, global.pSlaveImgShowWidth);
                CamVideoPreivew2.SetValue(Canvas.HeightProperty, global.pSlaveImgShowHeight);
            }
        }

        /*副摄像头打开关闭*/
        private void AssistBt_Click(object sender, RoutedEventArgs e)
        {
            if (global.isOpenCameraB)
            {
                global.isOpenCameraB = false;
                CloseDeviceB();
                this.CamVideoPreivew2.Source = null;
                string TipStr = "开启副头";
                if (global.pLangusge == 1) TipStr = "開啟副頭！";
                if (global.pLangusge == 2) TipStr = "Open the assist camera ";
                if (global.pLangusge == 3) TipStr = "Enciende la Cámara ";
                if (global.pLangusge == 4) TipStr = "サブヘッダを開く。";
                AssistBt.ToolTip = TipStr;
                GC.Collect();
            }
            else
            {
                toOpenSlaveCamera();
                string TipStr = "关闭副头";
                if (global.pLangusge == 1) TipStr = "關閉副頭";
                if (global.pLangusge == 2) TipStr = "Close the assist camera ";
                if (global.pLangusge == 3) TipStr = "Apaga la Cámara ";
                if (global.pLangusge == 4) TipStr = "副ヘッダを閉じる。";
                AssistBt.ToolTip = TipStr;
            }
        }

        /*副摄像头参数设置*/
        private void AssistSetBt_Click(object sender, RoutedEventArgs e)
        {
            if (global.pAssistSetDlgHaveRun)
                return;
            mAssistSetDlg = new AssistSetDlg();
            mAssistSetDlg.Owner = this;
            mAssistSetDlg.Show();
            global.pAssistSetDlgHaveRun = true;
        }

        private bool isMouseDown2 = false;
        private void CamVideoPreivew2_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            isMouseDown2 = true;
            StartPosition = e.GetPosition(CamVideoPreivew2);  
        }

        private void CamVideoPreivew2_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            isMouseDown2 = false;
        }

        private void CamVideoPreivew2_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed && isMouseDown2 == true)
            {
                if (global.isOpenCameraB == false)
                    return;
                EndPosition = e.GetPosition(CamVideoPreivew2);
                var X = EndPosition.X - StartPosition.X;
                var Y = EndPosition.Y - StartPosition.Y;
                global.pSlaveImgShowStartX += X;
                global.pSlaveImgShowStartY += Y;
                if (global.pSlaveImgShowStartX <= 0)
                    global.pSlaveImgShowStartX = 0;
                if (global.pSlaveImgShowStartY <= 0)
                    global.pSlaveImgShowStartY = 0;
                if (global.pSlaveImgShowStartX + global.pSlaveImgShowWidth >= CamPreivewBorder.ActualWidth)
                    global.pSlaveImgShowStartX = CamPreivewBorder.ActualWidth - global.pSlaveImgShowWidth;
                if (global.pSlaveImgShowStartY + global.pSlaveImgShowHeight >= CamPreivewBorder.ActualHeight)
                    global.pSlaveImgShowStartY = CamPreivewBorder.ActualHeight - global.pSlaveImgShowHeight;
                FN_SetSlaveImagePosition();
            }
        }
        /***************************************************************************
       ***********************************辅摄像头*********************************
       ****************************************************************************/


        public string FuncMainAssistImageJoin(int FileType, int pNameMode, bool isShowToList)
        {
            SetDpi(global.DpiType, global.DpiVal);
            if (global.isOpenCameraA && FrameCount > 2 && global.isOpenCameraB)
            {
                string fFormatStr = ".jpg";
                if (FileType == 0) fFormatStr = ".jpg";
                if (FileType == 1) fFormatStr = ".bmp";
                if (FileType == 2) fFormatStr = ".png";
                if (FileType == 3) fFormatStr = ".tif";
                if (FileType == 4) fFormatStr = ".pdf";
                string ImgName = DateTime.Now.ToString("yyyyMMddhhmmssfff");
                string imgpath = global.ImagesFolder + "\\" + ImgName + fFormatStr;

                int isBarcode = 0;
                if (pNameMode == 3)
                    isBarcode = 1;
                if (pNameMode == 4)
                    isBarcode = 2;


                //add at 2019.04.02
                if (pNameMode == 5)
                {
                        mFixedNameDlg = new FixedNameDlg();
                        //mFixedNameDlg.Owner = this;
                        mFixedNameDlg.ShowDialog();
                        if (global.pFixedNameStr == "")
                        {
                            mFixedNameDlg.Close();
                            return "";
                        }
                        imgpath = global.ImagesFolder + "\\" + global.pFixedNameStr + fFormatStr;                 
                }

                if (pNameMode == 1)
                {
                    string dateStr = DateTime.Now.ToString("yyyy-MM-dd");
                    imgpath = global.ImagesFolder + "\\" + dateStr;
                    if (!Directory.Exists(imgpath))
                        Directory.CreateDirectory(imgpath);
                    string timeStr = DateTime.Now.ToString("yyyyMMddhhmmssfff");
                    imgpath = imgpath + "\\" + timeStr + fFormatStr;
                }
                if (pNameMode == 2)
                {
                    global.CalculateSuffix();
                    string CountStr = global.SuffixSupplyZero(global.SuffixCount, global.SuffixLength);
                    imgpath = global.ImagesFolder + "\\" + global.PrefixNmae + "_" + CountStr + fFormatStr;
                    global.PreviousSuffixCount = global.SuffixCount;
                    global.SuffixCount = global.SuffixCount + global.IncreaseStep;
                    if (global.pSetDlgHaveRun)
                    {
                        CountStr = global.SuffixSupplyZero(global.SuffixCount, global.SuffixLength);
                        mSettingDlg.SuffixTextBox.Text = CountStr;
                    }
                }

                int PW = global.pHostCamera.PreWidth;
                int PH = global.pHostCamera.PreHeight;
                if (global.RotateCount == 1 || global.RotateCount == 3)
                {
                    PW = global.pHostCamera.PreHeight;
                    PH = global.pHostCamera.PreWidth;
                }
                int SlaveJoinW = (int)(PW * global.pSlaveImgShowWidth / global.pImgShowWidth);
                int SlaveJoinH = (int)(PH * global.pSlaveImgShowHeight / global.pImgShowHeight);

                if (global.pSlaveImgShowStartX <= (global.pImgShowStartX - global.pSlaveImgShowWidth))
                    global.pSlaveImgShowStartX = global.pImgShowStartX;
                if (global.pSlaveImgShowStartY <= (global.pImgShowStartY - global.pSlaveImgShowHeight))
                    global.pSlaveImgShowStartY = global.pImgShowStartY ;
                if (global.pSlaveImgShowStartX >= (global.pImgShowStartX + global.pImgShowWidth))
                    global.pSlaveImgShowStartX = global.pImgShowStartX + global.pImgShowWidth - global.pSlaveImgShowWidth;
                if (global.pSlaveImgShowStartY >= (global.pImgShowStartY + global.pImgShowHeight))
                    global.pSlaveImgShowStartY = global.pImgShowStartY + global.pImgShowHeight - global.pSlaveImgShowHeight;

                int SlaveJoinX = (int)((global.pSlaveImgShowStartX - global.pImgShowStartX) * 100 / global.pImgShowScale);
                int SlaveJoinY = (int)((global.pSlaveImgShowStartY - global.pImgShowStartY) * 100 / global.pImgShowScale);

                byte[] pBuf = Encoding.GetEncoding(global.pEncodType).GetBytes(imgpath);
                IntPtr namePtr = MainAssistJoinImages(pBuf, isBarcode, CamSlaveBuf, global.pSlaveCamera.PreWidth, global.pSlaveCamera.PreHeight,
                                                      SlaveJoinX, SlaveJoinY, SlaveJoinW, SlaveJoinH, global.JoinPosition);
                imgpath = Marshal.PtrToStringAnsi(namePtr);

                if (isShowToList)
                {
                    int pos = imgpath.LastIndexOf("\\");
                    string name = imgpath.Substring(pos + 1, imgpath.Length - pos - 1);
                    PreviewPhoto mPhoto = new PreviewPhoto();
                    string prePath = imgpath;
                    if (FileType == 4)
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
                    mPhoto.SourceImage = CreateImageSourceThumbnia(prePath, 120, 90);
                    if (File.Exists(prePath) && FileType == 4)
                        File.Delete(prePath);

                    if (FileType == 4)
                        mPhoto.LogoImage = new BitmapImage(new Uri(@"Images\pdfb.png", UriKind.Relative));
                    mPhoto.ImageName = name;
                    mPhoto.ImagePath = imgpath;
                    PreviewImgList.Items.Add(mPhoto);
                    PreviewImgList.ScrollIntoView(PreviewImgList.Items[PreviewImgList.Items.Count - 1]); //设置总显示最后一项
                    //global.pImagePathList.Add(imgpath);
                }

                global.PlaySound(); //播放声音

                return imgpath;
            }
            else
            {
                return "";
            }
        }

        public string FuncStillMainAssistImageJoin(int FileType, int pNameMode, bool isShowToList)
        {
            SetDpi(global.DpiType, global.DpiVal);
            if (global.isOpenCameraA && FrameCount > 2 && global.isOpenCameraB)
            {
                pStillImgShowToList = isShowToList;
                
                string fFormatStr = ".jpg";
                if (FileType == 0) fFormatStr = ".jpg";
                if (FileType == 1) fFormatStr = ".bmp";
                if (FileType == 2) fFormatStr = ".png";
                if (FileType == 3) fFormatStr = ".tif";
                if (FileType == 4) fFormatStr = ".pdf";
                string ImgName = DateTime.Now.ToString("yyyyMMddhhmmssfff");
                string imgpath = global.ImagesFolder + "\\" + ImgName + fFormatStr;

                int isBarcode = 0;
                if (pNameMode == 3)
                    isBarcode = 1;
                if (pNameMode == 4)
                    isBarcode = 2;

                //add at 2019.04.02
                if (pNameMode == 5)
                {
                        mFixedNameDlg = new FixedNameDlg();
                        //mFixedNameDlg.Owner = this;
                        mFixedNameDlg.ShowDialog();
                        if (global.pFixedNameStr == "")
                        {
                            mFixedNameDlg.Close();
                            return "";
                        }
                        imgpath = global.ImagesFolder + "\\" + global.pFixedNameStr + fFormatStr;                
                }

                if (pNameMode == 1)
                {
                    string dateStr = DateTime.Now.ToString("yyyy-MM-dd");
                    imgpath = global.ImagesFolder + "\\" + dateStr;
                    if (!Directory.Exists(imgpath))
                        Directory.CreateDirectory(imgpath);
                    string timeStr = DateTime.Now.ToString("yyyyMMddhhmmssfff");
                    imgpath = imgpath + "\\" + timeStr + fFormatStr;
                }
                if (pNameMode == 2)
                {
                    global.CalculateSuffix();
                    string CountStr = global.SuffixSupplyZero(global.SuffixCount, global.SuffixLength);
                    imgpath = global.ImagesFolder + "\\" + global.PrefixNmae + "_" + CountStr + fFormatStr;
                    global.PreviousSuffixCount = global.SuffixCount;
                    global.SuffixCount = global.SuffixCount + global.IncreaseStep;
                    if (global.pSetDlgHaveRun)
                    {
                        CountStr = global.SuffixSupplyZero(global.SuffixCount, global.SuffixLength);
                        mSettingDlg.SuffixTextBox.Text = CountStr;
                    }
                }

                int PW = global.pHostCamera.StillWidth;
                int PH = global.pHostCamera.StillHeight;
                if (global.RotateCount == 1 || global.RotateCount == 3)
                {
                    PW = global.pHostCamera.StillHeight;
                    PH = global.pHostCamera.StillWidth;
                }
                int SlaveJoinW = (int)(PW * global.pSlaveImgShowWidth / global.pImgShowWidth);
                int SlaveJoinH = (int)(PH * global.pSlaveImgShowHeight / global.pImgShowHeight);

                if (global.pSlaveImgShowStartX <= (global.pImgShowStartX - global.pSlaveImgShowWidth))
                    global.pSlaveImgShowStartX = global.pImgShowStartX;
                if (global.pSlaveImgShowStartY <= (global.pImgShowStartY - global.pSlaveImgShowHeight))
                    global.pSlaveImgShowStartY = global.pImgShowStartY;
                if (global.pSlaveImgShowStartX >= (global.pImgShowStartX + global.pImgShowWidth))
                    global.pSlaveImgShowStartX = global.pImgShowStartX + global.pImgShowWidth - global.pSlaveImgShowWidth;
                if (global.pSlaveImgShowStartY >= (global.pImgShowStartY + global.pImgShowHeight))
                    global.pSlaveImgShowStartY = global.pImgShowStartY + global.pImgShowHeight - global.pSlaveImgShowHeight;

                double scale = 100.0;
                double ImgWidth = global.pHostCamera.StillWidth;
                double ImgHeight = global.pHostCamera.StillHeight;
                if (global.RotateCount == 1 || global.RotateCount == 3)
                {
                    ImgHeight = global.pHostCamera.StillWidth;
                    ImgWidth = global.pHostCamera.StillHeight;
                }
                double ImageCtlWidth = CamPreivewBorder.ActualWidth - 2;  //边框占2个像素
                double ImageCtlHeight = CamPreivewBorder.ActualHeight - 2;  //边框占2个像素
                double multip1, multip2;
                if ((ImgWidth < ImageCtlWidth) && (ImgHeight < ImageCtlHeight))
                {
                    scale = 100.0;
                }
                else
                {
                    multip1 = (double)ImgWidth / ImgHeight;
                    multip2 = ImageCtlWidth / ImageCtlHeight;
                    if (multip1 < multip2)
                        scale = ImageCtlHeight * 100 / ImgHeight;
                    else
                        scale = ImageCtlWidth * 100 / ImgWidth;
                }
                int SlaveJoinX = (int)((global.pSlaveImgShowStartX - global.pImgShowStartX) * 100 / scale);
                int SlaveJoinY = (int)((global.pSlaveImgShowStartY - global.pImgShowStartY) * 100 / scale);
                //double bs1 = global.pHostCamera.StillWidth / global.pHostCamera.PreWidth;
                //double bs2 = global.pHostCamera.StillHeight / global.pHostCamera.PreHeight;
                //SlaveJoinX = (int)(SlaveJoinX * (global.pImgShowScale / global.pImgShowFitScale));
                //SlaveJoinY = (int)(SlaveJoinY * (global.pImgShowScale / global.pImgShowFitScale));

                byte[] pBuf = Encoding.GetEncoding(global.pEncodType).GetBytes(imgpath);
                StillMainAssistJoinImages(pBuf, isBarcode, CamSlaveBuf, global.pSlaveCamera.PreWidth, global.pSlaveCamera.PreHeight,
                                                       SlaveJoinX, SlaveJoinY, SlaveJoinW, SlaveJoinH, global.JoinPosition);

                //运行弹出等待对话框线程
                isRunWaitDialogThread = true;
                Thread thread = new Thread(WaitDialogRunPro);
                thread.Start();

                global.PlaySound(); //播放声音

                return imgpath;
            }
            else
            {
                return "";
            }
        }



        //录像
        private void RecordBt_Click(object sender, RoutedEventArgs e)
        {
            if (global.pRecordDlgHaveRun)
                return;
            mRecordDlg = new RecordDlg();
            mRecordDlg.Left = (SystemParameters.WorkArea.Width - 280) / 2;
            mRecordDlg.Top = this.Top;
            mRecordDlg.Owner = this;
            mRecordDlg.Show();
            global.pRecordDlgHaveRun = true;
 
        }





        //切换语言
        public void ChangeLanguage(int languageType)
        {
            ResourceDictionary langRd = null;
            try
            {         
                if (languageType == 0)
                    langRd = System.Windows.Application.LoadComponent(new Uri(@"zh-cn.xaml", UriKind.Relative)) as ResourceDictionary;
                if (languageType == 1)
                    langRd = System.Windows.Application.LoadComponent(new Uri(@"zh-tw.xaml", UriKind.Relative)) as ResourceDictionary;
                if (languageType == 2)
                    langRd = System.Windows.Application.LoadComponent(new Uri(@"en-us.xaml", UriKind.Relative)) as ResourceDictionary;
                if (languageType == 3)
                    langRd = System.Windows.Application.LoadComponent(new Uri(@"zh-spain.xaml", UriKind.Relative)) as ResourceDictionary;
                if (languageType == 4)
                    langRd = System.Windows.Application.LoadComponent(new Uri(@"zh-Japan.xaml", UriKind.Relative)) as ResourceDictionary;

                InitBtnSize();
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

                if (global.pSetDlgHaveRun)
                {
                    if ( mSettingDlg.Resources.MergedDictionaries.Count > 0)
                    {
                        mSettingDlg.Resources.MergedDictionaries.Clear();
                    }
                    mSettingDlg.Resources.MergedDictionaries.Add(langRd);
                }
                if (global.pMarkDlgHaveRun)
                {
                    if (mMarkDlg.Resources.MergedDictionaries.Count > 0)
                    {
                        mMarkDlg.Resources.MergedDictionaries.Clear();
                    }
                    mMarkDlg.Resources.MergedDictionaries.Add(langRd);
                }
                if (global.pMorePdfDlgHaveRun)
                {
                    if (mMorePdfDlg.Resources.MergedDictionaries.Count > 0)
                    {
                        mMorePdfDlg.Resources.MergedDictionaries.Clear();
                    }
                    mMorePdfDlg.Resources.MergedDictionaries.Add(langRd);
                }
                if (global.pJoinImgDlgHaveRun)
                {
                    if (mJoinImgDlg.Resources.MergedDictionaries.Count > 0)
                    {
                        mJoinImgDlg.Resources.MergedDictionaries.Clear();
                    }
                    mJoinImgDlg.Resources.MergedDictionaries.Add(langRd);
                }
                if (global.pIdCardDlgHaveRun)
                {
                    if (mIdCardDlg.Resources.MergedDictionaries.Count > 0)
                    {
                        mIdCardDlg.Resources.MergedDictionaries.Clear();
                    }
                    mIdCardDlg.Resources.MergedDictionaries.Add(langRd);
                }
                if (global.pRecordDlgHaveRun)
                {
                    if (mRecordDlg.Resources.MergedDictionaries.Count > 0)
                    {
                        mRecordDlg.Resources.MergedDictionaries.Clear();
                    }
                    mRecordDlg.Resources.MergedDictionaries.Add(langRd);
                }

                if (global.pTimerDlgHaveRun)
                {
                    if (mTimerDlg.Resources.MergedDictionaries.Count > 0)
                    {
                        mTimerDlg.Resources.MergedDictionaries.Clear();
                    }
                    mTimerDlg.Resources.MergedDictionaries.Add(langRd);
                }

                if (global.pWiseShootDlgHaveRun)
                {
                    if (mWiseDlg.Resources.MergedDictionaries.Count > 0)
                    {
                        mWiseDlg.Resources.MergedDictionaries.Clear();
                    }
                    mWiseDlg.Resources.MergedDictionaries.Add(langRd);
                }

                if (global.pAssistSetDlgHaveRun)
                {
                    if (mAssistSetDlg.Resources.MergedDictionaries.Count > 0)
                    {
                        mAssistSetDlg.Resources.MergedDictionaries.Clear();
                    }
                    mAssistSetDlg.Resources.MergedDictionaries.Add(langRd);
                }

                if (global.pReNameDlgHaveRun)
                {
                    if (mReNameDlg.Resources.MergedDictionaries.Count > 0)
                    {
                        mReNameDlg.Resources.MergedDictionaries.Clear();
                    }
                    mReNameDlg.Resources.MergedDictionaries.Add(langRd);
                }


                if (global.Is16MDevice)
                {
                    string btStr = "捕获";
                    if (global.pLangusge == 1) btStr = "捕獲";
                    if (global.pLangusge == 2) btStr = "Snap";
                    if (global.pLangusge == 3) btStr = "Capturar";
                    if (global.pLangusge == 4) btStr = "撮影";
                    CaptureBt.Content = btStr;
                }
                else 
                {
                    string btStr = "拍摄";
                    if (global.pLangusge == 1) btStr = "拍攝";
                    if (global.pLangusge == 2) btStr = "Capture";
                    if (global.pLangusge == 3) btStr = "Capturar";
                    if (global.pLangusge == 4) btStr = "撮影";
                    CaptureBt.Content = btStr;
                }

               
            }          
        }


        private void FScreenBt_Click(object sender, RoutedEventArgs e)
        {
            isFullScreen = true;
            PhotoPreivew.Source = null;
            FunctionBtsA.Visibility = Visibility.Collapsed;
            FunctionBtsB.Visibility = Visibility.Collapsed;
            PanelToolBt.Visibility = Visibility.Collapsed;
            PanelTop.Visibility = Visibility.Collapsed;
            PreviewImgList.Visibility = Visibility.Collapsed;
            PanelRight.Visibility = Visibility.Collapsed;
            PanelToolBtB.Visibility = Visibility.Visible;
            this.WindowStyle = WindowStyle.None;
            this.WindowState = WindowState.Maximized;
        }

        private void F_ExitFScreenBt_Click(object sender, RoutedEventArgs e)
        {
            isFullScreen = false;
            PhotoPreivew.Source = null;
            PanelToolBtB.Visibility = Visibility.Collapsed;
            FunctionBtsA.Visibility = Visibility.Visible;
            FunctionBtsB.Visibility = Visibility.Visible;
            PanelToolBt.Visibility = Visibility.Visible;
            PanelTop.Visibility = Visibility.Visible;
            PreviewImgList.Visibility = Visibility.Visible;
            PanelRight.Visibility = Visibility.Visible;
            this.WindowStyle = WindowStyle.SingleBorderWindow;
            this.WindowState = WindowState.Normal;
        }

        private void F_PhotoBt_Click(object sender, RoutedEventArgs e)
        {
            if (global.Is16MDevice)
            {
                CaptureStillBt_Click(null, null);
            }
            else
            {
                CaptureBt_Click(null,null);
                if (PreviewImgList.Items.Count > 0)
                {
                    string prePath = "";
                    if (global.FileFormat == 4)
                    {                       
                        prePath = System.Windows.Forms.Application.StartupPath + "\\tpdf.jpg";

                        PreviewPhoto mlistItem = (PreviewPhoto)PreviewImgList.Items[PreviewImgList.Items.Count - 1];
                        string imgpath = mlistItem.ImagePath; 
                        int pp1 = imgpath.LastIndexOf("\\");
                        int pp2 = imgpath.LastIndexOf(".");
                        if (pp2 > pp1)
                        {
                            string tmpname = imgpath.Substring(pp1 + 1, pp2-pp1-1);
                            prePath = System.Windows.Forms.Application.StartupPath + "\\" + tmpname + ".jpg";
                            //prePath = "C:\\" + tmpname + ".jpg";
                        }

                    }                  
                    else
                    {
                        PreviewPhoto mlistItem = (PreviewPhoto)PreviewImgList.Items[PreviewImgList.Items.Count - 1];
                        prePath = mlistItem.ImagePath;
                    }

                    if (File.Exists(prePath) && PreviewImgList.Items.Count > 0)
                        PhotoPreivew.Source = CreateImageSourceThumbnia(prePath, 120, 90);

                    if (File.Exists(prePath) && global.FileFormat == 4)
                        File.Delete(prePath);
                }
                
            }
        }

        private void PhotoPreivew_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                string openpath = "";
                if (PreviewImgList.Items.Count > 0)
                {
                    PreviewPhoto mlistItem = (PreviewPhoto)PreviewImgList.Items[PreviewImgList.Items.Count - 1];
                    openpath = mlistItem.ImagePath;
                    if (File.Exists(openpath))
                    {
                        global.OpenFileAndPreview(openpath);
                    }
                }
                
            }
        }


        private void DocumentBt_Click(object sender, RoutedEventArgs e)
        {
           
            if (global.Is16MDevice) //静态拍照
            {
                SetDelBlackEdge(1);
                SetDelBgColor(1);
                SetFormatType(global.FileFormat);
                FuncCaptureFromStill(global.FileFormat, global.NameMode, true);
            }
            else   //非静态拍照
            {
                SetDelBlackEdge(1);
                SetDelBgColor(1);
                SetFormatType(global.FileFormat);
                FuncCaptureFromPreview(global.FileFormat, global.NameMode, true);
                SetDelBlackEdge(global.isDelBlackEdge);
                SetDelBgColor(global.isDelBgColor);
            }
        }


        //调用OCR

        public bool StartProcess(string runFilePath, params string[] args)
        {
            string s = "";
            foreach (string arg in args)
            {
                s = s + arg + " ";
            }
            s = s.Trim();
            Process process = new Process();//创建进程对象    
            ProcessStartInfo startInfo = new ProcessStartInfo(runFilePath, s); // 括号里是(程序名,参数)
            process.StartInfo = startInfo;
            process.Start();
            return true;
        }


        List<string> ocrImagePathList = new List<string>();

        private void OcrBt_Click(object sender, RoutedEventArgs e)
        {

            //string exe_path = System.Windows.Forms.Application.StartupPath + "\\recog\\OCR.exe";
            //if (File.Exists(exe_path))
            //{
            //    string[] the_args = { "OVFC5iWw1V4b7CqHhChu2WtpbmdodW4=", "ZH-CN" };   // 被调exe接受的参数
            //    StartProcess(exe_path, the_args);
            //}


            ocrImagePathList.Clear();

            for (int i = 0; i < PreviewImgList.SelectedItems.Count; i++)
            {
                PreviewPhoto mlistItem = (PreviewPhoto)PreviewImgList.SelectedItems[i];
                string imgpath = mlistItem.ImagePath;
                if (File.Exists(imgpath))
                {
                    string suffix = imgpath.Substring(imgpath.Length - 3, 3);
                    if (suffix != "pdf")
                    {
                        ocrImagePathList.Add(imgpath);
                    }
                }            
            }


            if (ocrImagePathList.Count > 0)
            {
                string[] the_args = new string[ocrImagePathList.Count + 3];
                the_args[0] = "LjbMakew1V4b7CqHhChu2WtpbmdodW4=";
                the_args[1] = "ChinesePRC,English";
                the_args[2] = "doc";

                for (int i = 0; i < ocrImagePathList.Count; i++)
                {
                    string argStr = ocrImagePathList[i].ToString();
                    argStr = argStr.Replace(' ', '*');
                    the_args[i + 3] = argStr;
                }

                string exeName = "recog.exe";
                if (global.pLangusge >= 2)
                    exeName = "recog_en.exe";
                string exe_path = System.Windows.Forms.Application.StartupPath + "\\rg\\" + exeName;
                if (File.Exists(exe_path))
                    StartProcess(exe_path, the_args);
                else
                {
                    string TipStr = "打开OCR失败！";
                    if (global.pLangusge == 1) TipStr = "打開OCR失敗！";
                    if (global.pLangusge == 2) TipStr = "Open OCR failed!";
                    if (global.pLangusge == 3) TipStr = "Fallo al abrir OCR!";
                    if (global.pLangusge == 4) TipStr = "OCRを開けません。";
                    System.Windows.MessageBox.Show(TipStr);
                }

            }
            else
            {
                string[] the_args = new string[3];
                the_args[0] = "LjbMakew1V4b7CqHhChu2WtpbmdodW4=";
                the_args[1] = "ChinesePRC,English";
                the_args[2] = "doc";
                string exeName = "recog.exe";
                if (global.pLangusge >= 2)
                    exeName = "recog_en.exe";
                string exe_path = System.Windows.Forms.Application.StartupPath + "\\rg\\" + exeName;
                if (File.Exists(exe_path))
                    StartProcess(exe_path, the_args);
                else
                {
                    string TipStr = "打开OCR失败！";
                    if (global.pLangusge == 1) TipStr = "打開OCR失敗！";
                    if (global.pLangusge == 2) TipStr = "Open OCR failed!";
                    if (global.pLangusge == 3) TipStr = "Fallo al abrir OCR!";
                    if (global.pLangusge == 4) TipStr = "OCRを開けません。";
                    System.Windows.MessageBox.Show(TipStr);
                }
            }
           
        }

       



    }
}



public class PreviewPhoto
{
    public ImageSource SourceImage { get; set; }
    public ImageSource LogoImage { get; set; }
    public string ImageName { get; set; }
    public string ImagePath { get; set; }
}
