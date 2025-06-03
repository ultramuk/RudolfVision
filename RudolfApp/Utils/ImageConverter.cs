using OpenCvSharp;
using OpenCvSharp.WpfExtensions;
using System.Windows.Media;

namespace RudolfApp.Utils
{
    public static class ImageConverter
    {
        /// <summary>
        /// OpenCvSharp의 Mat 객체를 WPF에서 사용 가능한 ImageSource로 변환
        /// </summary>
        /// <param name="mat">OpenCvSharp Mat (BGR 이미지)</param>
        /// <returns>WPF용 ImageSource 객체</returns>
        public static ImageSource MatToImageSource(Mat mat)
        {
            return mat.ToWriteableBitmap();
        }
    }
}
