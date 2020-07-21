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
    public void SetOdata(ObjectData odata) {
        Odata = odata;
        nowammo = odata.MaxAmmo;
    }

    [SerializeField] PlayerDirector S_Pdirector;

    int nowammo = 0;    // 現在の弾薬数
    
    [SerializeField] List<GameObject> bullets;
    int bulletoffset = 0;

    void Start() {
        // 弾丸がプレイヤに合わせて動かないようにする
        bullets[0].transform.parent.transform.parent = null;
    }

    public int ShootBullet() {
        if (nowammo <= 0) return 0;

        Transform b = bullets[bulletoffset].transform;

        // 弾丸の初期化
        Set(b.transform);

        // 発射する
        Shoot(b.GetComponent<Rigidbody>());

        // 次の弾丸を指定
        bulletoffset++;
        if (bulletoffset >= bullets.Count) bulletoffset = 0;

        nowammo--;  // 弾薬を消費
        if (nowammo <= 0) Reload(); // 弾薬がなくなったらリロードする

        return nowammo;
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
        b.velocity = Vector3.zero;                                      // 弾丸を停止させる
        b.gameObject.SetActive(true);                                   // 弾丸を表示
        b.GetComponent<Bullet>().GetSetDamage = Odata.shootDamage;      // 弾丸にダメージを付与
        Vector3 force = b.transform.forward * Pdata.BulletSpeed;        // 飛ばす力を計算
        b.AddForce(force);                                              // 飛ばす
    }

    // リロード
    public void Reload() {
        if (nowammo < Odata.MaxAmmo) {  // 弾薬が最大じゃなかったら
            nowammo = 0;                        // 弾薬をなくす
            S_Pdirector.AmmoUpdate(nowammo);    // UIの表示
            StartCoroutine("ReloadAmmo");       // リロードのコルーチン呼び出し
        }
    }

    float checktime = 0.1f;
    IEnumerator ReloadAmmo() {
        float time = 0;
        while(nowammo <= 0) {
            if (time >= Pdata.BulletReloadSpeed) {
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
        S_Pdirector.AmmoUpdate(nowammo);
    }

}
