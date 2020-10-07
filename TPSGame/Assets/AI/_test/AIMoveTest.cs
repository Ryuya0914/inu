using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIMoveTest : MonoBehaviour
{
    [SerializeField] AIMove[] S_Amove;
    bool Mflag = false;

    void Start() {
        for (int i = 0; i < S_Amove.Length; i++) {
            S_Amove[i].SetMoveFlag = true;
        }
        Mflag = true;
    }

    void Update()
    {
        for (int i = 0; i < S_Amove.Length; i++) {
            S_Amove[i].SetDestPos(transform.position); // 目的地を変更
        }
        
    }
}
