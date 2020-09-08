// 旗を持ったり落としたりする
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFlag : MonoBehaviour
{
    // 陣地のタグ
    string[] zonename = new string[2];
    // 旗のタグ
    string[] flagname = new string[2];
    // 持っている旗のゲームオブジェクト(nullのときは持っていない)
    GameObject myFlag = null;

    // 敵と味方の旗・陣地のタグの名前を設定
    public void NameSet(string f1, string f2, string z1, string z2) {
        flagname[0] = f1;
        flagname[1] = f2;

        zonename[0] = z1;
        zonename[1] = z2;
    }

    // 旗を取得
    void GetFlag(GameObject f) {
        // エフェクト再生

        // 旗の状態を更新
        f.GetComponent<Flag>().Take();

        myFlag = f;
    }

    // 旗を落とす
    public void LostFlag() {
        if (myFlag != null) { 
            // エフェクト停止

            // 旗を落とす
            myFlag.GetComponent<Flag>().Drop(transform.position);

            myFlag = null;

        }
    }

    // 旗を拠点に戻す
    void ReturnFlag() {
        if (myFlag != null) {
            // エフェクト停止

            // 旗の状態を更新
            myFlag.GetComponent<Flag>().ResetPos();

            myFlag = null;
        }
    }

    // 得点取得
    void GetPoint() {
        // 得点を得たことを通知
        Debug.Log("PointGet");
        // 旗をなくす
        ReturnFlag();
    }



    // 陣地,旗の当たり判定取得
    void OnTriggerEnter(Collider col) {
        if (col.tag == zonename[0]) {           // 自分側の陣地
            if(myFlag != null) {  // 旗を持っていた時
                if (myFlag.tag == flagname[0])
                    ReturnFlag();   // 自分の旗を自分の陣地に戻す
                else if (myFlag.tag == flagname[1])
                    GetPoint();     // 敵の旗を持ってきたのでポイントゲット
            }
        } else if (col.tag == flagname[0]) {    // 自分側の旗
            if (myFlag == null) {
                if (col.GetComponent<Flag>().state == 1)// 道中に旗が落ちているなら
                    GetFlag(col.gameObject);
            }
        } else if (col.tag == flagname[1]) {    // 敵側の旗
            if (myFlag == null) {
                if (col.GetComponent<Flag>().state != 2)// 旗を誰も持っていないなら
                    GetFlag(col.gameObject);
            }
        }
    }

}
