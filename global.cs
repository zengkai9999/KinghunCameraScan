using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.IO;
using System.Windows;

namespace CameraScan
{
    class global
    {
        [DllImport("kernel32.dll")]
        public extern static int WritePrivateProfileString(string lpApplicationName, string lpKeyName, string lpString, string lpFileName);
        [DllImport("kernel32.dll")]
        public extern static int GetPrivateProfileString(string lpApplicationName, string lpKeyName, string lpDefault, StringBuilder lpReturnedString, int nSize, string lpFileName);
        [DllImport("kernel32")]
        public static extern bool WritePrivateProfileString(byte[] section, byte[] key, byte[] val, string filePath);
        [DllImport("kernel32")]
        public static extern int GetPrivateProfileString(byte[] section, byte[] key, byte[] def, byte[] retVal, int size, string filePath);


        [DllImport("DevCapture.dll", CallingConvention = CallingConvention.Cdecl)]
        public extern static int AddMark(int isAdd);
        [DllImport("DevCapture.dll", CallingConvention = CallingConvention.Cdecl)]
        public extern static int SetMark(int type, byte[] markContent, int fontType, byte[] fontName, int fontColor, double size, int trans, int xPos, int yPos, int isAddTimeMark);

        public static bool Is16MDevice = false;
        public static string ImagesFolder = System.Windows.Forms.Application.StartupPath + "\\GpyImages";
        //public static string ImagesFolder = "D:\\GpyImages";  
        public static bool isOpenCameraA = false;
        public static int CameraIndex = 0;  //0,主摄像头；1，副摄像头
        public static string OpenResolutionStr = "120*90";
        public static string OpenStillResolutionStr = "120*90";
        // public static int CameraShowWidth = 800;
        // public static int CameraShowHeight = 600;
        public static List<string> pCameraNamesList = new List<string>();

        //public static List<string> pImagePathList = new List<string>(); //用于保存拍照图片路径

        public static List<int> pLHostPreWidths = new List<int>();     //主头分辨率宽
        public static List<int> pLHostPreHeights = new List<int>();    //主头分辨率宽
        public static List<int> pLHostStillWidths = new List<int>();   //主头分辨率宽
        public static List<int> pLHostStillHeights = new List<int>();  //主头分辨率宽

        public struct CameraObj
        {
            public int PreWidth;
            public int PreHeight;
            public int StillWidth;
            public int StillHeight;
        }

        public static CameraObj pHostCamera;
        public static CameraObj pSlaveCamera;

        public static double pImgShowFitScale = 50;
        public static double pImgShowScale = 50;
        public static double pImgShowStartX = 0;   //
        public static double pImgShowStartY = 0;  //
        public static double pImgShowWidth = 640;   //
        public static double pImgShowHeight = 480;  //
        public static double RotateAngle = 0;
        public static int RotateCount = 0;

        public static int JpgQuality = 75;   //JPG图像质量
        public static int FileFormat = 0;   //文件格式
        public static int CutType = 0;   //裁切方式
        public static int ColorType = 0;   //色彩格式
        public static int isDelBlackEdge = 0;  //是否去黑边
        public static int isDelBgColor = 0;    //是否去底色
        public static int isDelShade = 0;     //是否去阴影
        public static int isDelGrayBg = 0;      //是否去灰底

        public static int IncreaseStep = 1;
        public static int NameMode = 0;   //命名方式：  0,时间方式  1,条码命名  2，日期文件夹  3，自定义
        public static string PrefixNmae = "IMG";  //命名前缀
        public static int PreviousSuffixCount = 0;  //命名后缀计数
        public static int SuffixCount = 0;  //命名后缀计数
        public static int SuffixLength = 4; //命名后缀位数长度
        public static int isScanVoice = 0;  //拍照声音
        public static int isSavePdfSoure = 0;  //多页PDF保存源图像
        public static int isSaveJoinSoure = 0;  //图像合并保存源图像

        public static int JoinPages = 2;  //合并页数
        public static int JoinDir = 0;    //合并方向

        public static int PrintType = 1;  //打印模式  //默认1:1 打印

        public static int pTimerInterval = 2;
        public static bool IsTimerScanCanDo = true;
        public static bool IsWiseScanCanDo = true;

        public static bool pWiseCaptureFormScanDo = false;  //子窗体扫描拍照委托响应标志
        public static bool pMorePdfFormScanDo = false;  //子窗体扫描拍照委托响应标志
        public static bool pIdCardFormScanDo = false;
        public static bool pJoinImgFormScanDo = false;

        public static bool pMorePdfDlgHaveRun = false;  //子窗体是否运行     
        public static bool pIdCardDlgHaveRun = false;  //子窗体是否运行    
        public static bool pJoinImgDlgHaveRun = false;  //子窗体是否运行
        public static bool pTimerDlgHaveRun = false;  //子窗体是否运行
        public static bool pSetDlgHaveRun = false;  //子窗体是否运行
        public static bool pMarkDlgHaveRun = false;  //子窗体是否运行
        public static bool pAssistSetDlgHaveRun = false;  //子窗体是否运行
        public static bool pRecordDlgHaveRun = false;  //子窗体是否运行
        public static bool pReNameDlgHaveRun = false;  //子窗体是否运行
        public static bool pWiseShootDlgHaveRun = false;

        public static bool isOpenCameraB = false;
        public static int isTakeSlaveCamImg = 1;  //是否副头拍照
        public static int isJoinMainCam = 0;      //是否与主画面合并
        public static int SlaveShowSize = 1;     //副头画面显示大小 0.5、1.0、2.0
        public static int JoinPosition = 0;      //与主画面合并位置
        public static double pSlaveImgShowStartX = 0;
        public static double pSlaveImgShowStartY = 0;
        public static double pSlaveImgShowWidth = 200;   //
        public static double pSlaveImgShowHeight = 150;  //
        public static List<int> pLSlavePreWidths = new List<int>();  //副头分辨率宽
        public static List<int> pLSlavePreHeights = new List<int>(); //副头分辨率高
        public static bool isSelectAddRes = false;

        public static int DpiType = 0;
        public static int DpiVal = 200;

        /**********视频保存参数**********/
        public struct VideoParamObj
        {
            public int Bright;   //亮度
            public int Contrast; //对比度
            public int Hun;      //色调
            public int Saturation; //饱和
            public int Sharpness;  //清晰度（锐度）
            public int Gamma;      //伽玛
            public int Balance;      //白平衡
            public int BacklightCs;    //逆光对比
            public int Gain;    //增益
            public int Exposure;  //曝光
            public int isAutoExp;
            public int isAutoBalance; //自动白平衡
        }
        public static VideoParamObj pVideoParamObj;


