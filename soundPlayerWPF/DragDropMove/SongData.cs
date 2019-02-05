using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace soundPlayerWPF
{
    public class SongData : INotifyPropertyChanged
    {
        private int id = 0;
        public int ID
        {
            get { return id; }
            set
            {
                id = value;
                OnPropertyChanged("ID");
            }
        }

        private string title = string.Empty;
        public string Title
        {
            get { return title; }
            set
            {
                title = value;
                OnPropertyChanged("Title");
            }
        }

        private string artist = string.Empty;
        public string Artist
        {
            get { return artist; }
            set
            {
                artist = value;
                OnPropertyChanged("Artist");
            }
        }

        private string filePath = string.Empty;
        public string FilePath
        {
            get { return filePath; }
            set
            {
                filePath = value;
                OnPropertyChanged("FilePath");
            }
        }

        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }

}
