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
        DEAD        // 死んだとき
    }
    // AIの現在の状態
    AIState nowState = AIState.WAIT;
    AIState nextState = AIState.GOFLAG;
    public int GetAIState { get { return (int)nowState; } }

    // スクリプト
    AIMove S_Amove;
    AIFlag S_Aflag;



    // 敵に関すること
    bool F_findEnemy = false;
    


    
    void Start()
    {
        S_Amove = GetComponent<AIMove>();
        S_Aflag = GetComponent<AIFlag>();
    }
    
    void Update()
    {

        switch(nowState) {
            case AIState.GOFLAG:

                break;
            case AIState.GOHOME:
                
                break;
            case AIState.WAIT:
                
                break;
            case AIState.DEAD:
                
                break;
        }


    }


    // ステートが切り替わった時に行う処理
    public void ChangeState(int newState) {
        // ステートを切り替える
        nowState = (AIState)newState;

        switch(nowState) {
            case AIState.GOFLAG:
                S_Amove.SetDestPos(S_Aflag.GetDestination());   // 目的地を変更

                break;
            case AIState.GOHOME:
                S_Amove.SetDestPos(S_Aflag.GetDestination());   // 目的地を変更


                break;
            case AIState.WAIT:
                

                break;
            case AIState.DEAD:
                S_Aflag.LostFlag(); // 旗を落とす

                break;
        }

    }



}
