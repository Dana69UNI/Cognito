using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityAlter : MonoBehaviour
{
    private Vector3 gravityVector;
    private Vector3 normalObjeto;
    private float gravityMag;

    public float groundedForceMultiplier = 0.2f;
    public float velocityThreshold = 0.05f;

    private void Start()
    {
        normalObjeto = this.gameObject.GetComponentInParent<Transform>().up;
        gravityMag = Physics.gravity.magnitude;
        gravityVector = -normalObjeto * gravityMag;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.attachedRigidbody != null)
        {
            Rigidbody rb = other.attachedRigidbody;
            rb.useGravity = false;

            if (rb.velocity.magnitude < velocityThreshold)
            {
                rb.AddForce(gravityVector * groundedForceMultiplier, ForceMode.Acceleration);
            }
            else
            {
                rb.AddForce(gravityVector, ForceMode.Acceleration);
            }

            if (other.TryGetComponent<AlyxGrabInteractable>(out var gravityInteractable))
            {
                gravityInteractable.customGravityMagnitude = gravityMag;
                gravityInteractable.customUpDirection = normalObjeto;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.attachedRigidbody != null)
        {
            Rigidbody rb = other.attachedRigidbody;
            rb.useGravity = true;

           
            if (other.TryGetComponent<AlyxGrabInteractable>(out var alyx))
            {
                alyx.customUpDirection = Vector3.up;
                alyx.customGravityMagnitude = Physics.gravity.magnitude;
            }
        }
    }
}