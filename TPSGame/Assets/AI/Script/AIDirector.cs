using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIDirector : MonoBehaviour
{
    enum AIState {
        WAIT,       // 待機中
        GOFLAG,     // 敵の旗を目指す
        GOHOME,     // 自分の陣地に帰る
        DEAD        // 死んだとき
    }

    
    void Start()
    {
        
    }
    
    void Update()
    {
        
    }
}
