using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class FMODEvents : MonoBehaviour
{

    [field: Header("Music")]
    [field: SerializeField] public EventReference music{ get; private set; }

    [field: Header("MemoriesCannedInForgetfulness")]
    [field: SerializeField] public EventReference MemoriesCannedInForgetfulness { get; private set; }

    [field: Header("TheInsideOfATroubledMind")]
    [field: SerializeField] public EventReference TheInsideOfATroubledMind { get; private set; }


    [field: Header("TickSound")]
    [field: SerializeField] public EventReference TickSound { get; private set; }

    [field: Header("Ambience")]
    [field: SerializeField] public EventReference Ambience { get; private set; }

    [field: Header("Tv")]
    [field: SerializeField] public EventReference Tv { get; private set; }

    [field: Header("SonidoMadera")]
    [field: SerializeField] public EventReference SonidoMadera { get; private set; }

    [field: Header("SonidoPlastico")]
    [field: SerializeField] public EventReference SonidoPlastico { get; private set; }

    [field: Header("SonidoBotellin")]
    [field: SerializeField] public EventReference SonidoBotellin { get; private set; }

    [field: Header("SonidoLibro")]
    [field: SerializeField] public EventReference SonidoLibro { get; private set; }

    [field: Header("UllAlerta")]
    [field: SerializeField] public EventReference UllAlerta { get; private set; }

    [field: Header("UllIdle")]
    [field: SerializeField] public EventReference UllIdle { get; private set; }

    [field: Header("UllAttack")]
    [field: SerializeField] public EventReference UllAttack { get; private set; }

    [field: Header("ominousEyes")]
    [field: SerializeField] public EventReference OminousEyes { get; private set; }

    [field: Header("eyeSpawn")]
    [field: SerializeField] public EventReference eyeSpawn { get; private set; }

    [field: Header("eyemort")]
    [field: SerializeField] public EventReference eyemort { get; private set; }

    [field: Header("fin")]
    [field: SerializeField] public EventReference fin { get; private set; }

    public static FMODEvents instance { get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Found more than one FMOD Events instance in the scene.");
        }
        instance = this;
    }
}