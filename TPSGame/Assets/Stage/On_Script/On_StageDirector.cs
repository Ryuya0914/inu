using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class On_StageDirector : MonoBehaviour
{
    GameObject[] S_obj_;
    GameObject[] M_obj_;
    GameObject[] L_obj_;
    GameObject[] S_objpos;
    GameObject[] M_objpos;
    GameObject[] L_objpos;
    GameObject player_;

    [SerializeField] GameObject[] stage;
    [SerializeField] GameObject[] objpos_pre;


    GameObject respawn_1;//プレイヤーのスポーン位置
    GameObject respawn_2;

    //プレイヤー
    [SerializeField] GameObject[] player;
    //配置するオブジェクト(ステージ1)
    [SerializeField] GameObject[] S_obj_1;
    [SerializeField] GameObject[] M_obj_1;
    [SerializeField] GameObject[] L_obj_1;
    //配置するオブジェクト(ステージ2)
    [SerializeField] GameObject[] S_obj_2;
    [SerializeField] GameObject[] M_obj_2;
    [SerializeField] GameObject[] L_obj_2;



    int[] S_rnd;
    int[] M_rnd;
    int[] L_rnd;


    void Start()
    {
        //ステージの選択
        //switch (stage_select.off_stage)
        //{
        //    case 0:
        //        Instantiate(stage[0]);
        //        Instantiate(objpos_pre[0]);
        //        respawn_1 = GameObject.Find("Respawn_1");
        //        respawn_2 = GameObject.Find("Respawn_2");
        //        S_obj_ = S_obj_1;
        //        M_obj_ = M_obj_1;
        //        L_obj_ = L_obj_1;
        //        player_ = player[0];
        //        break;
        //    case 1:
        //        Instantiate(stage[1]);
        //        Instantiate(objpos_pre[1]);
        //        respawn_1 = GameObject.Find("Respawn_1");
        //        respawn_2 = GameObject.Find("Respawn_2");
        //        S_obj_ = S_obj_2;
        //        M_obj_ = M_obj_2;
        //        L_obj_ = L_obj_2;
        //        player_ = player[1];
        //        break;
        //}

        //タグの種類ごとに配列にどーーーん
        S_objpos = GameObject.FindGameObjectsWithTag("Small_Item");
        M_objpos = GameObject.FindGameObjectsWithTag("Medium_Item");
        L_objpos = GameObject.FindGameObjectsWithTag("Large_Item");

        //置くものをランダムで決める
        objrnd();
        //ものの配置
        Instobj(S_obj_, S_objpos, S_rnd);
        Instobj(M_obj_, M_objpos, M_rnd);
        Instobj(L_obj_, L_objpos, L_rnd);
        //プレイヤーの配置
        Instplayer(player_, respawn_1, respawn_2);
    }

    //乱数作るやつ
    void objrnd()
    {
        S_rnd = new int[S_objpos.Length];
        M_rnd = new int[M_objpos.Length];
        L_rnd = new int[L_objpos.Length];
        for (int i = 0; i < S_objpos.Length; i++)
        {
            S_rnd[i] = Random.Range(0, S_obj_.Length);
        }
        for (int i = 0; i < M_objpos.Length; i++)
        {
            M_rnd[i] = Random.Range(0, M_obj_.Length);
        }
        for (int i = 0; i < L_objpos.Length; i++)
        {
            L_rnd[i] = Random.Range(0, L_obj_.Length);
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
}
