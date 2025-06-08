# RudolfVision - Face Detection with Rudolph Nose Effect

�絹��ó�� �� �ν� �� �ڿ� ���� ���� �ռ����ִ� Windows ��� �� �ν� ���ø����̼��Դϴ�.  
��ķ �Ǵ� �̹��� �Է��� ���� ���� �ǽð����� �ν��ϰ�, dlib landmark�� ������� �� ��ġ�� ���� ǥ���մϴ�.

---

## Folder Structure
```
RudolfVision/
������ RudolfApp/                       # C# WPF ���ø����̼�
��   ������ View/                        # XAML UI
��   ������ ViewModel/                  # MVVM ViewModel
��   ������ Model/                      # �� �ν� ��� ��
��   ������ Services/                   # ��ķ ó�� ����
��   ������ Utils/                      # RelayCommand �� ��ƿ��Ƽ
��   ������ Assets/                     # UI ���ҽ� (��: sample.png)
��   ������ bin/
��       ������ Release/
��           ������ net8.0-windows/     # ���� ���� ���� ��ġ
��               ������ RudolfApp.exe
��               ������ RudolfLib.dll
��               ������ opencv_world455.dll
��               ������ haarcascade_frontalface_default.xml
��               ������ rudolf_nose.png
��               ������ RudolfApp.runtimeconfig.json
��               ������ RudolfApp.deps.json
��               ������ ��Ÿ ���� ���� DLL �� ���ϵ�

������ RudolfLib/                      # C++ �� �ν� DLL ������Ʈ
��   ������ include/                    # ���� ��� (RudolfLib.h)
��   ������ src/                        # ���� ���� (RudolfLib.cpp, dllmain.cpp ��)
��   ������ Assert/                     # ���ҽ� ���� (xml, png)
��   ������ build/                      # CMake ���� �����
��       ������ Release/
��           ������ RudolfLib.dll

������ 3rdparty/                       # �ܺ� ���̺귯�� ���� ����
��   ������ opencv/                     # OpenCV Windows�� ���̳ʸ�
��   ������ dlib/                       # dlib �ҽ� �� ����

������ scripts/                        # (����) ���� �� ���� �ڵ�ȭ ��ũ��Ʈ
������ README.md                       # ���� ��� �ȳ� ����
������ CMakeLists.txt                  # RudolfLib�� CMake ����


```

## How to Run

### 1. ���� ���� ��ġ�� �̵�
�Ʒ� ������ �̵��մϴ�
`RudolfVision/RudolfAPP/bin/Release/net8.0-windows`

### 2. ���α׷� ����
**`RudolfApp.exe`**. ������ ���� Ŭ���Ͽ� �����մϴ�

### 3. ��� ������ ���:
- **�̹��� �ҷ�����**: ���� �̹������� �� �ν� ����
- **��ķ ����**: �ǽð� ���󿡼� �� �ν�

### 4. �νĵ� �� ��ġ�� ���� ���� ǥ�õ˴ϴ� (�絹�� ȿ��).

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

- �� ������Ʈ�� ������ ���� ���� �� �н������� �ۼ��Ǿ����ϴ�.
- OpenCV �� dlib�� �� ���̺귯���� ���̼����� �����ϴ�.