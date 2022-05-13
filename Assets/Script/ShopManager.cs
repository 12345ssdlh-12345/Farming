using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ShopData 
{
    public int shopID;
    public int shopBaseID;
    public int shopCount;
}
public class shopList 
{
    public List<ShopData> shopdatalist = new List<ShopData>();
}
public class ShopManager
{
    public static object _object = new object();
    public static ShopManager _instance;

    public static ShopManager instance 
    {
        get 
        {
            if (_instance == null) 
            {
                lock (_object) 
                {
                    if (_instance == null) 
                    {
                        _instance = new ShopManager();
                    }
                }
            }
            return _instance;
        }
    }
    public List<Seeds> realDataList = new List<Seeds>();
    public shopList shoplist = new shopList();
    public ShopManager() 
    {
        GetShopData();
    }
    public void GetShopData() 
    {
        RefreshShopData();
    }
    public void RefreshShopData() 
    {
        List<Seeds> list = new List<Seeds>();
        for (int i = 0; i < shoplist.shopdatalist.Count; i++)
        {
            Seeds temp = new Seeds(shoplist.shopdatalist[i].shopBaseID);
            temp.ID = shoplist.shopdatalist[i].shopID;
            temp.count = shoplist.shopdatalist[i].shopCount;
            list.Add(temp);
        }
        realDataList = list;
    }
}
