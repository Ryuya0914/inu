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
    // プレイヤのtransform
    Transform PlayerT;
    // プレイヤのtransformを登録
    public Transform SetPlayerT { set { this.PlayerT = value; } }
    // プレイヤのrigidbody
    Rigidbody PlayerR;
    // プレイヤのrigidbodyを登録
    public Rigidbody SetPlayerR { set { this.PlayerR = value; } }


    // 移動
    public void Move(float h, float v) {
        if (h != 0 || v != 0) {
            Vector3 vec = new Vector3(h, 0.0f, v);      // 入力さえれた値をまとめる
            vec = vec.normalized;                       // ノーマライズ
            Vector3 movevec = transform.forward * vec.z + transform.right * vec.x;  // 入力された値をプレイヤの向いている方向に合わせる
            movevec = movevec.normalized * Odata.MoveSpeed;        // 移動速度を設定
            movevec.y = PlayerR.velocity.y;
            PlayerR.velocity = movevec;     // 重力で移動する
        } else {
            PlayerR.velocity = new Vector3(0f, PlayerR.velocity.y, 0f); // 入力が無かったら移動しないようにする
        }
    }

    // ジャンプ(空中ジャンプは後で直す)
    public void Jump() {
        Vector3 v = PlayerR.velocity;
        PlayerR.velocity = new Vector3(v.x, Odata.JumpPower, v.z);
    }

}
