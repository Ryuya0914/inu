using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseBullet : MonoBehaviour
{
    // 変身中のオブジェクトサイズ毎のダメージ
    public int[] damage = new int[] { 30, 20, 10 };
    // 弾を発射する直前にダメージを設定する
    public int nowDamage = 0;
    // 弾の速度
    protected float speed = 1000f;
    // 物理演算
    Rigidbody rb;
    public GameObject owner;


    public void Awake() {
        rb = GetComponent<Rigidbody>();
    }

    protected void Start() {
        // 弾の所有者を設定
        BaseGun _basegun = GetComponentInParent<BaseGun>();
        owner = _basegun.gameObject;
        _basegun.bullets.Add(GetComponent<BaseBullet>());
        // 追従しないようにする
        transform.parent = null;
        // 非表示にする
        gameObject.SetActive(false);
    }


    // 弾を発射する
    public void Shoot(Vector3 _pos, Vector3 _dir, int _size) {
        // ダメージを設定
        nowDamage = damage[_size];

        // 停止させる
        rb.velocity = Vector3.zero;
        
        // 発射位置に移動
        transform.position = _pos;

        // 表示する
        gameObject.SetActive(true);

        // 力を加える
        rb.AddForce(_dir * speed);
        Debug.Log(_dir * speed);
    }


    // 当たり判定
    public void OnTriggerEnter(Collider col) {
        HitGameObject(col.gameObject);
    }

    protected void HitGameObject(GameObject obj) {
        if(obj.tag == "PlayerObj" || obj.tag == "AIObj") {
            // 体力を減少させる
            bool f = obj.GetComponent<BaseState>().DecreaseHP(this);
            // 弾を消す
            if (f) {
                rb.velocity = Vector3.zero;
                gameObject.SetActive(false);
            }
            
        } else {
            // 壁とか地面とかに当たったら停止する
            rb.velocity = Vector3.zero;
            gameObject.SetActive(false);
        }

    }

}
