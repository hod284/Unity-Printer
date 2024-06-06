using System;
using UnityEngine;
using System.Windows.Forms;
using System.Drawing.Printing;
using System.IO;

public class PrinterManager : MonoBehaviour
{
    public string m_ScreenCapturePath; // ���丮 �̸��� ���� �̸��� ��ĭ ���� ���� 
    public string m_ScreenCaptureFilePrefix = "CoderZero"; // ���丮 �̸��� ���� �̸��� ��ĭ ���� ���� 
    public string screenshotFilePath;
    // ī�޶�
    public Camera screenshotCamera ;
    int textureWidth = UnityEngine.Device.Screen.width; // ���� �ؽ�ó�� ���� ũ��
    int textureHeight = UnityEngine.Device.Screen.height; // ���� �ؽ�ó�� ���� ũ��

    // ����Ʈ�� ���� �ػ� ����
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
        // ��ũ���� ���
        if (screenshotCamera != null)
        {
            // ���� ������ �ؽ�ó�� ������ ��ũ���� ���
            RenderTexture renderTexture = screenshotCamera.targetTexture;
            Texture2D screenshotTexture = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.RGB24, false);
            screenshotCamera.Render();
            RenderTexture.active = renderTexture;
            screenshotTexture.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
            RenderTexture.active = null;
            // PNG �������� ��ũ���� ����
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
        // �� ĸó �� ���� �� ���丮�� �������� ������ ���丮 ���� 
        DirectoryInfo directoryInfo = new DirectoryInfo(filePath);

        if (!directoryInfo.Exists)
        {
            directoryInfo.Create();
        }

        // �� ĸó 
        string m_ScreenCaptureFilePath = filePath + filePrefix + DateTime.Now.ToString("yyyyMMddhhmmss") + ".png";
        ScreenCapture.CaptureScreenshot(m_ScreenCaptureFilePath);
    }

    private void PrintDIalog_winforms()
    {
        PrintDialog printDialog = new PrintDialog();
        PrinterSettings printerSettings = new PrinterSettings();
        // ��ȭ���ڸ� ��޷� ǥ���Ͽ� ����ڰ� ���� ������ ��ٸ��ϴ�.
        DialogResult result = printDialog.ShowDialog();
        printDialog.PrinterSettings = printerSettings;
        // ����ڰ� OK�� Ŭ������ ��
        if (result == DialogResult.OK)
        {
            System.Drawing.Image image = System.Drawing.Image.FromFile(screenshotFilePath);

            // �����Ϳ� �̹��� ���
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
        // ����ڰ� ��Ҹ� Ŭ������ ��
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
        // ��ũ���� ���
        if (screenshotCamera != null)
        {
            screenshotCamera.rect = new Rect(0,0,1,1);
            screenshotCamera.targetTexture = new RenderTexture(textureWidth, textureHeight, 24);
            // ī�޶� ������
            screenshotCamera.Render();

            // ���� ������ �ؽ�ó�� ������ ��ũ���� ���
            Texture2D screenshotTexture = new Texture2D(500, 500, TextureFormat.RGB24, false);

            // RenderTexture���� Texture2D�� ��ȯ
            RenderTexture.active = screenshotCamera.targetTexture;
            screenshotTexture.ReadPixels(new Rect(textureWidth -500, textureHeight -500,500,500), 0, 0,false);
            // RenderTexture �� ī�޶� ���� �ʱ�ȭ
            RenderTexture.active = null;
            screenshotCamera.targetTexture = null;
            // PNG �������� ��ũ���� ����
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
