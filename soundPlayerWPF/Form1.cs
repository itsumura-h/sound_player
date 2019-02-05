using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using Microsoft.WindowsAPICodePack.Shell;

namespace soundPlayerWPF
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.TopLevel = false;
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            explorerBrowser1.Navigate((ShellObject)KnownFolders.Music);
            explorerBrowser1.Height = this.Height;
            explorerBrowser1.Width = this.Width;
        }
    }

    public partial class MainWindow : Window
    {
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Form1 _Form1 = new Form1();
            formsHostFoldar.Child = _Form1;
            _Form1.Height = (int)formsHostFoldar.ActualHeight;
            _Form1.Width = (int)formsHostFoldar.ActualWidth;
        }
    }

}
