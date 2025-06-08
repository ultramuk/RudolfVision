using System;
using System.Runtime.InteropServices;

namespace RudolfApp.Services.Interop
{
    public static class RudolfInterop
    {
        [DllImport("RudolfLib.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void Initialize();

        [DllImport("RudolfLib.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void Release();

        [DllImport("RudolfLib.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void ProcessImage(IntPtr data, int width, int height, int channels);
    }
}
