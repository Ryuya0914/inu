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

    
    void Start()
    {
        
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
}
