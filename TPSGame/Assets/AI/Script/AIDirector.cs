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



    // 接敵判定フィールド
    bool F_findEnemy = false;               // 敵を見つけているかどうか
    public bool Get_findEnemyFlag { get { return this.F_findEnemy; } }
    string PlayerName = "Player_02(Clone)";         // プレイヤの名前
    Transform T_Player;                             // プレイヤのtransform
    [SerializeField] Transform MyObject;            // 変身するオブジェクトたち
    float visibleRange = 30f;                      // AIがプレイヤを発見することが出来る距離
    [SerializeField] float visibleAngle = 60f;      // AIの視野の広さ
    float lostTime = 3f;                            // プレイヤが見えなくなってからあきらめるまでの時間
    float nowLostTime = 0;                          // 現在見失い続けている時間
    [SerializeField] LayerMask layermask;           // 当たり判定取得用のlayer 

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
        

        RespawnPos = transform.position;            // リスポーン地点取得
        Invoke(nameof(GetPlayer), 0.5f);              // プレイヤを取得
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

                break;
            case AIState.GOHOME:
                if(SearchPlayer()) {                // プレイヤを探す
                    SearchPlayer_Find();
                }

                break; 
            case AIState.FIGHT:
                if (S_Pdire.PState == 2) {          // プレイヤが死んだとき
                    nowLostTime = 0f;
                    SearchPlayer_Lost();
                    ChangeState((int)nextState);
                    break;
                }
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
                    S_Amove.SetDestPos(S_Aflag.GetDestination());   // 目的地を変更

                    // ステートを切り替える
                    nowState = (AIState)newState;
                }

                break;

            case AIState.GOHOME:
                if(F_findEnemy) {    // プレイヤと接敵していたら状態を移行しない
                    nextState = (AIState)newState;
                } else {
                    S_Amove.SetDestPos(S_Aflag.GetDestination());   // 目的地を変更

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
                
                // ステートを切り替える
                nowState = (AIState)newState;
                break;
        }

    }



    // プレイヤのTransformを取得
    void GetPlayer() {
        T_Player = GameObject.Find(PlayerName).transform;   // 名前からプレイヤを取得
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

}
