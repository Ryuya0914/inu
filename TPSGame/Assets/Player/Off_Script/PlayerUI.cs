using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] Canvas Pcanvas;
    [SerializeField] Image ammo;
    [SerializeField] Image life;

    // プレイヤのUIの切り替え(アクティブ化)
    public void CursorSet() { 
        // マウスカーソルを消す
        Cursor.visible = false;
        // カーソルを動かないようにする
        Cursor.lockState = CursorLockMode.Locked;
        // プレイヤのUIを表示する
        Pcanvas.enabled = true;


    }

    // プレイヤのUIの切り替え(非アクティブ化)
    public void CursorDel() {
        // マウスカーソルを表示
        Cursor.visible = true;
        // カーソルを動くようにする
        Cursor.lockState = CursorLockMode.None;
        // プレイヤのUIを非表示にする
        Pcanvas.enabled = false;

    }

    // 弾薬のUIの更新
    public void AmmoUIUpdate(float f) {
        ammo.fillAmount = f;
    }

    // ライフのUIの更新
    public void LifeUIUpdate(float f) {
        life.fillAmount = f;
    }

}
