using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leg : MonoBehaviour
{

    // 移動スクリプト
    BaseMove moveScr;

    private void Start() {
        // 移動スクリプト取得
        moveScr = GetComponentInParent<BaseMove>();
    }

    // コライダに触れている間
    void OnTriggerStay(Collider col) {
        if(col.tag == "Ground" || col.tag == "NonObject" || col.tag == "Player" || col.tag == "Object") {
            if (moveScr != null) {
                moveScr.isGround = true;
            }
        }
    }

}
