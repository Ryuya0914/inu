using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerLife : MonoBehaviour
{
    // オブジェクトのデータの格納場
    ObjectData Odata;
    // オブジェクトのデータ登録
    public void SetOdata(ObjectData odata){
        Odata = odata;
    }

    [SerializeField] PlayerDirector S_Pdirector;

    int mylife = 100;

    // HPを減らす
    void DecreaseHP(int damage) {
        mylife -= damage * 100 / Odata.MaxLife;
        HPUpdate();

        if(mylife < 0) {

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
            DecreaseHP(col.gameObject.GetComponent<Bullet>().GetSetDamage);
            col.gameObject.SetActive(false);    // 弾を消す
        } else if (col.tag == "UnderGround") {  // 奈落に落ちた時死ぬ機能
            DecreaseHP(1000);
        }

    }


}
