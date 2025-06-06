using System;
using OpenCvSharp;
using RudolfApp.Services.Interop;

namespace RudolfApp.Services
{
    public class ImageProcessingService
    {
        public void ProcessSampleImage()
        {
            using var mat = Cv2.ImRead("Asserts/sample.png", ImreadModes.Color);
            IntPtr dataPtr = mat.Data;

            RudolfInterop.Initialize();
            RudolfInterop.ProcessImage(dataPtr, mat.Width, mat.Height, mat.Channels());
            RudolfInterop.Release();
        }
    }
}
