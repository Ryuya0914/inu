using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{


    void Start()
    {
        // カメラのスクリプト取得
        GameObject.Find("CameraParent").GetComponent<CameraController>().targetObj = gameObject;
        
    }


    void Update()
    {
        //// カメラ回転出来るとき、カメラのスクリプト取得出来ているとき
        //if (cameraInput && cameraScript != null) {
        //    // カメラを回転させる
        //    cameraScript.CameraRotate(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        //}
    }
}
