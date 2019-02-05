using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
//using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Forms;
using System.Windows.Forms.Integration;
using CSCore;
using CSCore.SoundOut;
using System.Collections.ObjectModel;
using CSCore.Codecs;
using System.Windows.Threading;

namespace soundPlayerWPF
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        //---------------定義---------------
        private readonly ClassPlaying _classPlaying = new ClassPlaying();
        DispatcherTimer _timerPlay = new DispatcherTimer(DispatcherPriority.Normal);
        double timespan1 = 0;
        bool stopSlideBarUpdate;
        int nowPlayingID;
            
        //---------------関数---------------
        private void PlayNextSong()
        {
            TimeSpan miliFive = new TimeSpan(0, 0, 0, 0, 500);
            if ((_classPlaying.Position + miliFive) >= _classPlaying.Length && _classPlaying._SoundOut != null)
            {
                //停止
                _classPlaying.Stop();

                if (nowPlayingID + 1 != _rowsList.Count)
                {
                    //次の曲をロード
                    _classPlaying.LoadFile(_rowsList[nowPlayingID + 1].FilePath);
                    labelTitle.Content = _rowsList[nowPlayingID + 1].Title;
                    labelArtist.Content = _rowsList[nowPlayingID + 1].Artist;
                    //再生
                    sliderPos.Value = 0;
                    _classPlaying.Play();
                    nowPlayingID += 1;
                }
                else
                {
                    //停止
                    _classPlaying.Stop();
                    _timerPlay.Stop();
                    buttonPlay.IsEnabled = true;
                    buttonPause.IsEnabled = false;
                    buttonStop.IsEnabled = false;
                    sliderPos.Value = 0;
                    labelNowTime.Content = "00:00:00 / 00:00:00";
                    //pictureBox1.Image = null;
                    nowPlayingID = 0;
                }
            }
        }


        //---------------動作---------------
        public MainWindow()
        {
            InitializeComponent();
            LoadSetting();
            //
            sliderVol.Value = _classPlaying.Volume;
            //タイマー
            _timerPlay.Interval = new TimeSpan(0, 0, 0, 0, 10);
            _timerPlay.Tick += new EventHandler(_timerPlay_tick);
            _timerPlay.IsEnabled = false;
            //
            //TimeSpan position = _classPlaying.Position;
            //TimeSpan length = _classPlaying.Length;
        }

        //再生
        private void buttonPlay_Click(object sender, RoutedEventArgs e)
        {
            if(_classPlaying._SoundOut != null) //再生中の曲があれば再開
            {
                _classPlaying.Resume();
            }
            else if(dataGrid.SelectedItem != null) //選択している曲を再生
            {
                //曲をロード
                _classPlaying.LoadFile(((TextBlock)dataGrid.Columns[3].GetCellContent(dataGrid.SelectedItem)).Text);
                _classPlaying.Play();
                labelTitle.Content = ((TextBlock)dataGrid.Columns[1].GetCellContent(dataGrid.SelectedItem)).Text;
                labelArtist.Content = ((TextBlock)dataGrid.Columns[2].GetCellContent(dataGrid.SelectedItem)).Text;
                nowPlayingID = int.Parse(((TextBlock)dataGrid.Columns[0].GetCellContent(dataGrid.SelectedItem)).Text);
            }
            else //1曲目を再生
            {
                _classPlaying.LoadFile(_rowsList[0].FilePath);
                _classPlaying.Play();
                labelTitle.Content = _rowsList[0].Title;
                labelArtist.Content = _rowsList[0].Artist;
                nowPlayingID = 0;
            }
            buttonPlay.IsEnabled = false;
            buttonPause.IsEnabled = true;
            buttonStop.IsEnabled = true;
            _timerPlay.Start();
        }

        //一時停止
        private void buttonPause_Click(object sender, RoutedEventArgs e)
        {
            _classPlaying.Pause();
            buttonPlay.IsEnabled = true;
            buttonPause.IsEnabled = false;
            _timerPlay.Stop();
            pictureBox1.Image = null;
        }

        //停止
        private void buttonStop_Click(object sender, RoutedEventArgs e)
        {
            _classPlaying.Stop();
            buttonPlay.IsEnabled = true;
            buttonPause.IsEnabled = false;
            buttonStop.IsEnabled = false;
            _timerPlay.Stop();
            sliderPos.Value = 0;
            labelNowTime.Content = "00:00:00 / 00:00:00";
            pictureBox1.Image = null;
            nowPlayingID = 0;
        }

        private void _timerPlay_tick(object sender, EventArgs e)
        {
            timespan1 += 2;
            if (timespan1 == 360)
            {
                timespan1 = 0;
            }

            //回転させる
            picMove.RenderTransform = new System.Windows.Media.RotateTransform
            {
                Angle = timespan1,
                CenterX = picMove.ActualWidth / 2,
                CenterY = picMove.ActualHeight / 2
            };

            //---
            TimeSpan position = _classPlaying.Position;
            TimeSpan length = _classPlaying.Length;

            if (position > length)
                length = position;
            labelNowTime.Content = string.Format(@"{0:hh\:mm\:ss} / {1:hh\:mm\:ss}", position, length);


            if (!stopSlideBarUpdate && length != TimeSpan.Zero && position != TimeSpan.Zero)
            {
                double numPos = position.TotalMilliseconds / length.TotalMilliseconds * sliderPos.Maximum;
                sliderPos.Value = (int)numPos;
            }

            //スペクトラム描画
            GenerateLineSpectrum(pictureBox1, _classPlaying);

            //次の曲を再生
            PlayNextSong();
        }

        //ボリュームバー
        private void sliderVol_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            _classPlaying.Volume = (int)Math.Truncate(sliderVol.Value);
        }

        //プログレスバー
        private void sliderPos_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            stopSlideBarUpdate = true;
        }

        private void sliderPos_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            stopSlideBarUpdate = false;
        }

        private void sliderPos_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (stopSlideBarUpdate && _classPlaying._SoundOut != null)
            {
                double numPos = sliderPos.Value / sliderPos.Maximum;
                TimeSpan position = TimeSpan.FromMilliseconds(_classPlaying.Length.TotalMilliseconds * numPos);
                _classPlaying.Position = position;
            }
            else
                return;
        }

        //閉じたとき
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            _classPlaying.Stop();
            _timerPlay = null;
            SaveSetting();
            pictureBox1.Dispose();
            _rowsList.Clear();
        }

        //DataGridダブルクリック→その曲を再生
        private void dataGrid_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            //再生中の曲があれば停止
            if (_classPlaying._SoundOut != null)
            {
                _classPlaying.Stop();
                sliderPos.Value = 0;
            }
            //選択中の曲を再生
            _classPlaying.LoadFile(((TextBlock)dataGrid.Columns[3].GetCellContent(dataGrid.SelectedItem)).Text);
            _classPlaying.Play();
            labelTitle.Content = ((TextBlock)dataGrid.Columns[1].GetCellContent(dataGrid.SelectedItem)).Text;
            labelArtist.Content = ((TextBlock)dataGrid.Columns[2].GetCellContent(dataGrid.SelectedItem)).Text;
            nowPlayingID = int.Parse(((TextBlock)dataGrid.Columns[0].GetCellContent(dataGrid.SelectedItem)).Text);

            buttonPlay.IsEnabled = false;
            buttonPause.IsEnabled = true;
            buttonStop.IsEnabled = true;
            _timerPlay.Start();
        }
    }


    public partial class ClassPlaying
    {
        //---------------宣言---------------
        ISoundOut _soundOut;
        public ISoundOut _SoundOut
        {
            get { return _soundOut; }
        }

        CSCore.ISampleSource _sampleSource;

        public int Volume
        {
            get
            {
                if (_soundOut != null)
                    return Math.Min(100, Math.Max((int)(_soundOut.Volume * 100), 0));
                return 100;
            }
            set
            {
                if (_soundOut != null)
                    _soundOut.Volume = Math.Min(1.0f, Math.Max(value / 100f, 0f));
            }
        }

        public void Play()
        {
            if (_soundOut != null)
                _soundOut.Play();
        }

        public void Resume()
        {
            if (_soundOut != null)
                _soundOut.Resume();
        }

        public void Pause()
        {
            if (_soundOut != null)
                _soundOut.Pause();
        }

        public void Stop()
        {
            if (_soundOut != null)
                _soundOut.Stop();
            CleanupPlayback();
        }

        public TimeSpan Position
        {
            get
            {
                if (_sampleSource != null)
                    return _sampleSource.GetPosition();
                return TimeSpan.Zero;
            }
            set
            {
                if (_sampleSource != null)
                    _sampleSource.SetPosition(value);
            }
        }

        public TimeSpan Length
        {
            get
            {
                if (_sampleSource != null)
                    return _sampleSource.GetLength();
                return TimeSpan.Zero;
            }
        }

        //---------------関数---------------
        //ファイルの中身をロード
        public void LoadFile(string path)
        {
            CleanupPlayback();

            _sampleSource =
                CodecFactory.Instance.GetCodec(path)
                .ToSampleSource()
                .ToMono();
            SetupSampleSource(_sampleSource);
            _soundOut = new WasapiOut() { Latency = 100 };
            _soundOut.Initialize(_waveSource);
        }

        //別ファイルが開かれた時に現在のインスタンス削除
        private void CleanupPlayback()
        {
            if (_soundOut != null)
            {
                _soundOut.Dispose();
                _soundOut = null;
            }
            if (_waveSource != null)
            {
                _waveSource.Dispose();
                _waveSource = null;
            }
        }

        //ファイルのサウンドタイプを返す 未使用
        public CSCore.SoundOut.ISoundOut GetSoundOut()
        {
            if (WasapiOut.IsSupportedOnCurrentPlatform)
                return new WasapiOut();
            else
                return new DirectSoundOut();
        }
    }
}
