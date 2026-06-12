using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeFog : MonoBehaviour
{
    public RadioAudioEmitter emitter;
    private void OnTriggerEnter(Collider colider)
    {
        if(colider.gameObject.CompareTag("Player"))
        {
            
            RenderSettings.fogColor = new Color32(160, 170, 163, 100);
            //RenderSettings.fogStartDistance = 8;
            //RenderSettings.fogEndDistance = 31;
            emitter.StartMusic();
        }
    }
}
