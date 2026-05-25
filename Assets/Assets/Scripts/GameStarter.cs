using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStarter : MonoBehaviour
{
    private void OnTriggerEnter(Collider colider)
    {
        if (colider.gameObject.CompareTag("Headset"))
        {
            SceneManager.LoadScene(1);
            
        }
    }
}
