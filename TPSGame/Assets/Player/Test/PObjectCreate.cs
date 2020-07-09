// テスト用にプレイヤにオブジェクトを生成させるスクリプト
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PObjectCreate : MonoBehaviour
{

    [SerializeField] GameObject[] objects;
    [SerializeField] GameObject player;

    void Start()
    {
        player.GetComponent<PlayerTransChange>().AddObject(objects);
    }
}
