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
using System.Text.RegularExpressions;

namespace CameraScan
{
    /// <summary>
    /// Setting.xaml 的交互逻辑
    /// </summary>
    public partial class Adjust : Window
    {

        [DllImport("DevCapture.dll", EntryPoint = "SetAdjustParms", CallingConvention = CallingConvention.Cdecl)]
        public extern static void SetAdjustParms(bool isOpen, int X, int Y, int Thickness, int AngleSnap);

        [DllImport("DevCapture.dll", EntryPoint = "SetAutoOffset", CallingConvention = CallingConvention.Cdecl)]
        public extern static void SetAutoOffset(double val);

        int countFlag = 0;

        public Adjust()
        {
            InitializeComponent();
        }

        //############界面初始化#################
        private void InitUIControl()
        {
            AdjustX.Text = Convert.ToString(global.AdjustX);
            AdjustY.Text = Convert.ToString(global.AdjustY);
            AdjustThick.Text = Convert.ToString(global.AdjustThickness);
            AdjustAngle.Text = Convert.ToString(global.AdjustAngleSnap);
            if (global.AdjustIsOpen)
                AdjustCheck.IsChecked = true;       
            else
                AdjustCheck.IsChecked = false;

            AutoCutOffset.Value = 0;
            LabelAutoCutOffset.Content = Convert.ToString(AutoCutOffset.Value);
            SetAutoOffset(AutoCutOffset.Value);
        }


        bool isSet = false;
        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            isSet = true;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            countFlag = 0;
            InitUIControl();
            ResourceDictionary langRd = null;
            try
            {
                //if (global.pLangusge == 0)
                langRd = System.Windows.Application.LoadComponent(new Uri(@"zh-cn.xaml", UriKind.Relative)) as ResourceDictionary;
                //if (global.pLangusge == 1)
                //    langRd = System.Windows.Application.LoadComponent(new Uri(@"zh-tw.xaml", UriKind.Relative)) as ResourceDictionary;
                //if (global.pLangusge == 2)
                //    langRd = System.Windows.Application.LoadComponent(new Uri(@"en-us.xaml", UriKind.Relative)) as ResourceDictionary;
                //if (global.pLangusge == 3)
                //    langRd = System.Windows.Application.LoadComponent(new Uri(@"zh-spain.xaml", UriKind.Relative)) as ResourceDictionary;
                //if (global.pLangusge == 4)
                //    langRd = System.Windows.Application.LoadComponent(new Uri(@"zh-Japan.xaml", UriKind.Relative)) as ResourceDictionary;
                //if (global.pLangusge == 5)
                //    langRd = System.Windows.Application.LoadComponent(new Uri(@"zh-Germany.xaml", UriKind.Relative)) as ResourceDictionary;
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
        }

        private void Window_Closed(object sender, EventArgs e)
        {         
            global.pAdjustDlgHaveRun = false;
            //if (SuffixTextBox.Text == "" || DpiTextBox.Text == "")
            //    return;
            //global.PrefixNmae = PrefixTextBox.Text;
            //global.SuffixLength = SuffixTextBox.Text.Length;    

            global.AdjustX = Convert.ToInt16(AdjustX.Text);
            global.AdjustY = Convert.ToInt16(AdjustY.Text);
            global.AdjustThickness = Convert.ToInt16(AdjustThick.Text);
            global.AdjustAngleSnap = Convert.ToInt16(AdjustAngle.Text);

            SetAdjustParms(global.AdjustIsOpen, global.AdjustX, global.AdjustY, global.AdjustThickness, global.AdjustAngleSnap);
            global.WriteConfigPramas();
        }

        private void ExitBt_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }



        //后缀只允许输入数字
        private void SuffixTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
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


        private  string SpecialCode(string s)
        {
            s = s.Replace(@"\", "");
            s = Regex.Replace(s, "[ \\^ \\*×~!/…<>《》|!！??？:：•`·；:;\"'‘’“”]", "").ToUpper();
            return s;
        }




       //只允许输入数字
       private void DpiTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
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

        private void AdjustUpdate_Click(object sender, RoutedEventArgs e)
        {
            global.AdjustX = Convert.ToInt16(AdjustX.Text);
            global.AdjustY = Convert.ToInt16(AdjustY.Text);
            global.AdjustThickness = Convert.ToInt16(AdjustThick.Text);
            global.AdjustAngleSnap = Convert.ToInt16(AdjustAngle.Text);

            SetAdjustParms(global.AdjustIsOpen, global.AdjustX, global.AdjustY, global.AdjustThickness, global.AdjustAngleSnap);
        }

        private void AdjustCheck_Checked(object sender, RoutedEventArgs e)
        {
            global.AdjustIsOpen = true;
        }

        private void AdjustCheck_Unchecked(object sender, RoutedEventArgs e)
        {
            global.AdjustIsOpen = false;
        }

        private void AutoCutOffset_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            SetAutoOffset(AutoCutOffset.Value);
            LabelAutoCutOffset.Content = Convert.ToString(AutoCutOffset.Value);
        }
    }
}
