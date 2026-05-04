using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static OjoBehaviour;

public class ObjectNoise : MonoBehaviour
{
    public bool NoiseEmitted = false;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.relativeVelocity.magnitude > 2)
        {
            StartCoroutine(CollisionNoise());
        }
    }

    private IEnumerator CollisionNoise()
    {
        NoiseEmitted = true;
        yield return new WaitForSeconds(1f);
        NoiseEmitted = false;
    }
}