        /**********浮水印保存参数**********/
        public static int MarkType = 0;

        public static int isAddMark = 0;  //是否添加水印
        public static int isAddTimeMark = 0;

        public static string txtMarkContent = "MarkContent";  //水印内容
        public static int txtMarkFontSize = 80;  //字体大小
        public static int txtMarkFontType = 0;  //字体样式
        public static string txtMarkFontName = "黑体";  //字体名称     
        public static int txtMarkColor = 0;  //字体颜色
        public static int txtMarkTrans = 255;  //透明度
        public static int txtMarkXPos = 100;  // X偏移
        public static int txtMarkYPos = 100;  // Y偏移

        public static string imgMarkPath = "";  //图片水印路径
        public static int imgMarkSize = 2;  //图像水印大小 0.3、0.5、1.0、1.5、2.0、3.0
        public static int imgMarkTrans = 255;  //透明度
        public static int imgMarkXPos = 100;  //X偏移
        public static int imgMarkYPos = 100;  //Y偏移


        //重定义数组
        public static Array Redim(Array origArray, int desiredSize)
        {
            Type t = origArray.GetType().GetElementType();
            Array newArray = Array.CreateInstance(t, desiredSize);
            Array.Copy(origArray, 0, newArray, 0, Math.Min(origArray.Length, desiredSize));
            return newArray;
        }


        public static int TimeInterval = 1000;      //定时器时间间隔
        public static int TimerScanNum = 0;         //定时器拍摄张数

        public static int pRecordType = 0;     //摄像头/录屏
        public static int pRecordFormat = 0;   //默认AVI格式
        public static int pRecordFps = 1;     //帧率，默认10帧

        public static int pLangusge = 0;     //默认汉语
        //public static int pLangusge = 2;     //默认英语
        public static string pEncodType = "utf-8";     //编码类型

        public static string pFixedNameStr = "";     //固定命名

        //与ini交互必须统一编码格式
        private static byte[] getBytes(string s, string encodingName)
        {
            return null == s ? null : Encoding.GetEncoding(encodingName).GetBytes(s);
        }
        public static string ReadString(string section, string key, string def, string fileName, string encodingName = "utf-8", int size = 1024)
        {
            byte[] buffer = new byte[size];
            int count = GetPrivateProfileString(getBytes(section, encodingName), getBytes(key, encodingName), getBytes(def, encodingName), buffer, size, fileName);
            return Encoding.GetEncoding(encodingName).GetString(buffer, 0, count).Trim();
        }
        public static bool WriteString(string section, string key, string value, string fileName, string encodingName = "utf-8")
        {
            return WritePrivateProfileString(getBytes(section, encodingName), getBytes(key, encodingName), getBytes(value, encodingName), fileName);
        }

        //************************************保存读取配置******************************************//

        public static string ConfigIniPath = System.Windows.Forms.Application.StartupPath + "\\KHCAMERA.ini";
        public static string ConfigIniUTF8 = System.Windows.Forms.Application.StartupPath + "\\PATH_MARK.ini";
        // public static string ConfigIniPath = "D:\\KHCAMERA.ini";

        #region "读取设置参数"

