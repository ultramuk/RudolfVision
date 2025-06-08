using System;
using System.ComponentModel;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using OpenCvSharp;
using RudolfApp.Utils;
using RudolfApp.Services.Interop;
using System.Windows;

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

        private string _customImagePath;
        public string CustomImagePath
        {
            get => _customImagePath;
            set
            {
                if (_customImagePath != value)
                {
                    _customImagePath = value;
                    OnPropertyChanged(nameof(CustomImagePath));
                }
            }
        }

        public ICommand LoadSampleCommand { get; }
        public ICommand LoadCustomImageCommand { get; }
        public ICommand StartWebcamCommand { get; }
        public ICommand StopWebcamCommand { get; }

        private readonly WebcamService _webcamService;

        public MainViewModel()
        {
            RudolfInterop.Initialize();

            _webcamService = new WebcamService();
            _webcamService.OnFrameReceived = image =>
            {
                System.Windows.Application.Current.Dispatcher.Invoke(() =>
                {
                    InputImage = image;
                });
            };

            LoadSampleCommand = new AsyncRelayCommand(LoadSampleImageAsync);
            LoadCustomImageCommand = new AsyncRelayCommand(LoadCustomImageAsync);
            StartWebcamCommand = new RelayCommand(_ => _webcamService.Start());
            StopWebcamCommand = new AsyncRelayCommand(_webcamService.StopAsync);
        }

        private async Task LoadSampleImageAsync()
        {
            await LoadImageAsync(Path.Combine(AppContext.BaseDirectory, "Assets", "sample.png"));
        }
        private async Task LoadCustomImageAsync()
        {
            if (string.IsNullOrWhiteSpace(CustomImagePath) || !File.Exists(CustomImagePath))
            {
                MessageBox.Show("유효한 이미지 경로를 입력하세오.", "오류", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            await LoadImageAsync(CustomImagePath);
        }

        private async Task LoadImageAsync(string imagePath)
        {
            try
            {
                await _webcamService.StopAsync();

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
            await _webcamService.StopAsync();
            RudolfInterop.Release();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
