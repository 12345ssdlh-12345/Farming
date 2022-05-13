using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCharacter : MonoBehaviour
{
    Rigidbody rigdbody;//刚体
    Animator animator;//动画管理器
    PlayerController playerController;//角色控制脚本

    public Text playerMoney;//玩家的金币显示
    public GameObject shopPlane;//种子商店页面
    public MoneyManager playermm;//玩家的ScriptableObject
    public UIManager uiManager;//UI管理脚本
    public GameObject CurrentLand;//获取当前的踩的土地
    public BagSeeds currentBagSeed;//点击背包得到的种子
    public Seeds currentSeed;//实际上当期的种子属性

    public float Speed;//移动速度
    public float s;//整合差量

    public float ReclaimTime;

    public bool canInput;//是否能移动
    public bool canReclaim;//是否能开垦
    public bool canPlant;//是否能种植
    public bool canWater;//是否能浇水
    public bool canCollect;//是否能收集果实
    public bool openShop;//进入商店

    public int behaviorID;//行为的ID

    Vector3 moveDir;//移动向量
 

    void Awake()
    {
        rigdbody = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();

        Speed = 3f;
        s = 0;

        ReclaimTime = 1.6f;

        canInput = true;
        canReclaim = false;
        canPlant = false;
        canWater = false;
        openShop = false;

        behaviorID = 1;
    }
    void Update()
    {
        ShowTheMoney();
        CheckTheShop();
    }
    public void Move(float h, float v)
    {
        if (h == 0 && v == 0) 
        {
            rigdbody.angularVelocity = Vector3.zero;
        }
        moveDir = new Vector3(h, 0, v);
        moveDir.Normalize();
        rigdbody.MovePosition(transform.position + moveDir * Speed * Time.fixedDeltaTime);
        if (Mathf.Abs(h) >= Mathf.Abs(v))
        {
            s = Mathf.Abs(h);
        }
        else 
        {
            s = Mathf.Abs(v);
        }
        animator.SetFloat("Speed", s);
    }
    public void Turn(float h, float v) 
    {
        Vector3 TurnDir = new Vector3(h, 0, v);
        transform.LookAt(TurnDir+transform.position);
    }
    public void StartReclaim()
    {
        if (canReclaim)
        {
            canInput = false;
            animator.SetTrigger("Work");
            CurrentLand.GetComponent<Field>().ReclamaLand();
            StartCoroutine(Reclaim(ReclaimTime));
        }
    }
    IEnumerator Reclaim(float time) 
    {
        yield return new WaitForSeconds(time);
        canInput = true;
        canReclaim = false;
    }
    public void StartPlant() 
    {
        if (canPlant) 
        {
            canPlant = false;
            canInput = false;
            Debug.Log("老子种了");
            CurrentLand.GetComponent<Field>().seedInLand = currentSeed.ID;
            CurrentLand.GetComponent<Field>().PlantLand();
            JsonDataReduce();
            StartCoroutine(Plant(1f));
        }
    }
    IEnumerator Plant(float time) 
    {
        yield return new WaitForSeconds(time);
        canInput = true;
        canPlant = false;
    }
    public void StartWater() 
    {
        if (canWater) 
        {
            canWater = false;
            Debug.Log("老子浇了");
            CurrentLand.GetComponent<Field>().WaterLand();
            StartCoroutine(Water(1f));
        }
    }
    IEnumerator Water(float time)
    {
        yield return new WaitForSeconds(time);
        canInput = true;
        canWater = false;
    }
    public void Collect() 
    {
        if (canCollect) 
        {
            canCollect = false;
            Debug.Log("老子割了");
            Seeds seedMoney = new Seeds(CurrentLand.GetComponent<Field>().seedInLand);
            playermm.money += seedMoney.price;
            CurrentLand.GetComponent<Field>().HarvestPlant();
        }
    }
    public void ShowTheMoney() 
    {
        playerMoney.text = string.Format("金钱：" + playermm.money);
    }
    public void CheckTheShop() 
    {
        Collider[] temp = Physics.OverlapSphere(transform.position, 2f, 1 << LayerMask.NameToLayer("Shop"));
        if (temp.Length <= 0)
        {
            openShop = false;
            if (!openShop) 
            {
                shopPlane.SetActive(false);
            }
            return;
        }
        foreach (var item in temp)
        {
            Debug.Log(item.name);
            if (item.CompareTag("seedShop"))
            {
                openShop = true;
            }
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(transform.position, 2f);
    }
    public void JsonDataReduce() 
    {
        currentSeed.count -= 1;
        currentBagSeed.Count.text = currentSeed.count.ToString();
        BagManager.instance.jsonList.jsonDatalist[currentSeed.ID].jsonCount -= 1;
        uiManager.Save();
        BagManager.instance.RefreshBagData();
        
    }
}
