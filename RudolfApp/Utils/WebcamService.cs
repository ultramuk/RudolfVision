using OpenCvSharp;
using System;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace RudolfApp.Utils
{
    public class WebcamService
    {
        private VideoCapture _capture;
        private CancellationTokenSource _cts;
        private Task _captureTask;

        public Action<ImageSource> OnFrameReceived;

        private bool _isRunning = false;

        public void Start()
        {
            if (_isRunning) return;
            _isRunning = true;

            _capture = new VideoCapture();
            _capture.Open(0, VideoCaptureAPIs.DSHOW);

            if(!_capture.IsOpened())
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

                            var matCopy = mat.Clone(); // 안전 복사
                            Application.Current.Dispatcher.Invoke(() =>
                            {
                                try
                                {
                                    var bitmap = BitmapSource.Create(
                                        matCopy.Width, matCopy.Height,
                                        96, 96, // dpi
                                        System.Windows.Media.PixelFormats.Bgr24,
                                        null,
                                        matCopy.Data,
                                        (int)(matCopy.Step() * matCopy.Height),
                                        (int)(matCopy.Step()));

                                    OnFrameReceived?.Invoke(bitmap);
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
                    catch(Exception ex)
                    {
                        Console.WriteLine("프레임 수신 중 예외 발생: " + ex.Message);
                    }

                    Thread.Sleep(30); // 약 30 FPS
                }
            });
        }

        public void Stop()
        {
            if (!_isRunning) return;
            _isRunning = false;

            _cts?.Cancel();

            try
            {
                _captureTask?.Wait();
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
