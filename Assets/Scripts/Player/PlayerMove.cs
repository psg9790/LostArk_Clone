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
        camFollower.SetPlayerTarget(this.transform);

        navAgent = GetComponent<NavMeshAgent>();
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            RaycastFloor();
        }
    }

    void RaycastFloor()
    {
        if (navAgent.hasPath)
            navAgent.ResetPath();

        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;
        Physics.Raycast(ray, out hitInfo, Mathf.Infinity);
        Debug.DrawRay(cam.transform.position, ray.direction * Mathf.Infinity, Color.red, 1f);
        if (hitInfo.transform.CompareTag("Floor"))   // 바닥 찍었으면
        {
            NavMeshPath path = new NavMeshPath();
            navAgent.CalculatePath(hitInfo.point, path);
            if (path.status == NavMeshPathStatus.PathComplete)
            {
                navAgent.SetPath(path);
            }
        }
    }
}
