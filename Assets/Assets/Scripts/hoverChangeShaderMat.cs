using UnityEngine;

public class hoverChangeShaderMat : MonoBehaviour
{
    public Shader hoverShader;
    private Shader originalShader;
    private Renderer rend;

    void Awake()
    {
        rend = GetComponent<Renderer>();
        if (rend != null)
        {
            
            originalShader = rend.material.shader;

          
        }

        if (hoverShader == null)
        {
            Debug.LogError("No se encontró el shader 'Custom/HoverURP'. Asegúrate de que el nombre en el archivo .shader sea correcto.");
        }
    }

    
    public void changeShader()
    {
        if (rend == null || hoverShader == null) return;

        rend.material.shader = hoverShader;

    }

    // Llamar desde XRIT: Hover Exited
    public void returnShader()
    {
        if (rend == null || originalShader == null) return;

        rend.material.shader = originalShader;

    }
}