using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIDirector : MonoBehaviour {
    // AIの状態関係 ＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊
    public enum AIState {                                               // AIの状態一覧
        WAIT,       // 待機中
        WALKSTART,  // 歩く準備
        WALK,       // 歩く
        WALKGOAL,   // 目的地にたどり着いたとき
        DEAD,       // 死んだとき
        RESPAWN     // 生き返る最中
    }
    public delegate void ChangeStateEvent(AIState _state);
    public delegate void ChangeFlagEvent(bool flag);

    // 状態
    ChangeStateEvent ChangeState;                                       // ステートが変わった時に呼び出すデリゲート
    public void RegisterEvent_ChangeState(ChangeStateEvent _event) {    // デリゲートにメソッドを追加する
        ChangeState += _event;
    }
    public AIState m_nowState = AIState.WAIT;                                     // 現在のステート
    public AIState NowState { get { return this.m_nowState; } set { ChangeState(value); } }    // ステートのゲッタ―セッター

    // 敵
    ChangeFlagEvent ChangeFindEnemyFlag;
    public void RegisterEvent_ChangeFindEnemyFlag(ChangeFlagEvent _event) {
        ChangeFindEnemyFlag += _event;
    }
    public bool m_findEnemyFlag = false;
    public bool FindEnemyFlag { get { return this.m_findEnemyFlag; } set { ChangeFindEnemyFlag(value); } }
    void SetFindEnemyFlag(bool flag) { m_findEnemyFlag = flag; m_nowEnemyLostTime = 0f; }

    // ＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊
    // 移動関係 ＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊
    AIMove S_Amove;
    AIRouteSearch S_Asearch;

    public List<Vector3> m_route = new List<Vector3>();    // 移動ルート
    public int m_routeIndex = 0;                           // 現在の移動ルート番号
    public Vector3 m_movePosition = Vector3.zero;             // 現在向かっている場所

    Vector3 m_moveVec = Vector3.forward;   // 動く方向
    float m_searchMoveVecInterval = 0.6f;  // 移動方向を調べる間隔
    float m_nowMoveTime = 0;               // 同じ方向に動いている時間

    [SerializeField] float PointSize = 2f;  // pointに到達した判定を取る長さ(半径)
    // ＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊
    // 敵関係＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊
    Transform[] m_enemyT;           // ステージ上のすべての敵
    public Transform m_fightingEnemy;      // 現在戦っている敵
    PlayerDirector m_fightingEnemyDirector;
    public bool m_enemyChaseFlag = false;  // 敵を追いかけるか

    float m_nowEnemyLostTime = 0f;  // 現在敵を見失っている時間
    float m_enemyLostTime = 4f;     // 敵を追うのをあきらめる時間 
    [SerializeField] float visibleRange = 30f;       // AIがプレイヤを発見することが出来る距離
    float visibleAngle = 60f;       // AIの視野の広さ
    [SerializeField] LayerMask layermask;           // 当たり判定取得用のlayer 
    [SerializeField] Transform MyObject;            // 変身するオブジェクトたち


    // ＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊
    // ライフ関係＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊
    float m_respawnTime = 5.0f;             // 生き返るまでの時間
    float m_nowRespawnTime = 0.0f;          // 死んでから経過した時間
    // ＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊
    // 変身関係＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊
    float m_transInterval = 7.0f;
    float m_nowTransInterval = 0.0f;
    // ＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊
    bool checkpointgoalFlag = false;
    bool checkpointgoalFlag2 = false;


    // スクリプト
    AIFlag S_Aflag;
    AIGun S_Agun;
    PlayerDirector S_Pdire;
    AILife S_Alife;
    AITransChange S_Atrans;

    //Off_StageDirector_2 unnti;


    // 移動ルートフィールド
    Vector3 GoalPos;
    Vector3 WayPoint;



    // その他
    [SerializeField] PlayerData Pdata;
    public PlayerData GetPData { get { return this.Pdata; } }
    ObjectData Odata;
    public ObjectData SetOdata { set { this.Odata = value; } }

    void Awake() {
        S_Amove = GetComponent<AIMove>();
        S_Aflag = GetComponent<AIFlag>();
        S_Agun = GetComponent<AIGun>();
        S_Asearch = GetComponent<AIRouteSearch>();
        S_Alife = GetComponent<AILife>();
        S_Atrans = GetComponent<AITransChange>();


        RegisterEvent_ChangeState(SetNowState);
        RegisterEvent_ChangeFindEnemyFlag(SetFindEnemyFlag);
        S_Aflag.RegisterEvent_ChangeHaveFlag(ChangeFlagState);


    }

    void Start() {

        
        Invoke(nameof(GetPlayer), 0.5f);              // プレイヤを取得
        //unnti = GameObject.Find("Stage_Director").GetComponent<Off_StageDirector_2>();

        Invoke(nameof(AActive), 1.5f);
        
    }


    // 毎フレーム処理 ＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊
    void Update() {
        switch(m_nowState) {
            case AIState.WAIT:
                WaitUpdate();
                break;

            case AIState.WALKSTART:
                WalkStartUpdate();
                break;
            
            case AIState.WALK:
                WalkUpdate();
                break;
            
            case AIState.WALKGOAL:
                WalkGoalUpdate();
                break;
            
            case AIState.DEAD:
                DeadUpdate();
                break;
            
            case AIState.RESPAWN:
                RespawnUpdate();
                break;
            
        }

    }

    // 待機中
    void WaitUpdate() {
        // ゲーム開始したら動くようにする
        //NowState = AIState.WALKSTART;
    }

    // 歩く準備
    void WalkStartUpdate() { 
        // 生きているか
        if (!CheckMyLife()) {
            NowState = AIState.DEAD;
            return;
        }
        m_nowMoveTime = m_searchMoveVecInterval;
        // 敵が見えているか
        if(FindEnemyFlag || CheckFindEnemy()) {
            // 敵までの位置を特定
            if(CheckEnemyPoint()) {
                m_nowState = AIState.WALK;
                return;
            }
        }
        // 目的地を設定
        CreateRoute();
        m_nowState = AIState.WALK;

    }  

    // 歩く
    void WalkUpdate() {
        // 生きているか
        if(!CheckMyLife()) {
            NowState = AIState.DEAD;
            return;
        }

        // 一定間隔で変身するようにする
        m_nowTransInterval += Time.deltaTime;
        if (m_nowTransInterval > m_transInterval) {
            // 変身させる
            if (S_Atrans.TransChange()) m_nowTransInterval = 0f;
        }

        // 一定間隔で移動方向を設定
        m_nowMoveTime += Time.deltaTime;
        if (m_nowMoveTime >= m_searchMoveVecInterval) {
            m_moveVec = S_Amove.SearchMovevec(m_route[m_routeIndex]);
            m_nowMoveTime = 0f;
        }

        // 敵が見えているか
        if(FindEnemyFlag) { // 敵を見つけているか
            if(m_enemyChaseFlag) {  // 敵を追いかけているか
                if(!FightEnemy()) { // 敵を見失っていないか
                    NowState = AIState.WALKSTART;
                    return;
                }
                S_Agun.SelectBullet(m_fightingEnemy.position);
                
                
            }
        } else {
            if(CheckFindEnemy()) {
                NowState = AIState.WALKSTART;
                return;
            }
        }

        // 目的地にたどり着いたか
        if(CheckWayPoint() && checkpointgoalFlag) {
            NowState = AIState.WALKGOAL;
            return;
        }



    }

    // 目的地にたどり着いたとき
    void WalkGoalUpdate() {
        checkpointgoalFlag = false;
        checkpointgoalFlag2 = false;
        // 生きているか
        if(!CheckMyLife()) {
            NowState = AIState.DEAD;
            return;
        }

        // 次の目的地に移動させる
        m_routeIndex++;
        if (m_routeIndex < m_route.Count) {
            m_nowMoveTime = m_searchMoveVecInterval;    // 移動方向を決定させるため
            m_movePosition = m_route[m_routeIndex];
            NowState = AIState.WALK;
            return;
        }
        if (m_routeIndex >= m_route.Count) {
            if(m_enemyChaseFlag) {
                m_routeIndex = m_route.Count - 1;
                m_route[m_routeIndex] = m_fightingEnemy.position;
                NowState = AIState.WALK;
                return;
            }
        }
        m_routeIndex = 0;
        NowState = AIState.WALKSTART;
    }

    // 死んだとき
    void DeadUpdate() {
        m_nowRespawnTime += Time.deltaTime;
        if(m_nowRespawnTime > m_respawnTime) {
            m_nowRespawnTime = 0f;
            NowState = AIState.RESPAWN;
        }
    }       

    // 生き返るとき
    void RespawnUpdate() { 
        S_Alife.Respawn();
        NowState = AIState.WALKSTART;
    }


    //  ＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊
    // 一定間隔毎処理 ＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊
    void FixedUpdate() {
        switch(m_nowState) {
            case AIState.WALK:
                WalkFixedUpdate();
                break;
            default:
                S_Amove.FixedWalk(Vector3.zero);
                break;
        }

    }

    // 歩く
    void WalkFixedUpdate() {
        if(FindEnemyFlag) {
            MyObject.transform.LookAt(transform.position + m_fightingEnemy.position);
        } else {
            MyObject.transform.LookAt(transform.position + m_moveVec);
        }
        // 移動させる
        S_Amove.FixedWalk(m_moveVec);
    }
    

    //  ＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊

    void GoalPoint() {
        checkpointgoalFlag = true;
    }
    // 現在の状態をチェックする＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊
    // 目的地までの距離
    bool CheckWayPoint() {

        Vector3 vec = m_route[m_routeIndex] - transform.position;
        if ((m_route.Count-1) == m_routeIndex) {
            if (vec.sqrMagnitude < PointSize) {
                if(!checkpointgoalFlag2) {
                    Invoke(nameof(GoalPoint), 2.0f);
                }
                checkpointgoalFlag2 = true;
                
                return true;
            }
            
        } else if (vec.sqrMagnitude < PointSize * PointSize) {
            checkpointgoalFlag = true;
            return true;
        }
        return false;
    }

    // 敵が見えているか
    bool CheckFindEnemy() {
        for (int i = 0; i < m_enemyT.Length; ++i) {
            // AIからプレイヤまでのベクトル
            Vector3 vec = m_enemyT[i].position - MyObject.position;

            // プレイヤまでの距離が一定以内だったら
            if(visibleRange >= vec.sqrMagnitude) {
                // AIの視界内にプレイヤがいるかどうか
                if(Vector3.Angle(MyObject.forward, vec) <= visibleAngle) {
                    // プレイヤと自身の間に障害物があるか調べる
                    if(Physics.Linecast(MyObject.position + Odata.BulletOffset, m_enemyT[i].position + new Vector3(0, 1f, 0), layermask.value)) {
                        continue;
                    }
                    m_fightingEnemy = m_enemyT[i];
                    m_fightingEnemyDirector = m_fightingEnemy.GetComponent<PlayerDirector>();
                    if (m_fightingEnemyDirector.PState == 2 ||m_fightingEnemyDirector.PState == 3) {
                        m_fightingEnemy = null;
                        m_fightingEnemyDirector = null;
                        continue;
                    }
                    FindEnemyFlag = true;
                    return true;
                }
            }
        }

        return false;
    } 
    // 敵を見失っていないか
    bool CheckLostEnemy(Vector3 enemypos) {
        // AIからプレイヤまでのベクトル
        Vector3 vec = enemypos - MyObject.position;

        // プレイヤまでの距離が一定以内だったら
        if(visibleRange >= vec.sqrMagnitude) {
            // AIの視界内にプレイヤがいるかどうか
            if(Vector3.Angle(MyObject.forward, vec) <= visibleAngle) {
                // プレイヤと自身の間に障害物があるか調べる
                if(Physics.Linecast(MyObject.position + Odata.BulletOffset, enemypos + new Vector3(0, 1f, 0), layermask.value)) {
                    return false;
                }
                return true;
            }
        }
        return false;
    }

    // 敵との位置関係
    bool CheckEnemyPoint() {
        List<Vector3> _list = S_Asearch.CheckSideBySide(MyObject.position, m_fightingEnemy.position);
        if (_list.Count > 0) {
            _list.Add(m_fightingEnemy.position);
            m_route.Clear();
            m_route.AddRange(_list);
            m_enemyChaseFlag = true;
            m_routeIndex = 0;
            return true;
        }
        m_enemyChaseFlag = false;
        return false;
    }

    // 生きているか
    public bool CheckMyLife() {
        return (S_Alife.GetHP() > 0);
    }
    
    // ＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊
    // その他処理 ＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊
    // 移動ルートを作る
    void CreateRoute() {
        List<Vector3> _list = new List<Vector3>();
        int num = S_Aflag.GetDestination();
        if (num == 2) {
            _list = S_Asearch.SearchRoute(S_Aflag.O_EneFlag.transform.position);
        } else if (num == 0) {
            _list = S_Asearch.SearchRoute(num);
            _list.Add(S_Aflag.O_myZone.transform.position);
        } else if (num == 1) {
            _list = S_Asearch.SearchRoute(num);
            _list.Add(S_Aflag.O_EneFlag.transform.position);
        }


        m_route.Clear();
        m_route = _list;
        m_routeIndex = 0;
    }
        
    // 移動中に敵を追いかけているときの処理
    bool FightEnemy() {
        // プレイヤが死んでいるか確認する
        if (m_fightingEnemyDirector.PState == 2 || m_fightingEnemyDirector.PState == 3) {
            m_nowEnemyLostTime = 0f;
            FindEnemyFlag = false;
            m_fightingEnemy = null;
            m_fightingEnemyDirector = null;
            return false;
        }

        if(!CheckLostEnemy(m_fightingEnemy.position)) {  // 敵を見失っていないか
            m_nowEnemyLostTime += Time.deltaTime;
            if(m_nowEnemyLostTime > m_enemyLostTime) { // 一定秒数以上見失っているか
                m_nowEnemyLostTime = 0f;
                FindEnemyFlag = false;
                m_fightingEnemy = null;
                m_fightingEnemyDirector = null;
                return false;
            }
            return true;
        } else {
            m_nowEnemyLostTime = 0f;
            return true;
        }

    }


    // ＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊
    // 状態が変わった時の処理＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊
    void ChangeFlagState(AIFlag.AIFlagState _state) {
        if (m_enemyChaseFlag) return;

        NowState = AIState.WALKSTART;

    }
    void SetNowState(AIState _state) { 
        m_nowState = _state;
        switch(_state) {
            case AIState.WAIT:

                break;
            case AIState.WALKSTART:

                break;
            case AIState.WALK:

                break;
            case AIState.WALKGOAL:

                break;
            case AIState.DEAD:
                S_Aflag.LostFlag();
                S_Alife.Dead();


                break;
            case AIState.RESPAWN:

                break;



        }
        
    }


    // ＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊



    // プレイヤのTransformを取得
    void GetPlayer() {
        // tagから取得
        GameObject[] _obj1 = GameObject.FindGameObjectsWithTag("PlayerParent");
        // 配列を初期化
        m_enemyT = new Transform[_obj1.Length];
        // for文でしまっていく
        for (int i = 0; i < _obj1.Length; ++i) {
            m_enemyT[i] = _obj1[i].transform;
        }
        //S_Pdire = T_Player.GetComponent<PlayerDirector>();
    }



    void AActive() {
        NowState = AIState.WALKSTART;
    }


}
