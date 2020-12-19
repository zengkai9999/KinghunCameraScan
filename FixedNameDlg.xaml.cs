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
using System.Text.RegularExpressions;

namespace CameraScan
{
    /// <summary>
    /// FixedNameDlg.xaml 的交互逻辑
    /// </summary>
    public partial class FixedNameDlg : Window
    {
        public FixedNameDlg()
        {
            InitializeComponent();
        }


        private string SpecialCode(string s)
        {
            s = s.Replace(@"\", "");
            s = Regex.Replace(s, "[ \\^ \\*×~!/…<>《》|!！??？:：•`·；:;\"'‘’“”]", "").ToUpper();
            return s;
        }

        private void RnTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            int pos = RnTextBox.SelectionStart;
            RnTextBox.Text = SpecialCode(RnTextBox.Text);
            RnTextBox.SelectionStart = pos;
        }


        private void ExitBt_Click(object sender, RoutedEventArgs e)
        {
            if (RnTextBox.Text.Length > 0)
            {
                global.pFixedNameStr = RnTextBox.Text;
                this.Close();
            }
            else
            {
                string TipStr = "文件名不能为空！";
                if (global.pLangusge == 1) TipStr = "文件名不能為空！";
                if (global.pLangusge == 2) TipStr = "File name cannot be empty";
                if (global.pLangusge == 3) TipStr = "El nombre de archivo no puede estar vacío.";
                if (global.pLangusge == 4) TipStr = "ファイル名を空白にすることはできません。";
                System.Windows.MessageBox.Show(TipStr);
                return;
            }
        }


        private void Window_Closed(object sender, EventArgs e)
        {
            ExitBt_Click(null, null);
        }


        private void RnTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            //if (e.Key == Key.Enter)
            //{
            //    ExitBt_Click(null, null);
            //}
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            global.pFixedNameStr = "";
            this.Focus();
        }
    }
}
