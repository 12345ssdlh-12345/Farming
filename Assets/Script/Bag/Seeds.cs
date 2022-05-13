using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seeds 
{
    public int ID;
    public int count;

    #region ��̬����
    public string name;
    public string description;
    public string[] Prefab;
    public string picPath;
    public float firstStageTime;
    public float secondStageTime;
    public float thirdStageTime;
    public int price; 
    #endregion
    public Seeds(int dataID) 
    {
        this.name = GetCSVdata.instance.CSVinfo[dataID].name;
        this.description = GetCSVdata.instance.CSVinfo[dataID].description;
        this.Prefab = GetCSVdata.instance.CSVinfo[dataID].Prefab;
        this.picPath = GetCSVdata.instance.CSVinfo[dataID].picPath;
        this.firstStageTime = GetCSVdata.instance.CSVinfo[dataID].firstStageTime;
        this.secondStageTime = GetCSVdata.instance.CSVinfo[dataID].secondStageTime;
        this.thirdStageTime = GetCSVdata.instance.CSVinfo[dataID].thirdStageTime;
        this.price = GetCSVdata.instance.CSVinfo[dataID].price;
    }
    //��ȡ����Ӧ���ӵ�����ģ��
    public List<GameObject> GetPlantPrefab(string[] prefab) 
    {
        List<GameObject> prefabList = new List<GameObject>();
        for (int i = 0; i < prefab.Length; i++)
        {
            GameObject go = (GameObject)Resources.Load("Prefab/" + prefab[i]);
            prefabList.Add(go);
        }
        return prefabList;
    }
    //��ȡ��ͬ���ӵĳɳ�ʱ��
    public List<int> GetPlantGrowthTime() 
    {
        List<int> growthTime = new List<int>();
        growthTime.Add((int)this.firstStageTime);
        growthTime.Add((int)this.secondStageTime);
        growthTime.Add((int)this.thirdStageTime);
        return growthTime;
    }
}

