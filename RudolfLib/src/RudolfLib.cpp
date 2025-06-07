#include "RudolfLib.h"

#include <opencv2/opencv.hpp>
#include "FaceResult.pb.h"

void Initialize() {
    // 초기화 필요시 구현
}

void Release() {
    // 리소스 해제 필요시 구현
}

void ProcessImage(unsigned char* data, int width, int height, int channels) {
    cv::Mat img(height, width, CV_8UC3, data);
    cv::circle(img, cv::Point(width / 2, height / 2), 30, cv::Scalar(0, 0, 255), -1);
}

int GetFaceResultSerialized(
    unsigned char* image_data,
    int width,
    int height,
    unsigned char* out_buffer,
    int max_len
) {
    rudolf::FaceResult result;
    result.set_nose_x(static_cast<float>(width) / 2);
    result.set_nose_y(static_cast<float>(height) / 2);
    result.set_confidence(0.95f);
    result.set_face_found(true);  // 필드명 정확히 확인

    std::string serialized;
    if (!result.SerializeToString(&serialized)) return -1;
    if (serialized.size() > static_cast<size_t>(max_len)) return -2;

    memcpy(out_buffer, serialized.data(), serialized.size());
    return static_cast<int>(serialized.size());
}
