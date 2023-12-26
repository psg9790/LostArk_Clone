using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Firebase.Auth;
using Firebase.Extensions;

public class TitleUI : MonoBehaviour
{
    FirebaseAuth auth;

    [SerializeField] TMP_InputField IF_email;
    [SerializeField] TMP_InputField IF_password;

    [SerializeField] GameObject panel_register;
    [SerializeField] TMP_InputField IF_register_email;
    [SerializeField] TMP_InputField IF_register_password;
    [SerializeField] TMP_InputField IF_register_nickname;

    void Awake()
    {
        auth = FirebaseAuth.DefaultInstance;
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
            Debug.Log(result.User.UserId);
            PopupMessageManager.Instance.PopupMessage("로그인 성공");
        });
    }

    void TryRegister(string email, string password, string nickname)
    {
        auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(t =>
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
            Debug.Log("닉네임 등록");
            AuthResult result = t.Result;
            Debug.Log(result.User.UserId);
            PopupMessageManager.Instance.PopupMessage("회원가입 성공");
        });
    }
}
