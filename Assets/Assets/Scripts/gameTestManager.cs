using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameTestManager : MonoBehaviour
{
    public GameObject tutorialCube;
    public TextDialogueHandler tutorialDialogue;
    public GameObject player;

    private int indexText;
    private bool cubeHasSpawned;
    private bool cubeThrown;
    private bool playerMoved;
    private GameObject tutorialCubeGO;
    private Rigidbody tutorialCubeRB;
    

    void Start()
    {
        
        tutorialDialogue.CallDialogue(0, 3);
    }

   
    void FixedUpdate()
    {
        checkIndex();
        checkCubeThrow();
        checkPlayerMovement();
    }

    private void checkPlayerMovement()
    {
        if(cubeThrown&&!playerMoved)
        {
            if(player.transform.position.z != 0)
            {
                tutorialDialogue.CallDialogue(4, 0);
                playerMoved = true;
            }
        }
    }

    private void checkCubeThrow()
    {
        if(tutorialCubeGO != null && tutorialCubeRB != null)
        {
            if(tutorialCubeRB.velocity.magnitude >= 6f)
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
