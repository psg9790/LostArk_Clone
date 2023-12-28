using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using TMPro;

public class NIcknameLaunch : MonoBehaviour
{
    void Start()
    {
        PhotonView pv = GetComponentInParent<PhotonView>();
        TMP_Text tmp = GetComponent<TMP_Text>();
        if (pv.IsMine)
            tmp.text = PhotonNetwork.NickName;
        else
            tmp.text = "";
    }

    void Update()
    {
        transform.forward = Camera.main.transform.forward;
    }
}
