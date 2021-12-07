using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    // 現在のカメラの角度
    float Hangle = 0, Vangle = 0;
    // カメラ動かせるかフラグ
    bool moveFlag = false;

    void Start() {
        moveFlag = true;
    }

    void Update() {
        // 追跡するターゲットがいない場合、カメラが動かせない場合は何もしない
        if(targetObj == null || moveFlag == false)
            return;

        // カメラを回転させる
        CameraRotate(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        // ターゲットに合わせて移動する
        CameraPosUpdate();
        // ターゲットをカメラと同じ方向向かせる
        LookCameraDirection();
    }

    // カメラの位置関係＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊
    // カメラのオフセット
    Vector3 offSet = new Vector3(0, 3f, 1.5f);
    // 追跡するオブジェクト
    public GameObject targetObj;
    // カメラの位置を変更(変身とかしたとき)
    public void SetCameraOffSet(Vector3 _vec) {
        offSet = _vec;
        // カメラの位置をオフセットに合わせる
        Camera.main.transform.localPosition = new Vector3 (0, 0, offSet.z);
    }

    public void CameraPosUpdate() {
        // カメラの中心の位置をプレイヤの位置に合わせて更新
        transform.position = targetObj.transform.position;
        transform.position += new Vector3(0, offSet.y, 0);
    }

    // カメラの回転＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊
    // カメラの回転速度
    public static float rotateSpeed = 10.0f;
    // カメラの上方向の角度制限
    float upLimit = 45f;
    // カメラの下方向の角度制限
    float downLimit = -45f;


    // カメラの角度計算
    public void CameraRotate(float h, float v) {
        // カメラの回転速度
        float _speed = rotateSpeed * 720f * Time.deltaTime;

        // 回転量を加算
        Hangle += h * rotateSpeed * 1.2f;    // 左右の角度加算
        Vangle -= v * rotateSpeed * 1.2f;    // 上下の角度加算
                                             // 上下の角度制限
        Vangle = Mathf.Clamp(Vangle, downLimit, upLimit);
        // 左右の角度の数値の調整
        if(Hangle >= 180f)
            Hangle -= 360f;
        else if(Hangle < -180f)
            Hangle += 360f;

        // カメラを回転させる
        transform.eulerAngles = new Vector3(Vangle, Hangle, 0.0f);

    }

    public void LookCameraDirection() {
        Vector3 dir = transform.forward;
        dir.y = 0;
        var lookRotation = Quaternion.LookRotation(dir, Vector3.up);
        targetObj.transform.rotation = Quaternion.Lerp(targetObj.transform.rotation, lookRotation, 0.8f);
    }



    //// カメラの角度計算
    //public void CameraRotate(float h, float v, bool b) {
    //    if(b) {
    //        float num = Phangle - Hangle;    // プレイヤとカメラの方向の差分
    //        float _speed = rotateSpeed * 720f * Time.deltaTime; // カメラの回転速度
    //        if(Mathf.Abs(num) < _speed) {   // プレイヤとカメラの向いている方向が近かったら
    //            Hangle = Phangle;
    //        } else {                        // プレイヤとカメラの向いている方向が離れていたら
    //            if(Phangle < 0) {       // プレイヤの向いている方向(world)が0度未満だったら
    //                if((0 + Phangle) <= Hangle && Hangle < (180 + Phangle)) // カメラの方向がプレイヤから見て右だったら
    //                    Hangle -= _speed;
    //                else
    //                    Hangle += _speed;
    //            } else {
    //                if((-180 + Phangle) < Hangle && Hangle <= (0 + Phangle)) // カメラの方向がプレイヤから見て左だったら
    //                    Hangle += _speed;
    //                else
    //                    Hangle -= _speed;
    //            }
    //            // 左右の角度の数値の調整
    //            if(Hangle >= 180f)
    //                Hangle -= 360f;
    //            else if(Hangle < -180f)
    //                Hangle += 360f;
    //        }
    //        if(num == 0) {

    //            // プレイヤの回転
    //            // 左右の回転量
    //            Phangle += h * rotateSpeed;
    //            // 左右の角度の数値の調整
    //            if(Phangle >= 180f)
    //                Phangle -= 360f;
    //            else if(Phangle < -180f)
    //                Phangle += 360f;
    //            // プレイヤを回転させる
    //            PlayerT.eulerAngles = new Vector3(0.0f, Phangle, 0.0f);
    //            //Pvangle -= v * Pdata.RotateSpeed;    // 上下の角度加算
    //            // 上下の角度制限
    //            //Pvangle = Mathf.Clamp(Pvangle, Pdata.CameraDownLimit, Pdata.CameraUpLimit);


    //            // カメラの回転
    //            Hangle += h * rotateSpeed;    // 左右の角度加算
    //            Vangle -= v * rotateSpeed;    // 上下の角度加算
    //                                                // 上下の角度制限
    //            Vangle = Mathf.Clamp(Vangle, downLimit, upLimit);
    //            // 左右の角度の数値の調整
    //            if(Hangle >= 180f)
    //                Hangle -= 360f;
    //            else if(Hangle < -180f)
    //                Hangle += 360f;

    //            //float num = Phangle - Hangle;    // プレイヤとカメラの方向の差分
    //            //float _speed = Pdata.RotateSpeed * 720f * Time.deltaTime; // カメラの回転速度
    //            //if (Mathf.Abs(num) < _speed) {
    //            //    Hangle = Phangle;
    //            //} else {
    //            //    if (num < 0) Hangle -= _speed;
    //            //    else Hangle += _speed;
    //            //}
    //        }

    //    } else {
    //        // 回転量を加算
    //        Hangle += h * rotateSpeed * 1.2f;    // 左右の角度加算
    //        Vangle -= v * rotateSpeed * 1.2f;    // 上下の角度加算
    //        // 上下の角度制限
    //        Vangle = Mathf.Clamp(Vangle, downLimit, upLimit);
    //        // 左右の角度の数値の調整
    //        if(Hangle >= 180f)
    //            Hangle -= 360f;
    //        else if(Hangle < -180f)
    //            Hangle += 360f;

    //    }
    //    // カメラを回転させる
    //    transform.eulerAngles = new Vector3(Vangle, Hangle, 0.0f);

    //}




    // カメラの位置更新


    //public void CameraPosUpdate() {
    //    Camera.main.transform.localPosition = Odata.cameraOffsetZ;
    //    // カメラの中心の位置をプレイヤの位置に合わせて更新
    //    transform.position = PlayerT.position;
    //    transform.position += Odata.cameraOffsetY;
    //}


}
