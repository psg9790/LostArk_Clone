using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.AI;

public class PlayerMove : MonoBehaviour
{

    Camera cam;
    CamFollower camFollower;
    NavMeshAgent navAgent;

    void Start()
    {
        cam = Camera.main;
        camFollower = cam.GetComponent<CamFollower>();
        navAgent = GetComponent<NavMeshAgent>();
    }
    void Update()
    {

    }

    void RaycastFloor()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;
        Physics.Raycast(ray, out hitInfo);
        if (hitInfo.transform.CompareTag("Floor"))   // 바닥 찍었으면
        {
            NavMeshPath path = new NavMeshPath();
            navAgent.CalculatePath(hitInfo.point, path);
            if (path.status == NavMeshPathStatus.PathComplete)
            {

            }
        }
    }
}
