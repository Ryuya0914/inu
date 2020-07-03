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

    // 変身するオブジェクトのリスト
    List<ObjectData> OdataList = new List<ObjectData>();
    List<GameObject> ObjectList = new List<GameObject>();



    // 変身するオブジェクトを探す
    void SearchObject() {

    }

    // 変身する
    void ChangeObject() {

    }


}
