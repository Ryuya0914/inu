using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BaseTransChange : MonoBehaviour
{
    // 変身できるかフラグ
    public bool transChangeFlag = false;

    // 変身する＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊
    
    // 変身したときに呼び出すイベント
    public UnityEvent transChangeEvent;
    // オブジェクトデータとゲームオブジェクトをまとめるクラス
    public class ObjectDataSet {
        // オブジェクトのデータ
        public ObjectData objData;
        // ゲームオブジェクト
        public GameObject obj;
    }
    // 変身できるオブジェクトのリスト
    public List<ObjectDataSet> objList = new List<ObjectDataSet>();
    // 現在変身中のオブジェクトがリストのどこか
    protected int objListOffset = 0;

    // 変身
    protected void ChangeObject(GameObject obj) {
        // 変身できるか確認
        if (transChangeFlag == false) return;
        // クールダウン中か確認
        if (coolDownFlag == true) return;

        // オブジェクトデータを取得する
        ObjectData _data = null;
        if(_data = obj.GetComponentInParent<ObjectDirector>().GetOdata) {
        } else { return; }

        // 取得したオブジェクトと自身のオブジェクトと同じか確認
        if (_data.ObjectNum == objList[objListOffset].objData.ObjectNum) return;

        // 変身する


        // 変身したことを他のスクリプトに知らせる
        transChangeEvent.Invoke();

        // クールダウン開始
        StartCoroutine(CoolDown());

    }


    // オブジェクトを差し替え、他のスクリプトに知らせる
    protected void Change(int num) {
        
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
