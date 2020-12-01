using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIMoveTest : MonoBehaviour
{
    List<int> lis;

    void Start() {
        lis = new List<int> { };
        lis.Add(1);
        lis.Add(1);
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Y)) {
            Debug.Log(lis.Capacity + " : " + lis.Count);

        }


    }
}
