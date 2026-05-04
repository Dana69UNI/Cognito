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
        gotoPatrolPoint();
    }

   
    void Update()
    {
        if (aiFOV.canSeePlayer == true) currentState = ai_states.Attack;
        if (aiFOV.sawThrow == true) currentState = ai_states.InvestigateMovement;
        if(noise.NoiseDetected == true) currentState = ai_states.InvestigateNoise;

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
        _agent.isStopped = true;
        yield return new WaitForSeconds(2f);
        _agent.isStopped = false;
        _agent.SetDestination(aiFOV.playerRef.transform.position);
        //Debug.Log("CAGASTE");
    }

    private IEnumerator investigateMovementRoutine()
    {
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
        _agent.isStopped = true;
        yield return new WaitForSeconds(2f);
        _agent.isStopped = false;
        _agent.SetDestination(noise.AudioSource);
        yield return new WaitForSeconds(5f);
        noise.NoiseDetected = false;
        currentState = ai_states.Patrol;
    }

}
