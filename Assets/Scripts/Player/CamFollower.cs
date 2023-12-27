using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CamFollower : MonoBehaviour
{
    Transform player;
    [SerializeField] CinemachineVirtualCamera vc;


    public void SetPlayerTarget(Transform trans)
    {
        player = trans;
        vc.Follow = player;
        vc.LookAt = player;
    }
}
