using System;
using System.Collections.Generic;
using SoftGear.Strix.Unity.Runtime;
using SoftGear.Strix.Unity.Runtime.Event;
using SoftGear.Strix.Client.Core.Model.Manager.Filter;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StrixRoomListGUI : MonoBehaviour {
    //public int page = 0;
    //public int maxItemCount = 10;
    //public StrixListView listView;
    //public Text pageText;
    //public Button prevPageButton;
    //public Button nextPageButton;
    public UnityEvent OnRoomJoined;
    private ICollection<RoomInfo> roomInfoCollection;
    //private bool isUpdated = false;
    public StrixEnterRoom enterRoom;
    public GameObject ScrollViewUI;
    public GameObject ListItemPrefab;
    public Button updateBtn;
    public bool includePrivateRoom = false;

    void OnEnable() {
        //page = 0;
        updateBtn.interactable = false;
        OnClickUpdate();
        //SearchRooms();
        //UpdateRoomList();
    }
    

    public void OnClickUpdate() {
        updateBtn.interactable = false;

        //if(!isUpdated)
        //    return;
        SearchRooms();
        //UpdateRoomList();

        //isUpdated = false;
    }

    //// ルーム一覧から戻る
    //public void OnBackButtonClick() {
    //    // サーバーとの通信切断
    //    StrixNetwork.instance.DisconnectMasterServer();
    //}

    //public void OnCreateRoomButtonClick() {
    //    CreateRoom();
    //}

    //public void OnJoinRandomRoomButtonClick() {
    //    StrixNetwork.instance.JoinRandomRoom(
    //        StrixNetwork.instance.playerName,
    //        args => {
    //            RoomJoined();
    //        },
    //        args => {
    //            CreateRoom();
    //        });
    //}

    //public void OnNextPageButtonClick() {
    //    if (roomInfoCollection == null || roomInfoCollection.Count > maxItemCount) {
    //        page++;
    //    }

    //    SearchRooms();
    //}

    //public void OnPrevPageButtonClick() {
    //    page--;

    //    if (page < 0) {
    //        page = 0;
    //    }

    //    SearchRooms();
    //}

    //private void CreateRoom() {
    //    RoomProperties roomProperties = new RoomProperties {
    //        name = "New Room",
    //        capacity = 10,
    //        state = 1,
    //    };

    //    RoomMemberProperties memberProperties = new RoomMemberProperties {
    //        name = StrixNetwork.instance.playerName
    //    };

    //    StrixNetwork.instance.CreateRoom(
    //        roomProperties,
    //        memberProperties,
    //        args => {
    //            RoomJoined();
    //        },
    //        args => {
    //            Debug.unityLogger.Log("Strix", "Create room failed!");
    //        }
    //    );
    //}

    private void SearchRooms() {
        if(includePrivateRoom) {
            StrixNetwork.instance.SearchRoom(
                new And(
                        new List<ICondition> {
                            new Equals(new Field("state"), new Value((double)1)),
                            new Equals(new Field("isJoinable"), new Value(true))
                        }
                    ), 100, 0, OnRoomSearch, null);
        } else {
            StrixNetwork.instance.SearchRoom(
                new And(
                        new List<ICondition> {
                            new Equals(new Field("state"), new Value((double)1)),
                            new Equals(new Field("isJoinable"), new Value(true)),
                            new Equals(new Field("isPasswordProtected"), new Value(false))
                        }
                    ), 100, 0, OnRoomSearch, null);
        }

        UpdateRoomList();


        //StrixNetwork.instance.SearchRoom(maxItemCount + 1, page * maxItemCount, OnRoomSearch, null);
        //prevPageButton.interactable = false;
        //nextPageButton.interactable = false;
    }

    private void UpdateRoomList() {
        //listView.ClearListItems();
        // リストアイテムを削除する
        foreach (Transform child in ScrollViewUI.transform) {
            Destroy(child.gameObject);
        }


        if (roomInfoCollection == null) {
            updateBtn.interactable = true;
            return;
        }


        //int count = 0;

        foreach (RoomInfo roomInfo in roomInfoCollection) {
            //GameObject item = listView.AddListItem();
            GameObject item = Instantiate(ListItemPrefab, ScrollViewUI.transform);
            StrixRoomListItem roomListItem = item.GetComponent<StrixRoomListItem>();
            roomListItem.roomInfo = roomInfo;
            roomListItem.roomList = this;
            roomListItem.enterRoom = enterRoom;

            roomListItem.UpdateGUI();

            //count++;

            //if (count >= maxItemCount) {
            //    break;
            //}
        }
        updateBtn.interactable = true;

    }

    private void OnRoomSearch(RoomSearchEventArgs args) {
        roomInfoCollection = args.roomInfoCollection;

        //pageText.text = "" + (page + 1);

        //isUpdated = true;
        //prevPageButton.interactable = true;
        //nextPageButton.interactable = true;
    }

    public void SetincludePrivateRoom(Toggle toggle) {
        includePrivateRoom = toggle.isOn;
    }


    private void RoomJoined() {
        OnRoomJoined.Invoke();
        gameObject.SetActive(false);
    }


}
