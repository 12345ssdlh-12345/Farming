using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;


public class UIManager : MonoBehaviour
{
    public BagSeeds[] shopSeeds;//商店的种子数组
    public BagSeeds[] bagSeeds;//背包的种子组
    public GameObject SeedTemp;//种子的空白预制体
    public Transform[] shopBar;//商店的种子格
    public GameObject seedBar;//种子框
    public GameObject seedBag;//种子的背包（用于打开和隐藏）

    public List<BagSeeds> ShopActualList;
    public List<BagSeeds> BagActualList;
    private void Start()
    {
        SeedTemp = (GameObject)Resources.Load("seed");

        ReadJson();
        ReadShopJson();
    }
    private void Update()
    {
        
    }
    public void ShowShopData() 
    {
        List<Seeds> seedlist = new List<Seeds>();
        seedlist = ShopManager.instance.realDataList;
        for (int i = 0; i < seedlist.Count; i++)
        {
            if (seedlist == null || seedlist.Count == 0) 
            {
                Debug.Log("kongde");
                return;
            }
            for (int j = 0; j < shopBar.Length; j++)
            {
                if (shopBar[j].transform.GetChild(0).childCount == 0) 
                {
                    GameObject go = Instantiate(SeedTemp, shopBar[j].transform.GetChild(0));
                    BagSeeds bagseed = go.GetComponent<BagSeeds>();
                    go.GetComponent<RectTransform>().sizeDelta = new Vector2(90, 90);
                    bagseed.Count.GetComponent<RectTransform>().sizeDelta = new Vector2(45, 45);
                    bagseed.Count.GetComponent<Text>().fontSize = 40;
                    bagseed.Count.GetComponent<Text>().color = Color.white;
                    if (bagseed == null) 
                    {
                        continue;
                    }
                    bagseed.SetData(ShopManager.instance.realDataList[i]);
                    break;
                }
            }
        }
    }
    public void ReadShopJson() 
    {
        JsonManager.instance.ReadShopData();
        ShowShopData();
        shopSeeds = GetComponentsInChildren<BagSeeds>();
        for (int i = 0; i < shopSeeds.Length; i++)
        {
            foreach (var item in shopSeeds)
            {
                ShopActualList.Add(item);
            }
        }
    }
    public void ShowBagData() 
    {
        List<Seeds> seedInfo = new List<Seeds>();
        seedInfo = BagManager.instance.seedsList;
        for (int i = 0; i < seedInfo.Count; i++)
        {
            if (seedInfo == null || seedInfo.Count == 0)
            {
                return;
            }
            GameObject go = Instantiate(SeedTemp, seedBar.transform);
            BagSeeds bagSeed = go.GetComponent<BagSeeds>();
            if (bagSeed == null) 
            {
                continue;
            }
            bagSeed.SetData(BagManager.instance.seedsList[i]);
        }
    }
    public void ReadJson() 
    {
        JsonManager.instance.ReadBagData();
        ShowBagData();
        bagSeeds = GetComponentsInChildren<BagSeeds>();
        for (int i = 0; i < bagSeeds.Length; i++)
        {
            foreach (var item in bagSeeds)
            {
                BagActualList.Add(item);
            }
        }
    }
    public void Save() 
    {
        JsonManager.instance.SaveBagData();
        JsonManager.instance.SaveShopData();
    }
    public void SeedBagDown() 
    {
        seedBag.transform.DOMoveY(-200, 1f);
    }
    public void SeedBagUp() 
    {
        seedBag.transform.DOMoveY(0, 1f);
    }
}
