using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class nameplate : MonoBehaviour
{
    private RectTransform rect;
    public Vector3 offset = new Vector3(0, 1.5f, 0);
    // Start is called before the first frame update
    void Start()
    {
        rect = GetComponent<RectTransform>();
        rect.transform.position += offset;
    }

    // Update is called once per frame
    void Update()
    {
        rect.LookAt(Camera.main.transform.position);   
    }
    public void Setoffset(Vector3 vec)
    {
        rect.transform.position = vec;
    }
}
