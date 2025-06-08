#include "pch.h"
#include "RudolfLib.h"
#include <opencv2/opencv.hpp>
#include <opencv2/core/ocl.hpp>
#include <vector>
#include <iostream>

#define DLIB_NO_LINKING
#define DLIB_NO_LINK_TO_DEBUG_LIBRARIES
#define DLIB_NO_AUTOLINK
#define DLIB_DISABLE_ASSERTS
#define DLIB_NO_WARNINGS

#include <dlib/image_processing/frontal_face_detector.h>
#include <dlib/image_processing.h>
#include <dlib/opencv.h>

static dlib::frontal_face_detector face_detector;
static dlib::shape_predictor landmark_predictor;
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

    face_detector = dlib::get_frontal_face_detector();

    try {
        dlib::deserialize("Assets/shape_predictor_68_face_landmarks.dat") >> landmark_predictor;
    }
    catch (const std::exception& e) {
        std::cerr << "landmark �� �ε� ����: " << e.what() << std::endl;
    }

    nose_overlay = cv::imread("rudolf_nose.png", cv::IMREAD_UNCHANGED); // 4ä�� PNG
    if (nose_overlay.empty()) {
        std::cerr << "�絹�� �� �̹��� �ε� ����" << std::endl;
    }
}

void Release() {
    nose_overlay.release();
}

void ProcessImage(unsigned char* data, int width, int height, int channels) {
    if (data == nullptr || width <= 0 || height <= 0 || channels != 3) {
        std::cerr << "�Է� �����Ͱ� ��ȿ���� ����" << std::endl;
        return;
    }

    cv::Mat img(height, width, CV_8UC3, data);

    if (img.empty()) {
        std::cerr << "empty frame" << std::endl;
        return;
    }

    if (nose_overlay.empty()) return;

    // dlib �̹����� ��ȯ
    dlib::cv_image<dlib::bgr_pixel> dlib_img(img);
    std::vector<dlib::rectangle> faces = face_detector(dlib_img);

    if (faces.empty()) return;

    // ù �󱼿� ���ؼ��� ó��
    dlib::full_object_detection shape = landmark_predictor(dlib_img, faces[0]);
    dlib::point nose_tip = shape.part(30);

    int desired_size = static_cast<int>(faces[0].width() * 0.3);
    if (desired_size < 10) desired_size = 10;

    cv::Mat resized_nose;
    cv::resize(nose_overlay, resized_nose, cv::Size(desired_size, desired_size), 0, 0, cv::INTER_LINEAR);

    cv::Point top_left(
        nose_tip.x() - resized_nose.cols / 2,
        nose_tip.y() - resized_nose.rows / 2
    );

    OverlayImage(img, resized_nose, top_left);
}
