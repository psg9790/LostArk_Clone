using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CharacterLoader : MonoBehaviour
{

    void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            //PopupMessageManager.Instance.PopupMessage("i am manager");
            PhotonNetwork.Instantiate("Player", Vector3.zero, Quaternion.Euler(Vector3.zero), 0);
        }

        // foreach (var player in PhotonNetwork.CurrentRoom.Players)
        // {

        //     PhotonNetwork.Instantiate("Player", Vector3.zero, Quaternion.Euler(Vector3.zero), 0);
        // }
    }
}
