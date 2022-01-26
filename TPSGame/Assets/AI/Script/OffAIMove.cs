using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OffAIMove : BaseMove
{
    // 移動方向を確認する周期
    float moveVecCheckInterval = 1.0f;
    float moveVecCheckTime = 0.0f;


    protected void Start() {
        base.Start();

        // シーン上のルートポイントを全取得
        Invoke(nameof(SetPoints), 0.5f);
        // フィールドの初期化
        myRoute = new List<int>();
        myRouteCount = 0;
    }


    void Update()
    {
        // 動けるか、生きているか確認、移動する目的地があるか確認
        if(!moveFlag || !aliveFlag || nextTargetPos == Vector3.zero) { 
            Move(0, 0);
            return; 
        }
        // 次の目的地についたか確認
        if (CheckNearTargetPos()) {
            // 次の目的地をセットする
            SetNextTarget();
            // 次の目的地を失った場合は何もしない
            if(nextTargetPos == Vector3.zero) {
                Move(0, 0);
                return; 
            }
        }

        // 移動する方向を決める
        moveVecCheckTime += Time.deltaTime;
        // 一定時間ごとに
        if (moveVecCheckTime >= moveVecCheckInterval) {
            moveVecCheckTime = 0f;
            // 移動方向を設定
            moveVec = SearchMovevec(nextTargetPos);
        }

        // 移動する
        Move(moveVec.x, moveVec.z);
    }

    // 変身時＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊
    protected override void SetObjData(ObjectData ob) {
        rayLength = ob.AImoveRayLength;
        rayPos = ob.AImoveRayPos;
        raySize = ob.AImoveRaySize;
    }

    // 移動する方向を決める＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊
    // 現在の目的地
    Vector3 nextTargetPos = Vector3.zero;
    // 現在の移動方向
    Vector3 moveVec = Vector3.zero;
    // 物理演算
    Rigidbody rb;
    // 変身オブジェクト
    [SerializeField] GameObject myObjects;

    // 飛ばすRayの長さ
    float[] rayLength = new float[] { };
    // Rayを飛ばす開始位置
    Vector3[] rayPos = new Vector3[] { };
    // Rayの大きさ
    Vector3[] raySize = new Vector3[] { };

    // Rayの衝突するオブジェクトを制限
    [SerializeField] LayerMask layermask;
    // Rayを飛ばす方向のパターン
    float[] rayVectors = new float[] { 0f, 45f, -45f, 90f, -90f, 135f, -135f, 180f };
    // 方向を決める際に左と右どちら周りで確認するか
    int RandCount = 0;
    int ramd = 0;

    // 目的地に到達したと判定する範囲の半径
    float pointSize = 1.0f;

    //進む方向を決めるメソッド
    public Vector3 SearchMovevec(Vector3 _pos) {

        // 経路探索時に右と左どちらを優先して調べるか決める
        if(RandCount > 3) {
            // ランダムで正面の障害物を確かめた後に左右どちらから調べるかを決める
            ramd = (Random.Range(0, 2) * 2) - 1;
            RandCount = 0;
        } else {
            RandCount++;
        }


        // 目的地へのベクトルを作成
        Vector3 forwardVec = (_pos - transform.position);
        forwardVec.y = 0f;
        forwardVec = forwardVec.normalized;
        // 右が0度左回り 弧度法
        float angleNow = Mathf.Atan2(forwardVec.z, forwardVec.x) * Mathf.Rad2Deg;

        for(int i = 0; i < rayVectors.Length; i++) {
            // 現在の方向から角度を変えて、ラジアンに変換する
            float angle = (angleNow + rayVectors[i] * ramd) * Mathf.Deg2Rad;

            // Rayを飛ばす方向
            Vector3 RayAngle = new Vector3(Mathf.Cos(angle), 0f, Mathf.Sin(angle));

            // 現在の方向から角度を変えて、ラジアンに変換する
            float anglePos = (angleNow + rayVectors[i] * ramd - 90f) * Mathf.Deg2Rad;

            // Rayの左右の方向
            Vector3 RayPosAngle = new Vector3(Mathf.Cos(anglePos), 0f, Mathf.Sin(anglePos));

            bool f = true;

            for(int j = 0; j < rayPos.Length; j++) {
                // Rayを飛ばす
                RaycastHit hit;
                Vector3 boxpos = transform.position + RayPosAngle * rayPos[j].x + RayAngle * rayPos[j].z + Vector3.up * rayPos[j].y;
                if(Physics.BoxCast(boxpos, raySize[j] / 2f, RayAngle, out hit, myObjects.transform.localRotation * Quaternion.Euler(0, -rayVectors[i] * ramd, 0), rayLength[j], layermask)) {
                    if(!(hit.normal.y > 0.1f)) {
                        f = false;

                        {
                            Color _color = new Color(1.0f, 0.1f, 0.1f);
                            Debug.DrawRay(boxpos, RayAngle * hit.distance, _color, 1.0f);
                            Debug.DrawRay(boxpos + RayPosAngle * raySize[j].x / 2f + RayAngle * hit.distance + Vector3.up * raySize[j].y / 2f, -RayPosAngle * raySize[j].x, _color, 1.0f);
                            Debug.DrawRay(boxpos + RayPosAngle * raySize[j].x / 2f + RayAngle * hit.distance - Vector3.up * raySize[j].y / 2f, -RayPosAngle * raySize[j].x, _color, 1.0f);
                        }

                        break;
                    }

                } else {

                    {
                        Color _color = new Color(0.1f, 0.1f, 0.1f);
                        Debug.DrawRay(boxpos, RayAngle * rayLength[j], _color, 1.0f);
                        Debug.DrawRay(boxpos + RayPosAngle * raySize[j].x / 2f + RayAngle * rayLength[j] + Vector3.up * raySize[j].y / 2f, -RayPosAngle * raySize[j].x, _color, 1.0f);
                        Debug.DrawRay(boxpos + RayPosAngle * raySize[j].x / 2f + RayAngle * rayLength[j] - Vector3.up * raySize[j].y / 2f, -RayPosAngle * raySize[j].x, _color, 1.0f);
                    }
                }

            }

            if(f) {
                // オブジェクトの角度を変える
                return RayAngle;
            }

        }


        return Vector3.forward;

    }

    // 目的地に到達しているか確認
    bool CheckNearTargetPos() {
        // 目的地が無かったら何もしない
        if(nextTargetPos == Vector3.zero)
            return true;

        // 次の目的地までの距離を算出
        Vector3 vec = nextTargetPos - transform.position;
        if (vec.sqrMagnitude < pointSize * pointSize) {
            return true;
        }
        return false;
    }

    // 移動するルートを決める＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊


    // 最終的な目的地
    Vector3 endTargetPos;

    // 経路探索用のオブジェクト
    public Transform[] PointT;
    public AIMovePoint[] S_AImoveP;
    public float[] myMoveCost;
    bool[] myMoveFlag;


    // 決定した経路を管理する
    public List<int> myRoute;　// ルートを配列に格納しておく
    int myRouteCount;       // 現在配列の何番目に来ているか


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
        for(int i = 0; i < PointT.Length; i++) {   // for文で全てのPointまでの距離を比べる
            float dis = (PointT[i].position - pos).magnitude;    // Pointまでの距離
            if(distance > dis) {
                nearPNum = i;
                distance = dis;
            }
        }

        return nearPNum;
    }

    // 二つの地点がpointで隣り合っているか調べる
    public bool CheckNear(Vector3 _vec0, Vector3 _vec1) {
        List<Vector3> _list = new List<Vector3>();

        int num0 = GetNearPoint(_vec0);
        int num1 = GetNearPoint(_vec1);

        if(num0 == num1) {
            return true;
        }

        return false;
    }

    // 経路を作る(Pointの番号指定)    0:赤に行くとき、1:青に行くとき
    public void SearchRoute(int num) {
        // 自分の場所から一番近いPointを探す
        int myPPos = GetNearPoint(transform.position);

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
            if(S_AImoveP[myRoute[con]].MyNumber == num || _flag) {
                nextTargetPos = PointT[myRoute[myRouteCount]].position;
                return;
            }

            // 目的地別にルートを探す
            if(num == 0) {         // 赤へ行くとき
                AIMovePoint p = S_AImoveP[myRoute[con]];
                if(p.GoRed.Length == 0) {
                    _flag = true;
                    continue;
                }
                int n = Random.Range(0, p.GoRed.Length);
                myRoute.Add(p.GoRed[n].MyNumber);
            } else if(num == 1) {  // 青へ行くとき
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
    public void SearchRoute(Vector3 goalPos) {

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
            return;
        }


        //// 保持していたルートが使いまわせないか調べる
        //if(myRoute.Count > 0) {
        //    int Gnum = myRoute.IndexOf(goalPointNum);
        //    int Snum = myRoute.IndexOf(myPointNum);

        //    // 要素が見つかったら
        //    if(Gnum != -1 && Snum != -1 && Snum <= Gnum) {
        //        for(int j = Snum; j <= Gnum; j++) {
        //            _route.Add(PointT[myRoute[j]].position);
        //        }
        //        _route.Add(goalPos);
        //        return _route;
        //    }
        //}

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

        nextTargetPos = PointT[myRoute[myRouteCount]].position;
        return;
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
            } else if(nextnums[i] == goalpoint) {
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
        for(i = 0; i < nextnums.Length; i++) {
            if(pointflag[i]) {
                list.Add(i);
            }
        }
        for(i = 1; i < list.Count; i++) {
            for(int j = 1; j < list.Count; j++) {
                if(nextcost[list[j]] < nextcost[list[j - 1]]) {
                    int n = list[j];
                    list[j] = list[j - 1];
                    list[j - 1] = n;
                }
            }

        }

        while(list.Count > 0) {
            int n = Search(startpoint, goalpoint, nextnums[list[0]], nextlengh[list[0]]);
            if(n != -1) {
                myRoute.Insert(0, n);
                return nextnums[list[0]];
            } else {
                list.RemoveAt(0);
            }
        }
        myMoveFlag[pointnum] = false;
        return -1;

    }


    // 目的地を次に変更する
    void SetNextTarget() {
        myRouteCount++;
        // 設定したルートの最後にたどり着いたか確認 
        if (myRoute.Count == myRouteCount) {
            // 目的地を設定
            nextTargetPos = endTargetPos;                       // ここら辺＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊：：：：
        } else if (myRoute.Count <= myRouteCount) {
            // 目的地をなくす
            nextTargetPos = Vector3.zero;
        } else {
            nextTargetPos = PointT[myRoute[myRouteCount]].position;
        }
    }

}
