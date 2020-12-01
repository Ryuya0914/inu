using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIRouteSearch : MonoBehaviour
{
    // 経路探索用のオブジェクト
    public Transform[] PointT;
    public AIMovePoint[] S_AImoveP;
    public float[] myMoveCost;


    // 決定した経路を管理する
    List<int> myRoute;          // ルートを配列に格納しておく
    int myRouteCount;       // 現在配列の何番目に来ているか
    
    
    void Start()
    {        
        // シーン上のルートポイントを全取得
        SetPoints();
        // フィールドの初期化
        myRoute = new List<int> { };
        myRouteCount = 0;
    }


    // Pointを配列に格納する
    void SetPoints() {
        // ステージ上のチェックポイントをすべて取得
        GameObject[] obj = GameObject.FindGameObjectsWithTag("AI_CheckPoint");
        PointT = new Transform[obj.Length];
        S_AImoveP = new AIMovePoint[obj.Length];
        myMoveCost = new float[obj.Length];

        // チェックポイントの情報を保存しておく
        for(int i = 0; i < obj.Length; i++) {
            AIMovePoint scr = obj[i].GetComponent<AIMovePoint>();       // スクリプト
            int num = scr.MyNumber;                                     // そのポイントに振り分けられた番号
            // Pointを番号順になるように配列に格納
            PointT[num] = obj[i].transform;     // ポイントの座標
            S_AImoveP[num] = scr;               // ポイントのスクリプト
            myMoveCost[num] = 0f;               // コスト
        }

        // 配列を初期化
        //points = new Point[obj.Length];
        //// チェックポイントの情報を保存しておく
        //for (int i = 0; i < points.Length; i++) {
        //    AIMovePoint scr = obj[i].GetComponent<AIMovePoint>();       // スクリプト
        //    int num = scr.MyNumber;                                     // そのポイントに振り分けられた番号
        //    // Pointを番号順になるように配列に格納
        //    points[num].PointT = obj[i].transform;           // ポイントの座標
        //    points[num].S_AImoveP = scr;                                // ポイントのスクリプト
        //}
    }
    

    // 一番近くのPointを探す
    int GetNearPoint(Vector3 pos) {
        int nearPNum = -1;                  // 一番近くのPointの番号
        float distance = float.MaxValue;    // 一番近くのPointまでの距離
        for (int i = 0; i < PointT.Length; i++) {   // for文で全てのPointまでの距離を比べる
            float dis = (PointT[i].position - pos).magnitude;    // Pointまでの距離
            if (distance > dis) {
                nearPNum = i;
                distance = dis;
            }
        }

        return nearPNum;
    }



    // 経路を作る(Pointの番号指定)    0:赤に行くとき、1:青に行くとき
    public Vector3 SearchRoute(int num) {
        // 自分の場所から一番近いPointを探す
        int myPPos = GetNearPoint(transform.position);


        // 保持していたルートが使いまわせないか調べる
        if(myRoute.Count > 0) {
            if (myRoute.LastIndexOf(0) == num) {    // 今回の目的地と前回の目的地が一緒だったら
                for(int i = 0; i < myRoute.Count; i++) {
                    if(myRoute[i] == myPPos) {
                        myRouteCount = i;
                        return PointT[myRoute[myRouteCount]].position;
                    }
                }
            }
        }


        // 初期化
        myRoute.Clear();
        myRouteCount = 0;

        // 一番最初の目的地を自分の近くのpointにする
        myRoute.Add(myPPos);

        int con = 0;
        // 経路を探索する
        while(true) {
            // 目的地まで探索出来たら終了する
            if (S_AImoveP[myRoute[con]].MyNumber == num) {
                return PointT[myRoute[myRouteCount]].position;
            }

            // 目的地別にルートを探す
            if (num == 0) {         // 赤へ行くとき
                AIMovePoint p = S_AImoveP[myRoute[con]];
                int n = Random.Range(0, p.GoRed.Length);
                myRoute.Add(p.GoRed[n].MyNumber);
            } else if (num == 1) {  // 青へ行くとき
                AIMovePoint p = S_AImoveP[myRoute[con]];
                int n = Random.Range(0, p.GoBlue.Length);
                myRoute.Add(p.GoBlue[n].MyNumber);
            }

            con++;

        }
    }

    // 経路を作る(world座標指定)
    void SearchRoute(Vector3 goalPos) {

    }

    // 目的地を返す
    public Transform GetDestinationT() {
        myRouteCount++;
        if (myRouteCount >= myRoute.Count) {
            return null;
        }
        return PointT[myRoute[myRouteCount]];
    }


}