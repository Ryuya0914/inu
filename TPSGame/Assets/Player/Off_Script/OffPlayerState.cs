using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OffPlayerState : BaseState
{
    // HPUIのプレハブ
    [SerializeField] GameObject lifeUIPrefab;
    // HPUI
    Image lifeUI;
    
    void Start() {
        // lifeUIを生成
        GameObject obj = Instantiate(lifeUIPrefab, GameObject.Find("PlayerCanvas").transform);
        lifeUI = obj.GetComponent<Image>();

        // HP減少時イベントに登録
        decreaseHPEvent += LifeUIUpdate;
        // リスポーンイベントに登録
        respawnEvent += LifeUIUpdate;

    }



    void Update() {
        if(Input.GetKeyDown(KeyCode.J)) {
            GetComponent<BaseState>().DecreaseHP();
        }
    }

    // LifeUIの更新
    void LifeUIUpdate(int num) {
        lifeUI.fillAmount = num / 100.0f;
    }
    // LifeUIの更新
    void LifeUIUpdate() {
        lifeUI.fillAmount = 1f;
    }


}
