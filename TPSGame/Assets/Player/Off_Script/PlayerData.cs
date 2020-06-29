// プレイヤの(オブジェクトに影響されない)共通パラメータ
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class PlayerData : ScriptableObject {
    public int shootDamage;                                 // 射撃ダメージ
    public float TransChageCoolTime, ShootInterval;         // 変身のクールタイム、射撃間隔
    public Vector3 CameraSpeedVertical,CameraSpeed_Horizon; // カメラ回転速度(縦)、カメラ回転速度(横)
}
