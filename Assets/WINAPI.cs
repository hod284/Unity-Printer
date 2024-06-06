using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class WINAPI : MonoBehaviour
{
        // Windows API ȣ���� ���� ��� �� �޼��� ����
    private const int PD_RETURNDEFAULT = 0x00000400;
    private const int PD_RETURNDC = 0x00000100;
    private const int PD_USEDEVMODECOPIES = 0x00040000;
 

    // ����Ʈ�� ���� �ػ� ����
    public int printResolution = 300;

    [DllImport("comdlg32.dll", CharSet = CharSet.Auto)]
    private static extern bool PrintDlg(ref PRINTDLG lpPrintDlg);

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    private struct PRINTDLG
    {
        public int lStructSize;
        public IntPtr hwndOwner;
        public IntPtr hDevMode;
        public IntPtr hDevNames;
        public IntPtr hDC;
        public int Flags;
        public short nFromPage;
        public short nToPage;
        public short nMinPage;
        public short nMaxPage;
        public short nCopies;
        public IntPtr hInstance;
        public IntPtr lCustData;
        public IntPtr lpfnPrintHook;
        public IntPtr lpfnSetupHook;
        public IntPtr lpPrintTemplateName;
        public IntPtr lpSetupTemplateName;
        public IntPtr hPrintTemplate;
        public IntPtr hSetupTemplate;
    }

    // ����Ƽ���� ȣ���� �޼���
    public void OpenPrintDialog()
    {
        PRINTDLG pd = new PRINTDLG();
        pd.lStructSize = Marshal.SizeOf(typeof(PRINTDLG));
        pd.Flags = PD_RETURNDC | PD_RETURNDEFAULT | PD_USEDEVMODECOPIES;

        if (PrintDlg(ref pd))
        {
            // ������ �����͸� ����Ͽ� �μ� �۾��� ������ �� �ֽ��ϴ�.
            Debug.Log("Printer selected.");
        }
        else
        {
            Debug.Log("Printing canceled.");
        }
    }


    // Update is called once per frame
    void Start()
    {
        OpenPrintDialog();
      
    }  
  
    

}
