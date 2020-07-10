using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class stage_select : MonoBehaviour
{
    public static int off_stage = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Off_select()
    {
        off_stage = 0;
        SceneManager.LoadScene("Off_Stage");
    }
}
