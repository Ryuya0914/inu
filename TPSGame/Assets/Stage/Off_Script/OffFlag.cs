using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OffFlag: MonoBehaviour
{
    // 旗の状態 -1:ステージ上に存在しない時  0:落ちている, 1:誰かが持っている
    public int state = 0;
    // 見た目
    [SerializeField] MeshRenderer Mren;
    // コライダ
    [SerializeField] CapsuleCollider Ccol;
    [SerializeField] BoxCollider Bcol;
    // どこ以下の高さに落ちたら拠点に戻るか
    [SerializeField] float under = -3.5f;
    
    void Start() {
        // 拠点の座標を取得
        spawnPos = gameObject.transform.position;
    }


    // 生成・削除＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊
    // 生成位置
    Vector3 spawnPos = Vector3.zero;
    // 生成位置を設定
    public void SetSpawnPos(Vector3 pos) { spawnPos = pos; }
    // 生成
    public void Spawn() {
        // 移動
        transform.position = spawnPos;
        // 状態の変更
        state = 0;
        // 見た目と当たり判定をアクティブ化
        EnableOnOff(true);
    }

    // 場所を指定して生成
    public void Spawn(Vector3 pos) {
        // 移動
        transform.position = pos;
        // 状態の変更
        state = 0;
        // 見た目と当たり判定をアクティブ化
        EnableOnOff(true);
    }

    public void Delete() {
        // 状態を変更
        state = -1;
        // 親子関係破棄
        transform.parent = null;
        // 見た目と当たり判定を非アクティブ化
        EnableOnOff(false);
    }


    // 取得、落とす＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊

    // 取得時イベント
    public event System.Action TakeFlagEvent;
    // 落下時イベント
    public event System.Action DropFlagEvent;

    // 取得された時
    void Take(Transform _trans) {
        transform.parent = _trans;
        transform.localPosition = Vector3.zero;
        EnableOnOff(false);
        TakeFlagEvent?.Invoke();
        state = 1;
    }

    // 旗を落とす
    public void Drop() {
        // プレイヤ・CPUとの親子関係を破棄
        transform.parent = null;

        // 奈落に落ちているか判定
        if(transform.position.y <= under) {
            // 初期地点に生成
            transform.position = spawnPos;
            EnableOnOff(true);
            state = 0;

        } else {
            // その場に生成
            EnableOnOff(true);
            state = 0;

        }

        DropFlagEvent?.Invoke();

    }
    
    // プレイヤ、CPUとの当たり判定
    private void OnTriggerEnter(Collider col) {
        // tagで判定
        if (col.tag == "Player" || col.tag == "AI") {
            // 旗を取得できる状態か判定
            if (state != 0) return;

            // プレイヤ・CPUが旗を取得できるか判定
            if(col.GetComponentInParent<BaseState>()) {
                // 旗を所持しているか確認
                if(col.GetComponentInParent<BaseState>().TakeFlag(this)) {
                    // 旗を取得状態にする
                    Take(col.transform);

                }
            }
        }
    }


    // 見た目、当たり判定の変更＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊
    void EnableOnOff(bool b) {
        Mren.enabled = b;
        if(Ccol != null)
            Ccol.enabled = b;
        if(Bcol != null)
            Bcol.enabled = b;
    }



}
