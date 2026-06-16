using FMOD.Studio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeFog : MonoBehaviour
{
    public RadioAudioEmitter emitter;
    private EventInstance ambient;

    private void Start()
    {
        ambient = AudioManager.instance.CreateInstance(FMODEvents.instance.Ambience);
        ambient.start();
    }
    private void OnTriggerEnter(Collider colider)
    {
        if(colider.gameObject.CompareTag("Player"))
        {
            
            RenderSettings.fogColor = new Color32(160, 170, 163, 100);
            //RenderSettings.fogStartDistance = 8;
            //RenderSettings.fogEndDistance = 31;
            ambient.stop(STOP_MODE.ALLOWFADEOUT);
            emitter.StartMusic();
        }
    }
}
