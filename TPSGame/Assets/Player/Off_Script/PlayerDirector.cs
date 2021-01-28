// ユーザの入力、パラメータの変更 (Updateメソッドはここにしかない)
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDirector : MonoBehaviour
{
    [SerializeField] PlayerData Pdata;      // プレイヤデータ
    [SerializeField] ObjectData Odata;      // オブジェクトデータ

    public ObjectData SetOdata { set { this.Odata = value; } }

    // 操作**********************************************
    string hmoveB = "Horizontal";       // 左右移動
    string vmoveB = "Vertical";         // 前後移動
    KeyCode jumpB = KeyCode.Space;      // ジャンプ
    string camerah = "Mouse X";         // カメラ横
    string camerav = "Mouse Y";         // カメラ縦
    int shootB = 0;                     // 射撃
    int transchangeB = 1;               // 変身 
    KeyCode respawn = KeyCode.Return;   // リスポーン
    KeyCode reloadB = KeyCode.R;        // リロード
    KeyCode cameraRotateB = KeyCode.LeftShift;    // カメラだけ回転する 

    // フラグ *************************************************
    bool InputFlag = false;         // 入力
    bool MoveFlag = false;          // 移動
    bool CameraMoveFlag = false;    // カメラ回転
    public bool JumpFlag = true;

    // スクリプト *******************************************
    CameraController S_CameraCon;
    [SerializeField] PlayerMove S_Pmove;
    [SerializeField] PlayerGun S_Pgun;
    [SerializeField] PlayerTransChange S_Ptranschange;
    [SerializeField] PlayerFlag S_Pflag;
    [SerializeField] PlayerLife S_Plife;
    [SerializeField] PlayerAudio S_Paudio;
    PlayerUI S_Pui;
    [SerializeField] EffectController S_effect;           // 死亡

    // データ ************************************************
    Vector3 StartPos;   // 初期位置(リスポーン地点)
    public int PState = 0;     // 0:ゲーム開始前, 1:ゲームプレイ時, 2:死亡時, 3:リスポーン中, 4:メニュー開いたとき 5:カメラだけ回転するとき
    string[] FlagName = new string[] { "Flag_2", "Flag_1" };    // 敵と自分の旗の区別をするためのタグ     　0:自分側, 1:敵側
    string[] ZoneName = new string[] { "Zone_2", "Zone_1" };    // 敵と自分の陣地の区別をするためのタグ     0:自分側, 1:敵側

    //Off_StageDirector_2 unnti;

    void Awake()
    {
        // カメラを見つけてくる
        S_CameraCon = Camera.main.transform.parent.GetComponent<CameraController>();
        // UIを見つけてくる
        S_Pui = GameObject.Find("PlayerCanvas").GetComponent<PlayerUI>();
        // データをそれぞれのスクリプトに読み込ませる
        PlayerDataLoad();
        ObjectDataLoad(Odata);

        S_Ptranschange.ResetChangeFlag();   // 初期で変身できるようにする
    }

    void Start() {
        StartPos = transform.position;  // リスポーン地点を設定
        S_Pflag.NameSet(FlagName[0], FlagName[1], ZoneName[0], ZoneName[1]);        // 敵と味方の旗を教える
        //unnti = GameObject.Find("Stage_Director").GetComponent<Off_StageDirector_2>();

        Invoke(nameof(PActive), 2.0f);
    }


    void Update()
    {
        if(PState == 1) {
            // メニュー画面開きたいとき
            if(Input.GetKeyDown(KeyCode.Escape)) {
                PNonActive();
            }
            // ユーザからの入力を受け取り、移動とかのメソッドを呼び出す
            if(InputFlag) { // 入力可能かどうか
                if(Input.GetKey(cameraRotateB)) {
                    S_Pmove.Move(0f, 0f);
                    MoveFlag = false;
                }
                if(MoveFlag) {  // 行動可能かどうか
                    InputMove();        // 移動
                    InputJump();        // ジャンプ
                    InputShoot();       // 射撃
                    InputTransChage();  // 変身
                }
                if(CameraMoveFlag) { // カメラ回転可能かどうか
                    InputCameraMove(MoveFlag);  // カメラ回転
                }
                MoveFlag = true;
            }

        } else if (PState == 4) {   // メニュー画面閉じる用
            if (Input.GetKeyDown(KeyCode.Escape))
                PActive();
        }
        
        JumpFlag = false;   // 無限ジャンプ防ぐため
    }

    // プレイヤのデータを読み込む (スタート時)
    void PlayerDataLoad() {
        S_CameraCon.SetPdata = Pdata;
        S_Ptranschange.SetPdata = Pdata;
        S_Pgun.SetPdata = Pdata;
        // それぞれのスクリプトにプレイヤを登録
        S_CameraCon.SetPlayerT = transform;
        S_Pmove.SetPlayerT = transform;
        S_Ptranschange.SetCameraT = Camera.main.transform;
        S_Pmove.SetPlayerR = GetComponent<Rigidbody>();

        S_Ptranschange.RegisterObj();
        S_CameraCon.StartAngleSet(transform.eulerAngles.y);


    }

    // オブジェクトのデータを読み込む (スタート時と変身したとき)
    void ObjectDataLoad(ObjectData objData) {
        Odata = objData;
        S_Pmove.SetOdata = objData;
        S_CameraCon.SetOdata = objData;
        S_Ptranschange.SetOdata(objData);
        S_Pgun.SetOdata(objData);
        S_Plife.SetOdata(objData);
    }

    // 移動のメソッド呼び出し
    void InputMove() {
        float h = Input.GetAxis(hmoveB);        // 左右移動の入力
        float v = Input.GetAxis(vmoveB);        // 前後移動の入力
        S_Pmove.Move(h, v);
    }
    // ジャンプのメソッド呼び出し
    void InputJump() {
        if(Input.GetKeyDown(jumpB) && JumpFlag) {   // ジャンプの入力があるか
            S_Pmove.Jump();
        }
    }
    // カメラ回転のメソッド呼び出し
    void InputCameraMove(bool f) {
        S_CameraCon.CameraPosUpdate();          // カメラの位置更新
        float h = Input.GetAxis(camerah);       // マウスの横方向の移動
        float v = Input.GetAxis(camerav);       // マウスの縦方向の移動
        S_CameraCon.CameraRotate(h, v, f);     // カメラを回転させる
        
    }
    // 射撃のメソッド呼び出し
    void InputShoot() {
        if(Input.GetKeyDown(reloadB)) {
            S_Pgun.Reload();    // リロードする
        } else if(Input.GetMouseButtonDown(shootB)) {  // 射撃の入力があるかどうか
            int _ammo = S_Pgun.ShootBullet();   // 射撃する
            AmmoUpdate(_ammo);                  // 弾薬のUI更新
        }
    }
    // 変身のメソッド呼び出し
    void InputTransChage() {
        if(Input.GetMouseButtonDown(transchangeB)) {  // 変身の入力があるかどうか
            ObjectData _data = S_Ptranschange.TransChange();    // 変身させる
            if (_data) ObjectDataLoad(_data);                   // オブジェクトデータ更新
        }
    }

    // プレイヤをアクティブにする
    public void PActive() {
        // 動けるようにする
        PState = 1;
        InputFlag = true;
        MoveFlag = true;
        CameraMoveFlag = true;
        // UIを表示
        S_Pui.CursorSet();
    }

    // プレイヤを非アクティブにする
    public void PNonActive() {
        // 動かないようにする
        InputFlag = false;
        MoveFlag = false;
        CameraMoveFlag = false;
        // UIを非表示
        S_Pui.CursorDel();
        PState = 4;
    }

    // 死亡時の処理
    void PlayerDead() {
        PNonActive();           // 動けないようにする
        S_Pflag.LostFlag();     // 旗を落とす
        S_effect.EffectPlay(transform.position);   // エフェクトを再生

        PState = 2;

        S_Pui.RespawnCount(Pdata.RespawnTime);
        Invoke(nameof(PlayerRespawn), Pdata.RespawnTime);

        //unnti.addP(-1);
    }

    // リスポーン
    void PlayerRespawn() {
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        transform.position = StartPos;  // リスポーン位置に移動
        S_Pflag.FlagGetFlag = true;     // 旗を拾えるようにする
        S_Plife.RecoveryHP();           // HP回復
        S_Pgun.ResetAmmo();             // 弾丸をもとに戻す
        PActive();                      // 動けるようにする
    }

    // HPの更新
    public void LifeUpdate(int life) {
        S_Pui.LifeUIUpdate(life/100f);
        if (life <= 0) {
            PlayerDead();
        }
    }

    // 弾薬の更新
    public void AmmoUpdate(float ammo) {
        S_Pui.AmmoUIUpdate(ammo / (Odata.MaxAmmo * 1.0f));
    }


}
