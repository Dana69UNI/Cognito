using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lightFlickerTV : MonoBehaviour
{
    private Light Spotlight;

    private float maxIntensityOn = 1.3f;
    private float minIntensityOn = 0.8f;
    private float minIntensityOff = 0.09f;
    private float maxIntensityOff = 0.8f;

    private float maxOffTime = 0.3f;
    private float minOffTime = 0.05f;

    private float maxOnTime = 0.5f;
    private float minOnTime = 0.05f;

    private bool isLightOn = true;


    void Start()
    {
        Spotlight = GetComponent<Light>();
        StartCoroutine(FlickerRoutine());
    }

    IEnumerator FlickerRoutine()
    {
        while (true)
        {
            if (isLightOn)
            {
            
                Spotlight.intensity = Random.Range(minIntensityOff, maxIntensityOff);
                isLightOn = false;

                float randomOffTime = Random.Range(minOffTime, maxOffTime);
                yield return new WaitForSeconds(randomOffTime);
            }
            else
            {
               
                Spotlight.intensity = Random.Range(minIntensityOn, maxIntensityOn);
                isLightOn = true;

                float randomOnTime = Random.Range(minOnTime, maxOnTime);
                yield return new WaitForSeconds(randomOnTime);
            }
        }
    }
}
