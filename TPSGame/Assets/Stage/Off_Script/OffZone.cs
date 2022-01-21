using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OffZone : MonoBehaviour
{
    // 得点をゲットしたときのイベント
    public System.Action<int> GetPointEvent;

    // 旗を自陣に持って行ったときに貰えるポイント
    [SerializeField] int flagPoint = 3; 


    // プレイヤとの当たり判定
    private void OnTriggerEnter(Collider other) {
        // タグでプレイヤかCPUか判定
        if (other.tag == "Player" || other.tag == "AI") {
            // 同じチームか確認
            if (other.GetComponentInParent<OffTeam>().m_teamColor != gameObject.GetComponentInParent<OffTeam>().m_teamColor) return;

            // スクリプト取得できるか確認
            if(other.GetComponentInParent<BaseState>()){ 
                // 旗を捨てさせる
                if(!other.GetComponentInParent<BaseState>().DeletFlag()) return;
                // 得点を加算する
                GetPointEvent?.Invoke(flagPoint);
            }
        }
    }
}
