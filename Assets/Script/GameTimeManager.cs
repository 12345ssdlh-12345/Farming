using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameTimeManager : MonoBehaviour
{
    public Text CurrentTimeText;
    public int hour;
    public int minute;
    public int second;
    public int year;
    public int month;
    public int day;
    void Start()
    {
        CurrentTimeText = GetComponent<Text>();
    }
    void Update()
    {
        hour = DateTime.Now.Hour;
        minute = DateTime.Now.Minute;
        second = DateTime.Now.Second;
        year = DateTime.Now.Year;
        month = DateTime.Now.Month;
        day = DateTime.Now.Day;

        string time = string.Format(year + "Äê" + month + "ÔÂ" + day + "ÈÕ"+ "n" + hour + ":" + minute + ":" + second);
        CurrentTimeText.text = time.Replace('n', '\n'); 
    }
}
