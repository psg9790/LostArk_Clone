using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PopupMessageManager : MonoBehaviour
{
    static PopupMessageManager instance;
    public static PopupMessageManager Instance => instance;

    [SerializeField] CanvasGroup cv_msg;
    [SerializeField] TMP_Text text_msg;

    void Awake()
    {
        if (instance != null)
        {
            Destroy(this.gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(this.gameObject);

        cv_msg.alpha = 0;
    }

    Coroutine co_popup;
    public void PopupMessage(string message, float appear_time = .1f, float visible_time = 1.5f, float disappear_time = .5f)
    {
        StopPopupCo();
        co_popup = StartCoroutine(PopupCo(message, appear_time, visible_time, disappear_time));
    }
    void StopPopupCo()
    {
        if (co_popup != null)
            StopCoroutine(co_popup);
        cv_msg.alpha = 0;
    }
    IEnumerator PopupCo(string message, float appear_time, float visible_time, float disappear_time)
    {
        text_msg.text = message;

        float elapsed = 0;
        while (elapsed < appear_time)
        {
            elapsed += Time.deltaTime;
            cv_msg.alpha = (elapsed / appear_time);
            yield return null;
        }

        cv_msg.alpha = 1;
        yield return new WaitForSeconds(visible_time);

        elapsed = 0;
        while (elapsed < disappear_time)
        {
            elapsed += Time.deltaTime;
            cv_msg.alpha = 1 - (elapsed / disappear_time);
            yield return null;
        }
        cv_msg.alpha = 0;
    }
}
