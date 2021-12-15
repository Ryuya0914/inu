using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OffPlayerGun : BaseGun
{

    // 弾発射するRayの長さ
    float rayRange = 10.0f;


    void Update()
    {
       // 射撃ボタン
       if(Input.GetMouseButtonDown(0)) {
            if (intervalEndFlag == true) {
                Shoot(GetDir());
            }
       }

       // リロードボタン
       if(Input.GetKeyDown(KeyCode.R)) {
            // リロード中か確認
            if (reloadFlag == false) {
                // リロード開始
                StartCoroutine(Reload());
            }
       }

    }

    // 弾を撃ちだす方向を取得
    protected Vector3 GetDir() {
        Vector3 vec = Vector3.zero;
        Transform cameraT = Camera.main.transform;
        // rayのスタート地点
        // Vector3 raypos = cameraT.position + (cameraT.forward * -Odata.cameraOffsetZ.z * 2.2f);
        Vector3 raypos = cameraT.position + (cameraT.forward * -cameraOffset.z * 2.2f);
        // rayを作成
        Ray ray = new Ray(raypos, cameraT.forward);

        // rayを可視化
        Debug.DrawRay(raypos, ray.direction.normalized * rayRange, Color.red, 2.0f);

        RaycastHit hit;
        // 弾を発射する位置
        Vector3 startpos = new Vector3(transform.forward.x * shootOffset.z, shootOffset.y, transform.forward.z * shootOffset.z);

        if(Physics.Raycast(ray, out hit, rayRange)) {
            Debug.Log("Hit");
            // 弾を発射する位置から弾を着弾させたい位置までのベクトル
            vec = (cameraT.forward * hit.distance) + raypos - (gameObject.transform.position + startpos);
            //Debug.DrawRay(gameObject.transform.position, vec * Pdata.ShootRange, Color.green, 2.0f);

        } else {
            // カメラの向いている方向　×　射程 - 弾の発射位置
            vec = (cameraT.forward * rayRange) + raypos - (gameObject.transform.position + startpos);
        }

        vec = vec.normalized;

        return vec;
    }



}
