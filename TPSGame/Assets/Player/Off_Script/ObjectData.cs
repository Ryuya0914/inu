// オブジェクトごとのパラメーターのデータ
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ObjectData : ScriptableObject {
    // オブジェクトの番号,最大ライフ,最大弾薬,射撃ダメージ,オブジェクトサイズ
    public int ObjectNum,       // オブジェクトの番号
                MaxLife,        // 最大ライフ
                MaxAmmo,        // 最大弾薬
                shootDamage,    // 射撃ダメージ
                ObjSizeNum;     // オブジェクトのサイズ 0:L, 1:M, 2:S

    // 移動速度,ジャンプの高さ,弾を発射する位置(前方向)
    public float MoveSpeed,     // 移動速度
                JumpPower;      // ジャンプの高さ

    // カメラまでの距離
    public Vector3 cameraOffsetY,   // カメラまでの距離
                cameraOffsetZ,      // カメラまでの距離
                BulletOffset,       // 弾を発射する位置
                AImoveLayPos,       
                AImoveLayScale;     
}
