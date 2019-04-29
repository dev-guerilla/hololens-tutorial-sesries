#include "pch.h"
#include "DevGuerillaCVPlugin.h"

#include <opencv2/core.hpp>
#include <opencv2/imgproc.hpp>

/// For any module we use in our code we need to include necessary dll to our unity project 
#pragma comment(lib, "opencv_core341.lib")
#pragma comment(lib, "opencv_imgproc341.lib")

namespace CVPlugin
{
	using namespace cv;

	extern "C" void __declspec(dllexport) _stdcall
	processFrameCanny(_IN_ FrameData * indata, _OUT_ unsigned char * outdata)
	{
		// Fill in Mat objects
		Mat inmat(Size(indata->width, indata->height), CV_8UC4, &indata->pixels[0]);
		Mat outmat(Size(indata->width, indata->height), CV_8UC1, &outdata[0]);

		// Convert to grayscale
		cvtColor(inmat, outmat, COLOR_RGBA2GRAY);
		// Blur out image
		GaussianBlur(outmat, outmat, Size(5, 5), 0, 0);
		// Extract contours
		Canny(outmat, outmat, 20, 50 * 4, 3);
	}
}
