using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class fade : MonoBehaviour
{
    public Fade Fade;
    public UnityEvent FadeinEnd;
    public UnityEvent FadeoutEnd;
    public float time=1.0f;
    // Start is called before the first frame update
    void Start()
     // Update is called once per frame
    {
        
    }
    void Update()
    {
        
    }
    public void FadeIn()
    {
        Fade.FadeIn(time, () => { FadeinEnd.Invoke(); });
    }
    public void FadeOut()
    {
        Fade.FadeOut(time,() => { FadeoutEnd.Invoke(); });

    }


}
