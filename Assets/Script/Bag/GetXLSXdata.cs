using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OfficeOpenXml;
using System.IO;
public class XLSXMessage 
{
    public string name;
    public string description;
    public string[] Prefab;
    public string picPath;
    public float firstStageTime;
    public float secondStageTime;
    public float thirdStageTime;
    public int price;
}
public class GetXLSXdata
{
    public Dictionary<int, XLSXMessage> XLSXinfo = new Dictionary<int, XLSXMessage>();
    public bool isOver = false;

    public static object _object = new object();
    public static GetXLSXdata _instance;
    public static GetXLSXdata instance
    {
        get
        {
            if (_instance == null)
            {
                lock (_object)
                {
                    if (_instance == null)
                    {
                        _instance = new GetXLSXdata();
                    }
                }
            }
            return _instance;
        }
    }
    public GetXLSXdata() 
    {
        GetExcelData();
    }
    public void GetExcelData() 
    {
        string filePath = Application.dataPath + "/Resources/ItemData/seed.xlsx";
        FileInfo fileInfo = new FileInfo(filePath);
        if (fileInfo.Length == 0) 
        {
            Debug.Log("文件不存在");
            return;
        }
        using (ExcelPackage excel = new ExcelPackage(fileInfo))
        {
            ExcelWorksheet worksheet = excel.Workbook.Worksheets[1];
            int maxRow = worksheet.Dimension.End.Row;
            int maxColum = worksheet.Dimension.End.Column;
            int id = 0;
            for (int i = 2; i <= maxRow; i++)
            {
                XLSXMessage info = new XLSXMessage();
                for (int j = 1; j <= maxColum; j++)
                {
                    switch (j)
                    {
                        case 1:
                            info.name = worksheet.Cells[i, j].Value.ToString();
                            break;
                        case 2:
                            info.description = worksheet.Cells[i, j].Value.ToString();
                            break;
                        case 3:
                            string temp = worksheet.Cells[i, j].Value.ToString();
                            info.Prefab = temp.Split('，');
                            break;
                        case 4:
                            info.picPath = worksheet.Cells[i, j].Value.ToString();
                            break;
                        case 5:
                            string time1 = worksheet.Cells[i, j].Value.ToString();
                            info.firstStageTime = float.Parse(time1);
                            break;
                        case 6:
                            string time2 = worksheet.Cells[i, j].Value.ToString();
                            info.secondStageTime = float.Parse(time2);
                            break;
                        case 7:
                            string time3 = worksheet.Cells[i, j].Value.ToString();
                            info.thirdStageTime = float.Parse(time3);
                            break;
                        case 8:
                            string moeny = worksheet.Cells[i, j].Value.ToString();
                            info.price = int.Parse(moeny);
                            if (i == maxRow)
                            {
                                isOver = true;
                            }
                            break;
                        default:
                            break;
                    }
                }
                XLSXinfo.Add(id, info);
                Debug.Log(XLSXinfo[id].name + " " + XLSXinfo[id].description + " " + XLSXinfo[id].Prefab[0] + " " + XLSXinfo[id].picPath + " " + XLSXinfo[id].firstStageTime + " " + XLSXinfo[id].secondStageTime + " " + XLSXinfo[id].thirdStageTime + " " + XLSXinfo[id].price);
                id += 1;
            }
        }
    }
}
