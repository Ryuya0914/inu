using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AITransChange : MonoBehaviour
{
    // 変身に関するデータ系
    ObjectData Odata;                   // オブジェクトのデータの格納場
    List<ObjectData> OdataList = new List<ObjectData>();                        // オブジェクトデータのリスト
    [SerializeField] List<GameObject> ObjectList = new List<GameObject>();      // ゲームオブジェクトのリスト

    // 変身に関するフィールド
    [SerializeField] int mynum = 0;     // 現在変身中のオブジェクトの番号(リストの中の番号)
    bool F_Change = true;               // 変身できるフラグ
    float ReChangeTime = 10f;           // 変身する頻度

    // Rayに関するフィールド
    [SerializeField] LayerMask mask;    // layer
    float RayStartY = 0.15f;            // Rayを飛ばす位置
    float RayRange = 2.5f;

    // その他
    AIDirector S_Adire;     // ディレクタースクリプト
    AIMove S_Amove;         // 移動スクリプト
    AIGun S_Agun;           // 射撃スクリプト
    AILife S_Alife;         // ライフスクリプト


    void Start() {
        S_Adire = GetComponent<AIDirector>();   // ディレクター取得
        S_Amove = GetComponent<AIMove>();       // 移動スクリプト取得
        S_Agun = GetComponent<AIGun>();         // 射撃スクリプト取得
        S_Alife = GetComponent<AILife>();         // ライフスクリプト取得
        RegisterObj();      // 変身するオブジェクトをリストに格納
        SetOdata(OdataList[mynum]);
        StartCoroutine(nameof(ChangeUpdate));
    }

    IEnumerator ChangeUpdate() {
        while(true) {
            yield return new WaitForSeconds(1.0f);
            if(F_Change && (S_Adire.GetAIState == 1 || S_Adire.GetAIState == 2)) {      // 変身できるときかつ待機中でも死んでいるときでもないとき
                TransChange();
            }
        }
    }


    // オブジェクトデータ更新
    public void SetOdata(ObjectData odata) {
        Odata = odata;
        S_Adire.SetOdata = odata;
        S_Amove.SetOdata(odata);
        S_Agun.SetOdata(odata);
        S_Alife.SetOdata(odata);
        F_Change = false;               // 変身出来なくする
        Invoke(nameof(Reload), ReChangeTime);
    }

    void Reload() {
        F_Change = true;
    }


    // 変身
    public void TransChange() {
        // rayを飛ばして変身したいオブジェクトの取得
        ObjectData _data = null;
        for(int i = 0; i < 4; i++) {
            _data = GoRay();
            if (_data != null) break;
        }
        if(_data && OdataList[mynum].ObjectNum != _data.ObjectNum) {     // rayでオブジェクトを取得できたか,現在のオブジェクトと同じじゃないか
            int listnum = SearchList(_data);    // リストの何番目にあるか探す
            if(listnum >= 0) {
                ChangeObject(listnum);              // 実際に変身する
            }
        }
    }

    // rayを飛ばす
    ObjectData GoRay() {
        // rayのスタート地点
        Vector3 raypos = transform.position;
        raypos.y += RayStartY;
        // rayを飛ばす方向をランダムで決める
        Vector3 rayvec = new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f));
        rayvec = rayvec.normalized;
        // rayを作成
        Ray ray = new Ray(raypos, rayvec);

        // rayを可視化
        Debug.DrawRay(ray.origin, ray.direction.normalized * RayRange, Color.green, RayRange);

        RaycastHit hit;
        // rayの衝突判定
        if(Physics.Raycast(ray, out hit, RayRange , mask.value)) {
            if(hit.collider.tag == "Object") {
                Transform hitObj = hit.collider.transform;
                for(int i = 0; i < 3; i++) {
                    ObjectDirector director;
                    if(director = hitObj.GetComponent<ObjectDirector>()) { // オブジェクトデータを取得
                        return director.GetOdata;
                    } else if(hitObj.parent == null) {
                        return null;
                    }
                    hitObj = hitObj.parent.transform;    // オブジェクトデータが見つからなかったときは親オブジェクトを探す
                    i++;
                }
            }
        }
        return null;
    }

    // 変身するオブジェクトをListから探す
    int SearchList(ObjectData _data) {
        for(int i = 0; i < OdataList.Count; i++) {     // リストからオブジェクトの場所を探す
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
        SetOdata(OdataList[listnum]);
    }


    // 変身するオブジェクトをListに追加
    void RegisterObj() {
        for(int i = 0; i < ObjectList.Count; i++)
            OdataList.Add(ObjectList[i].GetComponent<ObjectDirector>().GetOdata); // オブジェクトデータ格納
    }


}
