// プレイヤの射撃、当たり判定、体力の管理
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGun : MonoBehaviour
{
    // プレイヤデータ、オブジェクトのデータの格納場
    PlayerData Pdata;
    ObjectData Odata;
    // プレイヤデータ、オブジェクトのデータ登録
    public PlayerData SetPdata { set { this.Pdata = value; } }
    public ObjectData SetOdata { set { this.Odata = value; } }

    //[SerializeField] List<Rigidbody> bullets;
    [SerializeField] List<GameObject> bullets;
    int bulletoffset = 0;

    void Start() {
        // 弾丸がプレイヤに合わせて動かないようにする
        bullets[0].transform.parent.transform.parent = null;
    }

    public void ShootBullet() {
        Transform b = bullets[bulletoffset].transform;

        // 弾丸の初期化
        Set(b.transform);

        // 発射する
        Shoot(b.GetComponent<Rigidbody>());

        // 次の弾丸を指定
        bulletoffset++;
        if (bulletoffset >= bullets.Count) bulletoffset = 0;
    }


    // 弾丸の発射位置と向きをセットする
    void Set(Transform b) {
        Vector3 pos = Camera.main.transform.position;
        Vector3 vec = Camera.main.transform.eulerAngles;
        pos += vec * Odata.BulletOffset;
        b.position = pos;
        b.eulerAngles = vec;
    }

    // 弾丸を発射する
    void Shoot(Rigidbody b) {
        b.velocity = Vector3.zero;
        b.gameObject.SetActive(true);
        Vector3 force = b.transform.forward * Pdata.BulletSpeed;
        b.AddForce(force);
    }


    // 弾との当たり判定
    void OnTriggerEnter(Collider col) {
        if (col.tag == "Bullet") {
            col.gameObject.SetActive(false);    // 弾を消す
        }
    }


}
