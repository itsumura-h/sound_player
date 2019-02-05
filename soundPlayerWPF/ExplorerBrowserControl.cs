using Microsoft.WindowsAPICodePack.Shell;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using System.Windows.Forms;

namespace soundPlayerWPF
{

    public partial class MainWindow : Window
    {
        //---------------定義---------------
        string currentDirectory;
        string upperDirectory;

        private string providedFolderPath = "";
        public string ProvidedFolderPath
        {
            get { return providedFolderPath; }
            set { providedFolderPath = value; }
        }

        //---------------関数---------------
        private void MoveFolder(string path)
        {
            if (System.IO.Directory.Exists(path))
                explorerBrowser1.Navigate((ShellObject)FileSystemKnownFolder.FromFolderPath(path));
            else
                explorerBrowser1.Navigate((ShellObject)KnownFolders.Desktop);
        }

        //フルパスの文字列からファイル名だけを取り出す関数
        private string GetUpperFolderPath(string filePath, char separateChar)
        {
            try
            {
                string[] strArray = filePath.Split(separateChar);
                int a = strArray.Length - 1;
                string returnPath = "";
                string temporaryPath = "";
                for (int i = 0; i < a; i++)
                {
                    returnPath = temporaryPath + strArray[i] + separateChar;
                    temporaryPath = returnPath;
                }
                return returnPath;
            }
            catch
            {
                return "";
            }
        }
        //---------------動作---------------
        private void explorerBrowser1_Load(object sender, EventArgs e)
        {
            MoveFolder(ProvidedFolderPath);
            buttonProvidedFolder.ToolTip = ProvidedFolderPath;

            explorerBrowser1.Height = (int)formsHostFoldar.ActualHeight;
            explorerBrowser1.Width = (int)formsHostFoldar.ActualWidth;
        }

        //規定のフォルダに移動
        private void buttonProvidedFolder_Click(object sender, RoutedEventArgs e)
        {
            MoveFolder(ProvidedFolderPath);
        }

        //規定のフォルダを登録
        private void buttonFolderProvision_Click(object sender, RoutedEventArgs e)
        {
            DialogResult result = System.Windows.Forms.MessageBox.Show(
                "表示中のディレクトリを規定のディレクトリとして登録しますか？",
                "確認",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);
            if(result == System.Windows.Forms.DialogResult.Yes)
            {
                ProvidedFolderPath = explorerBrowser1.NavigationLog.CurrentLocation.ParsingName;
                buttonProvidedFolder.ToolTip = ProvidedFolderPath;
            }
        }

        //戻るボタン
        private void buttonFolderBack_Click(object sender, RoutedEventArgs e)
        {
            currentDirectory = explorerBrowser1.NavigationLog.CurrentLocation.ParsingName;
            upperDirectory = GetUpperFolderPath(currentDirectory, '\\');
            MoveFolder(upperDirectory);
        }
    }
}
