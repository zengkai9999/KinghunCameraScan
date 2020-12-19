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
using System.Windows.Markup;
using System.Runtime.InteropServices;

namespace CameraScan
{
    /// <summary>
    /// MarkDlg.xaml 的交互逻辑
    /// </summary>
    public partial class MarkDlg : Window
    {

        [DllImport("DevCapture.dll", CallingConvention = CallingConvention.Cdecl)]
        public extern static int AddMark(int isAdd);

        [DllImport("DevCapture.dll", CallingConvention = CallingConvention.Cdecl)]
        public extern static int SetMark(int type, byte[] markContent, int fontType, byte[] fontName, int fontColor, double size, int trans, int xPos, int yPos, int isAddTimeMark);

        bool isSet = false;
        int type = 0;

        public MarkDlg()
        {
            InitializeComponent();
        }

    #region  获取系统字体
        void GetSystemFonts()
        {
            foreach (FontFamily fontfamily in Fonts.SystemFontFamilies)
            {
                LanguageSpecificStringDictionary fontdics = fontfamily.FamilyNames;
                //判断该字体是不是中文字体
                if (fontdics.ContainsKey(XmlLanguage.GetLanguage("zh-cn")))
                {
                    string fontfamilyname = null;
                    if (fontdics.TryGetValue(XmlLanguage.GetLanguage("zh-cn"), out fontfamilyname))
                    {
                        textMarkFont.Items.Add(fontfamilyname);
                    }
                }
                //英文字体
                else
                {
                    string fontfamilyname = null;
                    if (fontdics.TryGetValue(XmlLanguage.GetLanguage("en-us"), out fontfamilyname))
                    {
                        //textMarkFont.Items.Add(fontfamilyname);
                    }
                }
            }

            if (textMarkFont.Items.Count > 0)
            {
                for (int i = 0; i < textMarkFont.Items.Count; i++)
                {
                    if (textMarkFont.Items[i].ToString() == global.txtMarkFontName)
                    {
                        textMarkFont.SelectedIndex = i;
                        break;
                    }
                }
            }
        }
    #endregion


        private void SetWaterMarkParams()
        {
            global.txtMarkContent = txtMarkContent.Text;
            global.imgMarkPath = imgMarkPath.Text;
            global.SetWaterMarkParameters();
        }



        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            isSet = true;
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            global.pMarkDlgHaveRun = false ;
            SetWaterMarkParams();   
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

            MarkTypeCbBox.SelectedIndex = global.MarkType;

            if (global.isAddMark == 0)
                AddMarkCheck.IsChecked=false;
            else
                AddMarkCheck.IsChecked = true;

            if (global.isAddTimeMark == 0)
                AddTimeMarkCheck.IsChecked = false;
            else
                AddTimeMarkCheck.IsChecked = true;

            txtMarkContent.Text = global.txtMarkContent;
            for (int i = 0; i < 40; i++)
            {
                int val = 10 + i * 5;
                textMarkSize.Items.Add(val.ToString());
            }
            for (int i = 0; i < 40; i++)
            {
                if (textMarkSize.Items[i].ToString() == global.txtMarkFontSize.ToString())
                {
                    textMarkSize.SelectedIndex = i;
                    break;
                }
            }
            GetSystemFonts(); //获取系统字体
            txtMarkContent.Text = global.txtMarkContent ;
            textMarkFtype.SelectedIndex = global.txtMarkFontType;
            textMarkColor.SelectedIndex = global.txtMarkColor;
            textMarkTrans.Value = global.txtMarkTrans;
            double transVal = (double)global.txtMarkTrans / 255;
            int valTrans = (int)(transVal * 100);
            textTransLabel.Content = valTrans.ToString() + "%";
            textMarkXpos.Value = global.txtMarkXPos;
            textXposLabel.Content = global.txtMarkXPos.ToString() + "%";
            textMarkYpos.Value = global.txtMarkYPos;
            textYposLabel.Content = global.txtMarkYPos.ToString() + "%";

            imgMarkPath.Text = global.imgMarkPath;
            imgMarkSize.SelectedIndex = global.imgMarkSize;
            imgMarkTrans.Value = global.imgMarkTrans;
            transVal = (double)global.imgMarkTrans / 255;
            valTrans = (int)(transVal * 100);
            imgTransLabel.Content = valTrans.ToString() + "%";
            imgMarkXpos.Value = global.imgMarkXPos;
            imgXposLabel.Content = global.imgMarkXPos.ToString() + "%";
            imgMarkYpos.Value = global.imgMarkYPos;
            imgYposLabel.Content = global.imgMarkYPos.ToString() + "%";
        }


