using FMOD.Studio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadioAudioEmitter : MonoBehaviour
{
    private EventInstance Musica;
    public bool final;
    private EventInstance Records;
    // Start is called before the first frame update
    void Start()
    {
        Musica = AudioManager.instance.CreateEventInstanceObj(FMODEvents.instance.MemoriesCannedInForgetfulness, gameObject.transform);
        Records = AudioManager.instance.CreateEventInstanceObj(FMODEvents.instance.fin, gameObject.transform);

    }
    void Update()
    {
        Musica.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(transform.position));
        Records.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(transform.position));
    }

    public void StartMusic()
    {
        Musica.start();
    }

    public void StartRecords()
    {
        Records.start();
    }
}
