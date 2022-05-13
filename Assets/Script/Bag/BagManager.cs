using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class DynamicData 
{
    public int jsonID;
    public int baseID;
    public int jsonCount;
}
public class JsonList 
{
    public List<DynamicData> jsonDatalist = new List<DynamicData>();
}
public class BagManager
{
    public static object _object = new object();
    public static BagManager _instance;
    public static BagManager instance
    {
        get
        {
            if (_instance == null)
            {
                lock (_object)
                {
                    if (_instance == null)
                    {
                        _instance = new BagManager();
                    }
                }
            }
            return _instance;
        }
    }

    public List<Seeds> seedsList=new List<Seeds>();

    public JsonList jsonList = new JsonList();

    public BagManager() 
    {
        GetBagData();
    }
    public void GetBagData() 
    {
        seedsList = new List<Seeds>();
        jsonList = new JsonList();
        RefreshBagData();
    }
    public void RefreshBagData()
    {
        List<Seeds> tempSeed = new List<Seeds>();
        for (int i = 0; i < jsonList.jsonDatalist.Count; i++)
        {
            if (GetCSVdata.instance.isOver) 
            {
                Seeds temp = new Seeds(jsonList.jsonDatalist[i].baseID);
                temp.ID = jsonList.jsonDatalist[i].jsonID;
                temp.count = jsonList.jsonDatalist[i].jsonCount;
                tempSeed.Add(temp);
            }
        }
        seedsList = tempSeed;
    }
}
