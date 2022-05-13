using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GameTime 
{
    public int day;
    public int time;
    public int lastTime;

}

public class TimeManager : MonoBehaviour
{
    public static GameTime gameTime = new GameTime();
    public Text TimeText;
    public int timeAdd;

    public int hour;
    public int minute;
    void Start()
    {
        timeAdd = 20;

        JsonManager.instance.ReadTimeData();
        ShowTheTime();
        StartCoroutine(TimeIncrease());
    }

    void Update()
    {
        
    }
    public IEnumerator TimeIncrease() 
    {
        while (true) 
        {
            while (gameTime.time <= 86400) 
            {
                gameTime.time += timeAdd;
                yield return new WaitForSeconds(1f);
                ShowTheTime();
            }
            gameTime.day += 1;
            ShowTheTime();
        }
    }

    IEnumerator TimeFunc()
    {
        while (true)
        {
            int newTime = (int)(DateTime.Now - DateTime.Parse("2021-7-7")).TotalSeconds;
            int paseTime = newTime - gameTime.lastTime;
            gameTime.time += paseTime * 2;

            gameTime.lastTime = newTime;

            yield return new WaitForSeconds(1);
        }


    }
    public void ShowTheTime() 
    {
        hour = (gameTime.time - gameTime.day * 86400) / 60;
        minute = gameTime.time - (gameTime.day * 86400 + hour * 60);
        TimeText.text = string.Format("游戏时间：第" + gameTime.day + "天" + hour + ":" + minute);
        JsonManager.instance.SaveTimeData();
    }
}
