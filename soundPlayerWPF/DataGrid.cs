using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using CSCore;
using CSCore.SoundOut;
using CSCore.Codecs;
using System.Collections.ObjectModel;
using Shell32;

namespace soundPlayerWPF
{
    public partial class MainWindow : Window
    {
        //---------------宣言---------------
        string[] dlagFilePathArray;
        string playingFilePath;
        ObservableCollection<SongData> _rowsList = new ObservableCollection<SongData>();
        string[] infoArray = new string[4];

        //---------------関数---------------
        //DDされたファイルタイプをチェック
        private bool CheckFileType(string filePath)
        {
            string[] extnArry = { "wav", "mp3", "mp4", "aac", "wma", "flac" };
            foreach (string extn in extnArry)
            {
                int dotLen = extn.Length;
                if (extn == filePath.Substring(filePath.Length - dotLen, dotLen))
                    return true;
            }
            return false;
        }

        //フルパスの文字列からファイル名だけを取り出す関数
        private string GetFileNameString(string filePath, char separateChar)
        {
            try
            {
                string[] strArray = filePath.Split(separateChar);
                return strArray[strArray.Length - 1];
            }
            catch
            {
                return "";
            }
        }

        //プロパティ取得
        private void GetProperty(string path)
        {
            string dir = System.IO.Path.GetDirectoryName(path); // ファイルのあるディレクトリ
            string file = System.IO.Path.GetFileName(path); //ファイル名
            ShellClass shell = new ShellClass();
            Folder f = shell.NameSpace(dir);
            FolderItem item = f.ParseName(file);

            infoArray[0] = f.GetDetailsOf(item, 21); //曲名
            infoArray[1] = f.GetDetailsOf(item, 0); //ファイル名
            infoArray[2] = f.GetDetailsOf(item, 13); //アーティスト
            infoArray[3] = f.GetDetailsOf(item, 14); //アルバム 未使用
        }

        //IDを整列
        private void ReAssignID()
        {
            for (int i = 0; i < _rowsList.Count; i++)
            {
                _rowsList[i].ID = i;
            }
        }

        //---------------動作---------------
        private void dataGrid_DragEnter(object sender, DragEventArgs e)
        {
            dlagFilePathArray = (string[])e.Data.GetData(DataFormats.FileDrop, false);

            //複数のファイルがDDされた場合、先頭のファイルパスを取得する
            //playingFilePath = dlagFilePathArray[0];

            //ドラッグされたのがファイルであるか確認
            for(int i= 0; i < dlagFilePathArray.Length; i++)
            {
                if (e.Data.GetDataPresent(DataFormats.FileDrop))
                {
                    if (CheckFileType(dlagFilePathArray[i]))
                    {
                        //データを受け取る
                        e.Effects = DragDropEffects.All;
                    }
                    else
                    {
                        //受け取らない
                        e.Effects = DragDropEffects.None;
                    }
                }
                else
                {
                    //受け取らない
                    e.Effects = DragDropEffects.None;
                }
            }
        }

        private void dataGrid_Drop(object sender, DragEventArgs e)
        {
            buttonPlay.IsEnabled = true;
            buttonPause.IsEnabled = false;
            buttonStop.IsEnabled = false;
            sliderPos.Value = 0;
            sliderPos.IsEnabled = true;
            //sliderVol.Value = _classPlaying.Volume;

            for (int i = 0; i < dlagFilePathArray.Length; i++)
            {
                int loadedFileDrop = 0;

                //DDされたファイルがロード済みでないかチェック
                for (int j = 0; j < _rowsList.Count; j++)
                {
                    if (dlagFilePathArray[i] == _rowsList[j].FilePath)
                        loadedFileDrop += 1;
                    else
                        loadedFileDrop += 0;
                }

                //タグをロード
                //TagInfo _mp3Info = MP3Infp.LoadTag(dlagFilePathArray[i]);
                GetProperty(dlagFilePathArray[i]);

                var _songData = new SongData();
                string rowTitle;
                string rowArtist = null;
                string rowPath;

                if (loadedFileDrop < 1)　//ロード済みファイルはロードしない
                {
                    if (infoArray[0] != "") //タイトルをロード、なければファイル名
                    {
                        _songData.Title = infoArray[0];
                        rowTitle = _songData.Title;
                    }
                    else
                    {
                        //_songData.Title = GetFileNameString(dlagFilePathArray[i], '\\');
                        _songData.Title = infoArray[1];
                        rowTitle = _songData.Title;
                    }

                    if (infoArray[2] != "") //アーティスト名をロード、なければ空
                    {
                        _songData.Artist = infoArray[2];
                        rowArtist = _songData.Artist;
                    }
                    Array.Clear(infoArray, 0, 4); //配列の削除

                    _songData.FilePath = dlagFilePathArray[i]; //ファイルパスをロード
                    rowPath = _songData.FilePath;

                    _rowsList.Add(new SongData { Title = rowTitle, Artist = rowArtist, FilePath = rowPath });
                }
            }
            //IDを整列
            ReAssignID();

            dataGrid.ItemsSource = _rowsList;
        }

        //右クリック→削除
        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            //選択されてない時は何もしない
            if (dataGrid.SelectedItem != null)
            {
                //インデックス番号の取得
                int a = int.Parse(((System.Windows.Controls.TextBlock)dataGrid.Columns[0].GetCellContent(dataGrid.SelectedItem)).Text);
                _rowsList.RemoveAt(a); //削除
                ReAssignID();
            }
            else return;
        }

        //右クリック→全て削除
        private void clearButton_Click(object sender, RoutedEventArgs e)
        {
            _rowsList.Clear();
        }
    }
}
