// オブジェクトごとのパラメーターのデータ
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ObjectData : ScriptableObject {
    // オブジェクトの番号,最大ライフ,最大弾薬,射撃ダメージ,オブジェクトサイズ
    public int ObjectNum,       // オブジェクトの番号
               ObjSizeNum;     // オブジェクトのサイズ 0:L, 1:M, 2:S

    // カメラまでの距離
    public Vector3  cameraOffset,   // カメラまでの距離
                    BulletOffset,   // 弾を発射する位置
                AImoveLayPos,
                AImoveLayScale;
}
