using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGround : MonoBehaviour
{
    [SerializeField] PlayerDirector S_Pdire;
    //string[] tagsname = new string[] { "Ground", "Object" , "NonObject", "" };

    void OnTriggerStay(Collider col) {  // コライダに触れている間
        //S_Pdire.JumpFlag = true;
    }

}
