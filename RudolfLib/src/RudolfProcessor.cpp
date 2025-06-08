#include "pch.h"
#include "RudolfProcessor.h"
#include <iostream>
#include <opencv2/core/ocl.hpp>

namespace rudolf {

    FaceOverlayProcessor::FaceOverlayProcessor() {
        cv::ocl::setUseOpenCL(false);
        face_detector_ = dlib::get_frontal_face_detector();

        try {
            dlib::deserialize(kLandmarkModelPath) >> landmark_predictor_;
        }
        catch (const std::exception& e) {
            std::cerr << "Failed to load landmark model: " << e.what() << std::endl;
        }

        nose_overlay_ = cv::imread(kNoseImagePath, cv::IMREAD_UNCHANGED);
        if (nose_overlay_.empty()) {
            std::cerr << "Failed to load nose image." << std::endl;
        }
    }

    FaceOverlayProcessor::~FaceOverlayProcessor() {
        nose_overlay_.release();
    }

    void FaceOverlayProcessor::process(unsigned char* data, int width, int height, int channels) {
        if (!data || width <= 0 || height <= 0 || channels != 3) {
            std::cerr << "Invalid input image data." << std::endl;
            return;
        }

        cv::Mat img(height, width, CV_8UC3, data);

        if (img.empty() || nose_overlay_.empty()) return;

        dlib::cv_image<dlib::bgr_pixel> dlib_img(img);
        std::vector<dlib::rectangle> faces = face_detector_(dlib_img);
        if (faces.empty()) return;

        dlib::full_object_detection shape = landmark_predictor_(dlib_img, faces[0]);
        dlib::point nose_tip = shape.part(30);

        int desired_size = std::max(10, static_cast<int>(faces[0].width() * 0.3));
        cv::Mat resized_nose;
        cv::resize(nose_overlay_, resized_nose, cv::Size(desired_size, desired_size), 0, 0, cv::INTER_LINEAR);

        cv::Point top_left(nose_tip.x() - resized_nose.cols / 2, nose_tip.y() - resized_nose.rows / 2);
        overlayImage(img, resized_nose, top_left);
    }

    void FaceOverlayProcessor::overlayImage(cv::Mat& background, const cv::Mat& foreground, cv::Point location) {
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

} // namespace rudolf
