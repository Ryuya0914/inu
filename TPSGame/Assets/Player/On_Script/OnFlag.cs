using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SoftGear.Strix.Unity.Runtime;

public class OnFlag : StrixBehaviour {
    // 旗の状態　0:拠点, 1:落ちている, 2:誰かが持っている
    public int state = 0;
    // 初期位置
    Vector3 startPos;
    // 見た目
    [SerializeField] MeshRenderer Mren;
    // コライダ
    [SerializeField] CapsuleCollider Ccol;
    [SerializeField] BoxCollider Bcol;
    // どこ以下の高さに落ちたら拠点に戻るか
    [SerializeField] float under = -3.5f;

    void Start() {
        startPos = gameObject.transform.position;   // 拠点の座標を取得
    }
    
    // 取得された時
    [StrixRpc]
    public void Take(Transform _trans) {
        transform.parent = _trans;
        transform.localPosition = Vector3.zero;
        EnableOnOff(false);
        state = 2;
    }

    // 指定した場所に旗を落とす
    [StrixRpc]
    public void Drop(Vector3 pos) {
        if(pos.y <= under) {
            transform.position = startPos;
            EnableOnOff(true);
            state = 0;

        } else {
            transform.position = pos;
            EnableOnOff(true);
            state = 1;

        }
    }

    // 旗を拠点に戻す
    [StrixRpc]
    public void ResetPos() {
        transform.position = startPos;
        EnableOnOff(true);
        state = 0;
    }

    // 見た目と当たり判定のオンオフ切り替え
    [StrixRpc]
    void EnableOnOff(bool b) {
        if(b)
            transform.parent = null;
        Mren.enabled = b;
        if(Ccol != null)
            Ccol.enabled = b;
        if(Bcol != null)
            Bcol.enabled = b;
    }

}
