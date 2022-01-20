using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class BaseTransChange : MonoBehaviour
{
    // 変身できるかフラグ
    protected bool transChangeFlag = false;
    public void SetTransChangeFlag(bool f) { transChangeFlag = f; }
    // 生きているフラグ
    protected bool aliveFlag = true;
    
    // 変身する準備＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊

    // オブジェクトデータとゲームオブジェクトをまとめるクラス
    public class ObjectDataSet {
        // オブジェクトのデータ
        public ObjectData objData;
        // ゲームオブジェクト
        public GameObject obj;
    }
    // 変身できるオブジェクトのリスト
    public List<ObjectDataSet> objList = new List<ObjectDataSet>();
    // 変身できるオブジェクトのリストに変身するオブジェクトを登録する
    public void RegisterObj(ObjectDirector _oData) {
        // オブジェクトデータセットを作成
        ObjectDataSet _ods = new ObjectDataSet();
        // オブジェクトのデータとオブジェクトを設定
        _ods.objData = _oData.GetOdata;
        _ods.obj = _oData.gameObject;
        // リストに登録
        objList.Add(_ods);
    }
    
    protected void Awake() {
        // 自信の子オブジェクトから変身できるオブジェクトを取得
        foreach (Transform child in transform) {
            ObjectDirector od;
            if (od = child.GetComponentInChildren<ObjectDirector>()) {
                RegisterObj(od);
                if (od.gameObject.activeSelf == true) {
                    objListOffset = objList.Count-1;
                }
            }
        }
    }

    void Start() {
        BaseState bs = GetComponent<BaseState>();
        bs.respawnEvent += () => { aliveFlag = true; };
        bs.dieEvent += () => { aliveFlag = false; };
        bs.respawnEvent += CoolDownEnd;
    }


    // 変身する＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊
    // 変身したときに呼び出すイベント
    public event System.Action<ObjectData> transChangeEvent;

    // 現在変身中のオブジェクトがリストのどこか
    protected int objListOffset = 0;

    // オブジェクトを受け取って変身出来たらする
    protected void ChangeObject(GameObject obj) {
        // オブジェクトがnullか確認
        if (obj == null) return;
        // 変身できるか確認
        if (!transChangeFlag || !aliveFlag) return;
        // クールダウン中か確認
        if (coolDownFlag == true) return;
        
        // オブジェクトデータを取得する
        ObjectData _data = null;
        if(obj.GetComponentInParent<ObjectDirector>()) {
            _data = obj.GetComponentInParent<ObjectDirector>().GetOdata;
        } else { return; }
        
        // 取得したオブジェクトと自身のオブジェクトと同じか確認
        if(_data.ObjectNum == objList[objListOffset].objData.ObjectNum) return;

        // リストから変身するオブジェクトを探して変身する
        for (int i = 0; i < objList.Count; i++) {
            if (objList[i].objData.ObjectNum == _data.ObjectNum) {
                Change(i);
                break;
            }
        }

        // クールダウン開始
        StartCoroutine(CoolDown());
    }

    // オブジェクトを差し替え、他のスクリプトに知らせる
    protected void Change(int num) {
        // 変身前のオブジェクトを非表示にする
        objList[objListOffset].obj.SetActive(false);

        // 変身後のオブジェクトを表示する
        objList[num].obj.SetActive(true);

        // 変身中オブジェクトのリストの位置を更新
        objListOffset = num;

        // 変身したことを他のスクリプトに知らせる
        //transChangeEvent?.Invoke(objList[objListOffset].objData);
        transChangeEvent?.Invoke(objList[objListOffset].objData);
    }


    // 変身のクールダウン＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊

    // クールダウン中=true
    protected bool coolDownFlag = false;
    // 変身できるまでの時間
    protected float coolTime = 2f;

    // 一定時間後に変身できるようにする
    protected IEnumerator CoolDown() {
        // クールダウン中にする
        coolDownFlag = true;
        // 待つ
        yield return new WaitForSeconds(coolTime);
        // クールダウン中を解除する
        coolDownFlag = false;
    }

    // 死亡時とかにクールダウンを強制終了させる
    public void CoolDownEnd() {
        StopCoroutine(CoolDown());
        coolDownFlag = false;
    }

}
