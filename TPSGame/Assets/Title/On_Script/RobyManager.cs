using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SoftGear.Strix.Unity.Runtime;

public class RobyManager : MonoBehaviour
{
    

    
    

    // ロビー内操作 ＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊

    
    // ロビーから退出
    public bool ExitRoby() {
        // 設定を変更できる状態か確認
        
        // ロビーから退出する処理

        return true;
    }

    // 準備状態切り替え
    public bool Ready(int _state) {
        // ゲーム開始中か確認

        // 状態切り替え

        return true;
    }

    // プレイヤネームの変更
    public bool ChangePlayerName(string s) {
        // 設定を変更できる状態か確認

        // 名前を変更する

        return true;
    }

    // マップの変更
    public bool ChangeMap(int num) {
        // 設定を変更できる状態か確認
        if (!StrixNetwork.instance.isRoomOwner) return false;

        // マップを変更する

        return true;
    }

    // 対戦人数の変更
    public bool ChangeMaxPlayer(int num) {
        // 設定を変更できる状態か確認
        if(!StrixNetwork.instance.isRoomOwner) return false;
        // 現在のプレイヤ数より少ないか確認

        // 最大プレイヤ数を変更

        return true;
    }

    // CPUが参加するか選択
    public bool ChangeJoinNPC(bool f) {
        // 設定を変更できる状態か確認



        return true;

    }


    // 試合時間変更

    // 何ポイント先取か設定




}
