using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum landState 
{
    canReclaim,
    canPlant,
    needWater,
    growth_1,
    growth_2,
    growth_3,
    mature,
}
public class Land 
{
    public int ID;
    public landState state;
    public Location landPos;
    public int landPrefab;
    public int seedID;
    public int growthTime;
    public int startTime;
    public int realTime;
}
public class Location 
{
    public int x;
    public int y;
    public int z;
}
public class LandList 
{
    public List<Land> landList = new List<Land>();
}
public class FieldManager
{
    public static object _object = new object();
    public static FieldManager _instance;
    public static FieldManager instance
    {
        get
        {
            if (_instance == null)
            {
                lock (_object)
                {
                    if (_instance == null)
                    {
                        _instance = new FieldManager();
                    }
                }
            }
            return _instance;
        }
    }
    public List<Land> allLandList= new List<Land>();
    public LandList lands = new LandList();

    public FieldManager() 
    {
        GetLandData();
    }

    public void GetLandData() 
    {
        List<Land> temp = new List<Land>();
        for (int i = 0; i < lands.landList.Count; i++)
        {
            Land _land = lands.landList[i];
            temp.Add(_land);
        }
        allLandList = new List<Land>(temp);
    }
}
