using FMOD.Studio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static OjoBehaviour;

public class ObjectNoise : MonoBehaviour
{
    public bool NoiseEmitted = false;

    [field: Header("SFX")]
    public bool MatCristal =false;
    public bool MatMadera=false;
    public bool MatLibroCarton = false;
    public bool MatPlastico = false;
    public bool Reloj = false;
    private EventInstance Sfx;
    private EventInstance tick;

    private void Start()
    {
        if(MatCristal) { Sfx = AudioManager.instance.CreateEventInstanceObj(FMODEvents.instance.SonidoBotellin, gameObject.transform); }
        if (MatMadera) { Sfx = AudioManager.instance.CreateEventInstanceObj(FMODEvents.instance.SonidoMadera, gameObject.transform); }
        if (MatLibroCarton) { Sfx = AudioManager.instance.CreateEventInstanceObj(FMODEvents.instance.SonidoLibro, gameObject.transform); }
        if (MatPlastico) { Sfx = AudioManager.instance.CreateEventInstanceObj(FMODEvents.instance.SonidoPlastico, gameObject.transform); }
        if(Reloj) { tick = AudioManager.instance.CreateEventInstanceObj(FMODEvents.instance.TickSound, gameObject.transform); }
        StartCoroutine(TickingSound());
    }

    private void Update()
    {
        tick.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(transform.position));
        Sfx.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(transform.position));
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.relativeVelocity.magnitude > 2)
        {
            StartCoroutine(CollisionNoise());
        }
    }

    private IEnumerator CollisionNoise()
    {
        Sfx.start();
        NoiseEmitted = true;
        yield return new WaitForSeconds(1f);
        NoiseEmitted = false;
    }

    private IEnumerator TickingSound()
    {
        while(true)
        {
            tick.start();
            yield return new WaitForSeconds(1f);
        }
       
    }
    
}
