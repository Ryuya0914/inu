using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameConfiguration : MonoBehaviour
{
    public GameObject Configuration;
    public GameObject canvas;
    public GameObject taikistage;
    public GameObject Stage_preset;
    private bool menu = true;
    void Start()
    {
        Configuration.SetActive(false);
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if (menu == true)
            {
                canvas.SetActive(false);
                taikistage.SetActive(false);
                Stage_preset.SetActive(false);
                Configuration.SetActive(true);
                menu = false;
                return;
            }
            if (menu == false)
            {
                canvas.SetActive(true);
                taikistage.SetActive(true);
                Stage_preset.SetActive(true);
                Configuration.SetActive(false);
                menu = true;
                return;
            }
        }

    }
    
}
