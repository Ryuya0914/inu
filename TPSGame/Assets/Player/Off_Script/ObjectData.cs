// オブジェクトごとのパラメーターのデータ
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ObjectData : ScriptableObject {
    // オブジェクトの番号,最大ライフ,最大弾薬,射撃ダメージ
    public int ObjectNum, MaxLife, MaxAmmo, shootDamage;
    // 移動速度,ジャンプの高さ,射撃間隔,射程
    public float MoveSpeed, JumpHeight, ShootInterval, ShootRange;
    // カメラまでの距離
    public Vector3 camerapos;
}
