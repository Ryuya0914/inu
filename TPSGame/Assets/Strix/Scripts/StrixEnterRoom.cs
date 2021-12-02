using System.Collections.Generic;
using SoftGear.Strix.Unity.Runtime;
using SoftGear.Strix.Unity.Runtime.Event;
using UnityEngine;
using UnityEngine.Events;

public class StrixEnterRoom : MonoBehaviour {

    // 1：試合時間
    double roomkey1 = 180;
    // 2：目標ポイント
    double roomkey2 = 50;
    // 3：フレンドリーファイア 0=なし
    double roomkey3 = 0;
    // 4：画面に味方の名前表示
    double roomkey4 = 1;
    // 5：マップに味方の名前表示
    double roomkey5 = 1;
    // 6：ステージ
    double roomkey6 = 1;

    /// ルーム入室完了時イベント
    public UnityEvent onRoomEntered;
    
    /// ルーム入室失敗時イベント
    public UnityEvent onRoomEnterFailed;


    // 部屋に参加する系＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊
    
    // ランダムな部屋に入る
    public void EnterRondomRoom() {
        StrixNetwork.instance.JoinRandomRoom(StrixNetwork.instance.playerName, args => {
            onRoomEntered.Invoke();
        }, args => {
            CreateRoom("New Room", 10);
        });
    }

    // 指定した部屋に入る
    public void EnterChoiseRoom(RoomInfo _info, SoftGear.Strix.Unity.Runtime.Event.FailureEventHandler _enterFailed) {
        StrixNetwork.instance.JoinRoom(_info.host, _info.port, _info.protocol, _info.roomId, StrixNetwork.instance.playerName,
            args => {
                OnRoomJoin(args);
            },
            args => { 
                _enterFailed.Invoke(args);
                OnRoomJoinFailed(args); 
            });

    }
    // 指定したパスワード付きの部屋に入る
    public void EnterChoiseRoom(RoomInfo _info, string _pass, SoftGear.Strix.Unity.Runtime.Event.FailureEventHandler _enterFailed) {
        StrixNetwork.instance.JoinRoom(
            new RoomJoinArgs{ 
                host =_info.host, 
                port = _info.port, 
                protocol = _info.protocol, 
                roomId = _info.roomId, 
                password = _pass}, 
            args => {
                OnRoomJoin(args);
            },
            args => { 
                _enterFailed.Invoke(args);
                OnRoomJoinFailed(args); 
            });

    }


    // 部屋作成系＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊

    // 部屋を作成
    public void CreateRoom(string _roomName, int _capacity) {
        RoomProperties roomProperties = new RoomProperties {
            capacity = _capacity,
            name = _roomName,
            state = 0,
            key1 = roomkey1,
            key2 = roomkey2,
            key3 = roomkey3,
            key4 = roomkey4,
            key5 = roomkey5,
            key6 = roomkey6
        };

        RoomMemberProperties memberProperties = new RoomMemberProperties {
            name = StrixNetwork.instance.playerName
        };


        StrixNetwork.instance.CreateRoom(roomProperties, memberProperties, args => {
            onRoomEntered.Invoke();
        }, args => {
            onRoomEnterFailed.Invoke();
        });
    }
    // 部屋を作成
    public void CreateRoom(string _roomName, int _capacity, SoftGear.Strix.Unity.Runtime.Event.FailureEventHandler _enterFailed) {
        RoomProperties roomProperties = new RoomProperties {
            capacity = _capacity,
            name = _roomName,
            state = 0,
            key1 = roomkey1,
            key2 = roomkey2,
            key3 = roomkey3,
            key4 = roomkey4,
            key5 = roomkey5,
            key6 = roomkey6
        };

        RoomMemberProperties memberProperties = new RoomMemberProperties {
            name = StrixNetwork.instance.playerName
        };


        StrixNetwork.instance.CreateRoom(roomProperties, memberProperties, args => {
            onRoomEntered.Invoke();
        }, args => {
            onRoomEnterFailed.Invoke();
        });
    }

    // パスワードつきの部屋を作成
    public void CreateRoom(string _roomName, int _capacity, string _password) {
        RoomProperties roomProperties = new RoomProperties {
            capacity = _capacity,
            name = _roomName,
            password = _password,
            state = 0,
            key1 = roomkey1,
            key2 = roomkey2,
            key3 = roomkey3,
            key4 = roomkey4,
            key5 = roomkey5,
            key6 = roomkey6
        };

        RoomMemberProperties memberProperties = new RoomMemberProperties {
            name = StrixNetwork.instance.playerName
        };


        StrixNetwork.instance.CreateRoom(roomProperties, memberProperties, args => {
            onRoomEntered.Invoke();
        }, args => {
            onRoomEnterFailed.Invoke();
        });
    }
    
    // パスワードつきの部屋を作成
    public void CreateRoom(string _roomName, int _capacity, string _password, SoftGear.Strix.Unity.Runtime.Event.FailureEventHandler _enterFailed) {
        RoomProperties roomProperties = new RoomProperties {
            capacity = _capacity,
            name = _roomName,
            password = _password,
            state = 0,
            key1 = roomkey1,
            key2 = roomkey2,
            key3 = roomkey3,
            key4 = roomkey4,
            key5 = roomkey5,
            key6 = roomkey6
        };

        RoomMemberProperties memberProperties = new RoomMemberProperties {
            name = StrixNetwork.instance.playerName
        };


        StrixNetwork.instance.CreateRoom(roomProperties, memberProperties, args => {
            onRoomEntered.Invoke();
        }, args => {
            onRoomEnterFailed.Invoke();
        });
    }


    // その他＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊

    // 通信を切断する
    public void DisConnectServer() {
        // サーバーとの通信切断
        StrixNetwork.instance.DisconnectMasterServer();
    }

    // 部屋に参加したときのイベント
    void OnRoomJoin(RoomJoinEventArgs args) {
        onRoomEntered.Invoke();
    }

    // 部屋に参加出来なかったときのイベント
    void OnRoomJoinFailed(FailureEventArgs args) {
        onRoomEnterFailed.Invoke();
    }



}
