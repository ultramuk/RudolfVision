using System;
using System.ComponentModel;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using OpenCvSharp;
using RudolfApp.Utils;
using RudolfApp.Services;
using RudolfApp.Services.Interop;
using System.Windows;
using Microsoft.Win32;

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

        public ICommand LoadFromFileCommand { get; }
        public ICommand StartWebcamCommand { get; }
        public ICommand StopWebcamCommand { get; }

        private readonly WebcamCaptureService _webcamCaptureService;

        public MainViewModel()
        {
            RudolfInterop.Initialize();

            _webcamCaptureService = new WebcamCaptureService();
            _webcamCaptureService.OnFrameReady = image =>
            {
                System.Windows.Application.Current.Dispatcher.Invoke(() =>
                {
                    InputImage = image;
                });
            };

            LoadFromFileCommand = new AsyncRelayCommand(LoadFromFileAsync);
            StartWebcamCommand = new RelayCommand(_ => _webcamCaptureService.Start());
            StopWebcamCommand = new AsyncRelayCommand(_webcamCaptureService.StopAsync);
        }

        private async Task LoadFromFileAsync()
        {
            var dialog = new OpenFileDialog
            {
                Title = "이미지 파일 선택",
                Filter = "이미지 파일 (*.png;*.jpg;*.jpeg;*.bmp)|*.png;*.jpg;*.jpeg;*.bmp"
            };

            if (dialog.ShowDialog() == true)
            {
                await LoadImageAsync(dialog.FileName);
            }
        }

        private async Task LoadImageAsync(string imagePath)
        {
            try
            {
                await _webcamCaptureService.StopAsync();

                if (!File.Exists(imagePath))
                {
                    Console.WriteLine("이미지 파일이 없습니다: " + imagePath);
                    return;
                }

                Console.WriteLine("이미지 파일 발견: " + imagePath);

                using var mat = new Mat(imagePath, ImreadModes.Color);
                if (mat.Empty())
                {
                    Console.WriteLine("이미지 로드 실패: 빈 이미지입니다.");
                    return;
                }

                RudolfInterop.ProcessImage(mat.Data, mat.Width, mat.Height, mat.Channels());
                InputImage = ImageConverter.MatToImageSource(mat);
            }
            catch (Exception ex)
            {
                Console.WriteLine("이미지 로딩 실패: " + ex.Message);
            }
        }

        public async Task CleanupAsync()
        {
            await _webcamCaptureService.StopAsync();
            RudolfInterop.Release();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
