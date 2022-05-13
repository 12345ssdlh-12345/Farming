using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Field : MonoBehaviour
{
    HighlightableObject arrow;//土地闪烁
    MeshFilter meshFilter;//土地的模型
    Transform center;//土地的中心点

    public List<int> PlantGrowingTime;//当前种下的种子的所有阶段需要的生长时间
    public List<GameObject> PlantModel;//当前种下的种子所有阶段的模型
    public newField newfield;//管理土地的脚本负责从json中实例化土地
    public GameObject canvas;//需要浇水的提醒

    public int ID;//土地的ID
    public int meshID;//土地的模型网格
    public int seedInLand;//玩家中下的种子
    public landState state;//土地当前的状态
    public Vector3 pos;//土地生成的位置
    public int startTime;//种下种子的时间
    public int plantedStage;//种下种子后的生长阶段标识

    Land land;//对应的FieldManager中的类


    void Awake()
    {
        arrow = GetComponent<HighlightableObject>();
        meshFilter = GetComponent<MeshFilter>();
        newfield = GameObject.Find("LandManger").GetComponent<newField>();
        center = transform.Find("Center");
    }
    void Update()
    {
        switch (state)
        {
            case landState.canReclaim:
                break;
            case landState.canPlant:
                break;
            case landState.needWater:
                if (center.childCount == 0)
                {
                    CreateSeedModel();
                }
                canvas.SetActive(true);
                break;
            case landState.growth_1:
                if (center.childCount == 0)
                {
                    CreateSeedModel();
                }
                growthFirstStage();
                break;
            case landState.growth_2:
                if (center.childCount == 0) 
                {
                    CreateSeedModel();
                }
                growthSecondStage();
                break;
            case landState.growth_3:
                if (center.childCount == 0)
                {
                    CreateSeedModel();
                }
                growthThirdStage();
                break;
            case landState.mature:
                if (center.childCount == 0)
                {
                    CreateSeedModel();
                }
                break;
        }
    }
    public void SetLandData(Land l)
    {
        if (l == null)
        {
            return;
        }
        this.land = l;
        this.ID = l.ID;
        this.meshID = l.landPrefab;
        this.seedInLand = l.seedID;
        this.state = l.state;
        this.pos = new Vector3(l.landPos.x, l.landPos.y, l.landPos.z);
        this.startTime = l.startTime;
        this.plantedStage = l.realTime;

        LoadMesh();

    }
    //更换田地的Mesh
    public void LoadMesh()
    {
        meshFilter.mesh = GetLandMesh(meshID);
    }
    //获取到田地的Mesh
    public Mesh GetLandMesh(int n)
    {
        newField f = GameObject.Find("LandManger").GetComponent<newField>();
        Mesh mesh = new Mesh();
        for (int i = 0; i < f.meshList.Count; i++)
        {
            if (i == n)
            {
                mesh = f.meshList[i];
            }
        }
        return mesh;
    }
    //开垦土地的操作
    public void ReclamaLand()
    {
        this.state = landState.canPlant;
        this.meshID = 1;
        GetLandMesh(meshID);
        LoadMesh();
        RefreshLandData();
        newfield.SaveLandJson();
    }
    //种植种子的操作
    public void PlantLand()
    {
        plantedStage = 0;
        this.state = landState.growth_1;
        this.startTime = (int)ComputatingTime();
        RefreshLandData();
        newfield.SaveLandJson();
    }
    //第一阶段的生长
    public void growthFirstStage() 
    {
        int currentTime = (int)ComputatingTime();
        if (currentTime - startTime >= PlantGrowingTime[plantedStage]) 
        {
            plantedStage = 1;
            state = landState.needWater; 
            RefreshLandData();
            newfield.SaveLandJson();
        }
    }
    //第二阶段的生长
    public void growthSecondStage() 
    {
        int currentTime = (int)ComputatingTime();
        if (currentTime - startTime >= PlantGrowingTime[plantedStage]) 
        {
            plantedStage = 2;
            state = landState.needWater;
            RefreshLandData();
            newfield.SaveLandJson();
        }
    }
    //第三阶段的生长
    public void growthThirdStage() 
    {
        int currentTime = (int)ComputatingTime();
        if (currentTime - startTime >= PlantGrowingTime[plantedStage]) 
        {
            state = landState.mature;
            RefreshLandData();
            newfield.SaveLandJson();
        }
    }
    //玩家收割成熟植株
    public void HarvestPlant() 
    {
        meshID = 0;
        state = landState.canReclaim;
        seedInLand = -1;
        startTime = 0;

        plantedStage = 0;
        DestroyOldSeedMode();
        LoadMesh();

        RefreshLandData();
        newfield.SaveLandJson();
    }
    //需要浇水的操作
    public void WaterLand() 
    {
        canvas.SetActive(false);
        if (plantedStage == 1)
        {
            DestroyOldSeedMode();
            startTime = (int)ComputatingTime();
            state = landState.growth_2;
        }
        else if (plantedStage == 2) 
        {
            DestroyOldSeedMode();
            startTime = (int)ComputatingTime();
            state = landState.growth_3;
        }
        RefreshLandData();
        newfield.SaveLandJson();
    }
    //生成对应阶段的种子预制体
    public void CreateSeedModel()
    {
        if (seedInLand == -1)
        {
            return;
        }
        Seeds seed = new Seeds(seedInLand);
        PlantGrowingTime = seed.GetPlantGrowthTime();
        PlantModel = seed.GetPlantPrefab(seed.Prefab);
        GameObject plant = Instantiate(PlantModel[plantedStage], center.position, center.rotation);
        plant.transform.parent = center;
    }
    //删除之前的预制体
    public void DestroyOldSeedMode()
    {
        if (center.childCount != 0)
        {
            for (int i = 0; i < center.childCount; i++)
            {
                Destroy(center.GetChild(i).gameObject);
            }
        }
    }
    //刷新FieldManager中的土地列表好写入json
    public void RefreshLandData()
    {
        FieldManager.instance.lands.landList[this.ID].landPrefab = this.meshID;
        FieldManager.instance.lands.landList[this.ID].state = this.state;
        FieldManager.instance.lands.landList[this.ID].seedID = this.seedInLand;
        FieldManager.instance.lands.landList[this.ID].startTime = this.startTime;
        FieldManager.instance.lands.landList[this.ID].realTime = this.plantedStage;
    }

    //计算当前的时间
    public long ComputatingTime() 
    {
        DateTime InitialTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(2021, 7, 2));
        long timeStamp = (long)(DateTime.Now - InitialTime).TotalSeconds;
        return timeStamp;
    }
    void OnTriggerStay(Collider collider)
    {
        if (collider.CompareTag("Player")) 
        {
            arrow.FlashingOn(Color.white, Color.red, 0.5f);
            collider.GetComponent<PlayerCharacter>().CurrentLand = this.gameObject;
            if (state == landState.canReclaim)
            {
                collider.GetComponent<PlayerCharacter>().behaviorID = 1;
                collider.GetComponent<PlayerCharacter>().canReclaim = true;
            }
            else if (state == landState.canPlant)
            {
                collider.GetComponent<PlayerCharacter>().behaviorID = 2;
                collider.GetComponent<PlayerCharacter>().canPlant = true;
            }
            else if (state == landState.needWater)
            {
                collider.GetComponent<PlayerCharacter>().behaviorID = 3;
                collider.GetComponent<PlayerCharacter>().canWater = true;
            }
            else if (state == landState.mature) 
            {
                collider.GetComponent<PlayerCharacter>().behaviorID = 4;
                collider.GetComponent<PlayerCharacter>().canCollect = true;
            }
        }
    }
    void OnTriggerExit(Collider collider) 
    {
        if (collider.CompareTag("Player")) 
        {
            arrow.FlashingOff();
            collider.GetComponent<PlayerCharacter>().canPlant = false;
            collider.GetComponent<PlayerCharacter>().canReclaim = false;
            collider.GetComponent<PlayerCharacter>().canWater = false;
            collider.GetComponent<PlayerCharacter>().canCollect = false;
        }
    }
}