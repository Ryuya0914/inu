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
        // rayを作成
        Ray ray = new Ray(cameraT.position, cameraT.forward);

        // rayを可視化
        //Debug.DrawRay(ray.origin, ray.direction.normalized * Pdata.ShootRange, Color.red, 2.0f);

        RaycastHit hit;

        if(Physics.Raycast(ray, out hit, Pdata.ShootRange)) {
            
            vec = (cameraT.forward * hit.distance) + cameraT.position - gameObject.transform.position;
            vec = vec.normalized;
            //Debug.DrawRay(gameObject.transform.position, vec * Pdata.ShootRange, Color.green, 2.0f);

        } else {
            vec = Camera.main.transform.forward;
        }



        return vec;
    }


    // 弾丸の発射位置と向きをセットする
    void Set(Transform b, Vector3 vec) {
        Vector3 pos = gameObject.transform.position;
        pos += vec * Odata.BulletOffset;
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
