using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using Firebase.Auth;
using Firebase.Firestore;
using Firebase.Extensions;

public class PhotonManager : MonoBehaviourPunCallbacks
{
    public static void StartPhotonNetworking()
    {
        // 씬 동기화
        PhotonNetwork.AutomaticallySyncScene = true;
        // 버전 할당
        PhotonNetwork.GameVersion = Application.version;

        // App ID
        FirebaseFirestore.DefaultInstance.Collection("users")
        .Document(FirebaseAuth.DefaultInstance.CurrentUser.UserId)
        .GetSnapshotAsync().ContinueWithOnMainThread(t =>
        {
            DocumentSnapshot snapshot = t.Result;
            Dictionary<string, object> dic = snapshot.ToDictionary();
            PhotonNetwork.NickName = (string)dic.GetValueOrDefault("nickname", "guest");

            // 포톤 서버 접속
            PhotonNetwork.ConnectUsingSettings();
            PopupMessageManager.Instance.PopupMessage("포톤 Master 접속중...");
        });
    }

    public override void OnConnectedToMaster()
    {
        PopupMessageManager.Instance.PopupMessage("포톤 Master 접속완료!");

        // 로비 접속
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        PopupMessageManager.Instance.PopupMessage("포톤 Lobby 접속완료!");

        PhotonNetwork.LoadLevel("Lobby");
    }
}
