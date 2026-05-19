using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CountDown : MonoBehaviour
{
    private int timeCountdown = 30;
    public GameObject enemigo;
    public Transform[] Spawns;
    int level = 1;
    void Start()
    {
        StartCoroutine(contador());
        
    }

    private IEnumerator contador()
    {
        for(int i = timeCountdown; i>0; i--)
        {
            Debug.Log(i);
            //SONIDITO[]
            yield return new WaitForSeconds(1);
            
        }
        spawnEnemigo();
    }

    void spawnEnemigo()
    {
        Debug.Log("ahi va");
        //SONIDITO[]
        if (enemigo == null)
        {
            Instantiate(enemigo);
        }
    }
}
