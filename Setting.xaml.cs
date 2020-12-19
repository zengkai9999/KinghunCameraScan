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
    public partial class Setting : Window
    {

        [DllImport("DevCapture.dll", EntryPoint = "SetJpgQuality", CallingConvention = CallingConvention.Cdecl)]
        public extern static void SetJpgQuality(int quality);

        int countFlag = 0;

        public Setting()
        {
            InitializeComponent();
        }

        //############界面初始化#################
        private void InitUIControl()
        {
            if (global.NameMode == 0)
                NameByTime.IsChecked = true;
            else if (global.NameMode == 1)
                NameByFolder.IsChecked = true;
            else if (global.NameMode == 2)
                NameByDIY.IsChecked = true;
            else if (global.NameMode == 3)
                NameByBarcode.IsChecked = true;
            else if (global.NameMode == 4)
                NameByQr.IsChecked = true;
            else if (global.NameMode == 5)
                NameByFixed.IsChecked = true;
            else
                NameByTime.IsChecked = true;

            if (global.PrintType == 0)
                ZoomPrint.IsChecked = true;
            if (global.PrintType == 1)
                TrueSizePrint.IsChecked = true;


            if (global.DpiType == 0)
            {
                DpiDefault.IsChecked = true;
                DpiTextBox.IsEnabled = false;
            }
            if (global.DpiType == 1)
            {
                DpiDiy.IsChecked = true;
                DpiTextBox.IsEnabled = true;
            }
            DpiTextBox.Text = Convert.ToString(global.DpiVal);   

            if (global.IncreaseStep == 1)
            {
                StepCheck.IsChecked = false;
            }         
            else
            {
                StepCheck.IsChecked = true;
            }
                

            PrefixTextBox.Text = global.PrefixNmae;

            SuffixTextBox.Text = global.SuffixSupplyZero(global.SuffixCount, global.SuffixLength);
         
            LabelJpgQuality.Content = Convert.ToString(global.JpgQuality) + "%";
            SliderJpgQuality.Value = global.JpgQuality;

            if (global.isScanVoice == 0) VoiceCheck.IsChecked = false;
            else VoiceCheck.IsChecked = true;

            LanguageCbBox.SelectedIndex = global.pLangusge;

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
        }

        private void Window_Closed(object sender, EventArgs e)
        {         
            global.pSetDlgHaveRun = false;
            //if (SuffixTextBox.Text == "" || DpiTextBox.Text == "")
            //    return;
            global.PrefixNmae = PrefixTextBox.Text;
            global.SuffixLength = SuffixTextBox.Text.Length;    
            if (SuffixTextBox.Text != "")
                global.SuffixCount = Convert.ToInt32(SuffixTextBox.Text);
            if (DpiTextBox.Text != "")
            {
                if (DpiTextBox.Text.Length > 5)
                {
                    DpiTextBox.Text = DpiTextBox.Text.Substring(0,5);
                }
                global.DpiVal = Convert.ToInt32(DpiTextBox.Text);
                if (global.DpiVal == 0)
                    global.DpiVal = 300;
            }
                  
            global.WriteConfigPramas();
        }

        private void ExitBt_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }


        private void NameByTime_Checked(object sender, RoutedEventArgs e)
        {
            if (NameByTime.IsChecked == true)
            {
                global.NameMode = 0;
                NameByFolder.IsChecked = false;
                NameByBarcode.IsChecked = false;
                NameByQr.IsChecked = false;
                NameByDIY.IsChecked = false;
                NameByFixed.IsChecked = false;
            }
                
        }

        private void NameByFolder_Checked(object sender, RoutedEventArgs e)
        {
            if (NameByFolder.IsChecked == true)
            {
                global.NameMode = 1;
                NameByTime.IsChecked = false;
                NameByBarcode.IsChecked = false;
                NameByQr.IsChecked = false;
                NameByDIY.IsChecked = false;
                NameByFixed.IsChecked = false;
            }
                
        }

        private void NameByDIY_Checked(object sender, RoutedEventArgs e)
        {
            if (NameByDIY.IsChecked == true)
            {
                global.NameMode = 2;
                NameByTime.IsChecked = false;
                NameByBarcode.IsChecked = false;
                NameByQr.IsChecked = false;
                NameByFolder.IsChecked = false;
                NameByFixed.IsChecked = false;
            }
                
        }


        private void NameByBarcode_Checked(object sender, RoutedEventArgs e)
        {
            if (NameByBarcode.IsChecked == true)
            {
                global.NameMode = 3;
                NameByTime.IsChecked = false;
                NameByDIY.IsChecked = false;
                NameByQr.IsChecked = false;
                NameByFolder.IsChecked = false;
                NameByFixed.IsChecked = false;
            }        
        }


        private void NameByQr_Checked(object sender, RoutedEventArgs e)
        {
            if (NameByQr.IsChecked == true)
            {
                global.NameMode = 4;
                NameByTime.IsChecked = false;
                NameByDIY.IsChecked = false;
                NameByBarcode.IsChecked = false;
                NameByFolder.IsChecked = false;
                NameByFixed.IsChecked = false;
            }        
        }

        private void NameByFixed_Checked(object sender, RoutedEventArgs e)
        {
            if (NameByFixed.IsChecked == true)
            {
                global.NameMode = 5;
                NameByTime.IsChecked = false;
                NameByDIY.IsChecked = false;
                NameByBarcode.IsChecked = false;
                NameByQr.IsChecked = false;
                NameByFolder.IsChecked = false;
            }        
        }


        private void VoiceCheck_Checked(object sender, RoutedEventArgs e)
        { 
            global.isScanVoice = 1;               
        }
        private void VoiceCheck_Unchecked(object sender, RoutedEventArgs e)
        {
            global.isScanVoice = 0;
        }


        private void SliderJpgQuality_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (isSet)
            {
                global.JpgQuality = (int)SliderJpgQuality.Value;
                LabelJpgQuality.Content = Convert.ToString(global.JpgQuality) + "%";
                SetJpgQuality(global.JpgQuality);
            }        
        }


        //自适应
        private void ZoomPrint_Checked(object sender, RoutedEventArgs e)
        {
            global.PrintType = 0;
        }

        //1：1
        private void TrueSizePrint_Checked(object sender, RoutedEventArgs e)
        {
            global.PrintType = 1;
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


       private void PrefixTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            int pos = PrefixTextBox.SelectionStart;
            PrefixTextBox.Text = SpecialCode(PrefixTextBox.Text);
            PrefixTextBox.SelectionStart = pos;
        }


       private void LanguageCbBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
       {
           global.pLangusge = LanguageCbBox.SelectedIndex;
           MainWindow mMainWindow = (MainWindow)this.Owner;
           mMainWindow.ChangeLanguage(global.pLangusge);
       }

       private void StepCheck_Checked(object sender, RoutedEventArgs e)
       {      
            global.IncreaseStep = 2;
        }
          

       private void StepCheck_Unchecked(object sender, RoutedEventArgs e)
       {
            global.IncreaseStep = 1;
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


       private void DpiDefault_Checked(object sender, RoutedEventArgs e)
       {
           global.DpiType = 0;
           DpiTextBox.IsEnabled = false;
       }

       private void DpiDiy_Checked(object sender, RoutedEventArgs e)
       {
           global.DpiType = 1;
           DpiTextBox.IsEnabled = true;
       }

      


    }
}
