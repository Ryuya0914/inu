using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OffPlayerTransChange : BaseTransChange
{


    void Update()
    {
        // 変身ボタン押したら
        if(Input.GetMouseButtonDown(1)) {
            ChangeObject(GoRay());
        }
        
    }


    // Rayを飛ばしてオブジェクトを取得する＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊
    // 変身するRayの長さ
    float rayRange = 10.0f;

    GameObject GoRay() {
        // rayのスタート地点
        Vector3 raypos = Camera.main.transform.position + (Camera.main.transform.forward * objList[objListOffset].objData.cameraOffset.z * 1.3f);
        // rayを作成
        Ray ray = new Ray(raypos, Camera.main.transform.forward);

        // rayを可視化
        Debug.DrawRay(ray.origin, ray.direction.normalized * rayRange, Color.green, rayRange);

        RaycastHit hit;
        // rayの衝突判定
        if(Physics.Raycast(ray, out hit, rayRange)) {
            // 当たったオブジェクトのタグが変身できるやつじゃなかったら何もしない
            if(hit.collider.tag != "Object") return null;

            // 当たったオブジェクトを返す
            return hit.collider.gameObject;

        }
        return null;
    }


}
