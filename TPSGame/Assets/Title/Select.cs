using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Select : MonoBehaviour
{
    public Text TimeText;
    private int Seconds;
    private int Minutes;
    void Start()
    {
        TimeText = GetComponent<Text>();
        Seconds = 0;
        Minutes = 5;
        Ttext();
        //TimeText.text = Minutes + ":" + Seconds;
    }

    void Update()
    {
        
    }
    public void Timer1()
    {
        Debug.Log("q");
        Seconds += 30;
        if (Seconds == 60)
        {
            Seconds = 0;
            Minutes += 1;
        }
        Ttext();
    }
    void Ttext()
    {
        TimeText.text = Minutes + ":" + Seconds;
    }
}
