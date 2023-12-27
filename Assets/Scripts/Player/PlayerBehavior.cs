using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.AI;
using Unity.VisualScripting;

public class PlayerBehavior : MonoBehaviour
{
    enum State
    {
        // move
        idle,
        moving,
        attack,
        skill,

        // battle
        stiff,
        knockdown,
        knockback,

    }

    Camera cam;
    CamFollower camFollower;
    NavMeshAgent navAgent;
    Animator anim;
    PhotonView photonView;

    void Start()
    {
        photonView = GetComponent<PhotonView>();
        if (photonView.IsMine == false)
            return;

        cam = Camera.main;
        camFollower = cam.GetComponent<CamFollower>();
        camFollower.SetPlayerTarget(this.transform);

        navAgent = GetComponent<NavMeshAgent>();

        anim = GetComponent<Animator>();
    }

    State curState = State.idle;
    void Update()
    {
        if (photonView.IsMine == false)
            return;

        InputsReceive();

        switch (curState)
        {
            case State.idle:
                IdleUpdate();
                break;
            case State.moving:
                MovingUpdate();
                break;
            case State.attack:
                AttackUpdate();
                break;
            case State.skill:
                break;
            case State.stiff:
                break;
            case State.knockdown:
                break;
            case State.knockback:
                break;
        }
    }

    Coroutine attackCoroutine;
    void InputsReceive()
    {
        if (Input.GetMouseButtonDown(0))  // 기본공격
        {
            if (curState != State.attack
                && curState != State.knockback
                && curState != State.knockdown
                && curState != State.stiff
                && curState != State.skill)
            {
                if (curState == State.moving)
                {
                    anim.SetBool("moving", false);
                    navAgent.ResetPath();
                }
                attackCoroutine = StartCoroutine(StartAttack1(1.1f, .75f, 0.75f));     // 1단공격 시작
            }
        }
        if (Input.GetMouseButton(1))    // 이동
        {
            if (curState != State.attack
                && curState != State.knockback
                && curState != State.knockdown
                && curState != State.stiff
                && curState != State.skill)
            {
                RaycastFloor();
            }
        }

        if (curState != State.knockback
                    && curState != State.knockdown
                    && curState != State.stiff
                    && curState != State.skill)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {

            }
            if (Input.GetKeyDown(KeyCode.W))
            {

            }
            if (Input.GetKeyDown(KeyCode.E))
            {

            }
            if (Input.GetKeyDown(KeyCode.R))
            {

            }
            if (Input.GetKeyDown(KeyCode.A))
            {

            }
            if (Input.GetKeyDown(KeyCode.S))
            {

            }
            if (Input.GetKeyDown(KeyCode.D))
            {

            }
            if (Input.GetKeyDown(KeyCode.F))
            {

            }
            if (Input.GetKeyDown(KeyCode.Space))
            {

            }
        }
    }

    void IdleUpdate()
    {

    }
    void MovingUpdate()
    {
        if (navAgent.hasPath == false || navAgent.remainingDistance <= 0.15f)
        {
            anim.SetBool("moving", false);
            curState = State.idle;
        }
    }
    void AttackUpdate()
    {

    }
    IEnumerator StartAttack1(float duration, float lowerBound, float attackTiming)
    {
        LookAtCursorPos();
        Debug.Log("attack1");
        curState = State.attack;
        anim.SetTrigger("attack");
        //anim.Play("base1");
        float elapsed = 0;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            if (lowerBound < elapsed)
            {
                if (Input.GetMouseButtonDown(0))     // 2단 공격 시작
                {
                    attackCoroutine = StartCoroutine(StartAttack2(1.1f, .75f, 0.75f));
                    yield break;
                }
            }
            yield return null;
        }
        curState = State.idle;
    }

    IEnumerator StartAttack2(float duration, float lowerBound, float attackTiming)
    {
        Debug.Log("attack2");
        LookAtCursorPos();
        curState = State.attack;
        anim.SetTrigger("attack2");
        //anim.Play("base2");
        float elapsed = 0;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            if (lowerBound < elapsed)
            {
                if (Input.GetMouseButtonDown(0))     // 2단 공격 시작
                {
                    attackCoroutine = StartCoroutine(StartAttack3(1.1f, .75f, 0.75f));
                    yield break;
                }
            }
            yield return null;
        }
        curState = State.idle;
    }
    IEnumerator StartAttack3(float duration, float lowerBound, float attackTiming)
    {
        Debug.Log("attack3");
        LookAtCursorPos();
        curState = State.attack;
        anim.SetTrigger("attack3");
        //anim.Play("base3");
        yield return new WaitForSeconds(duration);
        curState = State.idle;
        // 막타
    }

    Coroutine LookCursorCo;
    void LookAtCursorPos()
    {
        // 공격 방향 보기
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;
        Debug.DrawRay(cam.transform.position, ray.direction * Mathf.Infinity, Color.red, 1f);
        if (Physics.Raycast(ray, out hitInfo, Mathf.Infinity))
            if (hitInfo.transform.CompareTag("Floor"))   // 바닥 찍었으면
            {
                Vector3 dir = hitInfo.point - transform.position;
                dir.y = 0;

                if (LookCursorCo != null)
                    StopCoroutine(LookCursorCo);
                LookCursorCo = StartCoroutine(LookCursorPosCo(dir));
                //transform.forward = dir;
            }
    }
    IEnumerator LookCursorPosCo(Vector3 dir)
    {
        float elapsed = 0;
        while (elapsed < 0.4f)
        {
            elapsed += Time.deltaTime;
            transform.rotation = Quaternion.LookRotation(Vector3.Lerp(transform.forward, dir, 0.1f));
            yield return null;
        }
        transform.forward = dir;
    }

    void RaycastFloor()
    {
        if (navAgent.hasPath)
            navAgent.ResetPath();

        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;
        Debug.DrawRay(cam.transform.position, ray.direction * Mathf.Infinity, Color.red, 1f);
        if (Physics.Raycast(ray, out hitInfo, Mathf.Infinity))
            if (hitInfo.transform.CompareTag("Floor"))   // 바닥 찍었으면
            {
                NavMeshPath path = new NavMeshPath();
                navAgent.CalculatePath(hitInfo.point, path);
                if (path.status == NavMeshPathStatus.PathComplete)
                {
                    navAgent.SetPath(path);
                    anim.SetBool("moving", true);

                    curState = State.moving;
                    //movingCo = StartCoroutine(CheckMovingStopCo());
                }
            }
    }
}
