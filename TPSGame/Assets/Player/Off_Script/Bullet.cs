﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] Rigidbody rb;
    string[] tagNames = new string[] { "Player", "AI" };
    public TeamScript m_myTeam;
    int damage = 0;


    public int GetSetDamage { 
        get { return this.damage; }
        set { this.damage = value;} 
    }



    // プレイヤ以外と接触した場合は消す
    void OnTriggerEnter(Collider col) {
        for(int i = 0; i < tagNames.Length; ++i) {
            if(col.tag == tagNames[i]) {
                return;
            }

        }
        gameObject.SetActive(false);

    }
}
