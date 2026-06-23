using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ToggleOff : MonoBehaviour
{
    [Header("Configuración de Luces")]
    public Light luzDireccionalPrincipal;

    [Header("Configuración de Tiempo")]
    public float tiempoTotal = 33f; // Segundos exactos
    public string nombreSiguienteEscena;

    // Variables para almacenar el estado inicial
    private float intensidadOriginalLuz;
    private Color colorAmbienteOriginal;
    private Color colorNieblaOriginal;
    private float densidadNieblaOriginal;
    private bool nieblaActivadaAlInicio;

    void Start()
    {

        if (luzDireccionalPrincipal != null)
        {
            intensidadOriginalLuz = luzDireccionalPrincipal.intensity;
        }
        colorAmbienteOriginal = RenderSettings.ambientLight;

        
        nieblaActivadaAlInicio = RenderSettings.fog;
        colorNieblaOriginal = RenderSettings.fogColor;
        densidadNieblaOriginal = RenderSettings.fogDensity;

    }

    public void empezarFinal()
    {
        StartCoroutine(SecuenciaOscurecerYCambiar());
    }
    IEnumerator SecuenciaOscurecerYCambiar()
    {
        float tiempoTranscurrido = 0f;

        while (tiempoTranscurrido < tiempoTotal)
        {
            tiempoTranscurrido += Time.deltaTime;
            float porcentaje = tiempoTranscurrido / tiempoTotal;


            if (luzDireccionalPrincipal != null)
            {
                luzDireccionalPrincipal.intensity = Mathf.Lerp(intensidadOriginalLuz, 0f, porcentaje);
            }
            RenderSettings.ambientLight = Color.Lerp(colorAmbienteOriginal, Color.black, porcentaje);

            if (nieblaActivadaAlInicio)
            {
                
                RenderSettings.fogColor = Color.Lerp(colorNieblaOriginal, Color.black, porcentaje);
                RenderSettings.fogDensity = Mathf.Lerp(densidadNieblaOriginal, 0.5f, porcentaje);

            }

            yield return null;
        }

        if (luzDireccionalPrincipal != null) luzDireccionalPrincipal.intensity = 0f;
        RenderSettings.ambientLight = Color.black;

        if (nieblaActivadaAlInicio)
        {
            RenderSettings.fogColor = Color.black;
        }

        SceneManager.LoadScene(2);
    }
}
