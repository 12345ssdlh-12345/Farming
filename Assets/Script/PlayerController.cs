using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public PlayerCharacter Player;

    public float h;
    public float v;
    void Awake()
    {
        Player = GetComponent<PlayerCharacter>();
    }
    void Update()
    {
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");
        if (Player.canInput) 
        {
            Player.Move(h, v);
            if (h != 0 || v != 0)
            {
                Player.Turn(h, v);
            }
        }
        if (Input.GetKeyDown(KeyCode.Space)) 
        {
            if (Player.behaviorID == 1)
            {
                Player.StartReclaim();
            }
            else if (Player.behaviorID == 2)
            {
                Player.StartPlant();
            }
            else if (Player.behaviorID == 3)
            {
                Player.StartWater();
            }
            else if (Player.behaviorID == 4) 
            {
                Player.Collect();
            }
        }
        if (Input.GetKeyDown(KeyCode.Z) && Player.openShop) 
        {
            Player.shopPlane.SetActive(true);
        }
    }
}
