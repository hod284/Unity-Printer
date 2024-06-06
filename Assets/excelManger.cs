using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.Text;
using System.Diagnostics;

public class excelManger : MonoBehaviour
{
    List<List<string>> data = new List<List<string>>();
    public string folderfilePath;
    private string filePath;
    // Start is called before the first frame update
    void Start()
    {
        //overwrriteexcell(filePath);
        makeeexcel("showing.xlsx");
        Print();
    }

    public void overwrriteexcell(string filePath)
    {
        if (File.Exists(filePath) == false)
        {
            UnityEngine.Debug.LogError("File not found: " + filePath);
            return;
        }
        using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.ReadWrite))
        {
            // Load the workbook into memory
            IWorkbook workbook = new XSSFWorkbook(fileStream);

            // Get the first sheet
            ISheet sheet = workbook.GetSheetAt(0);

            // Modify data in the Excel sheet
            IRow row = sheet.GetRow(1); // Assuming the data to modify is in the second row
            if (row != null)
            {
                row.GetCell(1).SetCellValue(400000000); // Assuming we are modifying the age
            }

      

            // Create a new memory stream to write the workbook content
            using (MemoryStream memoryStream = new MemoryStream())
            {
                // Write the workbook content to the memory stream
                workbook.Write(memoryStream);
               
                // Save the memory stream content to the original file
                File.WriteAllBytes(filePath, memoryStream.ToArray());
            }
        }
    }

    public void makeeexcel(string filename)
    {
        if (!Directory.Exists(folderfilePath))
        {
            Directory.CreateDirectory(folderfilePath);
        }
        filePath = folderfilePath + filename;
        // Create a new Excel workbook
        IWorkbook workbook = new XSSFWorkbook();
        
        // Create a new Excel sheet
        ISheet sheet = workbook.CreateSheet("Sheet1");
        sheet.IsPrintGridlines = true;
      
        // Create header row
        IRow headerRow = sheet.CreateRow(0);
        headerRow.CreateCell(0).SetCellValue("Name");
        headerRow.CreateCell(1).SetCellValue("Age");
        headerRow.CreateCell(2).SetCellValue("Country");

        // Sample data
        string[] names = { "John", "Alice", "Bob" };
        int[] ages = { 30, 25, 35 };
        string[] countries = { "USA", "UK", "Canada" };

        // Write data to the Excel sheet
        for (int i = 0; i < names.Length; i++)
        {
            IRow row = sheet.CreateRow(i + 1);
            row.CreateCell(0).SetCellValue(names[i]);
            row.CreateCell(1).SetCellValue(ages[i]);
            row.CreateCell(2).SetCellValue(countries[i]);
        }

        // Write the workbook content to a file
        using (FileStream fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
        {
            workbook.Write(fileStream);
        }

        UnityEngine.Debug.Log("Excel file created at: " + filePath);
    }
    public void overrwrrite2(string filePath)
    {
        // Create a new Excel workbook
        IWorkbook workbook = new XSSFWorkbook();

        // Create a new Excel sheet
        ISheet sheet1 = workbook.CreateSheet("Sheet1");

        // Write some data to the Excel sheet
        sheet1.CreateRow(0).CreateCell(0).SetCellValue("Hello");
        sheet1.GetRow(0).CreateCell(1).SetCellValue("World");
        
        // Write the workbook content to a file
        using (FileStream fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
        {
            workbook.Write(fileStream);
        }

    }
    private void CreateExcelFile(string filename)
    {
        StringBuilder sb = new StringBuilder();
        string delimiter = ",";
        for (int i = 0; i < data.Count; i++)
        {
            string line = string.Empty;
            for (int j = 0; j < data[i].Count; j++)
            {
                line += data[i][j];
                if (j != data[i].Count - 1)
                    line += delimiter;
            }
            sb.AppendLine(line);
        }

        if (!Directory.Exists(folderfilePath))
        {
            Directory.CreateDirectory(folderfilePath);
        }

        StreamWriter outStream = System.IO.File.CreateText(folderfilePath+filename);
        outStream.Write(sb);
        outStream.Close();
    }
    private void Print()
    {
        ProcessStartInfo startInfo = new ProcessStartInfo
        {
            FileName = filePath,
            
            Verb = "Print"
        };
        Process.Start(startInfo);
    }
}
