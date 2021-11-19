using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SoftGear.Strix.Unity.Runtime;

public class OnPlayerUI : StrixBehaviour
{
    [SerializeField] Canvas Pcanvas;
    [SerializeField] Image cross;
    [SerializeField] Image ammo;
    [SerializeField] Image life;
    [SerializeField] Text countText2;
    [SerializeField] Text countText;
    [SerializeField] Text flagLabel;


    // プレイヤのUIの切り替え(アクティブ化)
    public void CursorSet() {
        // マウスカーソルを消す
        Cursor.visible = false;
        // カーソルを動かないようにする
        Cursor.lockState = CursorLockMode.Locked;
        // プレイヤのUIを表示する
        cross.enabled = true;
        ammo.enabled = true;
        life.enabled = true;
    }

    // プレイヤのUIの切り替え(非アクティブ化)
    public void CursorDel() {
        // マウスカーソルを表示
        Cursor.visible = true;
        // カーソルを動くようにする
        Cursor.lockState = CursorLockMode.None;
        // プレイヤのUIを非表示にする
        cross.enabled = false;
        ammo.enabled = false;
        life.enabled = false;

    }

    // 弾薬のUIの更新
    public void AmmoUIUpdate(float f) {
        ammo.fillAmount = f;
    }

    // ライフのUIの更新
    public void LifeUIUpdate(float f) {
        life.fillAmount = f;
    }


    // 死亡時にカウントダウンする
    public void RespawnCount(float time) {
        StartCoroutine(nameof(CountDown), time);
    }

    IEnumerator CountDown(int time) {
        int nowtime = time;
        countText2.enabled = true;       // 秒数のテキストを表示する
        countText.enabled = true;       // 秒数のテキストを表示する
        while(true) {
            if(nowtime > 0) {
                countText.text = nowtime.ToString();
            } else {
                countText2.enabled = false;     // 秒数のテキストを非表示にする
                countText.enabled = false;      // 秒数のテキストを非表示にする

                yield break;
            }
            nowtime--;
            yield return new WaitForSeconds(1.0f);

        }
    }

    // 旗が敵に取られた時に呼び出される
    public void FlagGetLavelOn() {

        StartCoroutine(nameof(FlagGetLabelOnOff));
    }

    public void FlagGetLavelOff() {

        StopCoroutine(nameof(FlagGetLabelOnOff));
        flagLabel.enabled = false;
    }


    // 一定秒数後に旗が取られた表示を消す
    IEnumerator FlagGetLabelOnOff() {
        while(true) {
            if(flagLabel.enabled == true) {
                flagLabel.enabled = false;
            } else {
                flagLabel.enabled = true;
            }
            yield return new WaitForSeconds(0.5f);

        }
    }


}
