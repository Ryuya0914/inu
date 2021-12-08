using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;



public class CountDown : MonoBehaviour
{
    public Text CountTex;//カウントダウンテキスト
    public float Countdown=5;
    int count;
    public UnityEvent CountDownEnd;
    


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        countdown();
    }
    public void countdown()
    {
        if (Countdown >= 0)
        {
            Countdown -= Time.deltaTime;
            count = (int)Countdown;
            CountTex.text = count.ToString();
            
        } 
        else
        {
            CountDownEnd.Invoke();
            Destroy(gameObject);
        }
        
    }
}
