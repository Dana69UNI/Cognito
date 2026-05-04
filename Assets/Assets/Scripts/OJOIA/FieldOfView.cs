using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FieldOfView : MonoBehaviour
{
    public float radius;
    [Range (0, 360)]
    public float angle;

    public GameObject playerRef;
    public Vector3 throwDetected;

    public LayerMask targetMask;
    public LayerMask throwMask;
    public LayerMask obstacleMask;

    public bool canSeePlayer;
    public bool sawThrow;
    public Vector3 sourceGuess;
    public float sensitivity = 1.5f; 
    public float minBacktrack = 4f; 
    public float maxBacktrack = 15f;
    public float detectionTimer = 0f;
    public float timeToDetect = 1.0f;
    

    private void Start()
    {
        playerRef = GameObject.FindGameObjectWithTag("Player");
        StartCoroutine(FOVRoutine());
    }

    private void Update()
    {
        //if (sawThrow == true) Debug.Log("epa");
    }
    private IEnumerator FOVRoutine()
    {
        float delay = 0.2f;
        WaitForSeconds wait = new WaitForSeconds(delay);

        while (true)
        {
            yield return wait;
            FieldOfViewEnemyCheck(delay);
            FieldOfViewThrowCheck();
        }
    }

    private void FieldOfViewEnemyCheck(float delay)
    {
        Collider[] rangeChecks = Physics.OverlapSphere(transform.position, radius, targetMask);
        bool playerInSightThisFrame = false;

        if (rangeChecks.Length != 0)
        {
            foreach (Collider targetCollider in rangeChecks)
            {
                Transform target = rangeChecks[0].transform;
                Vector3 directionToTarget = (target.position - transform.position).normalized;

                if (Vector3.Angle(transform.forward, directionToTarget) < angle / 2)
                {
                    float distanceToTarget = Vector3.Distance(transform.position, target.position);

                    if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstacleMask))
                    {
                        playerInSightThisFrame = true;

                    }
                    else
                    {
                        canSeePlayer = false;
                    }
                }
                else
                {
                    canSeePlayer = false;
                }
            }
        }
        else if (canSeePlayer)
            canSeePlayer = false;

        if (playerInSightThisFrame)
        {
            detectionTimer += delay;
        }
        else
        {
            detectionTimer -= delay * 1.5f;
        }

        detectionTimer = Mathf.Clamp(detectionTimer, 0, timeToDetect);

        canSeePlayer = (detectionTimer >= timeToDetect);
    }

    public void FieldOfViewThrowCheck()
    {
        if(sawThrow) return;
        Collider[] rangeChecks = Physics.OverlapSphere(transform.position, radius, throwMask);

        foreach (Collider targetCollider in rangeChecks)
        {
            Rigidbody rb = targetCollider.attachedRigidbody;
            if (rb == null || rb.velocity.magnitude < 3f) continue;

            Transform target = targetCollider.transform;
            Vector3 directionToTarget = (target.position - transform.position).normalized;

            if (Vector3.Angle(transform.forward, directionToTarget) < angle / 2)
            {
                if (!Physics.Raycast(transform.position, directionToTarget, Vector3.Distance(transform.position, target.position), obstacleMask))
                {
                    
                    float speed = rb.velocity.magnitude;
                    Vector3 throwDir = rb.velocity.normalized;

                  
                    throwDir.y = 0;
                    throwDir.Normalize();

                    float dynamicDistance = speed * sensitivity;
                    dynamicDistance = Mathf.Clamp(dynamicDistance, minBacktrack, maxBacktrack);

                    Vector3 rawSourceGuess = target.position + (throwDir * dynamicDistance);
                    rawSourceGuess.y = transform.position.y;

                    NavMeshHit navHit;
                    if (NavMesh.SamplePosition(rawSourceGuess, out navHit, 5.0f, NavMesh.AllAreas))
                    {
                        sourceGuess = navHit.position;
                        sawThrow = true;
                        break;
                    }
                }
            }
        }
    }

    //void cleanGuessBuff()
    //{
    //    sourceGuess = new Vector3(null, null, null) ;
    //}
}
