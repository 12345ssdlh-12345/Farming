using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Field : MonoBehaviour
{
    HighlightableObject arrow;//������˸
    MeshFilter meshFilter;//���ص�ģ��
    Transform center;//���ص����ĵ�

    public List<int> PlantGrowingTime;//��ǰ���µ����ӵ����н׶���Ҫ������ʱ��
    public List<GameObject> PlantModel;//��ǰ���µ��������н׶ε�ģ��
    public newField newfield;//�������صĽű������json��ʵ��������
    public GameObject canvas;//��Ҫ��ˮ������

    public int ID;//���ص�ID
    public int meshID;//���ص�ģ������
    public int seedInLand;//������µ�����
    public landState state;//���ص�ǰ��״̬
    public Vector3 pos;//�������ɵ�λ��
    public int startTime;//�������ӵ�ʱ��
    public int plantedStage;//�������Ӻ�������׶α�ʶ

    Land land;//��Ӧ��FieldManager�е���


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
    //������ص�Mesh
    public void LoadMesh()
    {
        meshFilter.mesh = GetLandMesh(meshID);
    }
    //��ȡ����ص�Mesh
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
    //�������صĲ���
    public void ReclamaLand()
    {
        this.state = landState.canPlant;
        this.meshID = 1;
        GetLandMesh(meshID);
        LoadMesh();
        RefreshLandData();
        newfield.SaveLandJson();
    }
    //��ֲ���ӵĲ���
    public void PlantLand()
    {
        plantedStage = 0;
        this.state = landState.growth_1;
        this.startTime = (int)ComputatingTime();
        RefreshLandData();
        newfield.SaveLandJson();
    }
    //��һ�׶ε�����
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
    //�ڶ��׶ε�����
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
    //�����׶ε�����
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
    //����ո����ֲ��
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
    //��Ҫ��ˮ�Ĳ���
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
    //���ɶ�Ӧ�׶ε�����Ԥ����
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
    //ɾ��֮ǰ��Ԥ����
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
    //ˢ��FieldManager�е������б��д��json
    public void RefreshLandData()
    {
        FieldManager.instance.lands.landList[this.ID].landPrefab = this.meshID;
        FieldManager.instance.lands.landList[this.ID].state = this.state;
        FieldManager.instance.lands.landList[this.ID].seedID = this.seedInLand;
        FieldManager.instance.lands.landList[this.ID].startTime = this.startTime;
        FieldManager.instance.lands.landList[this.ID].realTime = this.plantedStage;
    }

    //���㵱ǰ��ʱ��
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