using System;
using UnityEngine;
using System.Windows.Forms;
using System.Drawing.Printing;
using System.IO;

public class PrinterManager : MonoBehaviour
{
    public string m_ScreenCapturePath; // 디렉토리 이름과 파일 이름에 빈칸 없이 생성 
    public string m_ScreenCaptureFilePrefix = "CoderZero"; // 디렉토리 이름과 파일 이름에 빈칸 없이 생성 
    public string screenshotFilePath;
    // 카메라
    public Camera screenshotCamera ;
    int textureWidth = UnityEngine.Device.Screen.width; // 렌더 텍스처의 가로 크기
    int textureHeight = UnityEngine.Device.Screen.height; // 렌더 텍스처의 세로 크기

    // 프린트할 때의 해상도 설정
    public int printResolution = 300;

    // Update is called once per frame
    void Start()
    {
       // TakeScreenshot();
        //Capture(m_ScreenCapturePath, m_ScreenCaptureFilePrefix);
        TakeScreenshot_snap();
        PrintDIalog_winforms();
    }
  
    private void TakeScreenshot()
    {
        if(!File.Exists(m_ScreenCapturePath))
            File.Create(m_ScreenCapturePath);
        // 스크린샷 찍기
        if (screenshotCamera != null)
        {
            // 현재 렌더러 텍스처를 가져와 스크린샷 찍기
            RenderTexture renderTexture = screenshotCamera.targetTexture;
            Texture2D screenshotTexture = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.RGB24, false);
            screenshotCamera.Render();
            RenderTexture.active = renderTexture;
            screenshotTexture.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
            RenderTexture.active = null;
            // PNG 형식으로 스크린샷 저장
            byte[] bytes = screenshotTexture.EncodeToPNG();
            File.WriteAllBytes(screenshotFilePath, bytes);

            Debug.Log("Screenshot saved to: " + screenshotFilePath);
        }
        else
        {
            Debug.LogError("Screenshot camera is not assigned!");
        }
    }
  

    private void Capture(string filePath, string filePrefix = "")
    {
        // ① 캡처 후 저장 할 디렉토리가 존재하지 않으면 디렉토리 생성 
        DirectoryInfo directoryInfo = new DirectoryInfo(filePath);

        if (!directoryInfo.Exists)
        {
            directoryInfo.Create();
        }

        // ② 캡처 
        string m_ScreenCaptureFilePath = filePath + filePrefix + DateTime.Now.ToString("yyyyMMddhhmmss") + ".png";
        ScreenCapture.CaptureScreenshot(m_ScreenCaptureFilePath);
    }

    private void PrintDIalog_winforms()
    {
        PrintDialog printDialog = new PrintDialog();
        PrinterSettings printerSettings = new PrinterSettings();
        // 대화상자를 모달로 표시하여 사용자가 닫을 때까지 기다립니다.
        DialogResult result = printDialog.ShowDialog();
        printDialog.PrinterSettings = printerSettings;
        // 사용자가 OK를 클릭했을 때
        if (result == DialogResult.OK)
        {
            System.Drawing.Image image = System.Drawing.Image.FromFile(screenshotFilePath);

            // 프린터에 이미지 출력
            PrintDocument printDocument = new PrintDocument();
            printDocument.PrintPage += (sender, e) =>
            {
                e.Graphics.DrawImage(image, e.MarginBounds);
            };
            printDocument.PrinterSettings = printerSettings;
            printDocument.DefaultPageSettings.PrinterResolution = new PrinterResolution()
            {
                X = printResolution,
                Y = printResolution
            };
            printDocument.Print();
        }
        // 사용자가 취소를 클릭했을 때
        else if (result == DialogResult.Cancel)
        {
            Console.WriteLine("Printing canceled.");
        }

    }
    private void TakeScreenshot_snap()
    {
        if (!Directory.Exists(m_ScreenCapturePath))
        {
            Directory.CreateDirectory(m_ScreenCapturePath);
        }
        // 스크린샷 찍기
        if (screenshotCamera != null)
        {
            screenshotCamera.rect = new Rect(0,0,1,1);
            screenshotCamera.targetTexture = new RenderTexture(textureWidth, textureHeight, 24);
            // 카메라 렌더링
            screenshotCamera.Render();

            // 현재 렌더러 텍스처를 가져와 스크린샷 찍기
            Texture2D screenshotTexture = new Texture2D(500, 500, TextureFormat.RGB24, false);

            // RenderTexture에서 Texture2D로 변환
            RenderTexture.active = screenshotCamera.targetTexture;
            screenshotTexture.ReadPixels(new Rect(textureWidth -500, textureHeight -500,500,500), 0, 0,false);
            // RenderTexture 및 카메라 설정 초기화
            RenderTexture.active = null;
            screenshotCamera.targetTexture = null;
            // PNG 형식으로 스크린샷 저장
            byte[] bytes = screenshotTexture.EncodeToJPG();
            File.WriteAllBytes(m_ScreenCapturePath+ screenshotFilePath, bytes);

            Debug.Log("Screenshot saved to: " + m_ScreenCapturePath+ screenshotFilePath);
            screenshotFilePath = m_ScreenCapturePath + screenshotFilePath;
        }
        else
        {
            Debug.LogError("Screenshot camera is not assigned!");
        }
    }

}
