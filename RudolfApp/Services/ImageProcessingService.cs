using System;
using System.IO;
using System.Windows.Media;
using OpenCvSharp;
using RudolfApp.Services.Interop;
using RudolfApp.Utils;

namespace RudolfApp.Services
{
    public static class ImageProcessingService
    {
        /// <summary>
        /// 이미지 파일을 처리하여 루돌프 코가 합성된 결과를 ImageSource로 반환합니다.
        /// </summary>
        /// <param name="imagePath">처리할 이미지 경로</param>
        /// <returns>WPF용 ImageSource</returns>
        public static ImageSource ProcessImageFromFile(string imagePath)
        {
            if (!File.Exists(imagePath))
                throw new FileNotFoundException("이미지 파일이 존재하지 않습니다.", imagePath);

            using var mat = new Mat(imagePath, ImreadModes.Color);
            if (mat.Empty())
                throw new InvalidDataException("이미지 파일이 비어 있습니다.");

            // C++ DLL을 통해 루돌프 코 합성 처리
            RudolfInterop.ProcessImage(mat.Data, mat.Width, mat.Height, mat.Channels());

            // 합성된 이미지를 WPF용 이미지로 변환
            return ImageConverter.MatToImageSource(mat);
        }
    }
}
