#pragma once

#ifdef RUDOLFLIB_EXPORTS
#define RUDOLFLIB_API __declspec(dllexport)
#else
#define RUDOLFLIB_API __declspec(dllimport)
#endif

extern "C" {
	RUDOLFLIB_API void Initialize();
	RUDOLFLIB_API void Release();
	RUDOLFLIB_API void ProcessImage(unsigned char* data, int width, int height, int channels);
}