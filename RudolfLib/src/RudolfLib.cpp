#include "pch.h"
#include "RudolfLib.h"
#include <opencv2/opencv.hpp>

void Initialize() {

}

void Release() {

}

void ProcessImage(unsigned char* data, int width, int height, int channels) {
	cv::Mat img(height, width, CV_8UC3, data);
	cv::circle(img, cv::Point(width / 2, height / 2), 30, cv::Scalar(0, 0, 255), -1);
}