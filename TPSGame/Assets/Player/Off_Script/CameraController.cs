using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
    // プレイヤデータ、オブジェクトのデータの格納場
    PlayerData Pdata;
    ObjectData Odata;
    // プレイヤデータ、オブジェクトのデータ登録
    public PlayerData SetPdata { set { this.Pdata = value; } }
    public ObjectData SetOdata { set { this.Odata = value; } }
    // 追跡するプレイヤ
    Transform PlayerT;
    // プレイヤのtransformを登録
    public Transform SetPlayerT { set { this.PlayerT = value; } }
    // 現在のカメラの角度
    float Hangle = 0, Vangle = 0;

    void Start() {
        Cursor.visible = false;         // マウスカーソルを消す
        Cursor.lockState = CursorLockMode.Locked;   // カーソルを動かないようにする
    }

    public void StartAngleSet(float f) {
        CameraRotate(f, 0f);
    }

    // カメラの位置更新
    public void CameraPosUpdate() {
        Camera.main.transform.localPosition = Odata.cameraOffsetZ;
        // カメラの中心の位置をプレイヤの位置に合わせて更新
        transform.position = PlayerT.position;
        transform.position += Odata.cameraOffsetY;
    }

    // カメラの角度計算
    public void CameraRotate(float h, float v) {
        // 回転量を加算
        Hangle += h * Pdata.RotateSpeed;    // 左右の角度加算
        Vangle -= v * Pdata.RotateSpeed;    // 上下の角度加算
        // 上下の角度制限
        Vangle = Mathf.Clamp(Vangle, Pdata.CameraDownLimit, Pdata.CameraUpLimit);
        // 実際に回転させる
        transform.eulerAngles = new Vector3(Vangle, Hangle, 0.0f);
        // プレイヤも回転
        PlayerT.eulerAngles = new Vector3(0.0f, Hangle, 0.0f);
    }

}
