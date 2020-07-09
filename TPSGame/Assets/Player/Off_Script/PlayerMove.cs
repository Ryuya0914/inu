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
        Vector3 vec = new Vector3(h, 0.0f, v);      // 入力さえれた値をまとめる
        vec = vec.normalized;                       // ノーマライズ
        Vector3 movepos = transform.forward * vec.z + transform.right * vec.x;  // 入力された値をプレイヤの向いている方向に合わせる
        movepos = movepos.normalized * Odata.MoveSpeed * Time.deltaTime;        // 移動速度を設定
        PlayerT.transform.position += movepos;      // ポジションの更新
    }

    // ジャンプ(空中ジャンプは後で直す)
    public void Jump() {
        PlayerR.velocity = transform.up * Odata.JumpPower;
    }

}
