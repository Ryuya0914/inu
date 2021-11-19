using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SoftGear.Strix.Unity.Runtime;

public class TestPlayerMove : StrixBehaviour
{
    Rigidbody rb;
    Vector3 gravity = new Vector3(0, 0, 0);

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isLocal) return;

        if(Input.GetKey(KeyCode.A)) {
            gravity.x = -1;
        } else if(Input.GetKey(KeyCode.D)) {
            gravity.x = 1;
        } else {
            gravity.x = 0;
        }

        if(Input.GetKey(KeyCode.W)) {
            gravity.z = 1;
        } else if(Input.GetKey(KeyCode.S)) {
            gravity.z = -1;
        } else {
            gravity.z = 0;
        }

        Move(gravity.x, gravity.z);

    }

    void Move(float h, float v) {
        if(h != 0 || v != 0) {
            Vector3 vec = new Vector3(h, 0.0f, v);      // 入力さえれた値をまとめる
            vec = vec.normalized;                       // ノーマライズ
            Vector3 movevec = transform.forward * vec.z + transform.right * vec.x;  // 入力された値をプレイヤの向いている方向に合わせる
            movevec = movevec.normalized * 3f;        // 移動速度を設定
            movevec.y = rb.velocity.y;
            rb.velocity = movevec;     // 重力で移動する
        } else {
            rb.velocity = new Vector3(0f, rb.velocity.y, 0f); // 入力が無かったら移動しないようにする
        }
    }


}
