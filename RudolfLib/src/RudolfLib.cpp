#include "pch.h"
#include "RudolfProcessor.h"

static rudolf::FaceOverlayProcessor* g_processor = nullptr;

extern "C" __declspec(dllexport)
void Initialize() {
    if (!g_processor)
        g_processor = new rudolf::FaceOverlayProcessor();
}

extern "C" __declspec(dllexport)
void Release() {
    delete g_processor;
    g_processor = nullptr;
}

extern "C" __declspec(dllexport)
void ProcessImage(unsigned char* data, int width, int height, int channels) {
    if (g_processor)
        g_processor->process(data, width, height, channels);
}