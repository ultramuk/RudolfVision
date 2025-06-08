#pragma once

#ifdef RUDOLFLIB_EXPORTS
#define RUDOLFLIB_API __declspec(dllexport)
#else
#define RUDOLFLIB_API __declspec(dllimport)
#endif

#ifdef __cplusplus
extern "C" {
#endif

	// Initializes internal face processor
	RUDOLFLIB_API void Initialize();

	// Releases internal resources
	RUDOLFLIB_API void Release();

	// Processes BGR image (unsigned char* data), modifies in-place
	RUDOLFLIB_API void ProcessImage(unsigned char* data, int width, int height, int channels);

#ifdef __cplusplus
}
#endif
