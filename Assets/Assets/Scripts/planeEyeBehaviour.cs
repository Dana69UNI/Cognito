using FMOD.Studio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class planeEyeBehaviour : MonoBehaviour
{
    [Header("Configuración de Texturas")]
    [Tooltip("Asigna aquí las 3 o 4 texturas que quieras que use el ojo.")]
    public Material[] eyeMaterials;

    [Header("Configuración de Tiempo")]
    [Tooltip("Tiempo en segundos antes de que el objeto se destruya automáticamente si el jugador no lo mira.")]
    public float lifetime = 20f;

    private Renderer myRenderer;
    EventInstance spawnEye;
    EventInstance eyemort;

    void Start()
    {
        spawnEye = AudioManager.instance.CreateEventInstanceObj(FMODEvents.instance.eyeSpawn, gameObject.transform);
        eyemort = AudioManager.instance.CreateEventInstanceObj(FMODEvents.instance.eyemort, gameObject.transform);
        spawnEye.start();
        myRenderer = GetComponent<Renderer>();
        ApplyRandomTexture();
        Invoke("DestroyEye", lifetime);
    }

    private void ApplyRandomTexture()
    {
       
        if (myRenderer == null)
        {
            Debug.LogError("No se encontró un componente Renderer en " + gameObject.name, gameObject);
            return;
        }

        if (eyeMaterials == null || eyeMaterials.Length == 0)
        {
            Debug.LogWarning("No has asignado texturas en el array 'Eye Textures' de " + gameObject.name);
            return;
        }

       
        int randomIndex = Random.Range(0, eyeMaterials.Length);
        myRenderer.material = eyeMaterials[randomIndex];
    }

    public void DestroyEye()
    {
        CancelInvoke("DestroyEye");
        eyemort.start();
        Destroy(gameObject);
    }

    private void Update()
    {
        eyemort.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(transform.position));
        spawnEye.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(transform.position));
    }
}
