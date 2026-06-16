using FMOD.Studio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptsTriggers : MonoBehaviour
{
    public bool _gravityScript = false;
    public bool _distractionScript = false;
    public bool decals=false;
    private bool decalsDone=false;
    public GameObject cuadreGravetat;
    public GameObject distraccio;
    public GameObject decal;
    EventInstance ominousEye;


    private void Start()
    {
        decal.gameObject.SetActive(false);
        ominousEye = AudioManager.instance.CreateInstance(FMODEvents.instance.OminousEyes);
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            if(_gravityScript)
            {
                Destroy(cuadreGravetat);
            }
            if(_distractionScript)
            {
                Destroy(distraccio);
            }
            if(decals)
            {
                StartCoroutine(decalsActivate());
                ominousEye.start();
            }
        }
    }

    private IEnumerator decalsActivate()
    {
        if(!decalsDone)
        {
            decal.gameObject.SetActive(true);
            yield return new WaitForSeconds(5f);
            decalsDone = true;
            decal.gameObject.SetActive(false);
            ominousEye.stop(STOP_MODE.ALLOWFADEOUT);
        }
        
    }
}
