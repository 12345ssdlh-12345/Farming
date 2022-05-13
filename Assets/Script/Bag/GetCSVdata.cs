using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
public class CSVMessage
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
public class GetCSVdata
{
    public Dictionary<int, CSVMessage> CSVinfo = new Dictionary<int, CSVMessage>();
    public bool isOver = false;

    public static object _object = new object();
    public static GetCSVdata _instance;
    public static GetCSVdata instance
    {
        get
        {
            if (_instance == null)
            {
                lock (_object)
                {
                    if (_instance == null)
                    {
                        _instance = new GetCSVdata();
                    }
                }
            }
            return _instance;
        }
    }
    public GetCSVdata()
    {
        GetCSVData();
    }
    public void GetCSVData()
    {
        TextAsset seedData = Resources.Load<TextAsset>("ItemData/seed");
        if (seedData == null) 
        {
            Debug.Log("文件不存在");
            return;
        }
        int id = 0;
        string[] data = seedData.text.Split(new char[] { '\n' });
        for (int i = 1; i < data.Length; i++)
        {
            CSVMessage info = new CSVMessage();
            string[] oneRow = data[i].Split(new char[] { ',' });
            for (int j = 0; j < oneRow.Length; j++)
            {
                switch (j)
                {
                    case 0:
                        info.name = oneRow[j].ToString();
                        break;
                    case 1:
                        info.description = oneRow[j].ToString();
                        break;
                    case 2:
                        string tmep = oneRow[j].ToString();
                        info.Prefab = tmep.Split(new char[] { '|' });
                        break;
                    case 3:
                        info.picPath = oneRow[j].ToString();
                        break;
                    case 4:
                        info.firstStageTime = float.Parse(oneRow[j]);
                        break;
                    case 5:
                        info.secondStageTime = float.Parse(oneRow[j]);
                        break;
                    case 6:
                        info.thirdStageTime = float.Parse(oneRow[j]);
                        break;
                    case 7:
                        if (i == data.Length-1)
                        {
                            isOver = true;
                        }
                        info.price = int.Parse(oneRow[j]);
                        break;
                }
            }
            CSVinfo.Add(id, info);
            id += 1;
        }
    }
}