        //水印类型
        private void MarkTypeCbBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (MarkTypeCbBox.SelectedIndex == 0)
            {
                imgMarkLayout.Visibility = Visibility.Collapsed;
                textMarkLayout.Visibility = Visibility.Visible;                
            }
            else
            {                
                textMarkLayout.Visibility = Visibility.Collapsed;
                imgMarkLayout.Visibility = Visibility.Visible;
            }
            global.MarkType = MarkTypeCbBox.SelectedIndex;

            global.SetWaterMarkParameters();
        }

        private void AddMarkCheck_Checked(object sender, RoutedEventArgs e)
        {
            if (isSet == false)
                return;
            global.isAddMark = 1;
            AddMark(global.isAddMark);
            global.SetWaterMarkParameters();
        }
        private void AddMarkCheck_Unchecked(object sender, RoutedEventArgs e)
        {
            if (isSet == false)
                return;
            global.isAddMark = 0;
            AddMark(global.isAddMark);
            global.SetWaterMarkParameters();
        }
  

        private void AddTimeMarkCheck_Checked(object sender, RoutedEventArgs e)
        {
            if (isSet == false)
                return;
            global.isAddTimeMark = 1;
            global.SetWaterMarkParameters();
        }
        private void AddTimeMarkCheck_Unchecked(object sender, RoutedEventArgs e)
        {
            if (isSet == false)
                return;
            global.isAddTimeMark = 0;
            global.SetWaterMarkParameters();
        }


        private void textMarkSize_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (isSet == false)
                return;
            global.txtMarkFontSize = 10 + textMarkSize.SelectedIndex * 5;
            global.SetWaterMarkParameters();
        }

        private void textMarkFont_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (isSet == false)
                return;
            global.txtMarkFontName = textMarkFont.SelectedItem.ToString();
            global.SetWaterMarkParameters();
        }

        private void textMarkFtype_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (isSet == false)
                return;
            global.txtMarkFontType = textMarkFtype.SelectedIndex;
            global.SetWaterMarkParameters();
        }

        private void textMarkColor_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (isSet == false)
                return;
            global.txtMarkColor = textMarkColor.SelectedIndex;
            global.SetWaterMarkParameters();
        }

        private void textMarkTrans_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (isSet == false)
                return;
            global.txtMarkTrans = (int)textMarkTrans.Value;
            double transVal = (double)global.txtMarkTrans / 255;
            int val = (int)(transVal * 100);
            textTransLabel.Content = val.ToString() + "%";
            global.SetWaterMarkParameters();
        }

        private void textMarkXpos_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (isSet == false)
                return;
            global.txtMarkXPos = (int)textMarkXpos.Value;
            textXposLabel.Content = global.txtMarkXPos.ToString() + "%";
            global.SetWaterMarkParameters();
        }

        private void textMarkYpos_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (isSet == false)
                return;
            global.txtMarkYPos = (int)textMarkYpos.Value;
            textYposLabel.Content = global.txtMarkYPos.ToString() + "%";
            global.SetWaterMarkParameters();
        }

        private void imgMarkSize_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (isSet == false)
                return;
           global.imgMarkSize = imgMarkSize.SelectedIndex;
           global.SetWaterMarkParameters();
        }

        private void imgMarkTrans_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (isSet == false)
                return;
            global.imgMarkTrans = (int)imgMarkTrans.Value;
            double transVal = (double)global.imgMarkTrans / 255;
            int val = (int)(transVal * 100);
            imgTransLabel.Content = val.ToString() + "%";
            global.SetWaterMarkParameters();
        }

        private void imgMarkXpos_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (isSet == false)
                return;
            global.imgMarkXPos = (int)imgMarkXpos.Value;
            imgXposLabel.Content = global.imgMarkXPos.ToString() + "%";
            global.SetWaterMarkParameters();
        }

        private void imgMarkYpos_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (isSet == false)
                return;
            global.imgMarkYPos = (int)imgMarkYpos.Value;
            imgYposLabel.Content = global.imgMarkYPos.ToString() + "%";
            global.SetWaterMarkParameters();
        }

        
        //打开文件浏览对话框
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.OpenFileDialog openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            //openFileDialog1.InitialDirectory = "C:\\";
            openFileDialog1.Filter = "png file(*.png)|*.png|bmp file(*.bmp)|*.bmp|jpg file(*.jpg)|*.jpg";
            openFileDialog1.FilterIndex = 0;
            openFileDialog1.RestoreDirectory = true;
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                global.imgMarkPath = openFileDialog1.FileName;
                imgMarkPath.Text = global.imgMarkPath;
            }
            global.SetWaterMarkParameters();
        }

        private void OkBt_Click(object sender, RoutedEventArgs e)
        {
            SetWaterMarkParams();
        }

        private void ExitBt_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void OkBt_MouseDown(object sender, MouseButtonEventArgs e)
        {
            
        }

    }
}
