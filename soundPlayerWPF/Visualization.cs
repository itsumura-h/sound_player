using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.Integration;
using System.Windows;
using WinformsVisualization.Visualization;
using CSCore;
using CSCore.DSP;
using CSCore.Streams;
using System.Runtime.InteropServices;

namespace soundPlayerWPF
{
    public partial class MainWindow : Window
    {
        //スペクトラム描画
        private void GenerateLineSpectrum(System.Windows.Forms.PictureBox picbox, ClassPlaying newclass)
        {
            System.Drawing.Image image = picbox.Image;
            var newImage = newclass._lineSpectrum.CreateSpectrumLine(picbox.Size, System.Drawing.Color.Green, System.Drawing.Color.Red, System.Drawing.Color.Black, true);
            if (newImage != null)
            {
                picbox.Image = newImage;
                if (image != null)
                    image.Dispose();
            }
        }
    }

    public partial class ClassPlaying
    {
        //---------------定義---------------
        public LineSpectrum _lineSpectrum;
        public readonly System.Drawing.Bitmap _bitmap = new System.Drawing.Bitmap(2000, 600);
        public IWaveSource _waveSource;

        //---------------関数---------------
        public void SetupSampleSource(ISampleSource aSampleSource)
        {
            const FftSize fftSize = FftSize.Fft4096;
            //create a spectrum provider which provides fft data based on some input
            var spectrumProvider = new BasicSpectrumProvider(aSampleSource.WaveFormat.Channels,
                aSampleSource.WaveFormat.SampleRate, fftSize);

            //linespectrum and voiceprint3dspectrum used for rendering some fft data
            //in oder to get some fft data, set the previously created spectrumprovider 
            _lineSpectrum = new LineSpectrum(fftSize)
            {
                SpectrumProvider = spectrumProvider,
                UseAverage = true,
                BarCount = 50,
                BarSpacing = 2,
                IsXLogScale = true,
                ScalingStrategy = ScalingStrategy.Sqrt
            };

            //the SingleBlockNotificationStream is used to intercept the played samples
            var notificationSource = new SingleBlockNotificationStream(aSampleSource);
            //pass the intercepted samples as input data to the spectrumprovider (which will calculate a fft based on them)
            notificationSource.SingleBlockRead += (s, a) => spectrumProvider.Add(a.Left, a.Right);

            _waveSource = notificationSource.ToWaveSource(16);
        }

    }

}
