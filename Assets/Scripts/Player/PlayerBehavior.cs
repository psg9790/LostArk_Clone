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
        lastAttack,
        skill,
        space,

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
    PlayerSkillSpawner playerSkillSpawner;

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
        playerSkillSpawner = GetComponent<PlayerSkillSpawner>();
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
            case State.space:
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

    #region skillVariables
    Coroutine LookCursorCo;
    Coroutine attackCoroutine;
    Coroutine skillsCoroutine;
    #endregion

    void InputsReceive()
    {
        if (Input.GetKeyDown(KeyCode.Space)) // 회피
        {

        }

        if (Input.GetMouseButtonDown(0))  // 기본공격
        {
            if (curState != State.attack
                && curState != State.knockback
                && curState != State.knockdown
                && curState != State.stiff
                && curState != State.skill
                && curState != State.lastAttack)
            {
                if (curState == State.moving)
                {
                    CancelMoving();
                }
                if (curState == State.attack)
                {
                    CancelAttack();
                }
                attackCoroutine = StartCoroutine(StartAttack1(1.5f, .75f, 0.75f));     // 1단공격 시작
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
                if (curState == State.moving)
                {
                    CancelMoving();
                }
                if (curState == State.attack)
                {
                    CancelAttack();
                }
                Qskill();
            }
            if (Input.GetKeyDown(KeyCode.W))
            {
                if (curState == State.moving)
                {
                    CancelMoving();
                }
                if (curState == State.attack)
                {
                    CancelAttack();
                }
                Wskill();
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (curState == State.moving)
                {
                    CancelMoving();
                }
                if (curState == State.attack)
                {
                    CancelAttack();
                }
                Eskill();
            }
            if (Input.GetKeyDown(KeyCode.R))
            {
                if (curState == State.moving)
                {
                    CancelMoving();
                }
                if (curState == State.attack)
                {
                    CancelAttack();
                }
                Rskill();
            }
            if (Input.GetKeyDown(KeyCode.A))
            {
                if (curState == State.moving)
                {
                    CancelMoving();
                }
                if (curState == State.attack)
                {
                    CancelAttack();
                }
                Askill();
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                if (curState == State.moving)
                {
                    CancelMoving();
                }
                if (curState == State.attack)
                {
                    CancelAttack();
                }
                Sskill();
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                if (curState == State.moving)
                {
                    CancelMoving();
                }
                if (curState == State.attack)
                {
                    CancelAttack();
                }
                Dskill();
            }
            if (Input.GetKeyDown(KeyCode.F))
            {
                if (curState == State.moving)
                {
                    CancelMoving();
                }
                if (curState == State.attack)
                {
                    CancelAttack();
                }
                Fskill();
            }
        }
    }

    #region stateUpdate
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
    #endregion

    #region baseAttack
    void CancelAttack()
    {
        if (attackCoroutine != null)
            StopCoroutine(attackCoroutine);
        if (curState == State.attack || curState == State.lastAttack)
            curState = State.idle;
        anim.ResetTrigger("attack1");
        anim.ResetTrigger("attack2");
        anim.ResetTrigger("attack3");
    }
    IEnumerator StartAttack1(float duration, float lowerBound, float attackTiming)
    {
        LookAtCursorPos();
        // Debug.Log("attack1");
        curState = State.attack;
        anim.SetTrigger("attack");
        BaseAttack();
        //anim.Play("base1");
        float elapsed = 0;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            if (lowerBound < elapsed)
            {
                if (Input.GetMouseButtonDown(0))     // 2단 공격 시작
                {
                    attackCoroutine = StartCoroutine(StartAttack2(1.5f, .75f, 0.75f));
                    yield break;
                }
            }
            yield return null;

        }
        curState = State.idle;
    }

    IEnumerator StartAttack2(float duration, float lowerBound, float attackTiming)
    {
        // Debug.Log("attack2");
        LookAtCursorPos();
        curState = State.attack;
        anim.SetTrigger("attack2");
        BaseAttack();

        //anim.Play("base2");
        float elapsed = 0;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            if (lowerBound < elapsed)
            {
                if (Input.GetMouseButtonDown(0))     // 2단 공격 시작
                {
                    attackCoroutine = StartCoroutine(StartAttack3(1.5f, .75f, 0.75f));
                    yield break;
                }
            }
            yield return null;
        }
        curState = State.idle;
    }
    IEnumerator StartAttack3(float duration, float lowerBound, float attackTiming)
    {
        // Debug.Log("attack3");
        LookAtCursorPos();
        BaseAttack();

        curState = State.lastAttack;
        anim.SetTrigger("attack3");
        //anim.Play("base3");
        yield return new WaitForSeconds(duration);
        curState = State.idle;
        // 막타
    }
    #endregion
    #region spawnFx
    [SerializeField] GameObject attack_factory;
    [PunRPC]
    public void SpawnAttack()
    {
        Instantiate(attack_factory, transform.position + transform.forward * 1.5f + Vector3.up * 0.5f, Quaternion.LookRotation(transform.forward));
    }
    [SerializeField] GameObject q_factory;
    [PunRPC]
    public void SpawnQ()
    {
        Instantiate(q_factory, transform.position + transform.forward * 1.5f + Vector3.up * 0.5f, Quaternion.LookRotation(transform.forward));
    }
    [SerializeField] GameObject w_factory;
    [PunRPC]
    public void SpawnW()
    {
        Instantiate(w_factory, transform.position + transform.forward * 1.5f + Vector3.up * 0.5f, Quaternion.LookRotation(transform.forward));
    }
    [SerializeField] GameObject e_factory;
    [PunRPC]
    public void SpawnE()
    {
        Instantiate(e_factory, transform.position + transform.forward * 1.5f + Vector3.up * 0.5f, Quaternion.LookRotation(transform.forward));
    }
    [SerializeField] GameObject r_factory;
    [PunRPC]
    public void SpawnR()
    {
        GameObject newfx = Instantiate(r_factory, transform.position + transform.forward * 2f + Vector3.up * 0.5f, Quaternion.LookRotation(transform.forward));
    }
    [SerializeField] GameObject a_factory;
    [PunRPC]
    public void SpawnA()
    {
        GameObject newfx = Instantiate(a_factory, transform.position + transform.forward * 2f + Vector3.up * 0.5f, Quaternion.LookRotation(transform.forward));
    }
    [SerializeField] GameObject s_factory;
    [PunRPC]
    public void SpawnS()
    {
        GameObject newfx = Instantiate(s_factory, transform.position + transform.forward * 2f + Vector3.up * 0.5f, Quaternion.LookRotation(transform.forward));
    }
    [SerializeField] GameObject d_factory;
    [PunRPC]
    public void SpawnD()
    {
        GameObject newfx = Instantiate(d_factory, transform.position + transform.forward * 2f + Vector3.up * 0.5f, Quaternion.LookRotation(transform.forward));
    }
    [SerializeField] GameObject f_factory;
    [PunRPC]
    public void SpawnF()
    {
        GameObject newfx = Instantiate(f_factory, transform.position + transform.forward * 2f + Vector3.up * 0.5f, Quaternion.LookRotation(transform.forward));
    }
    #endregion

    #region skills

    void CancelSkillCo()
    {
        if (skillsCoroutine != null)
            StopCoroutine(skillsCoroutine);
        if (curState == State.skill)
            curState = State.idle;
        anim.ResetTrigger("q");
        anim.ResetTrigger("w");
        anim.ResetTrigger("e");
        anim.ResetTrigger("r");
        anim.ResetTrigger("a");
        anim.ResetTrigger("s");
        anim.ResetTrigger("d");
        anim.ResetTrigger("f");
    }
    void BaseAttack()
    {
        curState = State.skill;
        skillsCoroutine = StartCoroutine(BaseAttackCo());
    }
    IEnumerator BaseAttackCo()
    {
        float elapsed = 0;
        bool fxSpawned = false;

        while (elapsed < .7f)
        {
            elapsed += Time.deltaTime;
            if (fxSpawned == false)
                if (elapsed > 0.3f)
                {
                    photonView.RPC("SpawnAttack", RpcTarget.All);
                    fxSpawned = true;
                }
            yield return null;
        }
        curState = State.idle;
    }
    void Qskill()
    {
        curState = State.skill;
        skillsCoroutine = StartCoroutine(QskillCo());
    }
    IEnumerator QskillCo()
    {
        float elapsed = 0;
        bool fxSpawned = false;

        anim.SetTrigger("q");
        LookAtAnyCursorPos();
        while (elapsed < .7f)
        {
            elapsed += Time.deltaTime;
            if (fxSpawned == false)
                if (elapsed > 0.3f)
                {
                    photonView.RPC("SpawnQ", RpcTarget.All);
                    fxSpawned = true;
                }
            yield return null;
        }
        curState = State.idle;
    }
    void Wskill()
    {
        curState = State.skill;
        skillsCoroutine = StartCoroutine(WskillCo());
    }
    IEnumerator WskillCo()
    {
        float elapsed = 0;
        bool fxSpawned = false;

        anim.SetTrigger("w");
        LookAtAnyCursorPos();
        while (elapsed < .7f)
        {
            elapsed += Time.deltaTime;
            if (fxSpawned == false)
                if (elapsed > 0.3f)
                {
                    photonView.RPC("SpawnW", RpcTarget.All);
                    fxSpawned = true;
                }
            yield return null;
        }
        curState = State.idle;
    }
    void Eskill()
    {
        curState = State.skill;
        skillsCoroutine = StartCoroutine(EskillCo());
    }
    IEnumerator EskillCo()
    {
        float elapsed = 0;
        bool fxSpawned = false;

        anim.SetTrigger("e");
        LookAtAnyCursorPos();
        while (elapsed < .7f)
        {
            elapsed += Time.deltaTime;
            if (fxSpawned == false)
                if (elapsed > 0.3f)
                {
                    photonView.RPC("SpawnE", RpcTarget.All);
                    fxSpawned = true;
                }
            yield return null;
        }
        curState = State.idle;
    }
    void Rskill()
    {
        curState = State.skill;
        skillsCoroutine = StartCoroutine(RskillCo());
    }
    IEnumerator RskillCo()
    {
        float elapsed = 0;
        bool fxSpawned = false;

        anim.SetTrigger("r");
        LookAtAnyCursorPos();
        while (elapsed < .7f)
        {
            elapsed += Time.deltaTime;
            if (fxSpawned == false)
                if (elapsed > 0.3f)
                {
                    photonView.RPC("SpawnR", RpcTarget.All);
                    fxSpawned = true;
                }
            yield return null;
        }
        curState = State.idle;
    }
    void Askill()
    {
        curState = State.skill;
        skillsCoroutine = StartCoroutine(AskillCo());
    }
    IEnumerator AskillCo()
    {
        float elapsed = 0;
        bool fxSpawned = false;

        anim.SetTrigger("a");
        LookAtAnyCursorPos();
        while (elapsed < .7f)
        {
            elapsed += Time.deltaTime;
            if (fxSpawned == false)
                if (elapsed > 0.3f)
                {
                    photonView.RPC("SpawnA", RpcTarget.All);
                    fxSpawned = true;
                }
            yield return null;
        }
        curState = State.idle;
    }
    void Sskill()
    {
        curState = State.skill;
        skillsCoroutine = StartCoroutine(SskillCo());
    }
    IEnumerator SskillCo()
    {
        float elapsed = 0;
        bool fxSpawned = false;

        anim.SetTrigger("s");
        LookAtAnyCursorPos();
        while (elapsed < .7f)
        {
            elapsed += Time.deltaTime;
            if (fxSpawned == false)
                if (elapsed > 0.3f)
                {
                    photonView.RPC("SpawnS", RpcTarget.All);
                    fxSpawned = true;
                }
            yield return null;
        }
        curState = State.idle;
    }
    void Dskill()
    {
        curState = State.skill;
        skillsCoroutine = StartCoroutine(DskillCo());
    }
    IEnumerator DskillCo()
    {
        float elapsed = 0;
        bool fxSpawned = false;

        anim.SetTrigger("d");
        LookAtAnyCursorPos();
        while (elapsed < .7f)
        {
            elapsed += Time.deltaTime;
            if (fxSpawned == false)
                if (elapsed > 0.3f)
                {
                    photonView.RPC("SpawnD", RpcTarget.All);
                    fxSpawned = true;
                }
            yield return null;
        }
        curState = State.idle;
    }
    void Fskill()
    {
        curState = State.skill;
        skillsCoroutine = StartCoroutine(FskillCo());
    }
    IEnumerator FskillCo()
    {
        float elapsed = 0;
        bool fxSpawned = false;

        anim.SetTrigger("f");
        LookAtAnyCursorPos();
        while (elapsed < .7f)
        {
            elapsed += Time.deltaTime;
            if (fxSpawned == false)
                if (elapsed > 0.3f)
                {
                    photonView.RPC("SpawnF", RpcTarget.All);
                    fxSpawned = true;
                }
            yield return null;
        }
        curState = State.idle;
    }
    #endregion

    #region lookCursor
    void LookAtCursorPos()
    {
        // 공격 방향 보기
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;
        Debug.DrawRay(cam.transform.position, ray.direction * Mathf.Infinity, Color.red, 1f);
        int layerMask = 1 << LayerMask.NameToLayer("Player");
        if (Physics.Raycast(ray, out hitInfo, Mathf.Infinity, ~layerMask))
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
    void LookAtAnyCursorPos()
    {
        // 공격 방향 보기
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;
        Debug.DrawRay(cam.transform.position, ray.direction * Mathf.Infinity, Color.red, 1f);
        int layerMask = 1 << LayerMask.NameToLayer("Raycast");
        if (Physics.Raycast(ray, out hitInfo, Mathf.Infinity, layerMask))
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
        int layerMask = 1 << LayerMask.NameToLayer("Player");
        if (Physics.Raycast(ray, out hitInfo, Mathf.Infinity, ~layerMask))
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

    void CancelMoving()
    {
        anim.SetBool("moving", false);
        if (navAgent.hasPath)
            navAgent.ResetPath();
    }
    #endregion
}
