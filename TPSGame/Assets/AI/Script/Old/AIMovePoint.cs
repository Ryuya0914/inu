using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIMovePoint : MonoBehaviour
{
    public int MyNumber = -1;
    public AIMovePoint[] GoRed;         // 青から赤の陣地へのルート
    public AIMovePoint[] GoBlue;        // 赤から青の陣地へのルート
    
    

    void Start() {
        GetComponent<MeshRenderer>().enabled = false;
    }


    // ルートを可視化する処理
    void OnDrawGizmosSelected() {
        Gizmos.color = new Color(1.0f, 0.1f, 0.1f);
        for(int i = 0; i < GoRed.Length; i++) {
            if(GoRed[i] != null) {
                Vector3 vec = GoRed[i].transform.position - transform.position;
                Vector3 vec2 = Vector3.Cross(vec, vec + Vector3.up);
                Gizmos.DrawRay(transform.position + vec2.normalized * 0.5f, vec);
            }
        }

        Gizmos.color = new Color(0.1f, 0.1f, 1.0f);
        for(int i = 0; i < GoBlue.Length; i++) {
            if(GoBlue[i] != null) {
                Vector3 vec = GoBlue[i].transform.position - transform.position;
                Vector3 vec2 = Vector3.Cross(vec, vec + Vector3.up);
                Gizmos.DrawRay(transform.position + vec2.normalized * 0.5f, vec);
            }
        }

        Gizmos.color = new Color(0.1f, 1.0f, 0.1f);
        Gizmos.DrawWireSphere(transform.position, 1.5f*1.5f);

    }


}
