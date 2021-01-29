// 旗を持ったり落としたりする
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFlag : MonoBehaviour
{

    string[] FlagName = new string[] { "Flag_1", "Flag_2" };    // 敵と自分の旗の区別をするためのタグ     　0:自分側, 1:敵側
    string[] ZoneName = new string[] { "Zone_1", "Zone_2" };    // 敵と自分の陣地の区別をするためのタグ     0:自分側, 1:敵側


    // 持っている旗のゲームオブジェクト(nullのときは持っていない)
    GameObject myFlag = null;
    // エフェクト    1:旗取得時, 2:旗もっているとき
    [SerializeField] EffectController[] S_effect;
    // 旗を拾えるかフラグ
    public bool FlagGetFlag = true;
    GameObject unnti;
    Off_StageDirector _1;
    Off_StageDirector_2 _2;
    int stage = 0;
    public TeamScript m_team;
    int m_teamNum = -1;



    void Start()
    {

        unnti = GameObject.Find("Stage_Director");

        if(unnti != null) {

            if(unnti.GetComponent<Off_StageDirector>()) {
                _1 = unnti.GetComponent<Off_StageDirector>();
                stage = 1;
            } else if(unnti.GetComponent<Off_StageDirector_2>()) {
                _2 = unnti.GetComponent<Off_StageDirector_2>();
                stage = 2;
            }




        }

    }

    public void SetTeam() {
        m_team = GetComponent<TeamScript>();
        if(m_team.m_teamColor == TeamScript.TeamColor.REDTEAM) {
            m_teamNum = 0;
        } else {
            m_teamNum = 1;
        }


    }

    // 旗を取得
    void GetFlag(GameObject f) {
        if (!FlagGetFlag) return;
        // エフェクト再生
        S_effect[0].EffectPlay(transform.position);
        S_effect[1].EffectPlay(Vector3.zero);
        // 旗の状態を更新
        f.GetComponent<Flag>().Take(transform);

        myFlag = f;
    }

    // 旗を落とす
    public void LostFlag() {
        if (myFlag != null) { 
            // エフェクト停止
            S_effect[1].EffectStop();
            // 旗を落とす
            myFlag.GetComponent<Flag>().Drop(transform.position);

            myFlag = null;
            FlagGetFlag = false;
        }
    }

    // 旗を拠点に戻す
    void ReturnFlag() {
        if (myFlag != null) {
            // エフェクト停止
            S_effect[1].EffectStop();
            // 旗の状態を更新
            myFlag.GetComponent<Flag>().ResetPos();

            myFlag = null;
        }
    }

    // 得点取得
    void GetPoint() {
        // 得点を得たことを通知
        if (stage == 1)
        {
            if (m_team.m_teamColor == TeamScript.TeamColor.REDTEAM) {
                _1.addA(3);

            } else {
                _1.addP(3);

            }
        }

        if (stage == 2)
        {
            if(m_team.m_teamColor == TeamScript.TeamColor.REDTEAM) {
                _2.addA(3);

            } else {
                _2.addP(3);

            }
        }
        // 旗をなくす
        ReturnFlag();
    }



    // 陣地,旗の当たり判定取得
    void OnTriggerEnter(Collider col) {
        if (col.tag == ZoneName[m_teamNum]) {           // 自分側の陣地
            if(myFlag != null) {  // 旗を持っていた時
                if (myFlag.tag == FlagName[m_teamNum])
                    ReturnFlag();   // 自分の旗を自分の陣地に戻す
                else if (myFlag.tag == FlagName[1 - m_teamNum])
                    GetPoint();     // 敵の旗を持ってきたのでポイントゲット
            }
        } else if (col.tag == FlagName[m_teamNum]) {    // 自分側の旗
            if (myFlag == null) {
                if (col.GetComponent<Flag>().state == 1)// 道中に旗が落ちているなら
                    GetFlag(col.gameObject);
            }
        } else if (col.tag == FlagName[1 - m_teamNum]) {    // 敵側の旗
            if (myFlag == null) {
                if (col.GetComponent<Flag>().state != 2)// 旗を誰も持っていないなら
                    GetFlag(col.gameObject);
            }
        }
    }

    IEnumerator GetInterval() {
        yield return new WaitForSeconds(5.1f);
    }

}
