// プレイヤの(オブジェクトに影響されない)共通パラメータ
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class PlayerData : ScriptableObject {
    // 変身のクールタイム,カメラの回転速度,カメラの上下の角度制限,変身のRayの長さ,射程,弾速,リロード速度,復活までの時間
    public float TransChageCoolTime, RotateSpeed, CameraUpLimit, CameraDownLimit, RayRange, ShootRange, BulletSpeed, BulletReloadSpeed, RespawnTime;
    

}
