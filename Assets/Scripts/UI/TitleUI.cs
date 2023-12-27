using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Firebase.Auth;
using Firebase.Firestore;
using Firebase.Extensions;
using System;

public class TitleUI : MonoBehaviour
{
    FirebaseAuth auth;
    FirebaseFirestore store;

    [SerializeField] TMP_InputField IF_email;
    [SerializeField] TMP_InputField IF_password;

    [SerializeField] GameObject panel_register;
    [SerializeField] TMP_InputField IF_register_email;
    [SerializeField] TMP_InputField IF_register_password;
    [SerializeField] TMP_InputField IF_register_nickname;

    void Awake()
    {
        auth = FirebaseAuth.DefaultInstance;
        store = FirebaseFirestore.DefaultInstance;
    }

    void Start()
    {
        panel_register.SetActive(false);
    }

    public void OnLoginClick()
    {
        if (string.IsNullOrEmpty(IF_email.text) || string.IsNullOrEmpty(IF_password.text))
        {
            PopupMessageManager.Instance.PopupMessage("입력 필요");
            return;
        }
        PopupMessageManager.Instance.PopupMessage("로그인...");
        TryLogin(IF_email.text, IF_password.text);
    }

    public void OnRegisterClick_PopupUI()
    {
        panel_register.SetActive(true);
    }

    public void OnRegisterClick()
    {
        panel_register.SetActive(false);
        if (string.IsNullOrEmpty(IF_register_email.text) ||
        string.IsNullOrEmpty(IF_register_password.text) ||
        string.IsNullOrEmpty(IF_register_nickname.text))
        {
            PopupMessageManager.Instance.PopupMessage("입력 필요");
            return;
        }
        TryRegister(IF_register_email.text, IF_register_password.text, IF_register_nickname.text);
    }

    void TryLogin(string email, string password)
    {
        auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(t =>
        {
            if (t.IsCanceled)
            {
                Debug.LogError("Canceled");
                return;
            }
            if (t.IsFaulted)
            {
                Debug.LogError("Faulted\n" + t.Exception.Message);
                PopupMessageManager.Instance.PopupMessage(t.Exception.Message);
                return;
            }
            // 로그인 성공 -> 씬 전환
            AuthResult result = t.Result;
            PopupMessageManager.Instance.PopupMessage("로그인 성공");
        });
        PopupMessageManager.Instance.PopupMessage(FirebaseAuth.DefaultInstance.CurrentUser.UserId);
        Debug.Log(FirebaseAuth.DefaultInstance.CurrentUser.UserId);
        GameObject.FindObjectOfType<PhotonManager>().StartPhotonNetworking();
    }

    void TryRegister(string email, string password, string nickname)
    {
        auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(async t =>
        {
            if (t.IsCanceled)
            {
                Debug.LogError("Canceled");
                return;
            }
            if (t.IsFaulted)
            {
                Debug.LogError("Faulted\n" + t.Exception.Message);
                PopupMessageManager.Instance.PopupMessage(t.Exception.Message);
                return;
            }
            // 회원가입 성공
            AuthResult result = t.Result;

            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("email", result.User.Email);
            dic.Add("nickname", nickname);
            await store.Collection("users").Document(result.User.UserId).SetAsync(dic);
            PopupMessageManager.Instance.PopupMessage("회원가입 성공");
        });
    }
}
