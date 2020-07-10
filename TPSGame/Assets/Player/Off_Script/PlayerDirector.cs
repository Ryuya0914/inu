// ユーザの入力、パラメータの変更 (Updateメソッドはここにしかない)
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDirector : MonoBehaviour
{
    [SerializeField] PlayerData Pdata;      // プレイヤデータ
    [SerializeField] ObjectData Odata;      // オブジェクトデータ

    // 操作**********************************************
    string hmoveB = "Horizontal";       // 左右移動
    string vmoveB = "Vertical";         // 前後移動
    KeyCode jumpB = KeyCode.Space;      // ジャンプ
    string camerah = "Mouse X";         // カメラ横
    string camerav = "Mouse Y";         // カメラ縦
    int shootB = 0;                     // 射撃
    int transchangeB = 1;               // 変身

    // フラグ *************************************************
    bool InputFlag = false;         // 入力
    bool MoveFlag = false;          // 移動
    bool CameraMoveFlag = false;    // カメラ回転

    // スクリプト *******************************************
    CameraController S_CameraCon;
    [SerializeField] PlayerMove S_Pmove;
    [SerializeField] PlayerGun S_Pgun;
    [SerializeField] PlayerTransChange S_Ptranschange;
    [SerializeField] PlayerFlag S_Pflag;

    


    void Awake()
    {
        // カメラを見つけてくる
        S_CameraCon = Camera.main.transform.parent.GetComponent<CameraController>();
        // データをそれぞれのスクリプトに読み込ませる
        PlayerDataLoad();
        ObjectDataLoad(Odata);
        
// 後で消す 試合開始時にフラグをトゥルーにする
        FlagChange(true);
// ここまで後で消す
    }


    void Update()
    {
        // ユーザからの入力を受け取り、移動とかのメソッドを呼び出す
        if(InputFlag) { // 入力可能かどうか
            if(MoveFlag) {  // 行動可能かどうか
                InputMove();        // 移動
                InputJump();        // ジャンプ
                InputShoot();       // 射撃
                InputTransChage();  // 変身
            }
            if(CameraMoveFlag) { // カメラ回転可能かどうか
                InputCameraMove();  // カメラ回転
            }
        }
        
    }

    // プレイヤのデータを読み込む (スタート時)
    void PlayerDataLoad() {
        S_CameraCon.SetPdata = Pdata;
        S_Ptranschange.SetPdata = Pdata;
        // それぞれのスクリプトにプレイヤを登録
        S_CameraCon.SetPlayerT = transform;
        S_Pmove.SetPlayerT = transform;
        S_Ptranschange.SetCameraT = Camera.main.transform;
        S_Pmove.SetPlayerR = GetComponent<Rigidbody>();
        S_Ptranschange.RegisterObj();
    }

    // オブジェクトのデータを読み込む (スタート時と変身したとき)
    void ObjectDataLoad(ObjectData objData) {
        Odata = objData;
        S_Pmove.SetOdata = objData;
        S_CameraCon.SetOdata = objData;
    }

    // プレイヤのフラグを一括変更
    public void FlagChange(bool f) {
        InputFlag = f;
        MoveFlag = f;
        CameraMoveFlag = f;
    }

    // 移動のメソッド呼び出し
    void InputMove() {
        float h = Input.GetAxis(hmoveB);        // 左右移動の入力
        float v = Input.GetAxis(vmoveB);        // 前後移動の入力
        if(h != 0 || v != 0) {                  // 前後左右どこかに入力があるか
            S_Pmove.Move(h, v);
        }
    }
    // ジャンプのメソッド呼び出し
    void InputJump() {
        if(Input.GetKeyDown(jumpB)) {          // ジャンプの入力があるか
            S_Pmove.Jump();
        }
    }
    // カメラ回転のメソッド呼び出し
    void InputCameraMove() {
        S_CameraCon.CameraPosUpdate();          // カメラの位置更新
        float h = Input.GetAxis(camerah);       // マウスの横方向の移動
        float v = Input.GetAxis(camerav);       // マウスの縦方向の移動
        if(h != 0 || v != 0) {               // カメラ回転の入力があるか
            S_CameraCon.CameraRotate(h, v);     // カメラを回転させる
        }
    }
    // 射撃のメソッド呼び出し
    void InputShoot() {
        if(Input.GetMouseButtonDown(shootB)) {  // 射撃の入力があるかどうか
// 射撃のメソッド
        }
    }
    // 変身のメソッド呼び出し
    void InputTransChage() {
        if(Input.GetMouseButtonDown(transchangeB)) {  // 変身の入力があるかどうか
            ObjectData _data = S_Ptranschange.TransChange();    // 変身させる
            if (_data) ObjectDataLoad(_data);                   // オブジェクトデータ更新
        }
    }

}
