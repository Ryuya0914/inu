using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class stage_select : MonoBehaviour
{
    [SerializeField] GameObject credit;

    public void Off_select()
    {
        SceneManager.LoadScene("Off_Stage_2");
    }

    public void Credit()
    {
        credit.SetActive(true);
    }

    public void Close()
    {
        credit.SetActive(false);
    }
}
