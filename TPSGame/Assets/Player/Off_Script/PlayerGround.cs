using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGround : MonoBehaviour
{
    [SerializeField] PlayerDirector S_Pdire;
    string[] tagsname = new string[] { "Ground", "Object" };

    void OnTriggerStay(Collider col) {  // コライダに触れている間
        for (int i = 0; i < tagsname.Length; i++) {
            if (col.tag == tagsname[i])
                S_Pdire.JumpFlag = true;
        }
    }

}
