using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MAP : MonoBehaviour
{
    public GameObject MAPUI;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        MAPUIOPEN();
    }
    
    void MAPUIOPEN()
    {
        if (Input.GetKey(KeyCode.Q))
        {
            MAPUI.SetActive(true);
        }
        else
        {
            MAPUI.SetActive(false);

        }
    }
}
