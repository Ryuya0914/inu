using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SoftGear.Strix.Unity.Runtime;

public class OnPlayerLife : StrixBehaviour
{
    // オブジェクトのデータの格納場
    ObjectData Odata;
    // オブジェクトのデータ登録
    public void SetOdata(ObjectData odata) {
        Odata = odata;
    }

    [SerializeField] PlayerDirector S_Pdirector;
    TeamScript m_team;
    int mylife = 100;

    void Start() {
        Invoke(nameof(SetTeam), 0.5f);
    }

    void SetTeam() {
        m_team = GetComponent<TeamScript>();
    }


    // HPを減らす
    void DecreaseHP(int damage) {
        if(mylife > 0) {
            mylife -= damage * 100 / Odata.MaxLife;
            HPUpdate();
        }
    }

    // HP全回復
    public void RecoveryHP() {
        mylife = 100;
        HPUpdate();
    }

    // UIを変更
    void HPUpdate() {
        S_Pdirector.LifeUpdate(mylife);
    }

    // 弾との当たり判定
    void OnTriggerEnter(Collider col) {
        if(col.tag == "Bullet") {
            if(col.GetComponent<Bullet>().m_myTeam.m_teamColor != m_team.m_teamColor) {
                DecreaseHP(col.gameObject.GetComponent<Bullet>().GetSetDamage);
            }
            col.gameObject.SetActive(false);    // 弾を消す
        } else if(col.tag == "UnderGround") {  // 奈落に落ちた時死ぬ機能
            DecreaseHP(1000);
        }

    }



}
