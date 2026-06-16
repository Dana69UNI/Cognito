using FMOD.Studio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class OjoBehaviour : MonoBehaviour
{
    Vector2 myPos;
    [Header ("Refs")]
    [SerializeField] Transform[] PatrolPoints;

    [Header("Settings")]
    [SerializeField] float PatrolWait = 3f;

    private FieldOfView aiFOV;
    private NoiseDetector noise;
    private NavMeshAgent _agent;
    private int currentPatrolPoint;
    private bool isWaiting;
    private float stopDistance = 1;
    private int sentido = 1;
    public ai_states currentState;
    public GameObject ullTancat;
    public GameObject ullObert;
    public playerDeath _playerDies;
    EventInstance _ullIdle;
    EventInstance _ullAttack;
    EventInstance _ullAvis;
    private bool ullAvisCD;
    private bool ullAttackCD;

    public enum ai_states
    {
        Patrol,
        InvestigateNoise,
        InvestigateMovement,
        Attack
    }

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        aiFOV = GetComponent<FieldOfView>();
        noise = GetComponent<NoiseDetector>();
       

    }
    void Start()
    {
        _ullIdle = AudioManager.instance.CreateEventInstanceObj(FMODEvents.instance.UllIdle, gameObject.transform);
        _ullAvis = AudioManager.instance.CreateEventInstanceObj(FMODEvents.instance.UllAlerta, gameObject.transform);
        _ullAttack = AudioManager.instance.CreateEventInstanceObj(FMODEvents.instance.UllAttack, gameObject.transform);
        
        ullObert.SetActive(false);
        gotoPatrolPoint();
        _ullIdle.start();
    }

   
    void Update()
    {
        if (aiFOV.canSeePlayer == true) currentState = ai_states.Attack;
        if (aiFOV.sawThrow == true) currentState = ai_states.InvestigateMovement;
        if(noise.NoiseDetected == true) currentState = ai_states.InvestigateNoise;
        _ullIdle.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(transform.position));
        _ullAttack.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(transform.position));
        _ullAvis.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(transform.position));
        float distanciaAlJugador = Vector3.Distance(transform.position, aiFOV.playerRef.transform.position);
        if (aiFOV.canSeePlayer && distanciaAlJugador < 1.5f)
        {
            _playerDies.dead();
        }

        switch (currentState)
        {
            case ai_states.Patrol:
                patrolState();
                break;
            case ai_states.InvestigateNoise:
                investigateNoiseState();
                break;
            case ai_states.InvestigateMovement:
                investigateMovementState();
                break;
            case ai_states.Attack: 
                AttackState();
                break;
            
        }
    }

    void gotoPatrolPoint()
    {
        if (PatrolPoints.Length == 0) return;
        if (currentPatrolPoint == 0) sentido = 1;
        if (currentPatrolPoint >= PatrolPoints.Length - 1)
        {
            sentido *= -1;
        }

        

        _agent.SetDestination(PatrolPoints[currentPatrolPoint].position);

        currentPatrolPoint += sentido;

    }

    private IEnumerator patrolWait()
    {
        isWaiting = true;
        _agent.isStopped = true;

        yield return new WaitForSeconds(PatrolWait);
        
        _agent.isStopped = false;
        gotoPatrolPoint();
        isWaiting = false;

    }
    void patrolState()
    {
        if (isWaiting) return;
        if (!_agent.pathPending && _agent.remainingDistance <= stopDistance)
        {
            StartCoroutine(patrolWait());
        }
    }

    void investigateNoiseState()
    {
        StartCoroutine(investigateNoiseRoutine());
    }

    void investigateMovementState()
    {
        StartCoroutine(investigateMovementRoutine());
    }
    void AttackState()
    {
       
        
        StartCoroutine(attackRoutine());
    }

    private IEnumerator attackRoutine()
    {
        StartCoroutine(ullAttackSound());
        _agent.isStopped = true;
        yield return new WaitForSeconds(1f);
        _agent.isStopped = false;
        _agent.SetDestination(aiFOV.playerRef.transform.position);
        ullTancat.SetActive(false);
        ullObert.SetActive(true);
       

        //Debug.Log("CAGASTE");
    }

    private IEnumerator investigateMovementRoutine()
    {
        StartCoroutine(ullAvisSound());
        _agent.isStopped = true;
        yield return new WaitForSeconds(2f);
        _agent.isStopped = false;
        _agent.SetDestination(aiFOV.sourceGuess);
        Debug.Log("ESTO QUE WACHIN");
        yield return new WaitForSeconds(5f);
        aiFOV.sawThrow = false;

        currentState = ai_states.Patrol;
    }

    private IEnumerator investigateNoiseRoutine()
    {
        StartCoroutine(ullAvisSound());
        _agent.isStopped = true;
        yield return new WaitForSeconds(2f);
        _agent.isStopped = false;
        _agent.SetDestination(noise.AudioSource);
        yield return new WaitForSeconds(5f);
        noise.NoiseDetected = false;
        
        currentState = ai_states.Patrol;
    }

    private IEnumerator ullAvisSound()
    {
        if (!ullAvisCD)
        {
            _ullAvis.start();
            ullAvisCD = true;

        }
        else
        {
            yield return new WaitForSeconds(8f);
            ullAvisCD = false;
        }
        
    }

    private IEnumerator ullAttackSound()
    {
        if (!ullAttackCD)
        {
            _ullAttack.start();
            ullAttackCD = true;

        }
        else
        {
            yield return new WaitForSeconds(10f);
            ullAttackCD = false;
        }

    }

}
