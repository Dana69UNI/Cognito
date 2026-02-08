using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class AlyxGrabInteractable : XRGrabInteractable
{
    [Header("Alyx Mechanics")]
    public float minVel = 2f;
    public float jumpAngle = 60f;
    public float nearThreshold = 1f;

    private NearFarInteractor nearFarInteractor;
    private Vector3 previousPos;
    private bool canJump = false;
    private Rigidbody rbInteractable;

    protected override void Awake()
    {
        base.Awake();
        rbInteractable = GetComponent<Rigidbody>();
    }

    private void Update()
    {
      
        if (canJump && isSelected && nearFarInteractor != null)
        {
            
            Vector3 handVel = (nearFarInteractor.transform.position - previousPos) / Time.deltaTime;
            previousPos = nearFarInteractor.transform.position;

            
            if (handVel.magnitude > minVel)
            {
                PerformAlyxJump();
            }
        }
    }

    private void PerformAlyxJump()
    {
        if (canJump && isSelected && firstInteractorSelecting is NearFarInteractor)

        {

            Vector3 vel = (nearFarInteractor.transform.position - previousPos) / Time.deltaTime;

            previousPos = nearFarInteractor.transform.position;



            if (vel.magnitude > minVel)

            {

                Drop();

                rbInteractable.velocity = ComputeVelocity(); 

                canJump = false;

            }

        }
    }

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        float distance = Vector3.Distance(args.interactorObject.transform.position, transform.position);

  
        if (distance > nearThreshold)
        {
            trackPosition = false;
            trackRotation = false;
            throwOnDetach = false;

            nearFarInteractor = args.interactorObject as NearFarInteractor;
            previousPos = nearFarInteractor.transform.position;
            canJump = true;
        }
        else
        {
            trackPosition = true;
            trackRotation = true;
            throwOnDetach = true;
            canJump = false;
        }

        base.OnSelectEntered(args);
    }

    public Vector3 ComputeVelocity()
    {
        Vector3 diff = nearFarInteractor.transform.position - transform.position;
        Vector3 diffXZ = new Vector3(diff.x, 0, diff.z);
        float diffXZLength = diffXZ.magnitude;
        float diffYLength = diff.y;

        float angleInRadian = jumpAngle * Mathf.Deg2Rad;

       
        float jumpSpeed = Mathf.Sqrt(-Physics.gravity.y * Mathf.Pow(diffXZLength, 2) /
            (2 * Mathf.Cos(angleInRadian) * Mathf.Cos(angleInRadian) *
            (diffXZLength * Mathf.Tan(angleInRadian) - diffYLength)));

        if (float.IsNaN(jumpSpeed)) jumpSpeed = 5f; // Salvaguarda si el cálculo falla

        Vector3 jumpVelocity = diffXZ.normalized * jumpSpeed * Mathf.Cos(angleInRadian)
            + Vector3.up * jumpSpeed * Mathf.Sin(angleInRadian);

        return jumpVelocity;
    }
}