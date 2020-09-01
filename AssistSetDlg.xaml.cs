
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
using System.Windows.Shapes;
using System.Runtime.InteropServices;

namespace CameraScan
{
    /// <summary>
    /// AssistSetDlg.xaml 的交互逻辑
    /// </summary>
    public partial class AssistSetDlg : Window
    {

        [DllImport("DevCaptureB.dll", CallingConvention = CallingConvention.Cdecl)]
        public extern static int ShowCameraSettingWindowB();

        bool isSet = false;

        public AssistSetDlg()
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

            SizeCbBox.SelectedIndex = global.SlaveShowSize;
            PositionCbBox.SelectedIndex = global.JoinPosition;
            if (global.isTakeSlaveCamImg == 1)
                SlavePhotoCheck.IsChecked = true;
            else
                SlavePhotoCheck.IsChecked = false;

            if (global.isJoinMainCam == 1)
                JoinMainCamCheck.IsChecked = true;
            else
                JoinMainCamCheck.IsChecked = false;

            if (global.isOpenCameraB)
            {
                if (global.pLSlavePreWidths.Count > 0)
                {
                    for (int i = 0; i < global.pLSlavePreWidths.Count; i++)
                    {
                        int xWidth = global.pLSlavePreWidths[i];
                        int xHeight=global.pLSlavePreHeights[i];
                        string resStr = Convert.ToString(xWidth) + "*" + Convert.ToString(xHeight);
                        SlavePreResCbBox.Items.Add(resStr);
                    }

                    #if ADDRESOLUTION
                        if (global.isSelectAddRes == true)
                        {
                            SlavePreResCbBox.SelectedIndex = global.pLSlavePreWidths.Count - 1;
                            global.pSlaveCamera.PreWidth = 640;
                            global.pSlaveCamera.PreHeight = 480;

                        }
                        else 
                        {
                            for (int i = 0; i < global.pLSlavePreWidths.Count; i++)
                            {
                                int xWidth = global.pLSlavePreWidths[i];
                                int xHeight = global.pLSlavePreHeights[i];
                                if (xWidth == global.pSlaveCamera.PreWidth && xHeight == global.pSlaveCamera.PreHeight)
                                {
                                    SlavePreResCbBox.SelectedIndex = i;
                                    break;
                                }
                            }
                        }
                    #else
                        for (int i = 0; i < global.pLSlavePreWidths.Count; i++)
                        {
                            int xWidth = global.pLSlavePreWidths[i];
                            int xHeight = global.pLSlavePreHeights[i];
                            if (xWidth == global.pSlaveCamera.PreWidth && xHeight == global.pSlaveCamera.PreHeight)
                            {
                                SlavePreResCbBox.SelectedIndex = i;
                                break;
                            }
                        }
                    #endif

                }
            }
        }

        private void SlavePhotoCheck_Checked(object sender, RoutedEventArgs e)
        {
            if (isSet == false)
                return;
            global.isTakeSlaveCamImg = 1;
        }

        private void SlavePhotoCheck_Unchecked(object sender, RoutedEventArgs e)
        {
            if (isSet == false)
                return;
            global.isTakeSlaveCamImg = 0;
        }

        private void JoinMainCamCheck_Checked(object sender, RoutedEventArgs e)
        {
            if (isSet == false)
                return;
            global.isJoinMainCam = 1;
        }

        private void JoinMainCamCheck_Unchecked(object sender, RoutedEventArgs e)
        {
            if (isSet == false)
                return;
            global.isJoinMainCam = 0;
        }

        private void SizeCbBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (isSet == false)
                return;
            global.SlaveShowSize = SizeCbBox.SelectedIndex;
            MainWindow mMainWindow = (MainWindow)this.Owner;
            mMainWindow.FN_SetSlaveImagePosition();
                
        }

        private void PositionCbBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (isSet == false)
                return;
            global.JoinPosition = PositionCbBox.SelectedIndex;
        }


        private void SlavePreResCbBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (isSet == false)
                return;

            string resStr = SlavePreResCbBox.SelectedItem.ToString();
            int pos = resStr.LastIndexOf('*');
            int xWidth = Convert.ToInt32(resStr.Substring(0, pos));
            int xHeight = Convert.ToInt32(resStr.Substring(pos + 1, resStr.Length - pos - 1));


            #if ADDRESOLUTION
            if (xWidth == 358 && xHeight == 441)
            {
                global.isSelectAddRes = true;
                xWidth = 640;
                xHeight = 480;
            }
            else
            {
                global.isSelectAddRes = false;
            }
            #endif
           

            if (xWidth == global.pSlaveCamera.PreWidth && xHeight == global.pSlaveCamera.PreHeight)
            {
                return;
            }
            else 
            {
                global.pSlaveCamera.PreWidth = xWidth;
                global.pSlaveCamera.PreHeight = xHeight;
                MainWindow mMainWindow = (MainWindow)this.Owner;
                mMainWindow.toOpenSlaveCamera();        
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (global.isOpenCameraB)
                ShowCameraSettingWindowB();
        }

        private void ExitBt_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            global.pAssistSetDlgHaveRun = false;
        }


    }
}
