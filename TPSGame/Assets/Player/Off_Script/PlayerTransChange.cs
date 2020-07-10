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
    [SerializeField] List<GameObject> ObjectList = new List<GameObject>();

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
// rayを可視化
//        Debug.DrawRay(ray.origin, ray.direction.normalized * Pdata.RayRange, Color.red, Pdata.RayRange);


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
        mynum = listnum;                    // 自身のオブジェクト番号更新
    }


    // 変身するオブジェクトをListに追加
    public void RegisterObj() {
        for (int i = 0; i < ObjectList.Count; i++)
        OdataList.Add(ObjectList[i].GetComponent<ObjectDirector>().GetOdata); // オブジェクトデータ格納
    }
}
