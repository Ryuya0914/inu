using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIDirector : MonoBehaviour
{
    // AIの状態一覧
    enum AIState {
        WAIT,       // 待機中
        GOFLAG,     // 敵の旗を目指す
        GOHOME,     // 自分の陣地に帰る
        DEAD,       // 死んだとき
        FIGHT       // 戦ってるとき
    }
    // AIの現在の状態
    AIState nowState = AIState.WAIT;
    AIState nextState = AIState.GOFLAG;
    public int GetAIState { get { return (int)nowState; } }

    // スクリプト
    AIMove S_Amove;
    AIFlag S_Aflag;
    AIGun S_Agun;
    PlayerDirector S_Pdire;
    AIRouteSearch S_Asearch;
    //Off_StageDirector_2 unnti;


    // 接敵判定フィールド
    bool F_findEnemy = false;               // 敵を見つけているかどうか
    public bool Get_findEnemyFlag { get { return this.F_findEnemy; } }
    Transform T_Player;                             // プレイヤのtransform
    [SerializeField] Transform MyObject;            // 変身するオブジェクトたち
    float visibleRange = 30f;                      // AIがプレイヤを発見することが出来る距離
    [SerializeField] float visibleAngle = 60f;      // AIの視野の広さ
    float lostTime = 3f;                            // プレイヤが見えなくなってからあきらめるまでの時間
    float nowLostTime = 0;                          // 現在見失い続けている時間
    [SerializeField] LayerMask layermask;           // 当たり判定取得用のlayer 

    // 移動ルートフィールド
    Vector3 GoalPos;
    Vector3 WayPoint;




    // その他
    [SerializeField] PlayerData Pdata;
    public PlayerData GetPData { get { return this.Pdata; } }
    ObjectData Odata;
    public ObjectData SetOdata { set { this.Odata = value; } }
    Vector3 RespawnPos;


    void Start()
    {
        S_Amove = GetComponent<AIMove>();
        S_Aflag = GetComponent<AIFlag>();
        S_Agun = GetComponent<AIGun>();
        S_Asearch = GetComponent<AIRouteSearch>();
        

        RespawnPos = transform.position;            // リスポーン地点取得
        Invoke(nameof(GetPlayer), 0.5f);              // プレイヤを取得
        //unnti = GameObject.Find("Stage_Director").GetComponent<Off_StageDirector_2>();

        Invoke(nameof(AActive), 1.0f);
    }

    void Update()
    {
        if(Input.GetKey(KeyCode.H)) {
            Debug.DrawLine(transform.position, Vector3.Normalize(T_Player.position - transform.position) * visibleRange + transform.position);
        }


        switch(nowState) {
            case AIState.GOFLAG:
                if(SearchPlayer()) {                // プレイヤを探す
                    SearchPlayer_Find();
                }
                CheckWayPoint();


                break;
            case AIState.GOHOME:
                if(SearchPlayer()) {                // プレイヤを探す
                    SearchPlayer_Find();
                }

                CheckWayPoint();

                break; 
            case AIState.FIGHT:
                if (S_Pdire.PState == 2) {          // プレイヤが死んだとき
                    nowLostTime = 0f;
                    SearchPlayer_Lost();
                    ChangeState((int)nextState);
                    break;
                }
                S_Amove.SetDestPos(T_Player);   // 目的地を変更
                if(!SearchPlayer()) {               // プレイヤを探す
                    nowLostTime += Time.deltaTime;
                    if (nowLostTime > lostTime) {       // 一定秒数以上見失い続けていたら
                        nowLostTime = 0f;
                        SearchPlayer_Lost();
                        ChangeState((int)nextState);    // 状態を切り替える
                    }
                } else {
                    nowLostTime = 0f;
                }

                break;
            case AIState.WAIT:

                
                break;
            case AIState.DEAD:

                
                break;            
        }


    }


    // ステートが切り替わった時に行う処理
    public void ChangeState(int newState) {

        switch((AIState)newState) {
            case AIState.GOFLAG:
                if (F_findEnemy) {    // プレイヤと接敵していたら状態を移行しない
                    nextState = (AIState)newState;
                } else {
                    GoalPos = S_Aflag.GetDestination().position;
                    WayPoint = S_Asearch.SearchRoute(1);
                    S_Amove.SetDestPos(WayPoint);
                    CheckWayPoint();

                    // ステートを切り替える
                    nowState = (AIState)newState;
                }

                break;

            case AIState.GOHOME:
                if(F_findEnemy) {    // プレイヤと接敵していたら状態を移行しない
                    nextState = (AIState)newState;
                } else {
                    GoalPos = S_Aflag.GetDestination().position;
                    WayPoint = S_Asearch.SearchRoute(0);
                    S_Amove.SetDestPos(WayPoint);
                    CheckWayPoint();

                    // ステートを切り替える
                    nowState = (AIState)newState;
                }
                break;

            case AIState.FIGHT:
                nextState = nowState;

                S_Amove.SetDestPos(T_Player);   // 目的地を変更
                S_Agun.SetPlayerT = T_Player;



                // ステートを切り替える
                nowState = (AIState)newState;
                break;

            case AIState.WAIT:
                S_Agun.SetPlayerT = null;

                // ステートを切り替える
                nowState = (AIState)newState;
                break;

            case AIState.DEAD:
                nextState = AIState.GOFLAG;

                SearchPlayer_Lost();
                S_Aflag.LostFlag(); // 旗を落とす
                //unnti.addA(-1);
                // ステートを切り替える
                nowState = (AIState)newState;
                break;
        }

    }



    // プレイヤのTransformを取得
    void GetPlayer() {
        T_Player = GameObject.FindGameObjectWithTag("PlayerParent").transform;
        S_Pdire = T_Player.GetComponent<PlayerDirector>();
    }


    // プレイヤを探す
    bool SearchPlayer() {
        // AIからプレイヤまでのベクトル
        Vector3 vec = T_Player.position - MyObject.position;

        // プレイヤまでの距離が一定以内だったら
        if(visibleRange >= Mathf.Sqrt(vec.x * vec.x + vec.y * vec.y + vec.z * vec.z)) {
            // AIの視界内にプレイヤがいるかどうか
            if (Vector3.Angle(MyObject.forward, vec) <= visibleAngle) {
                // プレイヤと自身の間に障害物があるか調べる
                if (Physics.Linecast(transform.position + Odata.BulletOffset, T_Player.position + new Vector3(0, 1f, 0), layermask.value)) {
                    return false;
                }

                return true;
            }
        }
        
        return false;
    }

    // プレイヤを見つけた時
    void SearchPlayer_Find() {
        F_findEnemy = true;             // プレイヤ発見フラグをオンにする

        ChangeState((int)AIState.FIGHT);// 状態を切り替える


        //int selectAct = Random.Range(0, 3); // 次の行動をランダムで決める
        //switch(selectAct) {
        //    case 0:     // 戦う
        //        ChangeState((int)AIState.FIGHT);
        //        break;
        //    case 1:     // 止まる
        //        ChangeState((int)AIState.WAIT);
        //        break;
        //    case 2:     // 無視する

        //        break;
        //}



    }

    // プレイヤを見失ったとき
    void SearchPlayer_Lost() {
        F_findEnemy = false;            // プレイヤ発見フラグを消す
        S_Agun.SetPlayerT = null;       // プレイヤを撃たないようにする


    }

    // AIを動かす(試合が始まった時に呼んでもらう)
    public void AActive() {
        ChangeState((int)AIState.GOFLAG);
    }


    // 中間地点までの距離を調べ、近かったら次の中間地点を設定
    void CheckWayPoint() {
        Vector3 v = WayPoint - transform.position;
        if(v.magnitude <= 2.0f) {
            Transform t = S_Asearch.GetDestinationT();
            if(t != null) {
                WayPoint = t.position;
                S_Amove.SetDestPos(WayPoint);
            } else {
                S_Amove.SetDestPos(GoalPos);
            }
        }
    }

}
