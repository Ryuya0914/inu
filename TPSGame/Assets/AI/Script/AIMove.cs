// 一定間隔毎に呼び出されて、どこに進むかを選択する

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIMove : MonoBehaviour {
    // オブジェクトのデータ
    [SerializeField] ObjectData Odata;

    
    // 移動する方向に関する数値
    Vector3 destinationPos;     // 現在の目的地
    Vector3 nowMoveVec = Vector3.zero;  // 現在進む方向
    [SerializeField] float searchInterval = 0.8f;        // 経路を探索する間隔

    // Ray飛ばす用
    [SerializeField] GameObject RayPosParent;   // Rayを飛ばす位置のオブジェクトをAIオブジェクト中心に回転させる用
    [SerializeField] Transform[] raypos = new Transform[3];      // Rayを飛ばす位置にゲームオブジェクトを設置
    [SerializeField] float rayRange = 2.0f;     // Rayの長さ                                                                       シリアライズ後で消す
    [SerializeField] LayerMask layermask;       // Rayの衝突するオブジェクトを制限
    float[,] GoRayVectors = new float[,] { 
        { 0, 45, -45, 90, -90, 135, -135, 180 }, 
        { 0, -45, 45, -90, 90, -135, 135, 180 } 
    };
    
    // その他
    AIDirector S_Adire;
    Rigidbody rb;                       // 移動時に使う
    [SerializeField] GameObject myObjects;

    // オブジェクトが変わった時にrayを飛ばすオブジェクトの位置を変えるよう
    [SerializeField] Vector3[] rayposScales;

    // オブジェクトデータ変更
    public void SetOdata(ObjectData data) {
        this.Odata = data;
        //RayPosParent.transform.localScale = rayposScales[data.ObjSizeNum];
        Transform t = RayPosParent.transform.GetChild(0);
        t.transform.localPosition = data.AImoveLayPos;
        t.transform.localScale = data.AImoveLayScale;
        //int ObjNum = data.ObjectNum;
        //if(20 <= ObjNum && ObjNum <= 29) {
        //    RayPosParent.transform.localScale = rayposScales[0];
        //} else if(30 <= ObjNum && ObjNum <= 39) {
        //    RayPosParent.transform.localScale = rayposScales[1];
        //} else if(40 <= ObjNum && ObjNum <= 49) {
        //    RayPosParent.transform.localScale = rayposScales[2];
        //}

    }

    void Start() {
        S_Adire = GetComponent<AIDirector>();
        rb = GetComponent<Rigidbody>(); // rigidbodyを取得
        StartCoroutine(nameof(ReSearchRoute));  // 経路を一定間隔で再探索するコルーチンをスタートする
    }

    void FixedUpdate() {
        int num = S_Adire.GetAIState;
        if(num == 1 || num == 2 || num == 4) {
            // 加速度を使って移動させる
            Vector3 v = nowMoveVec * Odata.MoveSpeed;
            v.y = rb.velocity.y;
            rb.velocity = v;
        }
    }


    // 目的地を変更
    public void SetDestPos(Transform vec) {
        destinationPos = vec.position;
    }   
    // 目的地を変更
    public void SetDestPos(Vector3 vec) {
        destinationPos = vec;
    }


    int RandCount = 0;
    int ramd = 0;
    //進む方向を決めるメソッド
    public void SearchMovevec() {
        //// かくかく移動を軽減するために、最低2回同じ方向に進むようにする
        //if(SameVecCount <= 1) {
        //    if(GoRay(RayPosParent.transform.forward)) {
        //        SameVecCount++;
        //        return;
        //    }
        //}

        // 目的地へのベクトルを作成
        Vector3 destVec = destinationPos - transform.position;
        destVec.y = 0f;
        destVec = destVec.normalized;


        if (RandCount > 3) {
            // ランダムで正面の障害物を確かめた後に左右どちらから調べるかを決める
            ramd = Random.Range(0, 2);
            RandCount = 0;
        } else {
            RandCount++;
        }
        
        for (int i = 0; i < GoRayVectors.Length/2; i++) {
            // Rayを飛ばすオブジェクトの向きを変える
            RayPosParent.transform.LookAt(RayPosParent.transform.position + destVec);
            RayPosParent.transform.Rotate(new Vector3(0, GoRayVectors[ramd,i]));
            //Debug.Log(RayPosParent.transform.rotation);
            if(GoRay()) {  // 指定した方向に障害物がないか調べる     
                nowMoveVec = RayPosParent.transform.forward;
                if(!S_Adire.Get_findEnemyFlag) { // 敵を見つけていないとき
                    // 移動する方向を向く
                    myObjects.transform.LookAt(transform.position + nowMoveVec);
                }
                break;
            } else {
                nowMoveVec = destVec;
            }
        }
    }


    // Rayを飛ばして障害物がないか調べる
    bool GoRay() {
        // Rayを飛ばす方向にRayposオブジェクトを移動させる

        for (int i = 0; i < raypos.Length; i++) {
            // Rayを作成
            Ray ray = new Ray(raypos[i].position, raypos[i].forward);
            // デバッグ用にRayを可視化
            Debug.DrawRay(ray.origin, raypos[i].forward * rayRange, Color.red, 0.6f);                                                               // 後で消す

            // Rayを飛ばす
            RaycastHit hit;
            if(Physics.Raycast(ray, out hit, rayRange, layermask)) {
                //if (hit.collider.tag != "Ground")
                return false;
            }

        }
        // 障害物がなければtrueを返す
        return true;
    }

    IEnumerator ReSearchRoute() {
        while(true) {
            // 経路を探索する
            int num = S_Adire.GetAIState;
            if(num == 1 || num == 2 || num == 4) SearchMovevec();
            // 何秒待つか決める
            float waitTime = searchInterval + (Random.Range(-1, 2) / 10);
            // 一定秒数まつ
            yield return new WaitForSeconds(waitTime);
        }
    }

}
