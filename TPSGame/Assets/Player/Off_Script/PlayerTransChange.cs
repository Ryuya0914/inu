// プレイヤを変身させる
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTransChange : MonoBehaviour
{
    // プレイヤデータ、オブジェクトのデータの格納場
    PlayerData Pdata;
    ObjectData Odata;
    // プレイヤデータ、オブジェクトのデータ登録
    public PlayerData SetPdata { set { this.Pdata = value; } }
    public ObjectData SetOdata { set { this.Odata = value; } }
    // カメラのtransform
    Transform CameraT;
    // カメラのtransformを登録
    public Transform SetCameraT { set { this.CameraT = value; } }

    // 変身するオブジェクトのリスト
    List<ObjectData> OdataList = new List<ObjectData>();
    List<GameObject> ObjectList = new List<GameObject>();

    // 現在変身中のオブジェクトの番号(リストの中の番号)
    int mynum = 0;

    // layer
    [SerializeField] LayerMask mask;
    

    // 変身
    public ObjectData TransChange() {
        ObjectData _data = GoRay();             // rayを飛ばして変身したいオブジェクトの取得
        if(_data && OdataList[mynum].ObjectNum != _data.ObjectNum) {     // rayでオブジェクトを取得できたか,現在のオブジェクトと同じじゃないか
            int listnum = SearchList(_data);    // リストの何番目にあるか探す
            if (listnum < 0) return null;
            ChangeObject(listnum);              // 実際に変身する
            mynum = listnum;                    // 自身のオブジェクト番号更新
            return _data;
        }
        return null;
    }

    // rayを飛ばす
    ObjectData GoRay() {
        ObjectData _data = null;    // rayで取得したオブジェクトデータをしまう変数
        // rayを作成
        Ray ray = new Ray(CameraT.position, CameraT.forward);

        RaycastHit hit;
        // rayの衝突判定
        if (Physics.Raycast(ray, out hit, Pdata.RayRange, mask.value)) {
            _data = hit.collider.transform.parent.GetComponent<ObjectDirector>().GetOdata;   // オブジェクトデータを取得
        }
// 後で消す rayを可視化
        Debug.DrawRay(ray.origin, ray.direction.normalized * Pdata.RayRange, Color.red, Pdata.RayRange);
// ここまで後で消す

        return _data;
    }
    
    // 変身するオブジェクトをListから探す
    int SearchList(ObjectData _data) {
        for (int i = 1; i < OdataList.Count; i++) {     // リストからオブジェクトの場所を探す
            if(OdataList[i] == _data) {
                return i;   // オブジェクトの場所を返す
            }
        }

        return -1;
    }

    // 自身のオブジェクト変更
    void ChangeObject(int listnum) {
        ObjectList[mynum].SetActive(false);
        ObjectList[listnum].SetActive(true);
    }

    // 変身オブジェクトの作成
    public void AddObject(GameObject[] objs) {     // プレファブが入った配列を受け取る
        for (int i = 1; i < objs.Length; i++) {
            GameObject obj = CreateMyObj(objs[i]);  // 生成
            RegisterObj(obj);                       // 登録
        }
    }


    // instantiateメソッド
    GameObject CreateMyObj(GameObject obj) {
        GameObject newobj = Instantiate(obj);   // 生成
        newobj.transform.parent = transform;    // プレイヤの子オブジェクトに設定
        newobj.transform.localPosition = Vector3.zero;  // 初期位置をプレイヤの位置にセット
        foreach(Transform childT in newobj.transform) {     // 全ての子オブジェクトのtagとlayerを変更
            childT.gameObject.tag = "Player";                      // tagを変更
            childT.gameObject.layer = gameObject.layer;            // layerを変更
        }
        newobj.tag = "Player";                      // tagを変更
        newobj.layer = gameObject.layer;            // layerを変更
        newobj.SetActive(false);                    // 非表示にしておく
        return newobj;
    }

    // 変身するオブジェクトをListに追加
    public void RegisterObj(GameObject obj) {
        ObjectList.Add(obj);                                        // オブジェクト格納
        OdataList.Add(obj.GetComponent<ObjectDirector>().GetOdata); // オブジェクトデータ格納
    }
}