        public static int ReadConfigPramas()
        {
            if (File.Exists(ConfigIniUTF8))
            {
                ImagesFolder = ReadString("SET", "ImagesFolder", "", ConfigIniUTF8);
                txtMarkContent = ReadString("SET", "txtMarkContent", "", ConfigIniUTF8);
                imgMarkPath = ReadString("SET", "imgMarkPath", "", ConfigIniUTF8);
            }
            else
            {
                WriteString("SET", "ImagesFolder", ImagesFolder, ConfigIniUTF8);  //图片保存路径
                WriteString("SET", "txtMarkContent", txtMarkContent, ConfigIniUTF8);    
                WriteString("SET", "imgMarkPath", imgMarkPath, ConfigIniUTF8);
            }
            if (File.Exists(ConfigIniPath))
            {
                int iRest;
                StringBuilder Str = new StringBuilder(256);

                //iRest = GetPrivateProfileString("SET", "ImagesFolder", "", Str, 256, ConfigIniPath);
                //if (iRest == 0) WritePrivateProfileString("SET", "ImagesFolder", ImagesFolder, ConfigIniPath);
                //else ImagesFolder = Str.ToString();

                iRest = GetPrivateProfileString("SET", "JpgQuality", "", Str, 256, ConfigIniPath);
                if (iRest == 0) WritePrivateProfileString("SET", "JpgQuality", Convert.ToString(JpgQuality), ConfigIniPath);
                else JpgQuality = Convert.ToInt32(Str.ToString());

                iRest = GetPrivateProfileString("SET", "FileFormat", "", Str, 256, ConfigIniPath);
                if (iRest == 0) WritePrivateProfileString("SET", "FileFormat", Convert.ToString(FileFormat), ConfigIniPath);
                else FileFormat = Convert.ToInt32(Str.ToString());

                iRest = GetPrivateProfileString("SET", "CutType", "", Str, 256, ConfigIniPath);
                if (iRest == 0) WritePrivateProfileString("SET", "CutType", Convert.ToString(CutType), ConfigIniPath);
                else CutType = Convert.ToInt32(Str.ToString());

                iRest = GetPrivateProfileString("SET", "ColorType", "", Str, 256, ConfigIniPath);
                if (iRest == 0) WritePrivateProfileString("SET", "ColorType", Convert.ToString(ColorType), ConfigIniPath);
                else ColorType = Convert.ToInt32(Str.ToString());

                iRest = GetPrivateProfileString("SET", "isDelBlackEdge", "", Str, 256, ConfigIniPath);
                if (iRest == 0) WritePrivateProfileString("SET", "isDelBlackEdge", Convert.ToString(isDelBlackEdge), ConfigIniPath);
                else isDelBlackEdge = Convert.ToInt32(Str.ToString());

                iRest = GetPrivateProfileString("SET", "isDelBgColor", "", Str, 256, ConfigIniPath);
                if (iRest == 0) WritePrivateProfileString("SET", "isDelBgColor", Convert.ToString(isDelBgColor), ConfigIniPath);
                else isDelBgColor = Convert.ToInt32(Str.ToString());

                iRest = GetPrivateProfileString("SET", "isDelShade", "", Str, 256, ConfigIniPath);
                if (iRest == 0) WritePrivateProfileString("SET", "isDelShade", Convert.ToString(isDelShade), ConfigIniPath);
                else isDelShade = Convert.ToInt32(Str.ToString());

                iRest = GetPrivateProfileString("SET", "isDelGrayBg", "", Str, 256, ConfigIniPath);
                if (iRest == 0) WritePrivateProfileString("SET", "isDelGrayBg", Convert.ToString(isDelGrayBg), ConfigIniPath);
                else isDelGrayBg = Convert.ToInt32(Str.ToString());

                iRest = GetPrivateProfileString("SET", "NameMode", "", Str, 256, ConfigIniPath);
                if (iRest == 0) WritePrivateProfileString("SET", "NameMode", Convert.ToString(NameMode), ConfigIniPath);
                else NameMode = Convert.ToInt32(Str.ToString());
                if (NameMode > 5) NameMode = 0;

                iRest = GetPrivateProfileString("SET", "PrefixNmae", "", Str, 256, ConfigIniPath);
                if (iRest == 0) WritePrivateProfileString("SET", "PrefixNmae", PrefixNmae, ConfigIniPath);
                else PrefixNmae = Str.ToString();

                iRest = GetPrivateProfileString("SET", "SuffixLength", "", Str, 256, ConfigIniPath);
                if (iRest == 0) WritePrivateProfileString("SET", "SuffixLength", Convert.ToString(SuffixLength), ConfigIniPath);
                else SuffixLength = Convert.ToInt32(Str.ToString());

                iRest = GetPrivateProfileString("SET", "SuffixCount", "", Str, 256, ConfigIniPath);
                if (iRest == 0) WritePrivateProfileString("SET", "SuffixCount", Convert.ToString(SuffixCount), ConfigIniPath);
                else SuffixCount = Convert.ToInt32(Str.ToString());

                iRest = GetPrivateProfileString("SET", "isScanVoice", "", Str, 256, ConfigIniPath);
                if (iRest == 0) WritePrivateProfileString("SET", "isScanVoice", Convert.ToString(isScanVoice), ConfigIniPath);
                else isScanVoice = Convert.ToInt32(Str.ToString());

                iRest = GetPrivateProfileString("SET", "isSavePdfSoure", "", Str, 256, ConfigIniPath);
                if (iRest == 0) WritePrivateProfileString("SET", "isSavePdfSoure", Convert.ToString(isSavePdfSoure), ConfigIniPath);
                else isSavePdfSoure = Convert.ToInt32(Str.ToString());

                iRest = GetPrivateProfileString("SET", "isSaveJoinSoure", "", Str, 256, ConfigIniPath);
                if (iRest == 0) WritePrivateProfileString("SET", "isSaveJoinSoure", Convert.ToString(isSaveJoinSoure), ConfigIniPath);
                else isSaveJoinSoure = Convert.ToInt32(Str.ToString());

                iRest = GetPrivateProfileString("SET", "PrintType", "", Str, 256, ConfigIniPath);
                if (iRest == 0) WritePrivateProfileString("SET", "PrintType", Convert.ToString(PrintType), ConfigIniPath);
                else PrintType = Convert.ToInt32(Str.ToString());

                iRest = GetPrivateProfileString("SET", "MarkType", "", Str, 256, ConfigIniPath);
                if (iRest == 0) WritePrivateProfileString("SET", "MarkType", Convert.ToString(MarkType), ConfigIniPath);
                else MarkType = Convert.ToInt32(Str.ToString());

                iRest = GetPrivateProfileString("SET", "isAddMark", "", Str, 256, ConfigIniPath);
                if (iRest == 0) WritePrivateProfileString("SET", "isAddMark", Convert.ToString(isAddMark), ConfigIniPath);
                else isAddMark = Convert.ToInt32(Str.ToString());

                iRest = GetPrivateProfileString("SET", "isAddTimeMark", "", Str, 256, ConfigIniPath);
                if (iRest == 0) WritePrivateProfileString("SET", "isAddTimeMark", Convert.ToString(isAddTimeMark), ConfigIniPath);
                else isAddTimeMark = Convert.ToInt32(Str.ToString());

                //iRest = GetPrivateProfileString("SET", "txtMarkContent", "", Str, 256, ConfigIniPath);
                //if (iRest == 0) WritePrivateProfileString("SET", "txtMarkContent", txtMarkContent, ConfigIniPath);
                //else txtMarkContent = Str.ToString();

                iRest = GetPrivateProfileString("SET", "txtMarkFontSize", "", Str, 256, ConfigIniPath);
                if (iRest == 0) WritePrivateProfileString("SET", "txtMarkFontSize", Convert.ToString(txtMarkFontSize), ConfigIniPath);
                else txtMarkFontSize = Convert.ToInt32(Str.ToString());

                iRest = GetPrivateProfileString("SET", "txtMarkFontType", "", Str, 256, ConfigIniPath);
                if (iRest == 0) WritePrivateProfileString("SET", "txtMarkFontType", Convert.ToString(txtMarkFontType), ConfigIniPath);
                else txtMarkFontType = Convert.ToInt32(Str.ToString());

                iRest = GetPrivateProfileString("SET", "txtMarkFontName", "", Str, 256, ConfigIniPath);
                if (iRest == 0) WritePrivateProfileString("SET", "txtMarkFontName", txtMarkFontName, ConfigIniPath);
                else txtMarkFontName = Str.ToString();

                iRest = GetPrivateProfileString("SET", "txtMarkColor", "", Str, 256, ConfigIniPath);
                if (iRest == 0) WritePrivateProfileString("SET", "txtMarkColor", Convert.ToString(txtMarkColor), ConfigIniPath);
                else txtMarkColor = Convert.ToInt32(Str.ToString());

                iRest = GetPrivateProfileString("SET", "txtMarkTrans", "", Str, 256, ConfigIniPath);
                if (iRest == 0) WritePrivateProfileString("SET", "txtMarkTrans", Convert.ToString(txtMarkTrans), ConfigIniPath);
                else txtMarkTrans = Convert.ToInt32(Str.ToString());

                iRest = GetPrivateProfileString("SET", "txtMarkXPos", "", Str, 256, ConfigIniPath);
                if (iRest == 0) WritePrivateProfileString("SET", "txtMarkXPos", Convert.ToString(txtMarkXPos), ConfigIniPath);
                else txtMarkXPos = Convert.ToInt32(Str.ToString());

                iRest = GetPrivateProfileString("SET", "txtMarkYPos", "", Str, 256, ConfigIniPath);
                if (iRest == 0) WritePrivateProfileString("SET", "txtMarkYPos", Convert.ToString(txtMarkYPos), ConfigIniPath);
                else txtMarkYPos = Convert.ToInt32(Str.ToString());

                //iRest = GetPrivateProfileString("SET", "imgMarkPath", "", Str, 256, ConfigIniPath);
                //if (iRest == 0) WritePrivateProfileString("SET", "imgMarkPath", imgMarkPath, ConfigIniPath);
                //else imgMarkPath = Str.ToString();

                iRest = GetPrivateProfileString("SET", "imgMarkSize", "", Str, 256, ConfigIniPath);
                if (iRest == 0) WritePrivateProfileString("SET", "imgMarkSize", Convert.ToString(imgMarkSize), ConfigIniPath);
                else imgMarkSize = Convert.ToInt32(Str.ToString());

                iRest = GetPrivateProfileString("SET", "imgMarkTrans", "", Str, 256, ConfigIniPath);
                if (iRest == 0) WritePrivateProfileString("SET", "imgMarkTrans", Convert.ToString(imgMarkTrans), ConfigIniPath);
                else imgMarkTrans = Convert.ToInt32(Str.ToString());

                iRest = GetPrivateProfileString("SET", "imgMarkXPos", "", Str, 256, ConfigIniPath);
                if (iRest == 0) WritePrivateProfileString("SET", "imgMarkXPos", Convert.ToString(imgMarkXPos), ConfigIniPath);
                else imgMarkXPos = Convert.ToInt32(Str.ToString());

                iRest = GetPrivateProfileString("SET", "imgMarkYPos", "", Str, 256, ConfigIniPath);
                if (iRest == 0) WritePrivateProfileString("SET", "imgMarkYPos", Convert.ToString(imgMarkYPos), ConfigIniPath);
                else imgMarkYPos = Convert.ToInt32(Str.ToString());

                iRest = GetPrivateProfileString("SET", "Bright", "", Str, 256, ConfigIniPath);
                if (iRest == 0)
                {
                    pVideoParamObj.Bright = 10000;
                    WritePrivateProfileString("SET", "Bright", Convert.ToString(pVideoParamObj.Bright), ConfigIniPath);
                }
                else pVideoParamObj.Bright = Convert.ToInt32(Str.ToString());

                iRest = GetPrivateProfileString("SET", "Contrast", "", Str, 256, ConfigIniPath);
                if (iRest == 0)
                {
                    pVideoParamObj.Contrast = 10000;
                    WritePrivateProfileString("SET", "Contrast", Convert.ToString(pVideoParamObj.Contrast), ConfigIniPath);
                }
                else pVideoParamObj.Contrast = Convert.ToInt32(Str.ToString());

                iRest = GetPrivateProfileString("SET", "Hun", "", Str, 256, ConfigIniPath);
                if (iRest == 0)
                {
                    pVideoParamObj.Hun = 10000;
                    WritePrivateProfileString("SET", "Hun", Convert.ToString(pVideoParamObj.Hun), ConfigIniPath);
                }
                else pVideoParamObj.Hun = Convert.ToInt32(Str.ToString());

                iRest = GetPrivateProfileString("SET", "Saturation", "", Str, 256, ConfigIniPath);
                if (iRest == 0)
                {
                    pVideoParamObj.Saturation = 10000;
                    WritePrivateProfileString("SET", "Saturation", Convert.ToString(pVideoParamObj.Saturation), ConfigIniPath);
                }
                else pVideoParamObj.Saturation = Convert.ToInt32(Str.ToString());

                iRest = GetPrivateProfileString("SET", "Sharpness", "", Str, 256, ConfigIniPath);
                if (iRest == 0)
                {
                    pVideoParamObj.Sharpness = 10000;
                    WritePrivateProfileString("SET", "Sharpness", Convert.ToString(pVideoParamObj.Sharpness), ConfigIniPath);
                }
                else pVideoParamObj.Sharpness = Convert.ToInt32(Str.ToString());

                iRest = GetPrivateProfileString("SET", "Gamma", "", Str, 256, ConfigIniPath);
                if (iRest == 0)
                {
                    pVideoParamObj.Gamma = 10000;
                    WritePrivateProfileString("SET", "Gamma", Convert.ToString(pVideoParamObj.Gamma), ConfigIniPath);
                }
                else pVideoParamObj.Gamma = Convert.ToInt32(Str.ToString());

                iRest = GetPrivateProfileString("SET", "BacklightCs", "", Str, 256, ConfigIniPath);
                if (iRest == 0)
                {
                    pVideoParamObj.BacklightCs = 10000;
                    WritePrivateProfileString("SET", "BacklightCs", Convert.ToString(pVideoParamObj.BacklightCs), ConfigIniPath);
                }
                else pVideoParamObj.BacklightCs = Convert.ToInt32(Str.ToString());

                iRest = GetPrivateProfileString("SET", "Gain", "", Str, 256, ConfigIniPath);
                if (iRest == 0)
                {
                    pVideoParamObj.Gain = 10000;
                    WritePrivateProfileString("SET", "Gain", Convert.ToString(pVideoParamObj.Gain), ConfigIniPath);
                }
                else pVideoParamObj.Gain = Convert.ToInt32(Str.ToString());

                iRest = GetPrivateProfileString("SET", "Exposure", "", Str, 256, ConfigIniPath);
                if (iRest == 0)
                {
                    pVideoParamObj.Exposure = 10000;
                    WritePrivateProfileString("SET", "Exposure", Convert.ToString(pVideoParamObj.Exposure), ConfigIniPath);
                }
                else
                    pVideoParamObj.Exposure = Convert.ToInt32(Str.ToString());

                iRest = GetPrivateProfileString("SET", "isAutoExp", "", Str, 256, ConfigIniPath);
                if (iRest == 0)
                {
                    pVideoParamObj.isAutoExp = 1;
                    WritePrivateProfileString("SET", "isAutoExp", Convert.ToString(pVideoParamObj.isAutoExp), ConfigIniPath);
                }
                else pVideoParamObj.isAutoExp = Convert.ToInt32(Str.ToString());


                iRest = GetPrivateProfileString("SET", "Balance", "", Str, 256, ConfigIniPath);
                if (iRest == 0)
                {
                    pVideoParamObj.Balance = 100000;
                    WritePrivateProfileString("SET", "Balance", Convert.ToString(pVideoParamObj.Balance), ConfigIniPath);
                }
                else pVideoParamObj.Balance = Convert.ToInt32(Str.ToString());

                iRest = GetPrivateProfileString("SET", "isAutoBalance", "", Str, 256, ConfigIniPath);
                if (iRest == 0)
                {
                    pVideoParamObj.isAutoBalance = 1;
                    WritePrivateProfileString("SET", "isAutoBalance", Convert.ToString(pVideoParamObj.isAutoBalance), ConfigIniPath);
                }
                else pVideoParamObj.isAutoBalance = Convert.ToInt32(Str.ToString());


                iRest = GetPrivateProfileString("SET", "isTakeSlaveCamImg", "", Str, 256, ConfigIniPath);
                if (iRest == 0)
                {
                    isTakeSlaveCamImg = 1;
                    WritePrivateProfileString("SET", "isTakeSlaveCamImg", Convert.ToString(isTakeSlaveCamImg), ConfigIniPath);
                }
                else isTakeSlaveCamImg = Convert.ToInt32(Str.ToString());

                iRest = GetPrivateProfileString("SET", "isJoinMainCam", "", Str, 256, ConfigIniPath);
                if (iRest == 0)
                {
                    isJoinMainCam = 0;
                    WritePrivateProfileString("SET", "isJoinMainCam", Convert.ToString(isJoinMainCam), ConfigIniPath);
                }
                else isJoinMainCam = Convert.ToInt32(Str.ToString());

                iRest = GetPrivateProfileString("SET", "SlaveShowSize", "", Str, 256, ConfigIniPath);
                if (iRest == 0)
                {
                    SlaveShowSize = 1;
                    WritePrivateProfileString("SET", "SlaveShowSize", Convert.ToString(SlaveShowSize), ConfigIniPath);
                }
                else SlaveShowSize = Convert.ToInt32(Str.ToString());

                iRest = GetPrivateProfileString("SET", "JoinPosition", "", Str, 256, ConfigIniPath);
                if (iRest == 0)
                {
                    JoinPosition = 0;
                    WritePrivateProfileString("SET", "JoinPosition", Convert.ToString(JoinPosition), ConfigIniPath);
                }
                else JoinPosition = Convert.ToInt32(Str.ToString());

                iRest = GetPrivateProfileString("SET", "pRecordType", "", Str, 256, ConfigIniPath);
                if (iRest == 0)
                {
                    pRecordType = 0;
                    WritePrivateProfileString("SET", "pRecordType", Convert.ToString(pRecordType), ConfigIniPath);
                }
                else pRecordType = Convert.ToInt32(Str.ToString());

                iRest = GetPrivateProfileString("SET", "pRecordFormat", "", Str, 256, ConfigIniPath);
                if (iRest == 0)
                {
                    pRecordFormat = 0;
                    WritePrivateProfileString("SET", "pRecordFormat", Convert.ToString(pRecordFormat), ConfigIniPath);
                }
                else pRecordFormat = Convert.ToInt32(Str.ToString());

                iRest = GetPrivateProfileString("SET", "pRecordFps", "", Str, 256, ConfigIniPath);
                if (iRest == 0)
                {
                    pRecordFps = 1;
                    WritePrivateProfileString("SET", "pRecordFps", Convert.ToString(pRecordFps), ConfigIniPath);
                }
                else pRecordFps = Convert.ToInt32(Str.ToString());


                iRest = GetPrivateProfileString("SET", "DpiType", "", Str, 256, ConfigIniPath);
                if (iRest == 0)
                {
                    DpiType = 0;
                    WritePrivateProfileString("SET", "DpiType", Convert.ToString(DpiType), ConfigIniPath);
                }
                else DpiType = Convert.ToInt32(Str.ToString());

                iRest = GetPrivateProfileString("SET", "DpiVal", "", Str, 256, ConfigIniPath);
                if (iRest == 0)
                {
                    DpiVal = 300;
                    WritePrivateProfileString("SET", "DpiVal", Convert.ToString(DpiVal), ConfigIniPath);
                }
                else DpiVal = Convert.ToInt32(Str.ToString());


                iRest = GetPrivateProfileString("SET", "pLangusge", "", Str, 256, ConfigIniPath);
                if (iRest == 0)
                {
                    pLangusge = 0;
                    WritePrivateProfileString("SET", "pLangusge", Convert.ToString(pLangusge), ConfigIniPath);
                }
                else pLangusge = Convert.ToInt32(Str.ToString());

                //if (pLangusge == 1)
                //    global.pEncodType = "big5";
                //else
                //    global.pEncodType = "utf-8";

                //pLangusge = 2;  //英语界面


                if (global.IncreaseStep == 1)
                    global.PreviousSuffixCount = global.SuffixCount - 1;
                else
                    global.PreviousSuffixCount = global.SuffixCount - 2;
                if (global.PreviousSuffixCount < 0)
                    global.PreviousSuffixCount = 0;

                iRest = GetPrivateProfileString("SET", "Resolution", "", Str, 256, ConfigIniPath);
                if (iRest == 0) WritePrivateProfileString("SET", "Resolution", OpenResolutionStr, ConfigIniPath);
                else OpenResolutionStr = Str.ToString();

                iRest = GetPrivateProfileString("SET", "StillResolution", "", Str, 256, ConfigIniPath);
                if (iRest == 0) WritePrivateProfileString("SET", "StillResolution", OpenStillResolutionStr, ConfigIniPath);
                else OpenStillResolutionStr = Str.ToString();

            }
            else
            {

                //string Ci = System.Threading.Thread.CurrentThread.CurrentCulture.Name;
                //if (Ci == "zh-CN")
                //{
                //    MessageBox.Show("简体中文！");
                //}
                //else
                //{
                //    if (Ci == "Zh-TW")
                //    {
                //        MessageBox.Show("繁体中文！");
                //    }
                //    else
                //    {
                //        MessageBox.Show("繁体中文！");
                //    }
                //}   

                //WritePrivateProfileString("SET", "ImagesFolder", ImagesFolder, ConfigIniPath);  //图片保存路径
                WritePrivateProfileString("SET", "JpgQuality", Convert.ToString(JpgQuality), ConfigIniPath);  //图片质量
                WritePrivateProfileString("SET", "FileFormat", Convert.ToString(FileFormat), ConfigIniPath);  //文件格式
                WritePrivateProfileString("SET", "CutType", Convert.ToString(CutType), ConfigIniPath);       //裁切方式
                WritePrivateProfileString("SET", "ColorType", Convert.ToString(ColorType), ConfigIniPath);       //色彩模式
                WritePrivateProfileString("SET", "isDelBlackEdge", Convert.ToString(isDelBlackEdge), ConfigIniPath);       //是否去黑边
                WritePrivateProfileString("SET", "isDelBgColor", Convert.ToString(isDelBgColor), ConfigIniPath);       //是否去底色
                WritePrivateProfileString("SET", "isDelShade", Convert.ToString(isDelShade), ConfigIniPath);       //是否去阴影
                WritePrivateProfileString("SET", "isDelGrayBg", Convert.ToString(isDelGrayBg), ConfigIniPath);       //是否去灰底
                WritePrivateProfileString("SET", "NameMode", Convert.ToString(NameMode), ConfigIniPath);       //命名方式
                WritePrivateProfileString("SET", "PrefixNmae", PrefixNmae, ConfigIniPath);       //命名前缀
                WritePrivateProfileString("SET", "SuffixCount", Convert.ToString(SuffixCount), ConfigIniPath);       //命名计数
                WritePrivateProfileString("SET", "SuffixLength", Convert.ToString(SuffixLength), ConfigIniPath);
                WritePrivateProfileString("SET", "isScanVoice", Convert.ToString(isScanVoice), ConfigIniPath);       //拍照声音
                WritePrivateProfileString("SET", "isSavePdfSoure", Convert.ToString(isSavePdfSoure), ConfigIniPath);       //多页PDF保存源图像
                WritePrivateProfileString("SET", "isSaveJoinSoure", Convert.ToString(isSaveJoinSoure), ConfigIniPath);       //图像合并保存源图像
                WritePrivateProfileString("SET", "PrintType", Convert.ToString(PrintType), ConfigIniPath);
                WritePrivateProfileString("SET", "isTakeSlaveCamImg", Convert.ToString(isTakeSlaveCamImg), ConfigIniPath);
                WritePrivateProfileString("SET", "isJoinMainCam", Convert.ToString(isJoinMainCam), ConfigIniPath);
                WritePrivateProfileString("SET", "SlaveShowSize", Convert.ToString(SlaveShowSize), ConfigIniPath);
                WritePrivateProfileString("SET", "JoinPosition", Convert.ToString(JoinPosition), ConfigIniPath);
                WritePrivateProfileString("SET", "pRecordType", Convert.ToString(pRecordType), ConfigIniPath);
                WritePrivateProfileString("SET", "pRecordFormat", Convert.ToString(pRecordFormat), ConfigIniPath);
                WritePrivateProfileString("SET", "pRecordFps", Convert.ToString(pRecordFps), ConfigIniPath);
                WritePrivateProfileString("SET", "pLangusge", Convert.ToString(pLangusge), ConfigIniPath);
                WritePrivateProfileString("SET", "DpiType", Convert.ToString(DpiType), ConfigIniPath);
                WritePrivateProfileString("SET", "DpiVal", Convert.ToString(DpiVal), ConfigIniPath);

                WritePrivateProfileString("SET", "MarkType", Convert.ToString(MarkType), ConfigIniPath);
                WritePrivateProfileString("SET", "isAddMark", Convert.ToString(isAddMark), ConfigIniPath);
                WritePrivateProfileString("SET", "isAddTimeMark", Convert.ToString(isAddTimeMark), ConfigIniPath);
                //WritePrivateProfileString("SET", "txtMarkContent", txtMarkContent, ConfigIniPath);
                WritePrivateProfileString("SET", "txtMarkFontSize", Convert.ToString(txtMarkFontSize), ConfigIniPath);
                WritePrivateProfileString("SET", "txtMarkFontType", Convert.ToString(txtMarkFontType), ConfigIniPath);
                WritePrivateProfileString("SET", "txtMarkFontName", txtMarkFontName, ConfigIniPath);
                WritePrivateProfileString("SET", "txtMarkColor", Convert.ToString(txtMarkColor), ConfigIniPath);
                WritePrivateProfileString("SET", "txtMarkTrans", Convert.ToString(txtMarkTrans), ConfigIniPath);
                WritePrivateProfileString("SET", "txtMarkXPos", Convert.ToString(txtMarkXPos), ConfigIniPath);
                WritePrivateProfileString("SET", "txtMarkYPos", Convert.ToString(txtMarkYPos), ConfigIniPath);
                //WritePrivateProfileString("SET", "imgMarkPath", imgMarkPath, ConfigIniPath);
                WritePrivateProfileString("SET", "imgMarkSize", Convert.ToString(imgMarkSize), ConfigIniPath);
                WritePrivateProfileString("SET", "imgMarkTrans", Convert.ToString(imgMarkTrans), ConfigIniPath);
                WritePrivateProfileString("SET", "imgMarkXPos", Convert.ToString(imgMarkXPos), ConfigIniPath);
                WritePrivateProfileString("SET", "imgMarkYPos", Convert.ToString(imgMarkYPos), ConfigIniPath);

                pVideoParamObj.Bright = 10000;
                pVideoParamObj.Contrast = 10000;
                pVideoParamObj.Hun = 10000;
                pVideoParamObj.Saturation = 10000;
                pVideoParamObj.Sharpness = 10000;
                pVideoParamObj.Gamma = 10000;
                pVideoParamObj.BacklightCs = 10000;
                pVideoParamObj.Gain = 10000;
                pVideoParamObj.Exposure = 10000;
                pVideoParamObj.isAutoExp = 1;
                WritePrivateProfileString("SET", "Bright", Convert.ToString(pVideoParamObj.Bright), ConfigIniPath);
                WritePrivateProfileString("SET", "Contrast", Convert.ToString(pVideoParamObj.Contrast), ConfigIniPath);
                WritePrivateProfileString("SET", "Hun", Convert.ToString(pVideoParamObj.Hun), ConfigIniPath);
                WritePrivateProfileString("SET", "Saturation", Convert.ToString(pVideoParamObj.Saturation), ConfigIniPath);
                WritePrivateProfileString("SET", "Sharpness", Convert.ToString(pVideoParamObj.Sharpness), ConfigIniPath);
                WritePrivateProfileString("SET", "Gamma", Convert.ToString(pVideoParamObj.Gamma), ConfigIniPath);
                WritePrivateProfileString("SET", "BacklightCs", Convert.ToString(pVideoParamObj.BacklightCs), ConfigIniPath);
                WritePrivateProfileString("SET", "Gain", Convert.ToString(pVideoParamObj.Gain), ConfigIniPath);
                WritePrivateProfileString("SET", "Exposure", Convert.ToString(pVideoParamObj.Exposure), ConfigIniPath);
                WritePrivateProfileString("SET", "isAutoExp", Convert.ToString(pVideoParamObj.isAutoExp), ConfigIniPath);
                WritePrivateProfileString("SET", "Balance", Convert.ToString(pVideoParamObj.Balance), ConfigIniPath);
                WritePrivateProfileString("SET", "isAutoBalance", Convert.ToString(pVideoParamObj.isAutoBalance), ConfigIniPath);
                WritePrivateProfileString("SET", "Resolution", OpenResolutionStr, ConfigIniPath);
                WritePrivateProfileString("SET", "StillResolution", OpenStillResolutionStr, ConfigIniPath);
            }
            return 0;
        }

