// 一定間隔毎に呼び出されて、どこに進むかを選択する

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIMove : MonoBehaviour {
    // オブジェクトのデータ
    [SerializeField] ObjectData Odata;

    // Ray飛ばす用
    [SerializeField] AIObjectData aiOData;
    [SerializeField] LayerMask layermask;       // Rayの衝突するオブジェクトを制限
    float[] rayVectors = new float[] { 0f, 45f, -45f, 90f, -90f, 135f, -135f, 180f };

    // その他
    AIDirector S_Adire;
    Rigidbody rb;                       // 移動時に使う
    [SerializeField] GameObject myObjects;

    // オブジェクトデータ変更
    public void SetOdata(ObjectData data, AIObjectData data2) {
        this.Odata = data;
        this.aiOData = data2;
    }

    void Awake() {
        S_Adire = GetComponent<AIDirector>();
        rb = GetComponent<Rigidbody>(); // rigidbodyを取得
    }


    public void FixedWalk(Vector3 _vec) {
        // 加速度を使って移動させる
        Vector3 v = _vec * Odata.MoveSpeed;
        v.y = rb.velocity.y;
        rb.velocity = v;

    }

    int RandCount = 0;
    int ramd = 0;
    //進む方向を決めるメソッド
    public Vector3 SearchMovevec(Vector3 _pos) {

        // 経路探索時に右と左どちらを優先して調べるか決める
        if (RandCount > 3) {
            // ランダムで正面の障害物を確かめた後に左右どちらから調べるかを決める
            ramd = (Random.Range(0, 2) * 2) -1;
            RandCount = 0;
        } else {
            RandCount++;
        }


        // 目的地へのベクトルを作成
        Vector3 forwardVec = (_pos - transform.position);
        forwardVec.y = 0f;
        forwardVec = forwardVec.normalized;
        // 右が0度左回り 弧度法
        float angleNow = Mathf.Atan2(forwardVec.z, forwardVec.x) * Mathf.Rad2Deg;

        for(int i = 0; i < rayVectors.Length; i++) {
            // 現在の方向から角度を変えて、ラジアンに変換する
            float angle = (angleNow + rayVectors[i] * ramd) * Mathf.Deg2Rad;

            // Rayを飛ばす方向
            Vector3 RayAngle = new Vector3(Mathf.Cos(angle), 0f, Mathf.Sin(angle));

            // 現在の方向から角度を変えて、ラジアンに変換する
            float anglePos = (angleNow + rayVectors[i] * ramd - 90f) * Mathf.Deg2Rad;

            // Rayの左右の方向
            Vector3 RayPosAngle = new Vector3(Mathf.Cos(anglePos), 0f, Mathf.Sin(anglePos));
       
            bool f = true;

            for(int j = 0; j < aiOData.pos.Length; j++) {
                // Rayを飛ばす
                RaycastHit hit;
                Vector3 boxpos = transform.position + RayPosAngle * aiOData.pos[j].x + RayAngle * aiOData.pos[j].z + Vector3.up * aiOData.pos[j].y;
                if(Physics.BoxCast(boxpos, aiOData.size[j] / 2f, RayAngle, out hit, myObjects.transform.localRotation * Quaternion.Euler(0, -rayVectors[i] * ramd, 0), aiOData.rayLength[j], layermask)) {
                    if (!(hit.normal.y > 0.1f)) {
                        f = false;

                        {
                            Color _color = new Color(1.0f, 0.1f, 0.1f);
                            Debug.DrawRay(boxpos, RayAngle * hit.distance, _color, 1.0f);
                            Debug.DrawRay(boxpos + RayPosAngle * aiOData.size[j].x / 2f + RayAngle * hit.distance + Vector3.up * aiOData.size[j].y / 2f, -RayPosAngle * aiOData.size[j].x, _color, 1.0f);
                            Debug.DrawRay(boxpos + RayPosAngle * aiOData.size[j].x / 2f + RayAngle * hit.distance - Vector3.up * aiOData.size[j].y / 2f, -RayPosAngle * aiOData.size[j].x, _color, 1.0f);
                        }

                        break;
                    }

                } else {

                    {
                        Color _color = new Color(0.1f, 0.1f, 0.1f);
                        Debug.DrawRay(boxpos, RayAngle * aiOData.rayLength[j], _color, 1.0f);
                        Debug.DrawRay(boxpos + RayPosAngle * aiOData.size[j].x / 2f + RayAngle * aiOData.rayLength[j] + Vector3.up * aiOData.size[j].y / 2f, -RayPosAngle * aiOData.size[j].x, _color, 1.0f);
                        Debug.DrawRay(boxpos + RayPosAngle * aiOData.size[j].x / 2f + RayAngle * aiOData.rayLength[j] - Vector3.up * aiOData.size[j].y / 2f, -RayPosAngle * aiOData.size[j].x, _color, 1.0f);
                    }
                }
                
            }

            if(f) {
                // オブジェクトの角度を変える
                return RayAngle;
            }

        }


        return Vector3.forward;

    }
    
    
}
