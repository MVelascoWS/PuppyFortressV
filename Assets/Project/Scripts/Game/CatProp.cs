using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using WasdStudio.GameFlowManager;

[RequireComponent( typeof( NavMeshAgent ) )]
public class CatProp : BaseProp {
    protected NavMeshAgent agent;
    protected GameObject mainTarget;
    public GameObject currentTarget;

    public int scoreValue;
    public int baseHP;
    public int currentHP;

    public UnityEvent OnIdle = new UnityEvent();
    public UnityEvent OnWalk = new UnityEvent();
    public UnityEvent OnAttack = new UnityEvent();
    public UnityEvent OnHit = new UnityEvent();
    public UnityEvent OnDie = new UnityEvent();
    public UnityEvent OnTrap_1 = new UnityEvent();
    public UnityEvent OnTrap_2 = new UnityEvent();
    public UnityEvent OnTrap_3 = new UnityEvent();

    Animator anim;
    public bool trapped;   // Stop Bullet Kill
    public bool underTrap; // to avoid TriggerStay errors
    public bool walking;
    public bool attack;
    bool kill;
    float trapMinDistance = 1;

    protected override void Awake() {
        base.Awake();
        agent = GetComponent<NavMeshAgent>();
        mainTarget = GameObject.FindGameObjectWithTag( "MainTarget" );
        SetAnimationToEvents();
    }

    protected void FixedUpdate() {
        if ( !underTrap && currentTarget.CompareTag( "Trap" ) ) {
            if ( agent.isOnNavMesh && !agent.isStopped && agent.remainingDistance < trapMinDistance ) {
                underTrap = true;
                CheckTarget( currentTarget.name );
            }
        }
    }

    public override void OnSpawn() {
        currentHP = baseHP;
        currentTarget = mainTarget;
        trapped = false;
        underTrap = false;
        attack = false;
        kill = false;
        StartCoroutine( SetDestination( currentTarget.transform.position ) );
        StartCoroutine( Attack( 2f ) );
    }

    public override void OnHide() {
        PlayerManager.instance.AddScore( scoreValue );
        // Morir, fantasma?
        OnDie.Invoke();
    }

    protected override void OnCollisionEnter( Collision collision ) {
        if ( collision.gameObject.tag == "Bullet" ) // !trapped && 
        {
            if ( currentHP > 0 ) {
                GameObject temp = ObjectPool.instance.TakePoolObject( "HitParticle", true );
                temp.transform.position = collision.contacts[0].point;

                if ( --currentHP == 0 ) {
                    StartCoroutine( DieClock() );
                }
                else {
                    StartCoroutine( HurtClock() );
                }
            }
        }
    }

    private void Stop() {
        if ( !agent.isOnNavMesh ) {
            return;
        }
        if ( !agent.isStopped ) {
            agent.isStopped = true;
            OnIdle.Invoke();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "MainTarget" && currentTarget.tag == "MainTarget")
        {
            CheckTarget(currentTarget.name);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(!trapped && other.tag == "Trap")
        {
            trapped = true;
            currentTarget = other.gameObject;
            switch (other.name)
            {
                case "Trap_1":
                    trapMinDistance = 5;
                    break;
                case "Trap_2":
                    trapMinDistance = 1;
                    break;
                case "Trap_3":
                    trapMinDistance = 1;
                    break;
                default:
                    print("error TMD");
                    break;
            }
            if (!underTrap)
                StartCoroutine(SetDestination(currentTarget.transform.position));
        }
    }

    IEnumerator SetDestination(Vector3 target)
    {
        agent.enabled = false;        
        yield return null;          // Evitar el mal reposicionamiento debido al navmesh_agent
        agent.enabled = true;

        agent.destination = target;
        while (agent.pathPending) yield return null;

        walking = true;

        OnWalk.Invoke();
    }

    void CheckTarget(string name)
    {
        walking = false;
        switch (name)
        {
            case "Main":
                // Attack  
                attack = true;
                OnAttack.Invoke();
                break;

            case "Trap_1":
                OnTrap_1.Invoke();
                break;

            case "Trap_2":
                OnTrap_2.Invoke();
                break;

            case "Trap_3":
                OnTrap_3.Invoke();
                break;

            default:
                underTrap = true;
                print("error");
                break;
        }                
    }

    IEnumerator Attack(float repeat)
    {
        while (true)
        {
            yield return null;
            while (attack)
            {
                yield return new WaitForSeconds(repeat);
                PlayerManager.instance.HurtPlayer(0.05f);
            }
        }
    }

    public void SetKillTrap()
    {
        kill = true;
    }

    public void StartTrapClock(float time)
    {
        StartCoroutine(TrapClock(time));
    }

    public IEnumerator TrapClock(float delay)
    {
        yield return new WaitForSeconds(delay);
        if(GameFlowHelper.instance.Playing)
        {
            if (kill)
            {
                StartCoroutine(DieClock());
                while (true) yield return null;
            }          
            trapped = false;           
        }
        yield return null;
        yield return null;
        if (currentHP > 0)
        {
            if (trapped == false)
            {
                trapped = false;
                underTrap = false;
                currentTarget = mainTarget;
                StartCoroutine(SetDestination(currentTarget.transform.position));
            }
            else
            {
                StartCoroutine(TrapClock(delay));
                //CheckTarget(currentTarget.name);
            }
        }
        //else if(currentHP == 0)
        //{
        //    StartCoroutine(DieClock());
        //}
    }

    IEnumerator HurtClock()
    {
        agent.isStopped = true;
        OnHit.Invoke();
        yield return new WaitForSeconds(0.5f);//0.5
        agent.isStopped = false;
        if (walking)
            OnWalk.Invoke();
        else
            CheckTarget(currentTarget.name);
    }

    IEnumerator DieClock()
    {
        agent.isStopped = true;
        currentHP = -1; // ensure to avoid bullets
        OnDie.Invoke();
        yield return new WaitForSeconds(2.0f);
        agent.isStopped = false;
        Hide();
    }

    void SetAnimationToEvents()
    {
        anim = GetComponentInChildren<Animator>();
        //OnIdle, OnWalk, OnAttack, OnHit, OnDie, OnTrap_1, OnTrap_2, OnTrap_3
        OnIdle.AddListener(
            () => { anim.Play("Idle"); }
            );
        OnWalk.AddListener(
            () => { anim.Play("Walk"); }
            );
        OnAttack.AddListener(
            () => { anim.Play("Attack"); }
            );
        OnHit.AddListener(
            () => { anim.Play("Hit"); }
            );
        OnDie.AddListener(
            () => { anim.Play("Dead"); }
            );
        OnTrap_1.AddListener(
            () => { anim.Play("Idle"); }
            );
        OnTrap_2.AddListener(
            () => { anim.Play("Eat"); }
            );
        OnTrap_3.AddListener(
            () => { anim.Play("Eat"); }
            );
    }
}