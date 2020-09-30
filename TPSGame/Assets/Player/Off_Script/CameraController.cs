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
    // 現在のプレイヤの向いている角度
    float Phangle = 0, Pvangle = 0;

    public void StartAngleSet(float f) {
        CameraRotate(f, 0f, true);
    }

    // カメラの位置更新
    public void CameraPosUpdate() {
        Camera.main.transform.localPosition = Odata.cameraOffsetZ;
        // カメラの中心の位置をプレイヤの位置に合わせて更新
        transform.position = PlayerT.position;
        transform.position += Odata.cameraOffsetY;
    }

    // カメラの角度計算
    public void CameraRotate(float h, float v, bool b) {


        if(b) {
            float num = Phangle - Hangle;    // プレイヤとカメラの方向の差分
            float _speed = Pdata.RotateSpeed * 720f * Time.deltaTime; // カメラの回転速度
            if(Mathf.Abs(num) < _speed) {
                Hangle = Phangle;
            } else {
                if(num < 0)
                    Hangle -= _speed;
                else
                    Hangle += _speed;
            }
            if(num == 0) {

                // プレイヤの回転
                // 左右の回転量
                Phangle += h * Pdata.RotateSpeed;
                // 左右の角度の数値の調整
                if(Phangle >= 180f)
                    Phangle -= 360f;
                else if(Phangle < -180f)
                    Phangle += 360f;
                // プレイヤを回転させる
                PlayerT.eulerAngles = new Vector3(0.0f, Phangle, 0.0f);
                //Pvangle -= v * Pdata.RotateSpeed;    // 上下の角度加算
                // 上下の角度制限
                //Pvangle = Mathf.Clamp(Pvangle, Pdata.CameraDownLimit, Pdata.CameraUpLimit);


                // カメラの回転
                Hangle += h * Pdata.RotateSpeed;    // 左右の角度加算
                Vangle -= v * Pdata.RotateSpeed;    // 上下の角度加算
                                                    // 上下の角度制限
                Vangle = Mathf.Clamp(Vangle, Pdata.CameraDownLimit, Pdata.CameraUpLimit);
                // 左右の角度の数値の調整
                if(Hangle >= 180f)
                    Hangle -= 360f;
                else if(Hangle < -180f)
                    Hangle += 360f;

                //float num = Phangle - Hangle;    // プレイヤとカメラの方向の差分
                //float _speed = Pdata.RotateSpeed * 720f * Time.deltaTime; // カメラの回転速度
                //if (Mathf.Abs(num) < _speed) {
                //    Hangle = Phangle;
                //} else {
                //    if (num < 0) Hangle -= _speed;
                //    else Hangle += _speed;
                //}
            }

        } else {
                // 回転量を加算
                Hangle += h * Pdata.RotateSpeed * 1.2f;    // 左右の角度加算
                Vangle -= v * Pdata.RotateSpeed * 1.2f;    // 上下の角度加算
                                                           // 上下の角度制限
                Vangle = Mathf.Clamp(Vangle, Pdata.CameraDownLimit, Pdata.CameraUpLimit);
                // 左右の角度の数値の調整
                if(Hangle >= 180f)
                    Hangle -= 360f;
                else if(Hangle < -180f)
                    Hangle += 360f;
            
        }
        // カメラを回転させる
        transform.eulerAngles = new Vector3(Vangle, Hangle, 0.0f);

    }

}
