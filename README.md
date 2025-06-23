# RudolfVision - Face Detection with Rudolph Nose Effect

루돌프처럼 얼굴 인식 후 코에 빨간 원을 합성해주는 Windows 기반 얼굴 인식 애플리케이션입니다.  
웹캠 또는 이미지 입력을 통해 얼굴을 실시간으로 인식하고, dlib landmark를 기반으로 코 위치에 원을 표시합니다.

---
## Result
### Webcam
https://github.com/user-attachments/assets/56c8bc6d-1704-44f3-b7d6-d54182c0b0c0

### Image load
![2](https://github.com/user-attachments/assets/785361b4-6760-4fd7-b646-7db21e6fa9fa)

---

## Folder Structure
```
RudolfVision/
├── RudolfApp/                       # C# WPF 애플리케이션
│   ├── View/                        # XAML UI
│   ├── ViewModel/                  # MVVM ViewModel
│   ├── Model/                      # 얼굴 인식 결과 모델
│   ├── Services/                   # 웹캠 처리 서비스
│   ├── Utils/                      # RelayCommand 등 유틸리티
│   ├── Assets/                     # UI 리소스 (예: sample.png)
│   └── bin/
│       └── Release/
│           └── net8.0-windows/     # 최종 배포 파일 위치
│               ├── RudolfApp.exe
│               ├── RudolfLib.dll
│               ├── opencv_world455.dll
│               ├── haarcascade_frontalface_default.xml
│               ├── rudolf_nose.png
│               ├── RudolfApp.runtimeconfig.json
│               ├── RudolfApp.deps.json
│               └── 기타 실행 관련 DLL 및 파일들

├── RudolfLib/                      # C++ 얼굴 인식 DLL 프로젝트
│   ├── include/                    # 공개 헤더 (RudolfLib.h)
│   ├── src/                        # 구현 파일 (RudolfLib.cpp, dllmain.cpp 등)
│   ├── Assert/                     # 리소스 파일 (xml, png)
│   └── build/                      # CMake 빌드 결과물
│       └── Release/
│           └── RudolfLib.dll

├── 3rdparty/                       # 외부 라이브러리 수동 관리
│   ├── opencv/                     # OpenCV Windows용 바이너리
│   └── dlib/                       # dlib 소스 및 빌드

├── scripts/                        # (선택) 빌드 및 배포 자동화 스크립트
├── README.md                       # 실행 방법 안내 문서
└── CMakeLists.txt                  # RudolfLib용 CMake 설정


```

## 실행 파일 다운로드 및 실행 방법

### 1. 실행파일 다운로드

실행 파일이 포함된 패키지는 아래 링크에서 다운로드할 수 있습니다:

[RudolfVision 실행 파일 다운로드 (Google Drive)](https://drive.google.com/file/d/18sAc5CTCjf24uFaZz3wmZnW2HUblQ2bt/view?usp=sharing)

> 압축파일(`RudolfVision-Release.zip`)에는 실행에 필요한 모든 파일이 포함되어 있습니다.

### 2. 압축 해제 및 실행

1. 다운로드한 `.zip` 파일을 원하는 위치에 압축 해제합니다.
2. `RudolfApp.exe` 파일을 더블 클릭하여 실행합니다.
3. 다음 기능을 사용할 수 있습니다:
   - **이미지 불러오기**: 이미지에서 얼굴 인식 후 코에 빨간 원 표시
   - **웹캠 시작**: 실시간 얼굴 인식 후 코에 빨간 원 표시
---

## System Requirements

- **OS**: Windows 10 or later (64-bit)
- **.NET Runtime**: [.NET 8.0](https://dotnet.microsoft.com/en-us/download/dotnet/8.0) required
- **VC++ Redistributable**: Required if OpenCV DLLs fail to load

---

## Development Environment (for reference)

- **IDE**: Visual Studio 2022
- **Languages**: C# (.NET 8.0), C++17
- **Libraries**:
  - [OpenCV 4.5.5](https://opencv.org/)
  - [dlib 19.24](http://dlib.net/)
  - OpenCvSharp

---

## License

- 본 프로젝트는 비상업적 과제 제출 및 학습용으로 작성되었습니다.
- OpenCV 및 dlib는 각 라이브러리의 라이선스를 따릅니다.
