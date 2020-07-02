// プレイヤの(オブジェクトに影響されない)共通パラメータ
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class PlayerData : ScriptableObject {
    // 変身のクールタイム,カメラの回転速度,カメラの上下の角度制限
    public float TransChageCoolTime, RotateSpeed, CameraUpLimit, CameraDownLimit;

}
