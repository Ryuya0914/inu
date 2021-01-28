using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIRouteSearch : MonoBehaviour
{
    // 経路探索用のオブジェクト
    public Transform[] PointT;
    public AIMovePoint[] S_AImoveP;
    public float[] myMoveCost;
    bool[] myMoveFlag;


    // 決定した経路を管理する
    public List<int> myRoute;          // ルートを配列に格納しておく
    int myRouteCount;       // 現在配列の何番目に来ているか
    
    
    void Start()
    {        
        // シーン上のルートポイントを全取得
        Invoke(nameof(SetPoints), 0.5f);
        // フィールドの初期化
        myRoute = new List<int>();
        myRouteCount = 0;
    }


    // Pointを配列に格納する
    void SetPoints() {
        // ステージ上のチェックポイントをすべて取得
        GameObject[] obj = GameObject.FindGameObjectsWithTag("AI_CheckPoint");
        PointT = new Transform[obj.Length];
        S_AImoveP = new AIMovePoint[obj.Length];
        myMoveCost = new float[obj.Length];
        myMoveFlag = new bool[obj.Length];

        // チェックポイントの情報を保存しておく
        for(int i = 0; i < obj.Length; i++) {
            AIMovePoint scr = obj[i].GetComponent<AIMovePoint>();       // スクリプト
            int num = scr.MyNumber;                                     // そのポイントに振り分けられた番号
            // Pointを番号順になるように配列に格納
            PointT[num] = obj[i].transform;     // ポイントの座標
            S_AImoveP[num] = scr;               // ポイントのスクリプト
            myMoveCost[num] = 0f;               // コスト
        }

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

    // 二つの地点がpointで隣り合っているか調べる
    public List<Vector3> CheckSideBySide(Vector3 _vec0, Vector3 _vec1) {
        List<Vector3> _list = new List<Vector3>();

        int num0 = GetNearPoint(_vec0);
        int num1 = GetNearPoint(_vec1);
        
        if (num0 == num1) {
            _list.Add(_vec1);
        }
        
        for (int i = 0; i < S_AImoveP[num0].GoBlue.Length; i++) {
            if(S_AImoveP[num0].GoBlue[i].MyNumber == num1) {
                _list.Add(PointT[num0].position);
                _list.Add(PointT[num1].position);
                return _list;
            }
        }
        for (int i = 0; i < S_AImoveP[num0].GoRed.Length; i++) {
            if (S_AImoveP[num0].GoRed[i].MyNumber == num1) {
                _list.Add(PointT[num0].position);
                _list.Add(PointT[num1].position);
                return _list;
            }
        }

        return _list;
    }

    // 経路を作る(Pointの番号指定)    0:赤に行くとき、1:青に行くとき
    public List<Vector3> SearchRoute(int num) {
        List<Vector3> _route = new List<Vector3>();

        // 自分の場所から一番近いPointを探す
        int myPPos = GetNearPoint(transform.position);


        // 保持していたルートが使いまわせないか調べる
        if(myRoute.Count > 0) {
            if (myRoute.LastIndexOf(0) == num) {    // 今回の目的地と前回の目的地が一緒だったら
                for(int i = 0; i < myRoute.Count; i++) {
                    if(myRoute[i] == myPPos) {
                        for (int j = i; j < myRoute.Count; j++) {
                            _route.Add(PointT[myRoute[j]].position);
                        }
                        return _route;
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
        bool _flag = false;
        // 経路を探索する
        while(true) {
            // 目的地まで探索出来たら終了する
            if (S_AImoveP[myRoute[con]].MyNumber == num || _flag) {
                for (int i = 0; i < myRoute.Count; i++) {
                    _route.Add(PointT[myRoute[i]].position);
                }
                return _route;
            }

            // 目的地別にルートを探す
            if (num == 0) {         // 赤へ行くとき
                AIMovePoint p = S_AImoveP[myRoute[con]];
                if (p.GoRed.Length == 0) {
                    _flag = true;
                    continue;
                }
                int n = Random.Range(0, p.GoRed.Length);
                myRoute.Add(p.GoRed[n].MyNumber);
            } else if (num == 1) {  // 青へ行くとき
                AIMovePoint p = S_AImoveP[myRoute[con]];
                if(p.GoBlue.Length == 0) {
                    _flag = true;
                    continue;
                }
                int n = Random.Range(0, p.GoBlue.Length);
                myRoute.Add(p.GoBlue[n].MyNumber);
            }

            con++;

        }
    }

    // 経路を作る(world座標指定)
    public List<Vector3> SearchRoute(Vector3 goalPos) {
        List<Vector3> _route = new List<Vector3>();

        // 自分の場所から一番近いPointを探す
        int myPointNum = GetNearPoint(transform.position);
        // 目的地に一番近いPointを探す
        int goalPointNum = GetNearPoint(goalPos);

        // 目的地と現在位置が同じか調べる
        if(myPointNum == goalPointNum) {
            // 初期化
            myRoute.Clear();
            myRouteCount = 0;
            // 一番最初の目的地を自分の近くのpointにする
            myRoute.Add(myPointNum);
            for(int j = 0; j < myRoute.Count; j++) {
                _route.Add(PointT[myRoute[j]].position);
            }
            _route.Add(goalPos);
            return _route;
        }


        // 保持していたルートが使いまわせないか調べる
        if(myRoute.Count > 0) {
            int Gnum = myRoute.IndexOf(goalPointNum);
            int Snum = myRoute.IndexOf(myPointNum);

            // 要素が見つかったら
            if(Gnum != -1 && Snum != -1 && Snum <= Gnum) {
                for(int j = Snum; j <= Gnum; j++) {
                    _route.Add(PointT[myRoute[j]].position);
                }
                _route.Add(goalPos);
                return _route;
            }
        }

        // 経路を作る

        // 初期化
        myRoute.Clear();
        myRouteCount = 0;
        int i = 0;
        for(i = 0; i < myMoveCost.Length; i++) {
            myMoveCost[i] = -1;
            myMoveFlag[i] = true;
        }

        int num = Search(myPointNum, goalPointNum, myPointNum, 0);
        myRoute.Insert(0, num);

        // 一番最初の目的地を自分の近くのpointにする
        myRoute.Insert(0, myPointNum);

        
        for(int j = 0; j < myRoute.Count; j++) {
            _route.Add(PointT[myRoute[j]].position);
        }
        _route.Add(goalPos);
        return _route;
    }

    int Search(int startpoint, int goalpoint, int pointnum, float nowlength) {
        // 最終目的地のposition
        Vector3 gpos = PointT[goalpoint].position;

        // 開始point
        Vector3 spos = PointT[pointnum].position;
        // pointから進めるpointとその距離,移動コストをまとめる
        int num = S_AImoveP[pointnum].GoBlue.Length + S_AImoveP[pointnum].GoRed.Length;
        int[] nextnums = new int[num];
        float[] nextlengh = new float[num];
        float[] nextcost = new float[num];
        bool[] pointflag = new bool[num];
        int i = 0;
        for(; i < S_AImoveP[pointnum].GoBlue.Length; i++) {
            pointflag[i] = false;
            nextnums[i] = S_AImoveP[pointnum].GoBlue[i].MyNumber;
            if(nextnums[i] == startpoint || !myMoveFlag[nextnums[i]]) {
                nextlengh[i] = -1;
                nextcost[i] = -1;
                continue;
            } else if (nextnums[i] == goalpoint) {
                return nextnums[i];
            }
            Vector3 ppos = PointT[nextnums[i]].position;

            nextlengh[i] = (ppos - spos).sqrMagnitude + nowlength;
            nextcost[i] = (gpos - ppos).sqrMagnitude + nextlengh[i];
            if(myMoveCost[nextnums[i]] == -1 || myMoveCost[nextnums[i]] > nextcost[i]) {
                myMoveCost[nextnums[i]] = nextcost[i];
                pointflag[i] = true;
            }
        }
        for(int j = 0; j < S_AImoveP[pointnum].GoRed.Length; j++) {
            pointflag[i + j] = false;
            nextnums[i + j] = S_AImoveP[pointnum].GoRed[j].MyNumber;
            if(nextnums[i + j] == startpoint || !myMoveFlag[nextnums[i + j]]) {
                nextlengh[i] = -1;
                nextcost[i] = -1;
                continue;
            } else if(nextnums[i + j] == goalpoint) {
                return nextnums[i + j];
            }
            Vector3 ppos = PointT[nextnums[i + j]].position;

            nextlengh[i + j] = (ppos - spos).sqrMagnitude + nowlength;
            nextcost[i + j] = (gpos - ppos).sqrMagnitude + nextlengh[i + j];
            if(myMoveCost[nextnums[i + j]] == -1 || myMoveCost[nextnums[i + j]] >= nextcost[i + j]) {
                myMoveCost[nextnums[i + j]] = nextcost[i + j];
                pointflag[i + j] = true;
            }
        }
        
        // 結果から次のpointを選択する
        List<int> list = new List<int>();
        for (i = 0; i < nextnums.Length; i++) {
            if(pointflag[i]) {
                list.Add(i);
            }
        }
        for (i = 1; i < list.Count; i++) {
            for (int j = 1; j < list.Count; j++) {
                if(nextcost[list[j]] < nextcost[list[j - 1]]) {
                    int n = list[j];
                    list[j] = list[j - 1];
                    list[j - 1] = n;
                }
            }

        }
        
        while (list.Count > 0) {
            int n = Search(startpoint, goalpoint, nextnums[list[0]], nextlengh[list[0]]);
            if (n != -1) {
                myRoute.Insert(0, n);
                return nextnums[list[0]];
            } else {
                list.RemoveAt(0);
            }
        }
        myMoveFlag[pointnum] = false;
        return -1;

    }



}