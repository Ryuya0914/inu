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
    [SerializeField] EffectController S_effect;
    TeamScript m_team;
    AudioSource audio;
    [SerializeField] AudioClip clip;

    void Start() {
        S_Adire = GetComponent<AIDirector>();   // ディレクタ取得
        Pdata = S_Adire.GetPData;
        RespawnPos = transform.position;        // リスポーン地点取得
        audio = GetComponent<AudioSource>();
        Invoke(nameof(SetTeam), 0.5f);
    }
   
    void SetTeam() {
        m_team = GetComponent<TeamScript>();
    }


    // 現在のHPを返す
    public int GetHP() {
        return mylife;
    }

    // 死んだときの処理
    public void Dead() {
        S_effect.EffectPlay(transform.position);
    }


    // HPを減らす
    void DecreaseHP(int damage) {
        audio.PlayOneShot(clip);
        mylife -= damage * 100 / Odata.MaxLife;
    }

    // HP全回復
    public void RecoveryHP() {
        mylife = 100;
    }


    // 生き返る処理
    public void Respawn() {
        transform.position = RespawnPos;    // リスポーン地点へ移動
        RecoveryHP();                       // HP回復させる
    }
    

    // 弾との当たり判定
    void OnTriggerEnter(Collider col) {
        if(col.tag == "Bullet") {
            if (col.GetComponent<Bullet>().m_myTeam.m_teamColor != m_team.m_teamColor) {
                DecreaseHP(col.gameObject.GetComponent<Bullet>().GetSetDamage);
            }
            col.gameObject.SetActive(false);    // 弾を消す
        } else if(col.tag == "UnderGround") {  // 奈落に落ちた時死ぬ機能
            DecreaseHP(1000);
        }

    }
}
