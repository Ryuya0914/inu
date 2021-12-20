using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Log : MonoBehaviour
{
    public GameObject panel;
    private RectTransform logUI;
    public GameObject logup;
    public float x, y;
    bool logsize = true;
    List<string> playerLog = new List<string>();
    public GameObject textparent;
    public List<Text> texts = new List<Text>();
    // Start is called before the first frame update
    void Start()
    {
        logup.SetActive(false);
        for(int i=0;i<7;i++)
        {
            playerLog.Add("");
        }
     
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            LogUISize();
        }
        testlog();
        
    }
    void LogUISize()
    {
        logUI = GameObject.Find("Panel").GetComponent<RectTransform>();
        if (logsize ==true)
        {
            logUI.localScale = new Vector2(x, y);
            logsize = false;
            logup.SetActive(true);
        }
        else
        {
            logUI.localScale = new Vector2(1, 1);
            logsize = true;
            logup.SetActive(false);
        }

    }

    int a = 0;
    void testlog()
    {
        
        if(Input.GetKeyDown(KeyCode.Return))
        {
            playerLog.Add(a.ToString());
            playerLog.RemoveAt(0);
            a ++;
            for(int i=0;i<=6;i++)
            {
                texts[i].text=playerLog[i];
            }
        }
    }
}
