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
        // 死亡時に旗落とすメソッド呼び出すようにする
        dieEvent += DropFlag;
    }


    // ダメージ処理＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊
    // HP
    protected int life = 100;
    // 現在の最大体力
    int maxLife = 100;
    // オブジェクトサイズ毎の最大体力
    int[] maxLifes = new int[] {200, 150, 100};

    // フレンドリーファイア
    protected bool ff = false;

    // ダメージ判定とHP減少
    public bool DecreaseHP(BaseBullet bulletScr) {
        // HPを減らせるかチェック
        if (!stateFlag) return false;
        if (!aliveFlag) return false;
        // 自分自身か確認
        if (bulletScr.owner == gameObject) return false;
        // 同じチームからの攻撃か確認
        if (!ff && (bulletScr.owner.GetComponent<OffTeam>().m_teamColor == gameObject.GetComponent<OffTeam>().m_teamColor)) return false;

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


    // 旗の処理＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊

    // 旗の機能があるかフラグ
    protected bool flagFlag = false;
    public void SetflagFlag(bool f) { flagFlag = f; }

    // 旗のスクリプト(nullでない場合は旗を所持している)
    public OffFlag flagScr;

    // 旗を手に入れることができるか確認 ＆ 手に入れる
    public bool TakeFlag(OffFlag _flag) {
        // 旗を手に入れることができるか確認
        if (!flagFlag || !aliveFlag) return false;
        // 旗を既に所持しているか確認
        if (flagScr != null) return false;

        // 旗を取得する
        flagScr = _flag;
        return true;
    }

    // 旗を落とす
    public void DropFlag() {
        // 旗の機能確認
        if(!flagFlag)　return;
        // 旗を所持しているか確認
        if (flagScr == null) return;
        // 旗を落とす
        flagScr.Drop();
        flagScr = null;
    }

    // 得点を取得したときに旗を消す
    public bool DeletFlag() {
        // 旗の機能確認
        if (!flagFlag) return false;
        // 生きているか確認
        if (!aliveFlag) return false;
        // 旗を持っているか確認
        if(flagScr == null) return false;

        
        // 旗を消す
        flagScr.Delete();
        // 旗を捨てる
        flagScr = null;

        return true;

    }

}
