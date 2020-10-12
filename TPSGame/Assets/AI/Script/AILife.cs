using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AILife : MonoBehaviour
{
    // オブジェクトのデータの格納場
    ObjectData Odata;
    // オブジェクトのデータ登録
    public void SetOdata(ObjectData odata) {
        Odata = odata;
    }
    PlayerData Pdata;

    int mylife = 100;
    AIDirector S_Adire;         // ディレクタスクリプト
    Vector3 RespawnPos;         // リスポーン地点

    void Start() {
        S_Adire = GetComponent<AIDirector>();   // ディレクタ取得
        Pdata = S_Adire.GetPData;
        RespawnPos = transform.position;        // リスポーン地点取得
    }


    // HPを減らす
    void DecreaseHP(int damage) {
        if (mylife <= 0) {      // もともとHPが0以下だったら無視
            return;
        }
        mylife -= damage * 100 / Odata.MaxLife;
        if (mylife <= 0) {       // HPが0以下になったら
            S_Adire.ChangeState(3);     // AIのステート切り替え
            Invoke(nameof(Respawn), Pdata.RespawnTime);     // 一定時間後に生き返らせる
        }
    }

    // HP全回復
    public void RecoveryHP() {
        mylife = 100;
    }


    // 生き返る処理
    void Respawn() {
        transform.position = RespawnPos;    // リスポーン地点へ移動
        RecoveryHP();                       // HP回復させる
        S_Adire.ChangeState(1);             // ステートを旗を取得に切り替える
    }
    

    // 弾との当たり判定
    void OnTriggerEnter(Collider col) {
        if(col.tag == "Bullet") {
            DecreaseHP(col.gameObject.GetComponent<Bullet>().GetSetDamage);
            col.gameObject.SetActive(false);    // 弾を消す
        } else if(col.tag == "UnderGround") {  // 奈落に落ちた時死ぬ機能
            DecreaseHP(1000);
        }

    }
}
