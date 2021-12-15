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
    void Start()
    {
        Configuration.SetActive(false);
    }

    void Update()
    {
        if(Input.GetKey(KeyCode.Escape))
        {
            canvas.SetActive(false);
            taikistage.SetActive(false);
            Stage_preset.SetActive(false);
            Configuration.SetActive(true);

        }
    }
    
}
