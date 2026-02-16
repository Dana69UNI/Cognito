using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class gameTestManager : MonoBehaviour
{
    public GameObject tutorialCube;
    public TextDialogueHandler tutorialDialogue;
    public GameObject player;
    public Transform TeleDialogue; 
    public Transform targetPosition;
    public Transform targetPosition2;
    public Transform targetPosition3;
    public checkPoint checkpointFinal;
    

    private int indexText;
    private bool cubeHasSpawned;
    private bool cubeThrown;
    private bool playerMoved;
    private bool ladderClimbed;
    private GameObject tutorialCubeGO;
    private Rigidbody tutorialCubeRB;
    private float lerpDuration = 5f;


    void Start()
    {
        
        tutorialDialogue.CallDialogue(0, 3);
        StartCoroutine(TiempoLectura());
    }

    IEnumerator TiempoLectura()
    {
        for (int i = 0; i <=1; i++)
        {
            if(i == 0)
            {
                yield return new WaitForSeconds(15f);
                tutorialDialogue.NextLine();
            }
            else
            {
                yield return new WaitForSeconds(6f);
                tutorialDialogue.NextLine();
            }
               
        }
       
    }

    void FixedUpdate()
    {
        checkIndex();
        checkCubeThrow();
        checkPlayerMovement();
        checkLadderClimb();
        checkCheckPoint();
    }

    private void checkCheckPoint()
    {
        if(checkpointFinal.playerArrived)
        {
            SceneManager.LoadScene(1);
        }
    }

    private void checkLadderClimb()
    {
        if (playerMoved&&!ladderClimbed)
        {
            if (player.transform.position.y >= 3.3f)
            {
                tutorialDialogue.CallDialogue(5, 0);
                ladderClimbed = true;
                StartCoroutine(MoveDialogueLerp(targetPosition2));
            }
        }
    }
    IEnumerator MoveDialogueLerp(Transform destination)
    {
        float timeElapsed = 0;
        Vector3 startPosition = TeleDialogue.position;

        while (timeElapsed < lerpDuration)
        {
            
            TeleDialogue.position = Vector3.Lerp(startPosition, destination.position, timeElapsed / lerpDuration);
            timeElapsed += Time.deltaTime;
            yield return null; 
        }

        
        TeleDialogue.position = destination.position;
        if(destination == targetPosition2)
        {
            StartCoroutine(WaitFor5s());
        }
        
    }

    IEnumerator WaitFor5s()
    {
        yield return new WaitForSeconds(5);
        StartCoroutine(Final(targetPosition3));
    }
    IEnumerator Final(Transform destination)
    {
        float timeElapsed = 0;
        Vector3 startPosition = TeleDialogue.position;

        while (timeElapsed < lerpDuration)
        {

            TeleDialogue.position = Vector3.Lerp(startPosition, destination.position, timeElapsed / lerpDuration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }


        TeleDialogue.position = destination.position;
    }
    private void checkPlayerMovement()
    {
        if(cubeThrown&&!playerMoved)
        {
            if(player.transform.position.z != 0)
            {
                tutorialDialogue.CallDialogue(4, 0);
                playerMoved = true;
                StartCoroutine(MoveDialogueLerp(targetPosition));
            }
        }
    }

    private void checkCubeThrow()
    {
        if(tutorialCubeGO != null && tutorialCubeRB != null && !cubeThrown)
        {
            if(tutorialCubeRB.velocity.magnitude >= 4f)
            {
                tutorialDialogue.CallDialogue(3, 0);
                cubeThrown = true;
            }
        }
    }

    private void checkIndex()
    {
        indexText = tutorialDialogue.index;
        if (indexText == 2)
        {
            spawnCube();
        }
    }

    void spawnCube()
    {
        if(!cubeHasSpawned)
        {

            tutorialCubeGO=Instantiate(tutorialCube, new Vector3(0, 1, 3), Quaternion.identity);
            tutorialCubeRB = tutorialCubeGO.GetComponent<Rigidbody>();
            cubeHasSpawned = true;
            
        }
        
    }
}
