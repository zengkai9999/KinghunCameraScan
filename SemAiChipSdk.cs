using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
//using System.Threading.Tasks;

namespace CameraScan
{


    /*结果处理*/
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public delegate void CbResultHandle(SdkCmdCallBack eve);


    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct SdkCmdCallBack
    {
        public int errorCode;          //错误代码：0成功，大于0为不同的错误代码，如：10000为初始化失败，10001构建语法失败，10002
        public IntPtr backType;        //回调类型：binary音频流，cmd命令，tts合成
        public IntPtr operateHandler;          //操作处理：sdk(SDK中已处理),user(需要用户自行处理)
        public IntPtr operateAction;           //操作动作：如打开，关闭，我要买，买....
        public IntPtr operateObject;           //操作对象：我的电脑，我的文档....
        public IntPtr content;             //内容或错误信息
    };



    public class SemAiChipSdk
    {


        const string DllName = "SemMscRecog.dll";


        /*构造方法*/
        [DllImport(DllName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static void MscRecog();


        /*回调识别结果，需要放在InitRecog初始化之前*/
        [DllImport(DllName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static void SetResultCallBack([Out]CbResultHandle callback);

        /*初始化*/
        [DllImport(DllName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static int InitRecog();


        /*获取当前SDK版本号*/
        [DllImport(DllName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static IntPtr SdkVersion();



        /*设置相关参数：如自定义操作项、自定义网站、自动文件档，需要放在InitRecog初始化之前*/
        /**
        * @brief 设置相关参数
        * @params Json值 {"define":[{"title":"安徽智能技术","value":"http://www.sinsunvoice.com"},{"title":"产品简介","value":"d:\\test\\product.doc"},{"title":"公司简单","value":"d:\\test\\"}],"operate":[{"object":"声讯打印机","value":"semprint"},{"object":"高拍仪","value":"semscanner"}]}
        * @return 0; 0失败，1成功
        */
        [DllImport(DllName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static int SetParams(byte[] param);



        /*结果处理程序，也可以直接在SetParams中配置，唯一区别是此方法随处可使用，比较灵活：sdk,user
        * @resHandler sdk 交给SDK处理,user 用户自行处理
        * 部分自定义部分需要用户自行处理，结果回调中operateHandler如果是sdk表示已处理，如是user表示需要用户自行处理
        */
        [DllImport(DllName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static void SetResultHandler([MarshalAs(UnmanagedType.LPArray, SizeConst = 20)]byte[] resHandler);

        /*设置系统级处理
        *系统级处理对象：我的电脑|计算机|此电脑|我的文档|控制面板|网络连接|系统设置|计算器|记事本|浏览器|设置面板|显示器|分辨率|注册表|个性化|打印机|防火墙|写字板|c盘|d盘|e盘|f盘|g盘
        * @systemHandler
        *      true  不管结果处理是sdk,还是user,系统级处理对象都交给sdk进行处理
        *      false 结果处理为user时，所有操作都由用户来处理
        */
        [DllImport(DllName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static void SetSystemHandler(bool systemHandler);

        /*设置打开浏览器的默认网址
        * @defaultUrl  需要带协议头，如：http(s)://
        */
        [DllImport(DllName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static void SetDefaultUrl(byte[] defaultUrl);
        //public extern static void SetDefaultUrl([MarshalAs(UnmanagedType.LPArray, SizeConst = 500)]byte[] defaultUrl);



        /*打开文件或文件夹：如Word，excel等
        * @path 绝对路径
        * @outmsg 返回操作信息
        * return 1成功，0失败
        */
        [DllImport(DllName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static int OpenOperateFile(byte[] path, ref byte[] outmsg);


        /*打开可执行程序：如微信、360杀毒软件等
        * @path 绝对路径
        * @outmsg 返回操作信息
        * return 1成功，0失败
        */
        [DllImport(DllName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static int OpenOperateExe(byte[] path, ref byte[] outmsg);

        /*打开浏览器*/
        [DllImport(DllName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static int OpenBrowser(byte[] url);


        /*注册鼠标滚轮键
         * regMsg 是否需要监听消息，1为需要（控制台应用需要），0不需要
         * return 1成功，0失败
         */
        [DllImport(DllName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static int RegisterMouseWheel(int regMsg=0);

        /*注销鼠标滚轮键
        * return 1成功，0失败
        */
        [DllImport(DllName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static int UnRegisterMouseWheel();




        /*开始录音：1成功，0失败*/
        [DllImport(DllName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static int StartRecord();



        /*结束录音：1成功，0失败*/
        [DllImport(DllName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static int StopRecord();



        /*销毁*/
        [DllImport(DllName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static void OnDestroy();




    }


}
