using FMOD.Studio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadioAudioEmitter : MonoBehaviour
{
    private EventInstance Musica;
   
    // Start is called before the first frame update
    void Start()
    {
        Musica = AudioManager.instance.CreateEventInstanceObj(FMODEvents.instance.MemoriesCannedInForgetfulness, gameObject.transform);
        
    }
    void Update()
    {
        Musica.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(transform.position));
    }

    public void StartMusic()
    {
        Musica.start();
    }
}