        #endregion

        #region "保存设置参数"
        public static int WriteConfigPramas()
        {
            //WritePrivateProfileString("SET", "ImagesFolder", ImagesFolder, ConfigIniPath);  //图片保存路径
            WriteString("SET", "ImagesFolder", ImagesFolder, ConfigIniUTF8);  //图片保存路径
            WritePrivateProfileString("SET", "JpgQuality", Convert.ToString(JpgQuality), ConfigIniPath);  //图片质量
            WritePrivateProfileString("SET", "FileFormat", Convert.ToString(FileFormat), ConfigIniPath);  //文件格式
            WritePrivateProfileString("SET", "CutType", Convert.ToString(CutType), ConfigIniPath);       //裁切方式
            WritePrivateProfileString("SET", "ColorType", Convert.ToString(ColorType), ConfigIniPath);       //色彩模式
            WritePrivateProfileString("SET", "isDelBlackEdge", Convert.ToString(isDelBlackEdge), ConfigIniPath);       //是否去黑边
            WritePrivateProfileString("SET", "isDelBgColor", Convert.ToString(isDelBgColor), ConfigIniPath);       //是否去底色
            WritePrivateProfileString("SET", "isDelShade", Convert.ToString(isDelShade), ConfigIniPath);       //是否去阴影
            WritePrivateProfileString("SET", "isDelGrayBg", Convert.ToString(isDelGrayBg), ConfigIniPath);       //是否去灰底
            WritePrivateProfileString("SET", "NameMode", Convert.ToString(NameMode), ConfigIniPath);       //命名方式
            WritePrivateProfileString("SET", "PrefixNmae", PrefixNmae, ConfigIniPath);       //命名前缀
            WritePrivateProfileString("SET", "SuffixCount", Convert.ToString(SuffixCount), ConfigIniPath);       //命名计数
            WritePrivateProfileString("SET", "SuffixLength", Convert.ToString(SuffixLength), ConfigIniPath);
            WritePrivateProfileString("SET", "isScanVoice", Convert.ToString(isScanVoice), ConfigIniPath);       //拍照声音
            WritePrivateProfileString("SET", "isSavePdfSoure", Convert.ToString(isSavePdfSoure), ConfigIniPath);       //多页PDF保存源图像
            WritePrivateProfileString("SET", "isSaveJoinSoure", Convert.ToString(isSaveJoinSoure), ConfigIniPath);       //图像合并保存源图像
            WritePrivateProfileString("SET", "PrintType", Convert.ToString(PrintType), ConfigIniPath);
            WritePrivateProfileString("SET", "isTakeSlaveCamImg", Convert.ToString(isTakeSlaveCamImg), ConfigIniPath);
            WritePrivateProfileString("SET", "isJoinMainCam", Convert.ToString(isJoinMainCam), ConfigIniPath);
            WritePrivateProfileString("SET", "SlaveShowSize", Convert.ToString(SlaveShowSize), ConfigIniPath);
            WritePrivateProfileString("SET", "JoinPosition", Convert.ToString(JoinPosition), ConfigIniPath);
            WritePrivateProfileString("SET", "pRecordType", Convert.ToString(pRecordType), ConfigIniPath);
            WritePrivateProfileString("SET", "pRecordFormat", Convert.ToString(pRecordFormat), ConfigIniPath);
            WritePrivateProfileString("SET", "pRecordFps", Convert.ToString(pRecordFps), ConfigIniPath);
            WritePrivateProfileString("SET", "pLangusge", Convert.ToString(pLangusge), ConfigIniPath);
            WritePrivateProfileString("SET", "DpiType", Convert.ToString(DpiType), ConfigIniPath);
            WritePrivateProfileString("SET", "DpiVal", Convert.ToString(DpiVal), ConfigIniPath);

            WritePrivateProfileString("SET", "MarkType", Convert.ToString(MarkType), ConfigIniPath);
            WritePrivateProfileString("SET", "isAddMark", Convert.ToString(isAddMark), ConfigIniPath);
            WritePrivateProfileString("SET", "isAddTimeMark", Convert.ToString(isAddTimeMark), ConfigIniPath);
            //WritePrivateProfileString("SET", "txtMarkContent", txtMarkContent, ConfigIniPath);
            WriteString("SET", "txtMarkContent", txtMarkContent, ConfigIniUTF8);
            WritePrivateProfileString("SET", "txtMarkFontSize", Convert.ToString(txtMarkFontSize), ConfigIniPath);
            WritePrivateProfileString("SET", "txtMarkFontType", Convert.ToString(txtMarkFontType), ConfigIniPath);
            WritePrivateProfileString("SET", "txtMarkFontName", txtMarkFontName, ConfigIniPath);
            WritePrivateProfileString("SET", "txtMarkColor", Convert.ToString(txtMarkColor), ConfigIniPath);
            WritePrivateProfileString("SET", "txtMarkTrans", Convert.ToString(txtMarkTrans), ConfigIniPath);
            WritePrivateProfileString("SET", "txtMarkXPos", Convert.ToString(txtMarkXPos), ConfigIniPath);
            WritePrivateProfileString("SET", "txtMarkYPos", Convert.ToString(txtMarkYPos), ConfigIniPath);
            //WritePrivateProfileString("SET", "imgMarkPath", imgMarkPath, ConfigIniPath);
            WriteString("SET", "imgMarkPath", imgMarkPath, ConfigIniUTF8);
            WritePrivateProfileString("SET", "imgMarkSize", Convert.ToString(imgMarkSize), ConfigIniPath);
            WritePrivateProfileString("SET", "imgMarkTrans", Convert.ToString(imgMarkTrans), ConfigIniPath);
            WritePrivateProfileString("SET", "imgMarkXPos", Convert.ToString(imgMarkXPos), ConfigIniPath);
            WritePrivateProfileString("SET", "imgMarkYPos", Convert.ToString(imgMarkYPos), ConfigIniPath);

            WritePrivateProfileString("SET", "Bright", Convert.ToString(pVideoParamObj.Bright), ConfigIniPath);
            WritePrivateProfileString("SET", "Contrast", Convert.ToString(pVideoParamObj.Contrast), ConfigIniPath);
            WritePrivateProfileString("SET", "Hun", Convert.ToString(pVideoParamObj.Hun), ConfigIniPath);
            WritePrivateProfileString("SET", "Saturation", Convert.ToString(pVideoParamObj.Saturation), ConfigIniPath);
            WritePrivateProfileString("SET", "Sharpness", Convert.ToString(pVideoParamObj.Sharpness), ConfigIniPath);
            WritePrivateProfileString("SET", "Gamma", Convert.ToString(pVideoParamObj.Gamma), ConfigIniPath);
            WritePrivateProfileString("SET", "BacklightCs", Convert.ToString(pVideoParamObj.BacklightCs), ConfigIniPath);
            WritePrivateProfileString("SET", "Gain", Convert.ToString(pVideoParamObj.Gain), ConfigIniPath);
            WritePrivateProfileString("SET", "Exposure", Convert.ToString(pVideoParamObj.Exposure), ConfigIniPath);
            WritePrivateProfileString("SET", "isAutoExp", Convert.ToString(pVideoParamObj.isAutoExp), ConfigIniPath);
            WritePrivateProfileString("SET", "Balance", Convert.ToString(pVideoParamObj.Balance), ConfigIniPath);
            WritePrivateProfileString("SET", "isAutoBalance", Convert.ToString(pVideoParamObj.isAutoBalance), ConfigIniPath);
            WritePrivateProfileString("SET", "Resolution", OpenResolutionStr, ConfigIniPath);
            WritePrivateProfileString("SET", "StillResolution", OpenStillResolutionStr, ConfigIniPath);

            return 0;
        }
        #endregion



