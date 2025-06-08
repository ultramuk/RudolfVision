#pragma once
#include <opencv2/opencv.hpp>
#include <dlib/image_processing/frontal_face_detector.h>
#include <dlib/image_processing.h>
#include <dlib/opencv.h>

namespace rudolf {

    class FaceOverlayProcessor {
    public:
        FaceOverlayProcessor();
        ~FaceOverlayProcessor();

        void process(unsigned char* data, int width, int height, int channels);

    private:
        dlib::frontal_face_detector face_detector_;
        dlib::shape_predictor landmark_predictor_;
        cv::Mat nose_overlay_;

        void overlayImage(cv::Mat& background, const cv::Mat& foreground, cv::Point location);

        static constexpr const char* kLandmarkModelPath = "Assets/shape_predictor_68_face_landmarks.dat";
        static constexpr const char* kNoseImagePath = "Assets/rudolf_nose.png";
    };

} // namespace rudolf
