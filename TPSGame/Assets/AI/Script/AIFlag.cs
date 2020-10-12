using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIFlag : MonoBehaviour { 
    
    AIDirector S_Adire;     // ディレクタースクリプト  

    // 陣地のタグ
    string[] flagName = new string[] { "Flag_2", "Flag_1" };    // 敵と自分の旗の区別をするためのタグ     　0:自分側, 1:敵側
    string[] zoneName = new string[] { "Zone_2", "Zone_1" };    // 敵と自分の陣地の区別をするためのタグ     0:自分側, 1:敵側

    // 持っている旗のゲームオブジェクト(nullのときは持っていない)
    GameObject myFlag = null;
    // エフェクト    1:旗取得時, 2:旗もっているとき
    [SerializeField] EffectController[] S_effect;

    // 敵の旗と自分の陣地のゲームオブジェクト
    GameObject O_myZone;
    GameObject O_EneFlag;


    void Start()
    {
        // 自身のスクリプトを取得
        S_Adire = GetComponent<AIDirector>();
        // 敵の旗と自身の陣地のゲームオブジェクト取得
        Invoke(nameof(GetMyObj), 1.0f);
    }

    // 旗と陣地のゲームオブジェクトを取得
    void GetMyObj() {
        O_myZone = GameObject.FindGameObjectWithTag(zoneName[0]);
        O_EneFlag = GameObject.FindGameObjectWithTag(flagName[1]);
    }


    // 旗を取得
    void GetFlag(GameObject f) {
        int num = S_Adire.GetAIState;
        if(num == 3)
            return;
        // エフェクト再生
        S_effect[0].EffectPlay(transform.position);
        S_effect[1].EffectPlay(Vector3.zero);
        // 旗の状態を更新
        myFlag = f;
        f.GetComponent<Flag>().Take();

        S_Adire.ChangeState(2);
    }

    // 旗を落とす
    public void LostFlag() {
        if(myFlag != null) {
            // エフェクト停止
            S_effect[1].EffectStop();
            // 旗を落とす
            myFlag.GetComponent<Flag>().Drop(transform.position);

            myFlag = null;
        }
    }

    // 旗を拠点に戻す
    void ReturnFlag() {
        if(myFlag != null) {
            // エフェクト停止
            S_effect[1].EffectStop();
            // 旗の状態を更新
            myFlag.GetComponent<Flag>().ResetPos();
            myFlag = null;

            S_Adire.ChangeState(1);
        }
    }

    // 得点取得
    void GetPoint() {
        // 得点を得たことを通知
        Debug.Log("PointGet");
        // 旗をなくす
        ReturnFlag();
    }


    // 目的地を返すメソッド
    public Transform GetDestination() {
        if (myFlag == null) {   // 旗を持っていなかったら旗の位置を返す
            return O_EneFlag.transform;
        } else {                // 旗を持っていたら自分の陣地の位置を返す
            return O_myZone.transform;
        }
    }



    // 陣地,旗の当たり判定取得
    void OnTriggerEnter(Collider col) {
        if(col.tag == zoneName[0]) {           // 自分側の陣地
            if(myFlag != null) {  // 旗を持っていた時
                if(myFlag.tag == flagName[0])
                    ReturnFlag();   // 自分の旗を自分の陣地に戻す
                else if(myFlag.tag == flagName[1])
                    GetPoint();     // 敵の旗を持ってきたのでポイントゲット
            }
        } else if(col.tag == flagName[0]) {    // 自分側の旗
            if(myFlag == null) {
                if(col.GetComponent<Flag>().state == 1)// 道中に旗が落ちているなら
                    GetFlag(col.gameObject);
            }
        } else if(col.tag == flagName[1]) {    // 敵側の旗
            if(myFlag == null) {
                if(col.GetComponent<Flag>().state != 2)// 旗を誰も持っていないなら
                    GetFlag(col.gameObject);
            }
        }
    }
    
}
