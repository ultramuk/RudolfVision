using System;
using System.ComponentModel;
using System.Windows.Media;
using OpenCvSharp;
using RudolfApp.Utils;

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

        public MainViewModel()
        {
            LoadSampleImage();
        }

        private void LoadSampleImage()
        {
            try
            {
                string imagePath = System.IO.Path.Combine(AppContext.BaseDirectory, "Assets", "sample.png");

                if(!System.IO.File.Exists(imagePath))
                {
                    Console.WriteLine("이미지 파일이 없습니다: " + imagePath);
                    return;
                }

                Console.WriteLine("이미지 파일 발견: " + imagePath);

                Mat mat = Cv2.ImRead(imagePath);
                InputImage = ImageConverter.MatToImageSource(mat);
            }
            catch (Exception ex)
            {
                Console.WriteLine("이미지 로딩 실패: " + ex.Message);
            }
        }
    }
}
