using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Off_StageDirector_2 : MonoBehaviour
{
    GameObject[] S_objpos;
    GameObject[] M_objpos;
    GameObject[] L_objpos;
    GameObject[] GW_objpos;
    GameObject[] GB_objpos;

    bool[] GW_pos = new bool[] {true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true};
    bool[] GB_pos = new bool[] {true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true};

    GameObject respawn_1;//プレイヤーのスポーン位置
    GameObject respawn_2;

    //プレイヤー
    [SerializeField] GameObject player;
    [SerializeField] GameObject AI;
    //配置するオブジェクト(ステージ1)
    [SerializeField] GameObject[] S_obj_1;
    [SerializeField] GameObject[] M_obj_1;
    [SerializeField] GameObject[] L_obj_1;
    [SerializeField] GameObject[] GW_obj_1;
    [SerializeField] GameObject[] GB_obj_1;
    [SerializeField] GameObject[] respawn_obj;


    int[] S_rnd;
    int[] M_rnd;
    int[] L_rnd;
    int GW_rnd;
    int GB_rnd;
    int respawn_rnd;


    void Start()
    {

        //タグの種類ごとに配列にどーーーん
        S_objpos = GameObject.FindGameObjectsWithTag("Small_Item");
        M_objpos = GameObject.FindGameObjectsWithTag("Medium_Item");
        L_objpos = GameObject.FindGameObjectsWithTag("Large_Item");
        GW_objpos = GameObject.FindGameObjectsWithTag("siro");
        GB_objpos = GameObject.FindGameObjectsWithTag("kuro");

        //置くものをランダムで決める
        objrnd();
        //ものの配置
        respawnset(respawn_obj, GW_objpos, GB_objpos, respawn_rnd);
        Instobj(S_obj_1, S_objpos, S_rnd);
        Instobj(M_obj_1, M_objpos, M_rnd);
        Instobj(L_obj_1, L_objpos, L_rnd);
        InstGobj(GW_obj_1, GW_objpos, GW_pos);
        InstGobj(GB_obj_1, GB_objpos, GB_pos);
        respawn_1 = GameObject.Find("Respawn1");
        respawn_2 = GameObject.Find("Respawn2");
        //プレイヤーの配置
        Instplayer(player, AI, respawn_1, respawn_2);
        Invoke("Active", 3.0f);
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
        respawn_rnd = Random.Range(0, 7);
        GW_pos[respawn_rnd] = false;
        GB_pos[respawn_rnd] = false;

        for (int i = 0; i < 17; i++) 
        {
            GW_rnd = Random.Range(0, 23);
            if (GW_pos[GW_rnd] == false)
            {
                i--;
            }
            else
            {
                GW_pos[GW_rnd] = false;
            }
        }
        for(int i = 0; i < 17; i++)
        {
            GB_rnd = Random.Range(0, 23);
            if (GB_pos[GB_rnd] == false)
            {
                i--;
            }
            else
            {
                GB_pos[GB_rnd] = false;
            }
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

    void InstGobj(GameObject[] Gobj, GameObject[] Gobjpos, bool[] pos)
    {
        for(int i = 0; i < Gobj.Length; i++)
        {
            for (int j = 0; j < Gobjpos.Length; j++)
            {
                if (pos[j] == true)
                {
                    Instantiate(Gobj[i], Gobjpos[j].transform.position, Gobjpos[j].transform.rotation);
                    pos[j] = false;
                    break;
                }
            }
        }
    }

    void respawnset(GameObject[] _respawn, GameObject[] _GW_pos, GameObject[] _GB_pos, int _respawn_rnd)
    {
        Instantiate(_respawn[0], _GW_pos[respawn_rnd].transform.position, Quaternion.identity);
        Instantiate(_respawn[1], _GB_pos[respawn_rnd].transform.position, Quaternion.identity);
    }

    //プレイヤー・カメラ生成
    void Instplayer(GameObject player, GameObject AI, GameObject Respawn_1, GameObject Respawn_2)
    {
        Instantiate(player, Respawn_1.transform.position, Respawn_1.transform.rotation);
        Instantiate(AI, Respawn_2.transform.position, Respawn_2.transform.rotation);
    }

    void Active()
    {
        GameObject.Find("Player_02(Clone)").GetComponent<PlayerDirector>().PActive();
        GameObject.Find("AI(Clone)").GetComponent<AIDirector>().AActive();
    }
}
