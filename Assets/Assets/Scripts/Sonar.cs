using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit.Inputs.Readers;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Inputs.Haptics;

public class Sonar : MonoBehaviour
{
    public InputActionReference botonPrimario;

    [Header("Settings")]
    public float sonarRange = 20f;     
    public float blipDuration = 5f;   
    public LayerMask enemyLayer;

    [Header("UI References")]
    public RectTransform sonarCanvas; 
    public GameObject blipPrefab;
    private bool CoolDown =false;

    float hapticIntensity = 1f;
    float hapticDuration = 0.2f;

    private HapticImpulsePlayer hapticResponse;

    private void Start()
    {
        botonPrimario.action.started += ButtonWasPressed;
        botonPrimario.action.canceled += ButtonWasReleased;
        hapticResponse = GetComponent<HapticImpulsePlayer>();

    }

    void ButtonWasPressed(InputAction.CallbackContext context)
    {
        //sonido o whatever
        if (!CoolDown)
        {
            hapticResponse.SendHapticImpulse(hapticIntensity, hapticDuration);
        }
        else { hapticResponse.SendHapticImpulse(hapticIntensity/4, hapticDuration*2); }
            Debug.Log("Pulsó");
    }

    void ButtonWasReleased(InputAction.CallbackContext context)
    {
        if (!CoolDown)
        {
            TriggerPing();
        }
    }

    public void TriggerPing()
    {
        StartCoroutine(radarCooldown());
        Collider[] enemies = Physics.OverlapSphere(transform.position, sonarRange, enemyLayer);
        
        if(enemies.Length > 0)
        {
            foreach (Collider enemyCollider in enemies)
            {
                CreateBlip(enemyCollider.transform);
            }
        }
      
    }

    private void CreateBlip(Transform target)
    {
        float distance = Vector3.Distance(transform.position, target.position);
        if (distance > sonarRange) return;

        float linearRatio = distance / sonarRange;

        float curvedRatio = Mathf.Sqrt(linearRatio);

        
        Vector3 flatTargetPos = new Vector3(target.position.x, transform.position.y, target.position.z);
        Vector3 localPos = sonarCanvas.InverseTransformPoint(flatTargetPos);

      
        Vector2 direction = new Vector2(localPos.x, -localPos.z).normalized;

    
        float radius = sonarCanvas.rect.width / 2f;

       
        Vector2 uiPosition = direction * (curvedRatio * radius);

       
        GameObject blip = Instantiate(blipPrefab, sonarCanvas);
        RectTransform blipRect = blip.GetComponent<RectTransform>();

        blipRect.anchoredPosition = uiPosition;
        blipRect.localRotation = Quaternion.identity;

        
        blipRect.localPosition = new Vector3(blipRect.localPosition.x, blipRect.localPosition.y, -0.01f);

        Destroy(blip, blipDuration);
    }
    private void changeCooldown()
    {
        if (CoolDown) { CoolDown = false; }
        else { CoolDown = true; }
    }
    private IEnumerator radarCooldown()
    {
        changeCooldown();
        yield return new WaitForSeconds(blipDuration*2);
        changeCooldown();
        hapticResponse.SendHapticImpulse(hapticIntensity/2, hapticDuration);

    }

}

