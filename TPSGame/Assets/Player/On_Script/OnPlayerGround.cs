using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SoftGear.Strix.Unity.Runtime;

public class OnPlayerGround : StrixBehaviour
{
    [SerializeField] OnPlayerDirector S_Pdire;
    //string[] tagsname = new string[] { "Ground", "Object" , "NonObject", "" };

    void OnTriggerStay(Collider col) {  // コライダに触れている間
        if (!isLocal) return;

        S_Pdire.JumpFlag = true;
    }

}
