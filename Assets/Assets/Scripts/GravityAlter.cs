using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityAlter : MonoBehaviour
{
    private Rigidbody rb;
    private Vector3 gravity;

    private void Start()
    {
        gravity = Physics.gravity;
    }
    private void OnTriggerStay(Collider other)
    {
        if(other.attachedRigidbody != null)
        {
            rb = other.attachedRigidbody;
            rb.useGravity = false;
            rb.AddForce(-gravity, ForceMode.Acceleration);
        }
    }
}
