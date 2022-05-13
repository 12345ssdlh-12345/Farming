using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class BagSeeds : MonoBehaviour,IPointerClickHandler,IPointerEnterHandler,IPointerExitHandler
{
    public PlayerCharacter player;
    public GameObject descriptionPanel;
    public UIManager uiManager;

    public Image bagIcon;
    public Text Count;
    public Text Name;
    public Text info;

    public Seeds seed;
    void Awake() 
    {
        player = GameObject.Find("Player").GetComponent<PlayerCharacter>();
        uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        descriptionPanel = transform.Find("instructions").gameObject;
    }
    public void SetData(Seeds seed) 
    {
        if (seed == null) 
        {
            return;
        }
        this.seed = seed;

        this.bagIcon.sprite= Resources.Load<Sprite>("Art/"+this.seed.picPath);
        this.Count.text = this.seed.count.ToString();
        this.Name.text = this.seed.name;
        this.info.text = this.seed.description;
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (transform.parent.CompareTag("Grid"))
        {
            player.playermm.money -= seed.price;
            seed.count -= 1;
            ShopManager.instance.shoplist.shopdatalist[seed.ID].shopCount -= 1;

            BagManager.instance.jsonList.jsonDatalist[seed.ID].jsonCount += 1;
            for (int i = 0; i < uiManager.seedBar.transform.childCount; i++)
            {
                BagSeeds temp = uiManager.seedBar.transform.GetChild(i).GetComponent<BagSeeds>();
                if (seed.ID == temp.seed.ID) 
                {
                    temp.seed.count += 1;
                    temp.Count.text = temp.seed.count.ToString();
                }
            }
            Count.text = seed.count.ToString();
            uiManager.Save();
        }
        else if (transform.parent.CompareTag("Bag")) 
        {
            player.currentSeed = seed;
            player.currentBagSeed = this;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        descriptionPanel.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        descriptionPanel.SetActive(false);
    }
}
