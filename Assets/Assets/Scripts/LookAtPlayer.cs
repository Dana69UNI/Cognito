using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtPlayer : MonoBehaviour
{
    [Header("Referencia")]
    [Tooltip("El jugador al que queremos mirar. Si se deja vacío, lo buscará por el Tag 'Player'.")]
    public Transform playerTransform;

    [Header("Ajustes de Rotación")]
    [Tooltip("Si está activo, el objeto también se inclinará hacia arriba/abajo si el jugador salta o está en otra altura.")]
    public bool lockYAxis = true;

    [Tooltip("Activa esto si estás usando un 'Plane' nativo de Unity, ya que su cara mira hacia arriba por defecto.")]
    public bool isUnityPlane = true;

    void Start()
    {
        
        if (playerTransform == null)
        {
            GameObject playerObj = GameObject.FindWithTag("Player");
            if (playerObj != null)
            {
                playerTransform = playerObj.transform;
            }
            else
            {
                Debug.LogError("No se encontró ningún objeto con el Tag 'Player' en la escena.", gameObject);
            }
        }
    }

   
    void LateUpdate()
    {
        if (playerTransform == null) return;

        
        Vector3 targetPosition = playerTransform.position;

        if (lockYAxis)
        {
           
            targetPosition.y = transform.position.y;
        }

        Vector3 direction = targetPosition - transform.position;

        
        if (direction.sqrMagnitude > 0.001f)
        {
           
            Quaternion targetRotation = Quaternion.LookRotation(direction);

           
            if (isUnityPlane)
            {
             
                targetRotation *= Quaternion.Euler(90, 0, 0);
            }

         
            transform.rotation = targetRotation;
        }
    }
}
