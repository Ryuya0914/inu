using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectController : MonoBehaviour
{
    ParticleSystem _effect;
    [SerializeField] bool SetPlayerFlag = false;

    void Start() {
        // 自身のパーティクルを取得
        _effect = gameObject.GetComponent<ParticleSystem>();
        // プレイヤに追従しないようにする
        if (!SetPlayerFlag) gameObject.transform.parent = null;
    }
    
    // Effectを再生
    public void EffectPlay(Vector3 vec) {
        if(!_effect.isPlaying) {    // 現在再生中じゃなかったら
            transform.localPosition = vec;  // エフェクトの位置を更新
            _effect.Play();         // 再生
        }
    }

    // Effectを停止
    public void EffectStop() {
        if(!_effect.isStopped) {    // 現在停止中だったら
            _effect.Stop();         // 停止
        }
    }

}
