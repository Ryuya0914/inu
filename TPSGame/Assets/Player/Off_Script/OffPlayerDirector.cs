using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OffPlayerDirector : BaseDirector
{

    void Start() {
        base.Start();
        SetCamera();
    }

    public void SetCamera() {
        // カメラのスクリプトに自身を追尾させる
        OffCameraController cameraScript = GameObject.Find("CameraParent").GetComponent<OffCameraController>();
        cameraScript.targetObj = gameObject;
        cameraScript.SetMoveFlag(true);
    }

}