        //************************************打开文件并预览******************************************//
        public static void OpenFileAndPreview(string path)
        {
            try
            {
                //建立新的系统进程    
                System.Diagnostics.Process process = new System.Diagnostics.Process();
                //设置图片的真实路径和文件名    
                process.StartInfo.FileName = path;
                // //设置进程运行参数，这里以最大化窗口方法显示图片。    
                // process.StartInfo.Arguments = "rundl132.exe C://WINDOWS//system32//shimgvw.dll,ImageView_Fullscreen";
                // //此项为是否使用Shell执行程序，因系统默认为true，此项也可不设，但若设置必须为true    
                // process.StartInfo.UseShellExecute = true;
                //此处可以更改进程所打开窗体的显示样式，可以不设    
                process.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Normal;
                process.Start();
                process.Close();
            }
            catch (Exception e)
            {

            }

        }


        //************************************计算后缀数目******************************************//
        public static void CalculateSuffix()
        {
            if (global.SuffixCount == 0 && global.PreviousSuffixCount == 0)
            {
            }
            else
            {
                int cval = Math.Abs(global.SuffixCount - global.PreviousSuffixCount);
                if (global.IncreaseStep == 1)
                {
                    if (cval == 2)
                        global.SuffixCount = global.PreviousSuffixCount + 1;
                }
                else
                {
                    if (cval == 1)
                        global.SuffixCount = global.SuffixCount + 1;
                }

            }
        }

