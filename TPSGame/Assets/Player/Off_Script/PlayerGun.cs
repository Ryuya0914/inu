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
        ResetAmmo();
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
        // 弾薬が無かったら発射しない
        if (nowammo <= 0) return 0;

        Transform b = bullets[bulletoffset].transform;

        // 弾丸を飛ばす方向を求める
        Vector3 bulletforward = GetVec();

        // 弾丸の初期化
        Set(b.transform, bulletforward);

        // 発射する
        Shoot(b.GetComponent<Rigidbody>(), bulletforward);

        // 次の弾丸を指定
        bulletoffset++;
        if (bulletoffset >= bullets.Count) bulletoffset = 0;

        nowammo--;  // 弾薬を消費
        if (nowammo <= 0) Reload(); // 弾薬がなくなったらリロードする

        return nowammo;
    }


    // 弾丸を飛ばす方向を求める
    Vector3 GetVec() {
        Vector3 vec = Vector3.zero;
        Transform cameraT = Camera.main.transform;
        // rayのスタート地点
        Vector3 raypos = cameraT.position + (cameraT.forward * -Odata.cameraOffsetZ.z * 2.2f);
        // rayを作成
        Ray ray = new Ray(raypos, cameraT.forward);

        // rayを可視化
        //Debug.DrawRay(raypos, ray.direction.normalized * Pdata.ShootRange, Color.red, 2.0f);

        RaycastHit hit;
        // 弾を発射する位置
        Vector3 startpos = new Vector3(transform.forward.x * Odata.BulletOffset.z, Odata.BulletOffset.y, transform.forward.z * Odata.BulletOffset.z);

        if(Physics.Raycast(ray, out hit, Pdata.ShootRange)) {
            // 弾を発射する位置から弾を着弾させたい位置までのベクトル
            vec = (cameraT.forward * hit.distance) + raypos - (gameObject.transform.position + startpos);
            //Debug.DrawRay(gameObject.transform.position, vec * Pdata.ShootRange, Color.green, 2.0f);

        } else {
            // カメラの向いている方向　×　射程 - 弾の発射位置
            vec = (cameraT.forward * Pdata.ShootRange) + raypos - (gameObject.transform.position + startpos);
        }

        vec = vec.normalized;

        return vec;
    }


    // 弾丸の発射位置と向きをセットする
    void Set(Transform b, Vector3 vec) {
        Vector3 pos = gameObject.transform.position;
        // 弾を発射する位置
        Vector3 startpos = new Vector3(transform.forward.x * Odata.BulletOffset.z, Odata.BulletOffset.y, transform.forward.z * Odata.BulletOffset.z);
        pos += startpos;
        b.position = pos;
    }

    // 弾丸を発射する
    void Shoot(Rigidbody b, Vector3 vec) {
        b.velocity = Vector3.zero;                                      // 弾丸を停止させる
        b.gameObject.SetActive(true);                                   // 弾丸を表示
        b.GetComponent<Bullet>().GetSetDamage = Odata.shootDamage;      // 弾丸にダメージを付与
        Vector3 force = vec * Pdata.BulletSpeed;        // 飛ばす力を計算
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


    IEnumerator ReloadAmmo() {
        float time = 0;
        while(nowammo <= 0) {
            if (time >= Pdata.BulletReloadSpeed) {
                ResetAmmo();
                yield break;
            } else {
                S_Pdirector.AmmoUpdate((time / Pdata.BulletReloadSpeed) * Odata.MaxAmmo);
            }
            yield return new WaitForEndOfFrame();
            time += Time.deltaTime;
        }
        yield break;
    }


    public void ResetAmmo() {
        nowammo = Odata.MaxAmmo;
        S_Pdirector.AmmoUpdate(nowammo);
    }

}
