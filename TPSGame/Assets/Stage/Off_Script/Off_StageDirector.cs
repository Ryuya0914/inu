using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Off_StageDirector : MonoBehaviour
{
    GameObject[] obj_;
    GameObject[] S_obj_;
    GameObject[] M_obj_;
    GameObject[] L_obj_;
    GameObject[] S_objpos;
    GameObject[] M_objpos;
    GameObject[] L_objpos;

    [SerializeField] GameObject[] stage;
    [SerializeField] GameObject[] objpos_pre;

    //配置するオブジェクト(ステージ1)
    [SerializeField] GameObject[] obj_1;
    [SerializeField] GameObject[] S_obj_1;
    [SerializeField] GameObject[] M_obj_1;
    [SerializeField] GameObject[] L_obj_1;
    //配置するオブジェクト(ステージ2)
    [SerializeField] GameObject[] obj_2;
    [SerializeField] GameObject[] S_obj_2;
    [SerializeField] GameObject[] M_obj_2;
    [SerializeField] GameObject[] L_obj_2;



    int[] S_rnd;
    int[] M_rnd;
    int[] L_rnd;


    void Start()
    {
        switch (stage_select.off_stage)
        {
            case 0:
                Instantiate(stage[0]);
                Instantiate(objpos_pre[0]);
                obj_ = obj_1;
                S_obj_ = S_obj_1;
                M_obj_ = M_obj_1;
                L_obj_ = L_obj_1;
                break;
            case 1:
                Instantiate(stage[1]);
                Instantiate(objpos_pre[1]);
                obj_ = obj_2;
                S_obj_ = S_obj_2;
                M_obj_ = M_obj_2;
                L_obj_ = L_obj_2;
                break;
        }
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
    }

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
            L_rnd[i] = Random.Range(0, L_obj_.Length );
        }

    }

    void Instobj(GameObject[] obj, GameObject[] objpos, int[] rnd)
    {
        for (int i = 0; i < objpos.Length; i++)
        {
            Vector3 spawnpos = objpos[i].transform.position;
            Instantiate(obj[rnd[i]], spawnpos, obj[rnd[i]].transform.rotation * Quaternion.Euler(0.0f, Random.Range(0, 360), 0.0f));
            Destroy(objpos[i]);
        }
    }
}
