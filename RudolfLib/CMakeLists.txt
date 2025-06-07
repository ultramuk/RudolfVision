cmake_minimum_required(VERSION 3.16)
project(RudolfLib)

set(CMAKE_CXX_STANDARD 17)
set(CMAKE_WINDOWS_EXPORT_ALL_SYMBOLS ON)

add_library(RudolfLib SHARED
    src/RudolfLib.cpp
    src/dllmain.cpp
    generated/calculator.pb.cc
)

# Include directories
target_include_directories(RudolfLib PRIVATE
    ${CMAKE_CURRENT_SOURCE_DIR}/../3rdparty
    #${CMAKE_CURRENT_SOURCE_DIR}/../3rdparty/abseil-cpp
    ${CMAKE_CURRENT_SOURCE_DIR}/../3rdparty/opencv/include
    C:/protobuf-3.21.12/include
    ${CMAKE_CURRENT_SOURCE_DIR}/../3rdparty/glog/include
    ${CMAKE_CURRENT_SOURCE_DIR}/../3rdparty/eigen
    ${CMAKE_CURRENT_SOURCE_DIR}/../3rdparty/mediapipe
    ${CMAKE_CURRENT_SOURCE_DIR}/../3rdparty/mediapipe/third_party
    ${CMAKE_CURRENT_SOURCE_DIR}/include
    ${CMAKE_CURRENT_SOURCE_DIR}/generated
)

# Library directories
link_directories(
    ${CMAKE_CURRENT_SOURCE_DIR}/../3rdparty/opencv/x64/v15/lib
    C:/protobuf-3.21.12/lib
    ${CMAKE_CURRENT_SOURCE_DIR}/../3rdparty/glog/lib
)

# Link libraries
target_link_libraries(RudolfLib
    opencv_world455
    libprotobuf
    glog
)

# Define to suppress warnings
target_compile_definitions(RudolfLib PRIVATE _CRT_SECURE_NO_WARNINGS)
