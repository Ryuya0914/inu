// ユーザの入力、パラメータの変更
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
    int shootB = 1;                     // 射撃
    int transchangeB = 2;               // 変身

    // フラグ *************************************************
    bool InputFlag = false;         // 入力
    bool MoveFlag = false;          // 移動
    bool JumpFlag = false;          // ジャンプ
    bool CameraMoveFlag = false;    // カメラ回転
    bool ShootFlag = false;         // 射撃
    bool TransChangeFlag = false;   // 変身

    void Start()
    {
        
    }


    void Update()
    {
        // ユーザからの入力を受け取り、移動とかのメソッドを呼び出す
        if(InputFlag) { // 入力可能かどうか
            // 移動
            if(MoveFlag) InputMove();
            // ジャンプ
            if(JumpFlag) InputJump();
            // カメラ回転
            if(CameraMoveFlag) InputCameraMove();
            // 射撃
            if(ShootFlag) InputShoot();
            // 変身
            if(TransChangeFlag) InputTransChage();
        }
    }

    // プレイヤのデータを読み込む (スタート時)
    void PlayerDataLoad() {

    }
    // オブジェクトのデータを読み込む (スタート時と変身したとき)
    void ObjectDataLoad(ObjectData objData) {

    }

    // 移動のメソッド呼び出し
    void InputMove() {
        float h = Input.GetAxis(hmoveB);        // 左右移動の入力
        float v = Input.GetAxis(vmoveB);        // 前後移動の入力
        if(h != 0 || v != 0) {                 // 前後左右どこかに入力があるか
// 移動のメソッド
        }
    }
    // ジャンプのメソッド呼び出し
    void InputJump() {
        if(Input.GetKeyDown(jumpB)) {          // ジャンプの入力があるか
// ジャンプのメソッド
        }
    }
    // カメラ回転のメソッド呼び出し
    void InputCameraMove() {
        float h = Input.GetAxis(camerah);      // マウスの横方向の移動
        float v = Input.GetAxis(camerav);      // マウスの縦方向の移動
        if(h != 0 || v != 0) {               // カメラ回転の入力があるか
// カメラ回転のメソッド
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
// 変身のメソッド
        }
    }

}
