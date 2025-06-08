using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using OpenCvSharp;
using RudolfApp.Services.Interop;

namespace RudolfApp.Services
{
    public class WebcamCaptureService
    {
        private VideoCapture? _capture;
        private CancellationTokenSource? _cts;
        private Task? _captureTask;

        public Action<ImageSource>? OnFrameReady;

        private bool _isRunning = false;

        public void Start()
        {
            if (_isRunning) return;
            _isRunning = true;

            _capture = new VideoCapture();
            _capture.Open(0, VideoCaptureAPIs.DSHOW);

            if (!_capture.IsOpened())
            {
                Console.WriteLine("카메라 열기 실패");
                _isRunning = false;
                return;
            }

            _capture.Set(VideoCaptureProperties.FrameWidth, 640);
            _capture.Set(VideoCaptureProperties.FrameHeight, 480);

            _cts = new CancellationTokenSource();
            _captureTask = Task.Run(() =>
            {
                using var mat = new Mat();
                int consecutiveFailures = 0;
                const int maxAllowedFailures = 30;

                while (!_cts.IsCancellationRequested)
                {
                    try
                    {
                        _capture.Read(mat);
                        if (!mat.Empty())
                        {
                            consecutiveFailures = 0;

                            RudolfInterop.ProcessImage(mat.Data, mat.Width, mat.Height, mat.Channels());

                            var matCopy = mat.Clone();
                            Application.Current.Dispatcher.Invoke(() =>
                            {
                                try
                                {
                                    var bitmap = BitmapSource.Create(
                                        matCopy.Width, matCopy.Height,
                                        96, 96,
                                        PixelFormats.Bgr24,
                                        null,
                                        matCopy.Data,
                                        (int)(matCopy.Step() * matCopy.Height),
                                        (int)(matCopy.Step()));

                                    OnFrameReady?.Invoke(bitmap);
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine("프레임 처리 중 오류: " + ex.Message);
                                }
                            });
                        }
                        else
                        {
                            consecutiveFailures++;
                            if (consecutiveFailures == maxAllowedFailures)
                            {
                                Console.WriteLine("지속적 프레임 수신 실패 (카메라 연결 확인)");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("프레임 수신 중 예외 발생: " + ex.Message);
                    }

                    Thread.Sleep(30);
                }
            });
        }

        public async Task StopAsync()
        {
            if (!_isRunning) return;
            _isRunning = false;

            _cts?.Cancel();

            try
            {
                if (_captureTask != null)
                    await _captureTask;
            }
            catch (Exception ex)
            {
                Console.WriteLine("웹캠 작업 종료 중 예외: " + ex.Message);
            }

            if (_capture != null)
            {
                if (_capture.IsOpened())
                    _capture.Release();

                _capture.Dispose();
                _capture = null;
            }

            _cts = null;
            _captureTask = null;
        }
    }
}
