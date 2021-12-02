using System;
using System.Collections.Generic;
using SoftGear.Strix.Client.Core.Auth.Message;
using SoftGear.Strix.Client.Core.Error;
using SoftGear.Strix.Unity.Runtime;
using SoftGear.Strix.Unity.Runtime.Event;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StrixRoomListItem : MonoBehaviour
{
    public RoomInfo roomInfo;
    public Button button;
    public Text RoomNameText;
    public Text RoomCapacityText;

    public GameObject passwordUI;
    
    [HideInInspector]
    public StrixRoomListGUI roomList;
    public StrixEnterRoom enterRoom;
    public Canvas InputPasswordUI;
    

    public void UpdateGUI() {
        RoomNameText.text = roomInfo.name;
        RoomCapacityText.text = roomInfo.memberCount + " / " + roomInfo.capacity;
    }


    public void OnClick() {
        
        button.interactable = false;

        // パスワードつきか確認
        if(roomInfo.isPasswordProtected) {
            // パスワード入力UIを表示する
            InputPasswordUI.enabled = true;
            InputPasswordUI.GetComponent<StrixTittleSettings>().UpdateUI(roomInfo);
            button.interactable = true;

        } else {
            // パスワードがない場合はそのまま入室
            enterRoom.EnterChoiseRoom(roomInfo, args => { OnRoomJoinFailed(args); });
            //StrixNetwork.instance.JoinRoom(roomInfo.host, roomInfo.port, roomInfo.protocol, roomInfo.roomId, StrixNetwork.instance.playerName, OnRoomJoin, OnRoomJoinFailed);
        }

    }

    // ルームに参加出来た時に実行するメソッド
    private void OnRoomJoin(RoomJoinEventArgs args) {
        button.interactable = true;

        if (roomList != null) {
            roomList.OnRoomJoined.Invoke();
            roomList.gameObject.SetActive(false);
        }
    }

    // ルーム参加に失敗したときに実行するメソッド
    private void OnRoomJoinFailed(FailureEventArgs args) {
        button.interactable = true;

        // string error = "";

        //if (args.cause != null) {
        //    error = args.cause.Message;
        //}

        //Debug.unityLogger.Log("Strix", "Room join failed. " + error);
    }
}
