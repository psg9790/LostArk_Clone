using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CamFollower : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] Vector3 offset;
    CinemachineBrain cb;

    void Start()
    {
        cb = GetComponent<CinemachineBrain>();
        cb.enabled = false;
    }

    public void SetPlayerTarget(Transform trans)
    {
        player = trans;
    }

    void LateUpdate()
    {
        if (player != null)
        {
            if (cb.enabled == false)
            {
                transform.position = player.position + offset;
                transform.forward = player.position - transform.position;
            }
        }
    }
}
