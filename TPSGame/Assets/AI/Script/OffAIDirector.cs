using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OffAIDirector : BaseDirector
{

    [SerializeField] GameObject targetObj;

    void Start() {
        base.Start();
    }

    private void Update() {
        if(Input.GetKeyDown(KeyCode.I)) {
            GetComponent<OffAIMove>().SearchRoute(targetObj.transform.position);
        }
    }
}
