using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System;
using Unity.Collections;

public class CamFollower : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] Vector3 offset;
    CinemachineBrain cb;

    // int idx = 0;
    // [SerializeField][ReadOnly] Transform[] childs;

    void Start()
    {
        cb = GetComponent<CinemachineBrain>();
        cb.enabled = false;
    }

    public void SetPlayerTarget(Transform trans)
    {
        player = trans;

        // Transform[] childsTransform = player.Find("lookPos").GetComponentsInChildren<Transform>();
        // Array.Copy(childsTransform, 1, childs, 0, 3);
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
