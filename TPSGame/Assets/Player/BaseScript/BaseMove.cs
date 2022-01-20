using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseMove : MonoBehaviour
{
    // 移動できるかどうか
    protected bool moveFlag = false;
    public void SetMoveFlag(bool f) { moveFlag = f; }
    // 生きているかフラグ
    protected bool aliveFlag = true;
    
    // リジットボディ
    Rigidbody rb;

    void Awake() {
        // 自身のリジットボディ取得
        rb = GetComponent<Rigidbody>();
    }

    void Start() {
        BaseState bs = GetComponent<BaseState>();
        bs.respawnEvent += () => { aliveFlag = true; };
        bs.dieEvent += () => { aliveFlag = false; };
    }

    // 移動＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊

    // 移動速度
    float moveSpeed = 10.0f;

    // 移動
    protected void Move(float h, float v) {
        if((h != 0 || v != 0) && moveFlag == true && aliveFlag) {
            Vector3 vec = new Vector3(h, 0.0f, v);      // 入力さえれた値をまとめる
            vec = vec.normalized;                       // ノーマライズ
            Vector3 movevec = transform.forward * vec.z + transform.right * vec.x;  // 入力された値をプレイヤの向いている方向に合わせる
            movevec = movevec.normalized * moveSpeed;        // 移動速度を設定
            movevec.y = rb.velocity.y;
            rb.velocity = movevec;     // 重力で移動する
        } else {
            rb.velocity = new Vector3(0f, rb.velocity.y, 0f); // 入力が無かったら移動しないようにする
        }
    }



    // ジャンプ＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊

    // ジャンプ力
    float JumpPow = 10.0f;

    // ジャンプ
    protected void Jump() {
        if (!moveFlag || !aliveFlag) return;

        // 縦方向の加速度だけいじる
        Vector3 v = rb.velocity;
        rb.velocity = new Vector3(v.x, JumpPow, v.z);
    }



}
