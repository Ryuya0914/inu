// プレイヤの射撃、当たり判定、体力の管理
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGun : MonoBehaviour
{
    // オブジェクトのデータの格納場
    ObjectData Odata;
    // オブジェクトのデータ登録
    public ObjectData SetOdata { set { this.Odata = value; } }

    [SerializeField] List<GameObject> bullets;
    int bulletoffset = 0;

    public void ShootBullet() {

    }

    

}
