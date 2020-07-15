using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] Rigidbody rb;

    void OnDisable() {
        rb.velocity = Vector3.zero;
    }

    // プレイヤ意外と接触した場合は消す
    void OnTriggerEnter(Collider col) {
        if (col.tag != "Player") {
            gameObject.SetActive(false);
        }
    }
}
