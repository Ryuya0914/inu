using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDirector : MonoBehaviour
{
    // 自分のオブジェクト情報
    [SerializeField] ObjectData MyOdata;
    // オブジェクト情報のゲッタ―
    public ObjectData GetOdata { get { return MyOdata; } }

    

}
