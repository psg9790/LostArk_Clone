using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class LobbyUI : MonoBehaviour
{
    [SerializeField] GameObject roomItemFactory;

    void Start()
    {
        go_createRoom.SetActive(false);
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
    }

    public void OnCreateRoomCancelButtonClick()
    {
        // 방 생성 UI 끄기
        go_createRoom.SetActive(false);
    }
    #endregion
}
