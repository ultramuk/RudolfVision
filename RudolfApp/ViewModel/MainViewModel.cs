using System;
using System.ComponentModel;
using System.Security.RightsManagement;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace RudolfApp.ViewModel
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private ImageSource _inputImage;
        public ImageSource InputImage
        {
            get => _inputImage;
            set
            {
                if (_inputImage != value)
                {
                    _inputImage = value;
                    OnPropertyChanged(nameof(InputImage));
                }
            }
        }
    }
}
