using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class stage_select : MonoBehaviour
{
    [SerializeField] GameObject credit;

    public void Off_select1()
    {
        SceneManager.LoadScene("Off_Stage");
    }
    public void Off_select2()
    {
        SceneManager.LoadScene("Off_Stage_2");
    }
    public void Off_select3()
    {
        SceneManager.LoadScene("Off_Stage_3");
    }


    public void Credit()
    {
        credit.SetActive(true);
    }

    public void Close()
    {
        credit.SetActive(false);
    }

    void Start() {
        SetUI();
        // マウスカーソルを表示
        Cursor.visible = true;
        // カーソルを動くようにする
        Cursor.lockState = CursorLockMode.None;

    }

    // CPUの人数変更
    int num = 1;
    [SerializeField] Text uitext;
    public void SetUI() {
        uitext.text = "Lv." + num.ToString();
    }
    public void AddCPU() {
        num = (num < 9) ? num+1 : num;

        SetUI();
        SetCPU();
    }
    public void DecreasCPU() {
        num = (num > 1) ? num-1 : num;
        
        SetUI();
        SetCPU();
    }
    public void SetCPU() {
        Off_StageDirector.AINum = num;
        Off_StageDirector_2.AINum = num;
    }

    public void LLog(string s) {
        Debug.Log(s);
    }


}
