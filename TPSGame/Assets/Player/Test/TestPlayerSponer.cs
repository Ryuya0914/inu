using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SoftGear.Strix.Unity.Runtime;

public class TestPlayerSponer : StrixBehaviour
{
    public GameObject obj;
    
    void Start()
    {
        Instantiate(obj);
    }

}
