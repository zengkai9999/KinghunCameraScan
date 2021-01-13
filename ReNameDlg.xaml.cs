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
using System.IO;
using System.Text.RegularExpressions;

namespace CameraScan
{
    /// <summary>
    /// ReNameDlg.xaml 的交互逻辑
    /// </summary>
    public partial class ReNameDlg : Window
    {

        public string OldPath = "";
        private string foPath = global.ImagesFolder;
        private string OldName = "";
        private string suffixStr = "";
        public int index = 0;

        public ReNameDlg()
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

            if (OldPath.Length > 0 && File.Exists(OldPath))
            {
                int pos = OldPath.LastIndexOf("\\");
                foPath = OldPath.Substring(0, pos+1);
                OldName = OldPath.Substring(pos + 1, OldPath.Length - pos - 1 - 4);
                RnTextBox.Text = OldName;
                suffixStr = OldPath.Substring(OldPath.Length - 4, 4);
                suffixLabel.Content = suffixStr;
            }
            
        }

        private void ExitBt_Click(object sender, RoutedEventArgs e)
        {
            if (File.Exists(OldPath))
            {
                if (RnTextBox.Text.Length > 0)
                {
                    RnTextBox.Text = SpecialCode(RnTextBox.Text);
                    string desName = RnTextBox.Text;
                    string newPath = foPath + desName + suffixStr;
                    MainWindow mMainWindow = (MainWindow)this.Owner;
                    if (!File.Exists(newPath))
                    {
                        FileInfo fi = new FileInfo(OldPath);
                        fi.MoveTo(newPath);
                        PreviewPhoto mlistItem = (PreviewPhoto)mMainWindow.PreviewImgList.Items[index];
                        PreviewPhoto mPhoto = new PreviewPhoto();
                        mPhoto.SourceImage = mlistItem.SourceImage;
                        mPhoto.LogoImage = mlistItem.LogoImage;
                        mPhoto.ImageName = desName + suffixStr;
                        mPhoto.ImagePath = newPath;
                        mMainWindow.PreviewImgList.Items.RemoveAt(index);
                        mMainWindow.PreviewImgList.Items.Insert(index, mPhoto);
                    }
                    this.Close();
                    mMainWindow.Focus();
                }
                else
                {
                    string TipStr = "重命名时名字不能为空！";
                    if (global.pLangusge == 1) TipStr = "重命名時名字不能為空！";
                    if (global.pLangusge == 2) TipStr = "The name can not be empty ";
                    if (global.pLangusge == 3) TipStr = "El nombre no puede estar vacío al renombrar ";
                    if (global.pLangusge == 4) TipStr = "名前を変更する際、名前を空白にすることはできません。";
                    if (global.pLangusge == 5) TipStr = "Il nome non può essere vuoto durante la ridenominazione.";
                    if (global.pLangusge == 6) TipStr = "Le nom ne peut pas être vide lors du changement du nom.";
                    if (global.pLangusge == 7) TipStr = "Der Dateiname darf beim Umbenennen nicht leer sein.";
                    System.Windows.MessageBox.Show(TipStr);
                    return;
                }
            }
            else 
            {
                string TipStr = "原文件不存在，重命名失败！";
                if (global.pLangusge == 1) TipStr = "原文件不存在，重命名失敗！";
                if (global.pLangusge == 2) TipStr = "The original file does not exist and rename failed.";
                if (global.pLangusge == 3) TipStr = "El archivo original no existe. Fallo al renombrar ";
                if (global.pLangusge == 4) TipStr = "オリジナルファイルが存在しません。名前の変更ができませんでした。";
                if (global.pLangusge == 5) TipStr = "Il file originale non esiste, impossibile rinominare.";
                if (global.pLangusge == 6) TipStr = "Le fichier d'origine n'existe pas, le changement de nom a échoué.";
                if (global.pLangusge == 7) TipStr = "Die Originaldatei existiert nicht. Das Umbennen ist fehlgeschlagen.";
                System.Windows.MessageBox.Show(TipStr);
                return;
            }
            
        }

        private string SpecialCode(string s)
        {
            s = s.Replace(@"\", "");
            s = Regex.Replace(s, "[ \\^ \\*×~!/…<>《》|!！??？:：•`·；:;\"'‘’“”]", "").ToUpper();
            return s;
        }

        private void RnTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            //int pos = RnTextBox.SelectionStart;
            //RnTextBox.Text = SpecialCode(RnTextBox.Text);
            //RnTextBox.SelectionStart = pos;
        }

        
    }
}
