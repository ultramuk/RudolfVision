using System;
using System.ComponentModel;
using System.Windows.Input;
using System.Windows.Media;
using OpenCvSharp;
using RudolfApp.Utils;

namespace RudolfApp.ViewModel
{
    public class MainViewModel : INotifyPropertyChanged
    {
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

        public ICommand LoadSampleCommand { get;  }
        public ICommand StartWebcamCommand { get;  }
        public ICommand StopWebcamCommand { get; }

        private readonly WebcamService _webcamService;

        public MainViewModel()
        {
            _webcamService = new WebcamService();
            _webcamService.OnFrameReceived = image =>
            {
                System.Windows.Application.Current.Dispatcher.Invoke(() =>
                {
                    InputImage = image;
                });
            };

            LoadSampleCommand = new RelayCommand(_ => LoadSampleImage());
            StartWebcamCommand = new RelayCommand(_ => _webcamService.Start());
            StopWebcamCommand = new RelayCommand(_ => _webcamService.Stop());
        }

        private void LoadSampleImage()
        {
            try
            {
                _webcamService.Stop();
                string imagePath = System.IO.Path.Combine(AppContext.BaseDirectory, "Assets", "sample.png");

                if(!System.IO.File.Exists(imagePath))
                {
                    Console.WriteLine("이미지 파일이 없습니다: " + imagePath);
                    return;
                }

                Console.WriteLine("이미지 파일 발견: " + imagePath);

                var mat = new OpenCvSharp.Mat(imagePath);

                RudolfApp.Services.Interop.RudolfInterop.Initialize();
                RudolfApp.Services.Interop.RudolfInterop.ProcessImage(mat.Data, mat.Width, mat.Height, mat.Channels());
                RudolfApp.Services.Interop.RudolfInterop.Release();

                InputImage = ImageConverter.MatToImageSource(mat);
            }
            catch (Exception ex)
            {
                Console.WriteLine("이미지 로딩 실패: " + ex.Message);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
