using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] Rigidbody rb;

    int damage = 0;

    public int GetSetDamage { 
        get { return this.damage; }
        set { this.damage = value;} 
    }


    // プレイヤ以外と接触した場合は消す
    void OnTriggerEnter(Collider col) {
        if (col.tag != "Player") {
            gameObject.SetActive(false);
        }
    }
}
