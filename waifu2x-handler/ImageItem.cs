using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.ComponentModel;

namespace waifu2x_handler
{
    class ImageItem:INotifyPropertyChanged
    {
        private Canvas _processedSignal;
        public BitmapImage Thumbnail
        {
            get;
            set;
        }
        public string ThumbnailSize
        {
            get;
            set;
        }
        public string Name
        {
            get;
            set;
        }
        public string Path
        {
            get;
            set;
        }
        public string FullName
        {
            get;
            set;
        }
        public string Extension
        {
            get;
            set;
        }
        public Canvas ProcessedSignal{
            get
            {
                return _processedSignal;
            }
            set
            {
                _processedSignal = value;
                NotifyPropertyChanged("ProcessedSignal");
            }

         }
        public ImageItem(BitmapImage image,string name,string path,string fullname,string extension)
        {
            Thumbnail = image;
            Name = name;
            Path = path;
            FullName = fullname;
            Extension = extension;

            ThumbnailSize = image.PixelWidth + "x" + image.PixelHeight;

            _processedSignal = new Canvas();

            Rectangle BeforeProcessed=new Rectangle();
            BeforeProcessed.Width = 10;
            BeforeProcessed.Height = 10;
            BeforeProcessed.StrokeThickness = 2;
            BeforeProcessed.Stroke = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#353535"));
            BeforeProcessed.Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#b0b0b0"));

            _processedSignal.Children.Add(BeforeProcessed);
        }

        public void Complete()
        {
            ProcessedSignal = new Canvas();

            Rectangle BeforeProcessed = new Rectangle();
            BeforeProcessed.Width = 10;
            BeforeProcessed.Height = 10;
            BeforeProcessed.StrokeThickness = 2;
            BeforeProcessed.Stroke = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#63dd00"));
            BeforeProcessed.Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#c5ffa5"));

            _processedSignal.Children.Add(BeforeProcessed);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string propertyName)
        {
            if(propertyName!=null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

    }
}
