using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCharacter : MonoBehaviour
{
    Rigidbody rigdbody;//����
    Animator animator;//����������
    PlayerController playerController;//��ɫ���ƽű�

    public Text playerMoney;//��ҵĽ����ʾ
    public GameObject shopPlane;//�����̵�ҳ��
    public MoneyManager playermm;//��ҵ�ScriptableObject
    public UIManager uiManager;//UI����ű�
    public GameObject CurrentLand;//��ȡ��ǰ�Ĳȵ�����
    public BagSeeds currentBagSeed;//��������õ�������
    public Seeds currentSeed;//ʵ���ϵ��ڵ���������

    public float Speed;//�ƶ��ٶ�
    public float s;//���ϲ���

    public float ReclaimTime;

    public bool canInput;//�Ƿ����ƶ�
    public bool canReclaim;//�Ƿ��ܿ���
    public bool canPlant;//�Ƿ�����ֲ
    public bool canWater;//�Ƿ��ܽ�ˮ
    public bool canCollect;//�Ƿ����ռ���ʵ
    public bool openShop;//�����̵�

    public int behaviorID;//��Ϊ��ID

    Vector3 moveDir;//�ƶ�����
 

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
            Debug.Log("��������");
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
            Debug.Log("���ӽ���");
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
            Debug.Log("���Ӹ���");
            Seeds seedMoney = new Seeds(CurrentLand.GetComponent<Field>().seedInLand);
            playermm.money += seedMoney.price;
            CurrentLand.GetComponent<Field>().HarvestPlant();
        }
    }
    public void ShowTheMoney() 
    {
        playerMoney.text = string.Format("��Ǯ��" + playermm.money);
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
