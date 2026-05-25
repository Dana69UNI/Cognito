using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction : MonoBehaviour
{
    [SerializeField] private Transform[] interactors;

    private Vector4[] interactorPositions = new Vector4[4]; 
    private int interactorCount;

    void Update()
    {
        if (interactors == null || interactors.Length == 0) return;

        interactorCount = Mathf.Min(interactors.Length, 4);

        for (int i = 0; i < interactorCount; i++)
        {
            if (interactors[i] != null)
            {
               
                interactorPositions[i] = new Vector4(
                    interactors[i].position.x,
                    interactors[i].position.y,
                    interactors[i].position.z,
                    1.0f
                );
            }
        }
        Shader.SetGlobalVectorArray("_VRInteractorPositions", interactorPositions);
        Shader.SetGlobalInt("_VRInteractorCount", interactorCount);
    }
}