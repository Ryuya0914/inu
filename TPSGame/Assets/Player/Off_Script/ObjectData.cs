// オブジェクトごとのパラメーターのデータ
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ObjectData : ScriptableObject {
    public int ObjectNum, MaxLife, MaxAmmo;     // オブジェクトの番号、最大ライフ、最大弾薬
    public float MoveSpeed, JumpHeight;         // 移動速度、ジャンプの高さ
}