        //************************************后缀命名前面补0******************************************//
        public static string SuffixSupplyZero(int scount, int length)
        {
            int CmpVal = (int)Math.Pow(10.0, length);
            if (scount > CmpVal)
            {
                return scount.ToString();
            }
            else
            {
                return scount.ToString().PadLeft(length, '0'); // 一共4位,位数不够时从左边开始用0补
            }
        }


        public static void SetWaterMarkParameters()
        {
            AddMark(isAddMark);
            if (MarkType == 0)
            {
                byte[] pContentBuf = Encoding.GetEncoding(global.pEncodType).GetBytes(global.txtMarkContent);
                byte[] pFontNameBuf = Encoding.GetEncoding(global.pEncodType).GetBytes(global.txtMarkFontName);
                int mcolor = 0xff0000;
                if (global.txtMarkColor == 0) mcolor = 0xff0000;
                if (global.txtMarkColor == 1) mcolor = 0x00ff00;
                if (global.txtMarkColor == 2) mcolor = 0x0000ff;
                if (global.txtMarkColor == 3) mcolor = 0x00ffff;
                if (global.txtMarkColor == 4) mcolor = 0xff7f00;
                if (global.txtMarkColor == 5) mcolor = 0xffff00;
                if (global.txtMarkColor == 6) mcolor = 0x000000;
                if (global.txtMarkColor == 7) mcolor = 0xffffff;
                SetMark(MarkType, pContentBuf, global.txtMarkFontType, pFontNameBuf, mcolor,
                    global.txtMarkFontSize, global.txtMarkTrans, global.txtMarkXPos, global.txtMarkYPos, global.isAddTimeMark);
            }
            else
            {
                byte[] pContentBuf = Encoding.GetEncoding(global.pEncodType).GetBytes(global.imgMarkPath);
                byte[] pFontNameBuf = Encoding.GetEncoding(global.pEncodType).GetBytes(global.txtMarkFontName);
                double msize = 1.0;
                if (global.imgMarkSize == 0) msize = 0.3;
                if (global.imgMarkSize == 1) msize = 0.5;
                if (global.imgMarkSize == 2) msize = 1.0;
                if (global.imgMarkSize == 3) msize = 1.5;
                if (global.imgMarkSize == 4) msize = 2.0;
                if (global.imgMarkSize == 5) msize = 3.0;
                SetMark(MarkType, pContentBuf, global.txtMarkFontType, pFontNameBuf, global.txtMarkColor,
                    msize, global.imgMarkTrans, global.imgMarkXPos, global.imgMarkYPos, global.isAddTimeMark);
            }
        }

        //************************************播放声音******************************************//
        public static void PlaySound()
        {
            if (isScanVoice == 0)
                return;
            string soundPath = System.Windows.Forms.Application.StartupPath + "\\shoot.wav";
            if (File.Exists(soundPath))
            {
                System.Media.SoundPlayer player = new System.Media.SoundPlayer(soundPath);
                player.Play();//简单播放一遍
                //player.PlaySync();//另起线程播放
            }
        }

        public static Boolean isWriteLog = true;
        public static void WriteMessage(string msg)
        {
            if (isWriteLog == true)
            {
                using (FileStream fs = new FileStream(@"c:\log.txt", FileMode.OpenOrCreate, FileAccess.Write))
                {
                    using (StreamWriter sw = new StreamWriter(fs))
                    {
                        sw.BaseStream.Seek(0, SeekOrigin.End);
                        sw.WriteLine("{0}\n", msg, DateTime.Now);
                        sw.Flush();
                    }
                }
            }
           
        }
    }

}
