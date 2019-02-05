using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace soundPlayerWPF
{
    public partial class MainWindow : Window
    {
        private void LoadSetting()
        {
            //ユーザーにより値が設定されないうちはロードしない
            if (Properties.Settings.Default.Initialized)
            {
                ProvidedFolderPath = Properties.Settings.Default.ProvidedFolderPath;
            }
        }

        private void SaveSetting()
        {
            Properties.Settings.Default["Initialized"] = true;
            Properties.Settings.Default["ProvidedFolderPath"] = ProvidedFolderPath;
            Properties.Settings.Default.Save();
        }
    }
}
