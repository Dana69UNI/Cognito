using FMOD.Studio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CountDown : MonoBehaviour
{
    private EventInstance ticktick;
    private int timeCountdown = 30;
    public GameObject enemigo;
    public Transform[] Spawns;
    int level = 1;
    public Transform jugador;
    private float distanciaInicial = 500f;
    void Start()
    {
        StartCoroutine(contador());
        ticktick = AudioManager.instance.CreateEventInstanceObj(FMODEvents.instance.TickSound, gameObject.transform);
    }

    private IEnumerator contador()
    {
        float tiempoTotal = timeCountdown;
        float tiempoTranscurrido = 0f;

        
        Vector3 direccionInicial = Random.onUnitSphere;
        direccionInicial.y = 0; 
        direccionInicial.Normalize();

        Vector3 posicionInicial = jugador.position + (direccionInicial * distanciaInicial);
        transform.position = posicionInicial;

        float ultimoSegundoRegistrado = tiempoTotal;

        while (tiempoTranscurrido < tiempoTotal)
        {
            tiempoTranscurrido += Time.deltaTime;

            
            float t = tiempoTranscurrido / tiempoTotal;

          
            if (jugador != null)
            {
                Vector3 puntoOrigenActualizado = jugador.position + (direccionInicial * distanciaInicial);
                transform.position = Vector3.Lerp(puntoOrigenActualizado, jugador.position, t);
            }

            float tiempoRestante = tiempoTotal - tiempoTranscurrido;
            if (Mathf.CeilToInt(tiempoRestante) < ultimoSegundoRegistrado)
            {
                ultimoSegundoRegistrado = Mathf.CeilToInt(tiempoRestante);
                Debug.Log("Tiempo restante: " + ultimoSegundoRegistrado);
                ticktick.start();
            }

            yield return null; 
        }
        if (jugador != null) transform.position = jugador.position;

        spawnEnemigo();
    }

    void spawnEnemigo()
    {
        Debug.Log("ahi va");
        //SONIDITO[]
        if (enemigo == null)
        {
            Instantiate(enemigo);
        }
    }

    private void Update()
    {
        ticktick.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(transform.position));
    }
}
