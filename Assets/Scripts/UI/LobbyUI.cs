using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Firebase.Firestore;
using Firebase.Auth;
using Firebase.Extensions;

public class LobbyUI : MonoBehaviour
{
    [SerializeField] GameObject roomItemFactory;

    void Start()
    {
        go_createRoom.SetActive(false);
        //Debug.Log(FirebaseFirestore.DefaultInstance.App);
        // FirebaseFirestore.DefaultInstance.Collection("users")
        // .Document(FirebaseAuth.DefaultInstance.CurrentUser.UserId).GetSnapshotAsync()
        // .ContinueWithOnMainThread(t =>
        // {
        //     if (t.IsCanceled)
        //     {
        //         Debug.LogError("Canceled");
        //         return;
        //     }
        //     if (t.IsFaulted)
        //     {
        //         Debug.LogError("Faulted\n" + t.Exception.Message);
        //         PopupMessageManager.Instance.PopupMessage(t.Exception.Message);
        //         return;
        //     }
        //     DocumentSnapshot snapshot = t.Result;
        //     Dictionary<string, object> dic = snapshot.ToDictionary();
        //     PhotonNetwork.NickName = (string)dic.GetValueOrDefault("nickname", "guest");
        // });
    }


    #region create_rooom
    // 방 생성
    [SerializeField] GameObject go_createRoom;
    [SerializeField] TMP_InputField if_createRoomName;

    public void OnCreateRoomButtonClick()
    {
        // 방 생성 UI 띄우기
        go_createRoom.SetActive(true);
    }

    public void OnCreateRoomApplyButtonClick()
    {
        // 방 이름으로 생성
        RoomOptions ro = new RoomOptions();
        ro.IsVisible = true;
        ro.IsOpen = true;
        ro.MaxPlayers = 4;
        PhotonNetwork.CreateRoom(if_createRoomName.text, ro);
        PopupMessageManager.Instance.PopupMessage("방 생성 중...", visible_time: 3f);
    }

    public void OnCreateRoomCancelButtonClick()
    {
        // 방 생성 UI 끄기
        go_createRoom.SetActive(false);
    }
    #endregion

    [SerializeField] Transform content_roomlist;
    Dictionary<string, GameObject> roomDic = new Dictionary<string, GameObject>();
    public void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        GameObject tempRoom = null;
        foreach (RoomInfo info in roomList)
        {
            if (info.RemovedFromList)    // 룸이 삭제된 경우
            {
                roomDic.TryGetValue(info.Name, out tempRoom);
                Destroy(tempRoom.gameObject);
                roomDic.Remove(info.Name);
            }
            else    // 룸 정보 변경된 경우 (갱신/추가)
            {
                if (roomDic.ContainsKey(info.Name))  // 갱신
                {
                    roomDic.TryGetValue(info.Name, out tempRoom);
                    tempRoom.GetComponent<RoomItem>().SetTexts(info.Name, $"{info.PlayerCount}/{info.MaxPlayers}");
                }
                else    // 추가
                {
                    GameObject new_roomitem = Instantiate(roomItemFactory, content_roomlist);
                    new_roomitem.GetComponent<RoomItem>().SetTexts(info.Name, $"({info.PlayerCount}/{info.MaxPlayers})");
                    roomDic.Add(info.Name, new_roomitem);
                }
            }

        }
    }
}
