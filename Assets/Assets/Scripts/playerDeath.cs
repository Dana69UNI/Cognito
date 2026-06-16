using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class playerDeath : MonoBehaviour
{
    public GameObject player;
    public Transform respawn;
    private static bool died =false;

    private void Start()
    {
        if (died)
        {
            player.transform.position = respawn.position;
            player.transform.rotation = respawn.rotation;
        }
    }

    public void dead()
    {
       
            Debug.Log("moristelmao");
            died = true;
            UnityEngine.SceneManagement.Scene currentScene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(currentScene.name);
          
    }
}
