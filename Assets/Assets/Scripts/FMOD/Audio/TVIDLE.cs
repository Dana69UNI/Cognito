using FMOD.Studio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TVIDLE : MonoBehaviour
{

    EventInstance _tvIdle;
    // Start is called before the first frame update
    void Start()
    {
      
        _tvIdle= AudioManager.instance.CreateEventInstanceObj(FMODEvents.instance.Tv, gameObject.transform);
        _tvIdle.start();

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
