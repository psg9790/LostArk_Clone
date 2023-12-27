using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;

public class RoomItem : MonoBehaviour
{
    LobbyUI lobbyUI;
    [SerializeField] TMP_Text text_raidType;
    [SerializeField] TMP_Text text_roomName;
    [SerializeField] TMP_Text text_memberCounts;

    public void SetLobbyUI(LobbyUI v_lobbyUI)
    {
        lobbyUI = v_lobbyUI;
    }
    void SetRaidType(string raidType)
    {
        text_raidType.text = raidType;
    }
    void SetRoomName(string roomName)
    {
        text_roomName.text = roomName;
    }
    void SetMemberCounts(string memberCounts)
    {
        text_memberCounts.text = memberCounts;
    }

    public void SetTexts(string raidType, string roomName, string memberCounts)
    {
        SetRaidType(raidType);
        SetRoomName(roomName);
        SetMemberCounts(memberCounts);
    }
    public void SetTexts(string roomName, string memberCounts)
    {
        SetRoomName(roomName);
        SetMemberCounts(memberCounts);
    }

    public void OnRoomItemClick()
    {
        lobbyUI.SetCurRoomName(text_roomName.text);
        lobbyUI.OnJoinButtonClick();
        Vector3 mousePos = Input.mousePosition;
        mousePos.y -= Screen.height;
        lobbyUI.GetJoinUI().GetComponent<RectTransform>().anchoredPosition = mousePos;
        //Debug.Log(mousePos);
    }
}
