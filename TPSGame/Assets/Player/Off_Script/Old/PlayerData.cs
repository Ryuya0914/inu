// プレイヤの(オブジェクトに影響されない)共通パラメータ
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class PlayerData : ScriptableObject {
    // 変身のクールタイム,カメラの回転速度,カメラの上下の角度制限,変身のRayの長さ,射程,弾速,リロード速度,復活までの時間
    public float TransChageCoolTime,    // 変身クールタイム
        RotateSpeed,                    // カメラ回転速度
        CameraUpLimit,                  // カメラの上下の角度制限
        CameraDownLimit,                // カメラの上下の角度制限
        RayRange,                       // 変身のrayの長さ
        ShootRange,                     // 射程
        BulletSpeed,                    // 弾速
        BulletReloadSpeed,              // リロード速度
        RespawnTime;                    // 復活までの時間
    

}
