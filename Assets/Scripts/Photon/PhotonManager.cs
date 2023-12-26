using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using Firebase.Auth;
using Firebase.Firestore;
using Firebase.Extensions;
using Photon.Realtime;
using System;

public class PhotonManager : MonoBehaviourPunCallbacks
{
    /// <summary>
    /// 포톤 연결을 시작하는 전역 함수
    /// 파이어베이스 로그인이 완료되면 실행됨
    /// </summary>
    public void StartPhotonNetworking()
    {
        // 씬 동기화
        PhotonNetwork.AutomaticallySyncScene = true;
        // 버전 할당
        PhotonNetwork.GameVersion = Application.version;
        PopupMessageManager.Instance.PopupMessage(Application.version);


        // 포톤 서버 접속
        PhotonNetwork.ConnectUsingSettings();
        PopupMessageManager.Instance.PopupMessage("포톤 서버 접속중...");
    }

    public override void OnConnectedToMaster()
    {
        //Debug.Log(FirebaseAuth.DefaultInstance.CurrentUser.UserId);
        // App ID
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
        // 로비 접속
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        PopupMessageManager.Instance.PopupMessage("포톤 서버 접속완료!");

        PhotonNetwork.LoadLevel("Lobby");
    }

    public override void OnJoinedRoom()
    {
        PopupMessageManager.Instance.PopupMessage("방 입장 완료!");
        PhotonNetwork.LoadLevel("InGame");
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        PopupMessageManager.Instance.PopupMessage("방 입장 실패...!");
    }

    /// <summary>
    /// 방 리스트 UI 업데이트
    /// </summary>
    /// <param name="roomList"></param>
    LobbyUI lobby_ui = null;
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        if (lobby_ui == null)
            lobby_ui = GameObject.FindObjectOfType<LobbyUI>();
        lobby_ui.OnRoomListUpdate(roomList);
    }
}
