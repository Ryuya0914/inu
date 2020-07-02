// 前後左右の移動、ジャンプ
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    // オブジェクトのデータの格納場
    ObjectData Odata;
    // オブジェクトのデータ登録
    public ObjectData SetOdata { set { this.Odata = value; } }


    // 移動
    public void Move(float h, float v) {
        Vector3 vec = transform.forward;
        vec.x *= v;

    }

    // ジャンプ
    public void Jump() {

    }

}
