using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuUI : MonoBehaviour
{
    public GameObject menuUI;
    bool UION = false;
    // Start is called before the first frame update
    void Start()
    {
        menuUI.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            MenuOpen();
        }
            
    }
    void MenuOpen()
    {
        if (UION == false)
        {
            UION = true;
            menuUI.SetActive(true);

        }
        else
        {
            UION = false;
            menuUI.SetActive(false);
        }      
        
    }
    public void OnClicktitleButton()
    {
        SceneManager.LoadScene("Title");
    }
    public void OnClickRobyButton()
    {
        SceneManager.LoadScene("Roby");
    }
}
