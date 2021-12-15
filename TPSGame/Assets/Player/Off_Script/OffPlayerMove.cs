using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OffPlayerMove : BaseMove
{

    void Update()
    {
        // 移動の入力を受け付ける
        // 左右移動の入力
        float h = Input.GetAxis("Horizontal");
        // 前後移動の入力
        float v = Input.GetAxis("Vertical");
        // 移動メソッド呼び出し
        Move(h, v);

        // ジャンプの入力を受け付ける
        if(Input.GetKeyDown(KeyCode.Space)) {
            Jump();
        }

    }
}
