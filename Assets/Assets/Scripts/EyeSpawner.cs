using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeSpawner : MonoBehaviour
{
    [Header("Configuración del Prefab")]
    [Tooltip("El objeto que se va a spawnear.")]
    public GameObject prefabToSpawn;

    [Header("Rangos de Aparición (Relativos al Spawner)")]
    public float minX = -5f;
    public float maxX = 5f;
    public float minY = 0f;
    public float maxY = 2f;
    public float minZ = -5f;
    public float maxZ = 5f;


    [Header("Zona Muerta (Distancia Mínima)")]
    [Tooltip("El objeto NO podrá spawnear más cerca de esta distancia en X y Z.")]
    private float deadzoneX = 0.3f;
    private float deadzoneZ = 0.3f;


    [Header("Ajustes de Probabilidad")]
    [Range(0f, 10000f)]
    [Tooltip("Probabilidad de éxito para spawnear (0% a 100%).")]
    public float spawnChance = 1000f;

    void Start()
    {
        
        StartCoroutine(TrySpawn());
    }

    private IEnumerator TrySpawn()
    {
        while (true)
        {
            if (prefabToSpawn == null)
            {
                Debug.LogWarning("No has asignado ningún Prefab en el Inspector de " + gameObject.name);
                yield return null;
            }


            float randomRoll = Random.Range(0f, 10000f);


            if (randomRoll <= spawnChance)
            {
                SpawnObject();

            }
            else
            {

            }
            yield return new WaitForSeconds(1f);
        }
    }

    private void SpawnObject()
    {

        float randomX = (Random.value > 0.5f) ? Random.Range(deadzoneX, maxX) : Random.Range(-maxX, -deadzoneX);

        float randomY = Random.Range(0f, maxY);

        float randomZ = (Random.value > 0.5f) ? Random.Range(deadzoneZ, maxZ) : Random.Range(-maxZ, -deadzoneZ);

        Vector3 spawnOffset = new Vector3(randomX, randomY, randomZ);
        Vector3 finalSpawnPosition = transform.position + spawnOffset;

        Instantiate(prefabToSpawn, finalSpawnPosition, prefabToSpawn.transform.rotation, transform);
        Debug.Log("spawnee");
    }

    
}
