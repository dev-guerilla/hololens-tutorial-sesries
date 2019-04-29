#pragma once

// Use this macros just to keep track of arguments we are using
#define _IN_
// Use this macros just to keep track of arguments we are using
#define _OUT_

namespace CVPlugin
{
	// Our income frame data
	typedef struct FRAME_DATA
	{
		int width;
		int height;
		int channels;
		unsigned char * pixels;
	} FrameData;

	// Our first processing function
	extern "C" void __declspec(dllexport) _stdcall
	processFrameCanny(_IN_ FrameData *, _OUT_ unsigned char *);
}
