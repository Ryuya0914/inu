using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Select : MonoBehaviour
{
    public Text TimeText;
    public Text Wintext;
    public Text FriendlyFire;
    public Text StageSelect;
    private static int Seconds;
    private static int Minutes;
    private static int WinPoint;
    private static string Friendlyfire;
    private static string Stageselect;
    void Start()
    {
        Seconds = 00;
        Minutes = 05;
        Ttext();
        WinPoint = 20;
        Wintext.text = WinPoint.ToString();
        Friendlyfire = "On";
        FriendlyFire.text = "あり";
        Stageselect = "Random";
        StageSelect.text = "ランダム";
    }
    public void Timer1()
    {
        Seconds += 30;
        if (Seconds == 60)
        {
            Seconds = 00;
            Minutes += 1;
        }
        Ttext();
    }
    public void Timer2()
    {
        Seconds -= 30;
        if (Seconds == -30)
        {
            Seconds = 30;
            Minutes -= 1;
        }
        if (Minutes == 0)
        {
            Minutes = 1;
            Seconds = 0;
        }
        Ttext();
    }

    void Ttext()
    {
       TimeText.text = Minutes + ":" + Seconds;
    }
    public void Win()
    {
        WinPoint += 10;
        Wintext.text = WinPoint.ToString();
    }
    public void Win1()
    {
        WinPoint -= 10;
        if (WinPoint == 0)
        {
            WinPoint = 10;
        }
        Wintext.text = WinPoint.ToString();

    }
    public void Friend()
    {
        if (Friendlyfire == "on")
        {
            FriendlyFire.text = "なし";
            Friendlyfire = "off";
        }
        else
        {
            FriendlyFire.text = "あり";
            Friendlyfire = "on";
        }
    }
    public void Selectst()
    {
        if (Stageselect == "Random")
        {
            Stageselect = "Stage1";
            StageSelect.text = "ステージ1";
        } else if (Stageselect == "Stage1")
        {
            Stageselect = "Stage2";
            StageSelect.text = "ステージ2";
        } else if (Stageselect == "Stage2")
        {
            Stageselect = "Stage3";
            StageSelect.text = "ステージ3";
        } else if (Stageselect == "Stage3")
        {
            Stageselect = "Random";
            StageSelect.text = "ランダム";
        }
    }
    public void Selectst1()
    {
        if (Stageselect == "Random")
        {
            Stageselect = "Stage3";
            StageSelect.text = "ステージ3";
        }
        else if (Stageselect == "Stage3")
        {
            Stageselect = "Stage2";
            StageSelect.text = "ステージ2";
        }
        else if (Stageselect == "Stage2")
        {
            Stageselect = "Stage1";
            StageSelect.text = "ステージ1";
        }
        else if (Stageselect == "Stage1")
        {
            Stageselect = "Random";
            StageSelect.text = "ランダム";
        }
    }

}
