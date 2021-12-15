using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BaseDirector : MonoBehaviour
{
    [SerializeField] int mode = -1;
    protected void Start() {
        switch(mode) {
            case -1:
                Debug.Log("DebugStart");
                debugEvent.Invoke();
                break;
            case 1:
                Debug.Log("RobyStart");
                robyEvent.Invoke();
                break;
        }
    }


    // 試合＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊
    // 試合開始時に実行するメソッドたち
    public UnityEvent startEvent;
    // 試合開始時にステージディレクターから呼ばれるメソッド
    public void GameStart() {
        if (startEvent != null) {
            startEvent.Invoke();
        }
    }
    // 試合開始時に実行するメソッドたち
    public UnityEvent endEvent;
    // 試合終了時にステージディレクターから呼ばれるメソッド
    public void GameEnd() {
        if(endEvent != null) {
            endEvent.Invoke();
        }
    }

    // ロビー＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊
    // ロビー参加時に実行するメソッドたち
    public UnityEvent robyEvent;

    // デバッグ＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊
    // デバッグ開始時に実行するメソッドたち
    public UnityEvent debugEvent;


}
