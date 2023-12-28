using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Firestore;
using Firebase.Extensions;
using Photon.Pun;
using System;

public class GameManager : MonoBehaviour
{
    static GameManager instance;
    public static GameManager Instance => instance;

    string user_email;
    string user_nickname = "guest";

    void Awake()
    {
        if (instance != null)
        {
            Destroy(this.gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    void Start()
    {
        Screen.SetResolution(1920, 1080, FullScreenMode.Windowed, 60);
    }

    public void SetUserEmail(string v_email)
    {
        user_email = v_email;
        // try
        // {
        //     FirestoreNicknameLoad();
        // }
        // catch (Exception e)
        // {

        // }
    }
    public void SetUserNickname(string v_nickname)
    {
        user_nickname = v_nickname;
    }
    public string GetUserEmail()
    {
        return user_email;
    }
    public string GetUserNickname()
    {
        return user_nickname;
    }
    void FirestoreNicknameLoad()
    {
        CollectionReference colRef = FirebaseFirestore.DefaultInstance.Collection("users");
        colRef.Document(user_email).GetSnapshotAsync().ContinueWithOnMainThread(t =>
        {
            // if (t.IsCanceled)
            // {
            //     Debug.LogError("Canceled");
            //     return;
            // }
            // if (t.IsFaulted)
            // {
            //     Debug.LogError("Faulted\n" + t.Exception.Message);
            //     PopupMessageManager.Instance.PopupMessage(t.Exception.Message);
            //     return;
            // }
            DocumentSnapshot snapshot = t.Result;
            if (snapshot.Exists)
            {
                Dictionary<string, object> dic = snapshot.ToDictionary();
                // foreach (KeyValuePair<string, object> pair in dic)
                // {
                //     Debug.Log(string.Format("{0}: {1}", pair.Key, pair.Value));
                // }
                user_nickname = dic["nickname"].ToString();
                Debug.Log(user_nickname);
                PhotonNetwork.NickName = GetUserNickname();
            }
            else
            {
                Debug.Log("Document does not exist");
            }
        });
    }
}
