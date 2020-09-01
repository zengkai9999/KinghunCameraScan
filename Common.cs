using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace CameraScan
{
    public class Common
    {
        private static string currentLanguageFile = "zh-cn.xaml";

        /// <summary>
        /// 获取或设置当前程序使用语言的资源文件
        /// </summary>
        public static string CurrentLanguageFile
        {
            get { return currentLanguageFile; }
            set
            {

                currentLanguageFile = value;
                LoadLanguage(currentLanguageFile);
            }
        }

        /// <summary>
        /// 根据资源文件切换当前应用程序使用的语言
        /// </summary>
        /// <param name="currentLanguageFile"></param>
        private static void LoadLanguage(string currentLanguageFile)
        {
            var rd = new ResourceDictionary() { Source = new Uri(currentLanguageFile, UriKind.RelativeOrAbsolute) };

            if (Application.Current.Resources.MergedDictionaries.Count == 0)
                Application.Current.Resources.MergedDictionaries.Add(rd);
            else
                Application.Current.Resources.MergedDictionaries[0] = rd;
        }
    }
}
