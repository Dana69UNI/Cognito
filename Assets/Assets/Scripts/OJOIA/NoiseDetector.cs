using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseDetector : MonoBehaviour
{
    public Vector3 AudioSource;
    public bool NoiseDetected;
    private void OnTriggerStay(Collider other)
    {
        
        if(other.GetComponent<ObjectNoise>()  != null)
        {
           NoiseDetected = other.GetComponent<ObjectNoise>().NoiseEmitted;
            Vector3 randomOffset = Random.insideUnitSphere * 2;
            randomOffset.y = 0;
            if (NoiseDetected) AudioSource = other.transform.position+randomOffset;
        }
    }
}
