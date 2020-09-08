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
    [SerializeField] MeshRenderer Mr;
    // コライダ
    [SerializeField] CapsuleCollider Ccol;


    void Start() {
        startPos = gameObject.transform.position;   // 拠点の座標を取得
    }


    // 取得された時
    public void Take() {
        state = 2;
    }

    // 指定した場所に旗を落とす
    public void Drop(Vector3 pos) {
        transform.position = pos;
        state = 1;
    }

    // 旗を拠点に戻す
    public void ResetPos() {
        transform.position = startPos;
        state = 0;
    }

}
