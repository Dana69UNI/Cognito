using FMOD.Studio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class AnimateDecals : MonoBehaviour
{
    public Material ullsOberts;
    public Material ullsTancats;
   
    private DecalProjector decalProjector;

    private void awake()
    {
        decalProjector = GetComponent<DecalProjector>();
        StartCoroutine(parpadeoDecals());
        
        
    }

    IEnumerator parpadeoDecals()
    {
        while (true)
        {
           
            decalProjector.material = ullsTancats;
            yield return new WaitForSeconds(0.07f);
            decalProjector.material = ullsOberts;
            yield return new WaitForSeconds(3f);
            decalProjector.material = ullsTancats;
            yield return new WaitForSeconds(0.07f);
            decalProjector.material = ullsOberts;
            yield return new WaitForSeconds(3f);
            decalProjector.material = ullsTancats;
            yield return new WaitForSeconds(0.1f);
            decalProjector.material = ullsOberts;
            yield return new WaitForSeconds(0.3f);
            decalProjector.material = ullsTancats;
            yield return new WaitForSeconds(0.05f);
            decalProjector.material = ullsOberts;
            yield return new WaitForSeconds(5f);
        }
        
    }
}
