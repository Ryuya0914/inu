using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIGun : MonoBehaviour
{
    // オブジェクトのデータ
    ObjectData Odata;
    // プレイヤのデータ
    PlayerData Pdata;

    // 射撃関係フィールド
    int nowammo = 0;                                // 現在の弾薬数
    [SerializeField] List<GameObject> bullets;      // 弾のリスト
    int bulletoffset = 0;                           // リストの何番目の弾を使うか
    float ShootInterval = 0.5f;                     // 射撃の間隔
    Transform PlayerT;                              // プレイヤのTransform
    public Transform SetPlayerT { set { this.PlayerT = value; } }

    // その他
    AIDirector S_Adire;
    [SerializeField] GameObject MyObject;

    
    void Start()
    {
        S_Adire = GetComponent<AIDirector>();   // ディレクタスクリプト取得
        Pdata = S_Adire.GetPData;               // プレイヤデータ取得
        bullets[0].transform.parent.transform.parent = null;    // 弾丸がプレイヤに合わせて動かないようにする
        StartCoroutine(nameof(ShootUpdate));    // 一定間隔で射撃
    }


    // オブジェクトのデータ登録
    public void SetOdata(ObjectData odata) {
        Odata = odata;
        ResetAmmo();
    }

    IEnumerator ShootUpdate() {
        while(true) {
            yield return new WaitForSeconds(ShootInterval);

            if (PlayerT != null) {
                // 自身の オブジェクトを敵の方向に向ける
                Vector3 Ppos = PlayerT.position;
                Ppos.y = MyObject.transform.position.y;
                MyObject.transform.LookAt(Ppos);
                // 弾丸を撃つ
                ShootBullet();
            }
        }
    }


    
    public int ShootBullet() {
        // 弾薬が無かったら発射しない
        if(nowammo <= 0)
            return 0;

        // 発射する
        Shoot(bullets[bulletoffset].transform);

        // 次の弾丸を指定
        bulletoffset++;
        if(bulletoffset >= bullets.Count)
            bulletoffset = 0;

        nowammo--;  // 弾薬を消費
        if(nowammo <= 0)
            Reload(); // 弾薬がなくなったらリロードする
        


        return nowammo;
    }


    // 弾丸を発射する
    void Shoot(Transform t) {
        Rigidbody b = t.GetComponent<Rigidbody>();  // 弾丸のrigidbody取得
        b.velocity = Vector3.zero;                  // 弾丸を停止させる

        // 弾丸の位置を設定
        Vector3 startPosOffset = new Vector3(MyObject.transform.forward.x * Odata.BulletOffset.z, Odata.BulletOffset.y, MyObject.transform.forward.z * Odata.BulletOffset.z);   // 発射offset
        Vector3 shootVec = PlayerT.position;
        shootVec.y += 0.5f;
        shootVec -= MyObject.transform.position + startPosOffset;     // 飛ばす方向
        shootVec = shootVec.normalized;
        t.position = gameObject.transform.position + startPosOffset;  // world座標上の弾丸の発射位置


        // 弾丸を飛ばす
        b.gameObject.SetActive(true);                                   // 弾丸を表示
        b.GetComponent<Bullet>().GetSetDamage = Odata.shootDamage;      // 弾丸にダメージを付与
        Vector3 force = shootVec * Pdata.BulletSpeed;                        // 飛ばす力を計算
        b.AddForce(force);                                              // 飛ばす
    }

    // リロード
    public void Reload() {
        if(nowammo < Odata.MaxAmmo) {  // 弾薬が最大じゃなかったら
            nowammo = 0;                        // 弾薬をなくす
            StartCoroutine("ReloadAmmo");       // リロードのコルーチン呼び出し
        }
    }

    float checktime = 0.1f;
    IEnumerator ReloadAmmo() {
        float time = 0;
        while(nowammo <= 0) {
            if(time >= Pdata.BulletReloadSpeed) {
                ResetAmmo();
                yield break;
            }
            yield return new WaitForSeconds(checktime);
            time += checktime;
        }
        yield break;
    }


    public void ResetAmmo() {
        nowammo = Odata.MaxAmmo;
    }


}
