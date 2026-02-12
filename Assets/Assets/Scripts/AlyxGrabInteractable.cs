using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class AlyxGrabInteractable : XRGrabInteractable
{
    [Header("Alyx Mechanics")]
    public float minVel = 2f;
    public float jumpAngle = 60f;
    public float nearThreshold = 0.5f;
    public float maxSelectVel = 4f;

    [Header("Custom Gravity Settings")]
    public Vector3 customUpDirection = Vector3.up;
    public float customGravityMagnitude = 9.81f;

    private NearFarInteractor nearFarInteractor;
    private Vector3 previousPos;
    private bool canJump = false;
    private Rigidbody rbInteractable;

    protected override void Awake()
    {
        base.Awake();
        rbInteractable = GetComponent<Rigidbody>();
    }

    /// <summary>
    /// Método para que tus triggers actualicen la dirección del salto.
    /// </summary>
    public void SetGravityContext(Vector3 upDir, float magnitude)
    {
        customUpDirection = upDir.normalized;
        customGravityMagnitude = magnitude;
    }

    public override bool IsSelectableBy(IXRSelectInteractor interactor)
    {
        bool canSelect = base.IsSelectableBy(interactor);
        if (canSelect)
        {
            float distance = Vector3.Distance(interactor.transform.position, transform.position);
            if (distance > nearThreshold && rbInteractable.velocity.magnitude > maxSelectVel)
            {
                return false;
            }
        }
        return canSelect;
    }

    private void Update()
    {
        if (canJump && isSelected && nearFarInteractor != null)
        {
            Vector3 handPos = nearFarInteractor.transform.position;
            Vector3 handVel = (handPos - previousPos) / Time.deltaTime;
            previousPos = handPos;

            if (handVel.magnitude > minVel)
            {
                ExecuteLaunch();
            }
        }
    }

    private void ExecuteLaunch()
    {
        Vector3 launchVelocity = ComputeVelocity();
        Drop();

        rbInteractable.velocity = Vector3.zero;
        rbInteractable.angularVelocity = Vector3.zero;
        rbInteractable.AddForce(launchVelocity, ForceMode.VelocityChange);

        canJump = false;
    }

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        rbInteractable.WakeUp();
        float distance = Vector3.Distance(args.interactorObject.transform.position, transform.position);

        if (distance > nearThreshold)
        {
            trackPosition = false;
            trackRotation = false;
            throwOnDetach = false;

            nearFarInteractor = args.interactorObject as NearFarInteractor;
            previousPos = nearFarInteractor.transform.position;
            canJump = true;

            base.OnSelectEntered(args);
            rbInteractable.isKinematic = false;
        }
        else
        {
            trackPosition = true;
            trackRotation = true;
            throwOnDetach = true;
            canJump = false;
            base.OnSelectEntered(args);
        }
    }

    public Vector3 ComputeVelocity()
    {
        if (nearFarInteractor == null) return Vector3.up * 5f;

        Vector3 target = nearFarInteractor.transform.position;
        Vector3 origin = transform.position;
        Vector3 diff = target - origin;

        Vector3 currentUp;
        float g;

        if (rbInteractable.useGravity)
        {
            currentUp = Vector3.up;
            g = Physics.gravity.magnitude;
        }
        else
        {
            currentUp = (customUpDirection.sqrMagnitude < 0.01f) ? Vector3.up : customUpDirection.normalized;
            g = customGravityMagnitude;
        }

        float y = Vector3.Dot(diff, currentUp);

        Vector3 diffPlanar = diff - (currentUp * y);
        float x = diffPlanar.magnitude;

        if (x < 0.1f) return diff.normalized * 5f;

        float angleRad = jumpAngle * Mathf.Deg2Rad;

        if (g < 0.01f) g = 9.81f;

        float cosTheta = Mathf.Cos(angleRad);
        float speedSq = (g * x * x) / (2 * cosTheta * cosTheta * (x * Mathf.Tan(angleRad) - y));

        if (speedSq <= 0 || float.IsNaN(speedSq) || float.IsInfinity(speedSq))
        {
            return (diff.normalized + currentUp * 0.5f).normalized * 6f;
        }

        float speed = Mathf.Sqrt(speedSq);
        Vector3 velocity = diffPlanar.normalized * speed * Mathf.Cos(angleRad) + currentUp * speed * Mathf.Sin(angleRad);
        return Vector3.ClampMagnitude(velocity, 25f);
    }
}