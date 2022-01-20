using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseState : MonoBehaviour
{
    // ライフの機能があるかフラグ
    protected bool stateFlag = false;
    public void SetStateFlag(bool f) { stateFlag = f; }


    // HP減少イベント
    public event System.Action<int> decreaseHPEvent;
    // 死亡時イベント
    public event System.Action dieEvent;
    // リスポーン時イベント
    public event System.Action respawnEvent;
        

    // 生きているかのフラグ
    protected bool aliveFlag = true;
    

    // オブジェクトの変更
    public virtual void SetObjData(ObjectData _objData) {
        // 最大HP変更
        maxLife = maxLifes[_objData.ObjSizeNum];
    }


    // Awake処理＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊
    public virtual void Awake() {
        // リスポーン地点設定
        respawnPos = transform.position;
    }


    // Start処理＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊



    // ダメージ処理＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊
    // HP
    protected int life = 100;
    // 現在の最大体力
    int maxLife = 100;
    // オブジェクトサイズ毎の最大体力
    int[] maxLifes = new int[] {200, 150, 100};

    // ダメージ判定とHP減少
    public bool DecreaseHP(BaseBullet bulletScr) {
        // HPを減らせるかチェック
        if (!stateFlag) return false;
        if (!aliveFlag) return false;

        // HPを減らす
        life -= bulletScr.nowDamage * 100 / maxLife;

        // HP減少時イベント実行
        decreaseHPEvent?.Invoke(life);

        // 死亡したか判定
        if (life <= 0) {
            // 死亡時のメソッド呼び出し
            Die();
        }


        return true;
    }    

    // ダメージ判定とHP減少(奈落に落ちた時とか)
    public bool DecreaseHP() {
        // HPを減らせるかチェック
        if (!stateFlag) return false;
        if (!aliveFlag) return false;

        // HPを減らす
        life -= 20;

        // HP減少時イベント実行
        decreaseHPEvent?.Invoke(life);

        // 死亡したか判定
        if (life <= 0) {
            // 死亡時のメソッド呼び出し
            Die();
        }


        return true;
    }

    


    // 死亡時処理＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊

    // リスポーンにかかる時間
    float respawnTime = 5.0f;
    // リスポーン地点
    public Vector3 respawnPos;
    
    // 死亡時に呼ばれる
    public void Die() {
        // ライフ機能チェック
        if(!stateFlag) return;

        // 生きているフラグをオフにする
        aliveFlag = false;

        // 死亡時イベント実行
        dieEvent?.Invoke();

        // リスポーンコルーチン開始
        StartCoroutine(Respawn());
    }


    // 一定時間後にリスポーン
    protected IEnumerator Respawn() {
        // 一定時間遅延
        yield return new WaitForSeconds(respawnTime);
        // 生き返らせる
        Revive();
    }

    // リスポーン処理
    protected void Revive() {
        // ライフ機能チェック
        if(!stateFlag)　return;

        // 体力回復
        life = 100;

        // リスポーン地点に移動
        transform.position = respawnPos;

        // リスポーン時イベントを実行
        respawnEvent?.Invoke();
        // 生きているフラグをオンにする
        aliveFlag = true;

    }

}
