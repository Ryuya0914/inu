using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIFlag : MonoBehaviour {

    // 旗の状態関係 ＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊
    public enum AIFlagState {
        NONE,   // 旗を持っていないとき
        MINE,   // 自分の陣地の旗を持っているとき
        OTHER   // 敵の陣地の旗を持っているとき
    }
    public delegate void ChangeHaveEvent(AIFlagState _state);

    ChangeHaveEvent ChangeHaveFlag;
    public void RegisterEvent_ChangeHaveFlag(ChangeHaveEvent _event) {
        ChangeHaveFlag += _event;
    }
    AIFlagState m_haveFlag = 0;
    public AIFlagState HaveFlag { get { return this.m_haveFlag; } set { ChangeHaveFlag(value); } }
    void SetHaveFlag(AIFlagState _flag) { m_haveFlag = _flag; }
    // 旗の状態関係 ＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊


    AIDirector S_Adire;     // ディレクタースクリプト  

    // 陣地のタグ
    string[] flagName = new string[] { "Flag_1", "Flag_2" };    // 敵と自分の旗の区別をするためのタグ     　0:自分側, 1:敵側
    string[] zoneName = new string[] { "Zone_1", "Zone_2" };    // 敵と自分の陣地の区別をするためのタグ     0:自分側, 1:敵側

    // 持っている旗のゲームオブジェクト(nullのときは持っていない)
    GameObject myFlag = null;
    // エフェクト    1:旗取得時, 2:旗もっているとき
    [SerializeField] EffectController[] S_effect;

    // 敵の旗と自分の陣地のゲームオブジェクト
    public GameObject O_myZone;
    public GameObject O_EneFlag;
    Flag m_enemyFlag;

    Score unnti;

    PlayerUI S_Pui;

    TeamScript m_team;

    void Awake() {
        // 自身のスクリプトを取得
        S_Adire = GetComponent<AIDirector>();
        m_team = GetComponent<TeamScript>();
        RegisterEvent_ChangeHaveFlag(SetHaveFlag);
    }

    void Start()
    {
        // 敵の旗と自身の陣地のゲームオブジェクト取得
        Invoke(nameof(GetMyObj), 0.5f);

        unnti = GameObject.Find("Stage_Director").GetComponent<Score>();

        S_Pui = GameObject.Find("PlayerCanvas").GetComponent<PlayerUI>();

    }

    // 旗と陣地のゲームオブジェクトを取得
    void GetMyObj() {
        O_myZone = GameObject.FindGameObjectWithTag(zoneName[(int)m_team.m_teamColor]);
        O_EneFlag = GameObject.FindGameObjectWithTag(flagName[1 - (int)m_team.m_teamColor]);
        m_enemyFlag = O_EneFlag.GetComponent<Flag>();
    }


    // 旗を取得
    void GetFlag(GameObject f) {
        
        // エフェクト再生
        S_effect[0].EffectPlay(transform.position);
        S_effect[1].EffectPlay(Vector3.zero);
        // 旗の状態を更新
        myFlag = f;
        f.GetComponent<Flag>().Take(transform);

        //S_Adire.ChangeState(2);
    }

    // 旗を落とす
    public void LostFlag() {
        if(HaveFlag != AIFlagState.NONE) {
            HaveFlag = AIFlagState.NONE;
            S_Pui.FlagGetLavelOff();

            // エフェクト停止
            S_effect[1].EffectStop();
            // 旗を落とす
            myFlag.GetComponent<Flag>().Drop(transform.position);

            myFlag = null;
        }
    }

    // 旗を拠点に戻す
    void ReturnFlag() {
        if(HaveFlag != AIFlagState.NONE) {
            HaveFlag = AIFlagState.NONE;

            S_Pui.FlagGetLavelOff();
            // エフェクト停止
            S_effect[1].EffectStop();
            // 旗の状態を更新
            myFlag.GetComponent<Flag>().ResetPos();
            myFlag = null;

           // S_Adire.ChangeState(1);
        }
    }

    // 得点取得
    void GetPoint() {
        // 得点を得たことを通知
        if (m_team.m_teamColor == TeamScript.TeamColor.REDTEAM) {
            unnti.addA(3);
        } else {
            unnti.addP(3);
        }
        // 旗をなくす
        ReturnFlag();
    }


    // 目的地を返すメソッド
    public int GetDestination() {

    
        switch(HaveFlag) {
            case AIFlagState.NONE:
                if (m_enemyFlag.state == 0) return 1 - (int)m_team.m_teamColor;
                return 2;
            case AIFlagState.MINE:
                return (int)m_team.m_teamColor;
                
            case AIFlagState.OTHER:
                return (int)m_team.m_teamColor;
        }

        return -1;
    }



    // 陣地,旗の当たり判定取得
    void OnTriggerEnter(Collider col) {
        if(S_Adire.NowState == AIDirector.AIState.WALK || S_Adire.NowState == AIDirector.AIState.WALKSTART || S_Adire.NowState == AIDirector.AIState.WALKGOAL) {
            if(col.tag == zoneName[(int)m_team.m_teamColor]) {           // 自分側の陣地
                switch(HaveFlag) {
                    case AIFlagState.MINE:
                        ReturnFlag();   // 自分の旗を自分の陣地に戻す
                        break;
                    case AIFlagState.OTHER:
                        GetPoint();     // 敵の旗を持ってきたのでポイントゲット
                        break;
                }

            } else if(col.tag == flagName[(int)m_team.m_teamColor]) {    // 自分側の旗
                if(HaveFlag == AIFlagState.NONE) {
                    if(col.GetComponent<Flag>().state == 1) {// 道中に旗が落ちているなら
                        HaveFlag = AIFlagState.MINE;
                        GetFlag(col.gameObject);
                    }
                }

            } else if(col.tag == flagName[1 - (int)m_team.m_teamColor]) {    // 敵側の旗
                if(HaveFlag == AIFlagState.NONE) {
                    if(col.GetComponent<Flag>().state != 2) {// 旗を誰も持っていないなら
                        S_Pui.FlagGetLavelOn();
                        HaveFlag = AIFlagState.OTHER;
                        GetFlag(col.gameObject);

                    }
                }
            }
        }
    }

}
