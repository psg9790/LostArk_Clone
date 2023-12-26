using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RoomItem : MonoBehaviour
{
    [SerializeField] TMP_Text text_raidType;
    [SerializeField] TMP_Text text_roomName;
    [SerializeField] TMP_Text text_memberCounts;

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
}
