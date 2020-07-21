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
    public void SetOdata(ObjectData odata) {
        Odata = odata;
        Flag_reload = true;         // リロード中にする
        StartCoroutine("ReloadChange"); // リロードのコルーチンを開始
    }
    // カメラのtransform
    Transform CameraT;
    // カメラのtransformを登録
    public Transform SetCameraT { set { this.CameraT = value; } }

    // 変身するオブジェクトのリスト
    List<ObjectData> OdataList = new List<ObjectData>();
    [SerializeField] List<GameObject> ObjectList = new List<GameObject>();

    // 現在変身中のオブジェクトの番号(リストの中の番号)
    [SerializeField] int mynum = 0;

    // layer
    [SerializeField] LayerMask mask;
    
    // 変身できるフラグ
    bool Flag_reload = false;

    // 変身
    public ObjectData TransChange() {
        if (Flag_reload) return null;
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

// rayを可視化
        Debug.DrawRay(ray.origin, ray.direction.normalized * Pdata.RayRange, Color.red, Pdata.RayRange);

        RaycastHit hit;
        // rayの衝突判定
        if (Physics.Raycast(ray, out hit, Pdata.RayRange, mask.value)) {
            GameObject hitObj = hit.collider.gameObject;
            for (int i = 0; i < 3; i++) {
                ObjectDirector director;
                if(director = hitObj.GetComponent<ObjectDirector>()) { // オブジェクトデータを取得
                    return director.GetOdata;
                } 
                hitObj = hitObj.transform.parent.gameObject;    // オブジェクトデータが見つからなかったときは親オブジェクトを探す
                i++;
            }
        }
        return null;
    }
    
    // 変身するオブジェクトをListから探す
    int SearchList(ObjectData _data) {
        for (int i = 0; i < OdataList.Count; i++) {     // リストからオブジェクトの場所を探す
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



    float checktime = 0.1f;
    // 変身のリロード
    public IEnumerator ReloadChange() {
        float time = 0;
        while(Flag_reload) {
            if(time >= Pdata.TransChageCoolTime) { // 一定時間経過したら
                ResetChangeFlag();
                yield break;
            }
            yield return new WaitForSeconds(checktime);
            time += checktime;
        }

        yield break;
    }

    // 変身できるようにする
    public void ResetChangeFlag() {
        Flag_reload = false;
    }
}
