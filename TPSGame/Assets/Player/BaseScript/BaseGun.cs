using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseGun : MonoBehaviour
{
    // 弾を撃てるかどうか
    protected bool shootFlag = false;
    public void SetShootFlag(bool f) { shootFlag = f; }



    // 弾を準備する＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊
    // 弾のスクリプトのリスト
    public List<BaseBullet> bullets;

    // 弾を発射する＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊
    // 現在のオブジェクトサイズ
    protected int objSize = 0;

    // オブジェクトサイズ毎のマガジンサイズ
    protected int[] magazinSize = new int[] { 10, 15, 20 };
    // マガジンサイズ
    protected int m_magazineSize = 10;
    // 残り弾薬数
    int ammoCount = 10;

    
    // カメラのオフセット
    protected Vector3 cameraOffset = new Vector3(0f, 3f, 1.5f);

    // 弾を撃ちだす位置
    protected Vector3 shootOffset = new Vector3(0f, 2f, 1f);

    // リスト内の何番目の弾を使うか
    int bulletOffset = 0;

    // 射撃の間隔
    protected float shootInterval = 0.2f;
    // 射撃のインターバルが終わっていればtrue
    protected bool intervalEndFlag = true;

    // 弾を発射する
    protected void Shoot(Vector3 dir) {
        // 射撃できるか確認
        if (shootFlag == false) return;

        // 射撃のインターバル確認
        if (intervalEndFlag == false) return;

        // 残弾確認
        if (ammoCount <= 0) return;

        // 弾を飛ばす
        bullets[bulletOffset].Shoot(transform.position + new Vector3(transform.forward.x * shootOffset.z, shootOffset.y, transform.forward.z * shootOffset.z), dir, objSize);
        bulletOffset++;
        if (bulletOffset >= bullets.Count) bulletOffset = 0;

        // 射撃のインターバル開始
        StartCoroutine(Interval());

        // 残弾を減らす
        ammoCount--;
        if (ammoCount <= 0) {
            StartCoroutine(Reload());
        }

    }

    // 射撃インターバル
    protected IEnumerator Interval() {
        intervalEndFlag = false;
        yield return new WaitForSeconds(shootInterval);
        intervalEndFlag = true;
    }


    // リロードする＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊
    // リロードの時間
    float reloadTime = 3f;
    // リロード中か
    protected bool reloadFlag = false;

    protected IEnumerator Reload() {
        reloadFlag = true;
        yield return new WaitForSeconds(reloadTime);
        reloadFlag = false;
        ammoCount = m_magazineSize;
    }

    // 変身時や死亡時にリロードを強制終了させる
    public void ReloadEnd() {
        StopCoroutine(Reload());
        reloadFlag = false;
        ammoCount = m_magazineSize;
    }

}
