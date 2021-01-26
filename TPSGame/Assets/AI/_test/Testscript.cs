using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testscript : MonoBehaviour
{

    public float[] _f;
    public int[] _i;
    public List<int> list;

    void Start()
    {
        for (int i = 0; i < _i.Length; i++) {
            list.Add(_i[i]);
        }
        list.Insert(0, 10);
    }


    void Update() {
        if(Input.GetKeyDown(KeyCode.A)) {
            Debug.Log(list.Count);
        }
    }

}
