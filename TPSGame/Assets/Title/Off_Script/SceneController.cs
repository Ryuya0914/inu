using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public bool StartFadeOut = false;

    void Start() {
        if (StartFadeOut) FadeOut();
    }


    // タイトルへ移行
    public void LoadTitle() {
        SceneManager.LoadScene(0);
    }

    // ステージの番号を指定してシーン移行
    public void LoadStage(int num) {
        SceneManager.LoadScene(num+1);
    }

    public void LoadRoby() {
        SceneManager.LoadScene(1);
    }


    public void FadeIn() {

    }

    public void FadeOut() {

    }

}
