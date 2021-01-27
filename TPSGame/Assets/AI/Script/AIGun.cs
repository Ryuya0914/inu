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
    float ShootInterval = 1.0f;                     // 射撃の間隔
    bool m_shootIntervalFlag = true;
    // その他
    AIDirector S_Adire;
    [SerializeField] GameObject MyObject;

    
    void Start()
    {
        S_Adire = GetComponent<AIDirector>();   // ディレクタスクリプト取得
        Pdata = S_Adire.GetPData;
        bullets[0].transform.parent.transform.parent = null;    // 弾丸がプレイヤに合わせて動かないようにする
    }


    // オブジェクトのデータ登録
    public void SetOdata(ObjectData odata) {
        Odata = odata;
        ResetAmmo();
    }

    void ShootUpdate() {
        // 弾丸を撃てるようにする
        m_shootIntervalFlag = true;
    }
    


    
    public int SelectBullet(Vector3 _vec) {
        // 弾薬が無かったら発射しない
        if(nowammo <= 0 || !m_shootIntervalFlag)
            return 0;

        // 発射する
        Shoot(bullets[bulletoffset].transform, _vec);
        m_shootIntervalFlag = false;
        Invoke(nameof(ShootUpdate), ShootInterval);


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
    void Shoot(Transform t, Vector3 _vec) {
        Rigidbody b = t.GetComponent<Rigidbody>();  // 弾丸のrigidbody取得
        b.velocity = Vector3.zero;                  // 弾丸を停止させる

        // 弾丸の位置を設定
        Vector3 _forward = (_vec - MyObject.transform.position).normalized;
        Vector3 startPosOffset = new Vector3(_forward.x * Odata.BulletOffset.z, Odata.BulletOffset.y, _forward.z * Odata.BulletOffset.z);   // 発射offset
        _vec.y += 0.5f;
        _vec -= MyObject.transform.position + startPosOffset;     // 飛ばす方向
        _vec = _vec.normalized;
        t.position = gameObject.transform.position + startPosOffset;  // world座標上の弾丸の発射位置


        // 弾丸を飛ばす
        b.gameObject.SetActive(true);                                   // 弾丸を表示
        b.GetComponent<Bullet>().GetSetDamage = Odata.shootDamage;      // 弾丸にダメージを付与
        Vector3 force = _vec * Pdata.BulletSpeed;                        // 飛ばす力を計算
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
