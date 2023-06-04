using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject[] enemyPrefab; // The enemy prefab to spawn.
    public ColorsEnum spawnerColor;

    public void SpawnCar(Transform parent)
    {
        if (!GameManager.flagNodes.ContainsKey(spawnerColor))
        {
            GameManager.Singleton.SpawnFlag(spawnerColor);
            Debug.Log("Set Flag instead");
            return;
        }

        int randomCarIndex = Random.Range(0, enemyPrefab.Length);
        GameObject spawnedCar = Instantiate(enemyPrefab[randomCarIndex], transform.position, Quaternion.identity);
        spawnedCar.transform.parent = parent;
    }
}

