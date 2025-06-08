#include "pch.h"
#include "RudolfLib.h"
#include <opencv2/opencv.hpp>
#include <opencv2/core/ocl.hpp>
#include <vector>
#include <iostream>

static cv::CascadeClassifier face_cascade;
static cv::Mat nose_overlay;

void OverlayImage(cv::Mat& background, const cv::Mat& foreground, cv::Point location) {
    CV_Assert(background.type() == CV_8UC3);
    CV_Assert(foreground.type() == CV_8UC4);

    for (int y = 0; y < foreground.rows; ++y) {
        int bgY = y + location.y;
        if (bgY < 0 || bgY >= background.rows) continue;

        for (int x = 0; x < foreground.cols; ++x) {
            int bgX = x + location.x;
            if (bgX < 0 || bgX >= background.cols) continue;

            cv::Vec4b fgPixel = foreground.at<cv::Vec4b>(y, x);
            cv::Vec3b& bgPixel = background.at<cv::Vec3b>(bgY, bgX);

            float alpha = fgPixel[3] / 255.0f;
            for (int c = 0; c < 3; ++c) {
                bgPixel[c] = static_cast<uchar>(
                    alpha * fgPixel[c] + (1.0f - alpha) * bgPixel[c]
                    );
            }
        }
    }
}

void Initialize() {
    cv::ocl::setUseOpenCL(false);

    if (!face_cascade.load("haarcascade_frontalface_default.xml")) {
        std::cerr << "얼굴 검출 모델 로드 실패" << std::endl;
    }

    nose_overlay = cv::imread("rudolf_nose.png", cv::IMREAD_UNCHANGED); // 4채널 PNG
    if (nose_overlay.empty()) {
        std::cerr << "루돌프 코 이미지 로드 실패" << std::endl;
    }
}

void Release() {
    face_cascade = cv::CascadeClassifier();
    nose_overlay.release();
}

void ProcessImage(unsigned char* data, int width, int height, int channels) {
    if (data == nullptr || width <= 0 || height <= 0 || channels != 3) {
        std::cerr << "입력 데이터가 유효하지 않음" << std::endl;
        return;
    }

    cv::Mat img(height, width, CV_8UC3, data);

    if (img.empty()) {
        std::cerr << "empty frame" << std::endl;
        return;
    }

    if (face_cascade.empty()) {
        std::cerr << "CascadeClassifier 로드 실패" << std::endl;
        return;
    }

    std::vector<cv::Rect> faces;
    face_cascade.detectMultiScale(img, faces);

    if (faces.empty() || nose_overlay.empty()) return;

    cv::Rect face = faces[0];
    cv::Point nose_center(
        face.x + face.width / 2,
        face.y + face.height * 3 / 5 
    );

    int desired_size = static_cast<int>(face.width * 0.3);
    if (desired_size < 10) desired_size = 10;

    cv::Mat resized_nose;
    cv::resize(nose_overlay, resized_nose, cv::Size(desired_size, desired_size), 0, 0, cv::INTER_LINEAR);

    cv::Point top_left(
        nose_center.x - resized_nose.cols / 2,
        nose_center.y - resized_nose.rows / 2
    );

    OverlayImage(img, resized_nose, top_left);
}
