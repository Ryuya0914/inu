using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIMoveTest : MonoBehaviour {


    float[] rayVectors = new float[] { 0f, 45f, -45f, 90f, -90f, 135f, -135f, 180f };

    [SerializeField] AIObjectData oo;
    [SerializeField] LayerMask layer;
    [SerializeField] Transform T;

    void OnDrawGizmosSelected() {
        if(oo == null) return;

        Gizmos.color = new Color(1.0f, 0.1f, 0.1f);

        //Vector3 forwardVec = Enemy.transform.position - transform.position;
        Vector3 forwardVec = transform.forward;

        // 右が0度左回り 弧度法
        float angleNow = Mathf.Atan2(forwardVec.z, forwardVec.x) * Mathf.Rad2Deg;


        int i = 0;
 //       for(; i < rayVectors.Length; i++) {

            // 現在の方向から角度を変えて、ラジアンに変換する
            float angle = (angleNow + rayVectors[i]) * Mathf.Deg2Rad;
            // Rayを飛ばす方向
            Vector3 RayAngle = new Vector3(Mathf.Cos(angle), 0f, Mathf.Sin(angle));

            // 現在の方向から角度を変えて、ラジアンに変換する
            float anglePos = (angleNow + rayVectors[i] - 90f) * Mathf.Deg2Rad;
            // Rayの左右の方向
            Vector3 RayPosAngle = new Vector3(Mathf.Cos(anglePos), 0f, Mathf.Sin(anglePos));

            
            for(int j = 0; j < oo.pos.Length; j++) {

                if(Physics.BoxCast(transform.position + RayPosAngle * oo.pos[j].x + RayAngle * oo.pos[j].z + Vector3.up * oo.pos[j].y, oo.size[j] / 2f, RayAngle, out RaycastHit hit, T.localRotation * Quaternion.Euler(0, -rayVectors[i], 0), oo.rayLength[j], layer)) {
                    
                    Gizmos.color = new Color(0.1f, 0.1f, 1.0f);
                    Gizmos.DrawRay(transform.position + RayPosAngle * oo.pos[j].x + RayAngle * oo.pos[j].z + Vector3.up * oo.pos[j].y + RayPosAngle * oo.size[j].x / 2f + Vector3.up * oo.size[j].y /2, RayAngle * hit.distance);
                    Gizmos.DrawRay(transform.position + RayPosAngle * oo.pos[j].x + RayAngle * oo.pos[j].z + Vector3.up * oo.pos[j].y + -RayPosAngle * oo.size[j].x / 2f + Vector3.up * oo.size[j].y / 2, RayAngle * hit.distance);
                    Gizmos.DrawRay(transform.position + RayPosAngle * oo.pos[j].x + RayAngle * oo.pos[j].z + Vector3.up * oo.pos[j].y + RayPosAngle * oo.size[j].x / 2f + RayAngle * hit.distance + Vector3.up * oo.size[j].y / 2, -RayPosAngle * oo.size[j].x);
                
                    Gizmos.DrawRay(transform.position + RayPosAngle * oo.pos[j].x + RayAngle * oo.pos[j].z + Vector3.up * oo.pos[j].y + RayPosAngle * oo.size[j].x / 2f - Vector3.up * oo.size[j].y /2, RayAngle * hit.distance);
                    Gizmos.DrawRay(transform.position + RayPosAngle * oo.pos[j].x + RayAngle * oo.pos[j].z + Vector3.up * oo.pos[j].y + -RayPosAngle * oo.size[j].x / 2f - Vector3.up * oo.size[j].y / 2, RayAngle * hit.distance);
                    Gizmos.DrawRay(transform.position + RayPosAngle * oo.pos[j].x + RayAngle * oo.pos[j].z + Vector3.up * oo.pos[j].y + RayPosAngle * oo.size[j].x / 2f + RayAngle * hit.distance - Vector3.up * oo.size[j].y / 2, -RayPosAngle * oo.size[j].x);


                } else {
                    Gizmos.color = new Color(1.0f, 0.1f, 0.1f);
                    Gizmos.DrawRay(transform.position + RayPosAngle * oo.pos[j].x + RayAngle * oo.pos[j].z + Vector3.up * oo.pos[j].y + RayPosAngle * oo.size[j].x / 2f + Vector3.up * oo.size[j].y / 2, RayAngle * oo.rayLength[j]);
                    Gizmos.DrawRay(transform.position + RayPosAngle * oo.pos[j].x + RayAngle * oo.pos[j].z + Vector3.up * oo.pos[j].y + -RayPosAngle * oo.size[j].x / 2f + Vector3.up * oo.size[j].y / 2, RayAngle * oo.rayLength[j]);
                    Gizmos.DrawRay(transform.position + RayPosAngle * oo.pos[j].x + RayAngle * oo.pos[j].z + Vector3.up * oo.pos[j].y + RayPosAngle * oo.size[j].x / 2f + RayAngle * oo.rayLength[j] + Vector3.up * oo.size[j].y / 2, -RayPosAngle * oo.size[j].x);
                    Gizmos.DrawRay(transform.position + RayPosAngle * oo.pos[j].x + RayAngle * oo.pos[j].z + Vector3.up * oo.pos[j].y + RayPosAngle * oo.size[j].x / 2f - Vector3.up * oo.size[j].y / 2, RayAngle * oo.rayLength[j]);
                    Gizmos.DrawRay(transform.position + RayPosAngle * oo.pos[j].x + RayAngle * oo.pos[j].z + Vector3.up * oo.pos[j].y + -RayPosAngle * oo.size[j].x / 2f - Vector3.up * oo.size[j].y / 2, RayAngle * oo.rayLength[j]);
                    Gizmos.DrawRay(transform.position + RayPosAngle * oo.pos[j].x + RayAngle * oo.pos[j].z + Vector3.up * oo.pos[j].y + RayPosAngle * oo.size[j].x / 2f + RayAngle * oo.rayLength[j] - Vector3.up * oo.size[j].y / 2, -RayPosAngle * oo.size[j].x);
                }


            }
        //}
    }
}
