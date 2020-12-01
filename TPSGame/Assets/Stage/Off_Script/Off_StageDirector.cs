using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Off_StageDirector : MonoBehaviour
{
    GameObject[] S_objpos;
    GameObject[] M_objpos;
    GameObject[] L_objpos;

    GameObject respawn_1;//プレイヤーのスポーン位置
    GameObject respawn_2;

    //プレイヤー
    [SerializeField] GameObject player;
    //配置するオブジェクト(ステージ1)
    [SerializeField] GameObject[] S_obj_1;
    [SerializeField] GameObject[] M_obj_1;
    [SerializeField] GameObject[] L_obj_1;

    int[] S_rnd;
    int[] M_rnd;
    int[] L_rnd;


    void Start()
    {
        respawn_1 = GameObject.Find("Respawn_1");
        respawn_2 = GameObject.Find("Respawn_2");

        //タグの種類ごとに配列にどーーーん
        S_objpos = GameObject.FindGameObjectsWithTag("Small_Item");
        M_objpos = GameObject.FindGameObjectsWithTag("Medium_Item");
        L_objpos = GameObject.FindGameObjectsWithTag("Large_Item");

        //置くものをランダムで決める
        objrnd();
        //ものの配置
        Instobj(S_obj_1, S_objpos, S_rnd);
        Instobj(M_obj_1, M_objpos, M_rnd);
        Instobj(L_obj_1, L_objpos, L_rnd);
        //プレイヤーの配置
        Instplayer(player, respawn_1, respawn_2);
        Invoke(nameof(Active), 2.0f);
    }

    //乱数作るやつ
    void objrnd()
    {
        S_rnd = new int[S_objpos.Length];
        M_rnd = new int[M_objpos.Length];
        L_rnd = new int[L_objpos.Length];
        for (int i = 0; i < S_objpos.Length; i++)
        {
            S_rnd[i] = Random.Range(0, S_obj_1.Length);
        }
        for (int i = 0; i < M_objpos.Length; i++)
        {
            M_rnd[i] = Random.Range(0, M_obj_1.Length);
        }
        for (int i = 0; i < L_objpos.Length; i++)
        {
            L_rnd[i] = Random.Range(0, L_obj_1.Length);
        }

    }

    //オブジェクト生成
    void Instobj(GameObject[] obj, GameObject[] objpos, int[] rnd)
    {

        for (int i = 0; i < objpos.Length; i++)
        {
            Vector3 spawnpos = objpos[i].transform.position;
            Instantiate(obj[rnd[i]], spawnpos, obj[rnd[i]].transform.rotation * Quaternion.Euler(0.0f, Random.Range(0, 360), 0.0f));
            Destroy(objpos[i]);
        }
    }

    //プレイヤー・カメラ生成
    void Instplayer(GameObject player, GameObject Respawn_1, GameObject Respawn_2)
    {
        Instantiate(player, Respawn_1.transform.position, Respawn_1.transform.rotation);
    }


    void Active() {
        GameObject.Find("Player_01(Clone)").GetComponent<PlayerDirector>().PActive();
        //GameObject.Find("AI_01(Clone)").GetComponent<AIDirector>().AActive();
        //GameObject.Find("AI_01").GetComponent<AIDirector>().AActive();
    }
}
