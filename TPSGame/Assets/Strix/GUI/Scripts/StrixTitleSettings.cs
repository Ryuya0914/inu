using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SoftGear.Strix.Unity.Runtime;

public class StrixTitleSettings : MonoBehaviour
{
    public StrixEnterRoom enterRoom;


    // 部屋を作成するときのやつ＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊
    // 部屋の名前
    string roomName = "New Room";
    [SerializeField] InputField roomNameField;
    // 参加人数
    int roomCapa = 10;
    [SerializeField] Text roomCapaTextLeft;
    [SerializeField] Text roomCapaTextRight;
    // 部屋のパスワード
    string roomPass = "";
    [SerializeField] InputField roomPassField;
    // 部屋作成ボタン
    [SerializeField] Button createRoomBtn;

    // 部屋を作る
    public void CreateRoom() {
        createRoomBtn.interactable = false;
        if (roomPass == "") {
            // 部屋作る
            enterRoom.CreateRoom(roomName, roomCapa, args => { createRoomBtn.interactable = true; });
        } else {
            // プライベートな部屋作る
            enterRoom.CreateRoom(roomName, roomCapa, roomPass, args => { createRoomBtn.interactable = true; });
        }
    }
    
    // 部屋の名前を変更する
    public void SetRoomName(InputField _field) {
        string s = MojiFilter(_field.text);
        roomName = s;
        roomNameField.text = s;
    }

    // 参加人数を変更する
    public void SetRoomCapacity_Increase() {
        roomCapa += 2;
        roomCapa = (roomCapa > 10) ? 10 : roomCapa;
        roomCapaTextLeft.text = "" + (roomCapa/2);
        roomCapaTextRight.text = "" + (roomCapa/2);
    }
    public void SetRoomCapacity_Decrease() {
        roomCapa -= 2;
        roomCapa = (roomCapa < 2) ? 2 : roomCapa;
        roomCapaTextLeft.text = "" + (roomCapa / 2);
        roomCapaTextRight.text = "" + (roomCapa / 2);
    }

    // パスワードを変更する
    public void SetPassword(InputField _field) {
        roomPass = _field.text;
        //roomPassField.text = _field.text;
    }

    // 部屋に参加する時のやつ＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊
    // 部屋の情報
    RoomInfo roomInfo;
    [SerializeField] Text roomNameText;
    [SerializeField] Text roomCapacityText;
    // 部屋のパスワード(参加時)
    string privateRoomPass = "";
    [SerializeField] InputField privateRoomPassField;
    // 参加するボタン
    [SerializeField] Button JoinPrivateRoomBtn;



    public void UpdateUI(RoomInfo _info) {
        roomInfo = _info;
        roomNameText.text = roomInfo.name;
        roomCapacityText.text = roomInfo.memberCount + " / " + roomInfo.capacity;
    }

    // プライベートな部屋に参加する
    public void JoinPrivateRoom() {
        JoinPrivateRoomBtn.interactable = false;
        enterRoom.EnterChoiseRoom(
            roomInfo, roomPass, 
            args => { JoinPrivateRoomBtn.interactable = true; UpdateUI(roomInfo); }
        );
    }

    // パスワードを入力する
    public void SetPrivateRoomPass(InputField _field) {
        roomPass = _field.text;
        //roomPassField.text = _field.text;
    }

    // その他＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊
    // プレイヤの名前
    [SerializeField] InputField playerNameField;

    // プレイヤ名を変更する
    public void SetPlayerName(InputField _field) {
        string s = MojiFilter(_field.text);
        StrixNetwork.instance.playerName = s;
        playerNameField.text = s;
    }


    // 文字の規制
    string MojiFilter(string moji) {
        



        return moji;
    }


}
