using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIMoveTest : MonoBehaviour
{
    [SerializeField] AIDirector[] S_Adire;



    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Y)) {
            for(int i = 0; i < S_Adire.Length; i++) {
                S_Adire[i].ChangeState(1);
            }

        }


    }
}
