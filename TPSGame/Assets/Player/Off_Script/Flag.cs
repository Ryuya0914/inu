using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flag : MonoBehaviour
{
    // 旗の状態　0:拠点, 1:落ちている, 2:誰かが持っている
    public int state = 0;
    // 初期位置
    Vector3 startPos;
    // 見た目
    [SerializeField] MeshRenderer Mren;
    // コライダ
    [SerializeField] SphereCollider Scol;


    void Start() {
        startPos = gameObject.transform.position;   // 拠点の座標を取得
    }


    // 取得された時
    public void Take() {
        EnableOnOff(false);
        state = 2;
    }

    // 指定した場所に旗を落とす
    public void Drop(Vector3 pos) {
        transform.position = pos;
        EnableOnOff(true);
        state = 1;
    }

    // 旗を拠点に戻す
    public void ResetPos() {
        transform.position = startPos;
        EnableOnOff(true);
        state = 0;
    }

    // 見た目と当たり判定のオンオフ切り替え
    void EnableOnOff(bool b) {
        Mren.enabled = b;
        Scol.enabled = b;
    }

}
