using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System.IO;

public class JsonManager
{
    string Seedpath = Application.dataPath + "/Resources/ItemData/seedData.json";
    string Landpath = Application.dataPath + "/Resources/LandData/land.json";
    string Timepath = Application.dataPath + "/Resources/TimeData/time.json";
    string Shoppath = Application.dataPath + "/Resources/ShopData/shop.json";

    public static object _object = new object();
    public static JsonManager _instance;

    public static JsonManager instance 
    {
        get 
        {
            if (_instance == null) 
            {
                lock (_object) 
                {
                    if (_instance == null) 
                    {
                        _instance = new JsonManager();
                    }
                }
            }
            return _instance;
        }
    }
    //��ȡ����������
    public void ReadBagData() 
    {
        if (!File.Exists(Seedpath)) 
        {
            return;
        }
        using (StreamReader sr = new StreamReader(new FileStream(Seedpath, FileMode.Open))) 
        {
            string json = sr.ReadToEnd();
            JsonList tempList = new JsonList();
            tempList = JsonMapper.ToObject<JsonList>(json);
            BagManager.instance.jsonList = tempList;
        }
        BagManager.instance.RefreshBagData();
    }
    //�洢����������
    public void SaveBagData() 
    {
        string json = JsonMapper.ToJson(BagManager.instance.jsonList);
        if (!File.Exists(Seedpath)) 
        {
            return;
        }
        using (StreamWriter sw=new StreamWriter(new FileStream(Seedpath,FileMode.Truncate))) 
        {
            sw.Write(json);
        }
    }
    //��ȡ�̵������
    public void ReadShopData() 
    {
        if (!File.Exists(Shoppath)) 
        {
            return;
        }
        using (StreamReader sr = new StreamReader(new FileStream(Shoppath, FileMode.Open))) 
        {
            string json = sr.ReadToEnd();
            shopList templist = new shopList();
            templist = JsonMapper.ToObject<shopList>(json);
            ShopManager.instance.shoplist = templist;
        }
        ShopManager.instance.RefreshShopData();
    }
    //�洢�̵������
    public void SaveShopData() 
    {
        string json = JsonMapper.ToJson(ShopManager.instance.shoplist);
        if (!File.Exists(Shoppath)) 
        {
            return;
        }
        using (StreamWriter sw = new StreamWriter(new FileStream(Shoppath, FileMode.Truncate))) 
        {
            sw.Write(json);
        }
    }
    //��ȡ���ص�json
    public void ReadLandData() 
    {
        if (!File.Exists(Landpath)) 
        {
            Debug.Log("�ļ�����");
            return;
        }
        using (StreamReader sr = new StreamReader(new FileStream(Landpath, FileMode.Open))) 
        {
            string json = sr.ReadToEnd();
            LandList templist = new LandList();
            templist = JsonMapper.ToObject<LandList>(json);
            FieldManager.instance.lands= templist;
        }
        FieldManager.instance.GetLandData();
    }
    //�洢���ص�����
    public void SaveLandData() 
    {
        string json = JsonMapper.ToJson(FieldManager.instance.lands);
        if (!File.Exists(Landpath)) 
        {
            return;
        }
        using (StreamWriter sw = new StreamWriter(new FileStream(Landpath, FileMode.Truncate))) 
        {
            sw.Write(json);
        }
    }
    //��ȡ��Ϸʱ�������
    public void ReadTimeData() 
    {
        if (!File.Exists(Timepath)) 
        {
            return;
        }
        using (StreamReader sr = new StreamReader(new FileStream(Timepath, FileMode.Open))) 
        {
            string json = sr.ReadToEnd();

            GameTime temp = new GameTime();
            temp = JsonMapper.ToObject<GameTime>(json);
            TimeManager.gameTime = temp;
        }
    }
    //�洢��Ϸʱ�������
    public void SaveTimeData() 
    {
        string json = JsonMapper.ToJson(TimeManager.gameTime);
        if (!File.Exists(Timepath)) 
        {
            return;
        }
        using (StreamWriter sw = new StreamWriter(new FileStream(Timepath, FileMode.Truncate))) 
        {
            sw.Write(json);
        }
    }
}
