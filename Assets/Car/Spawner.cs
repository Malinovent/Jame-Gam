using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject enemyPrefab; // The enemy prefab to spawn.
    public ColorsEnum spawnerColor;

    public void SpawnCar()
    {
        if (!GameManager.targetNodes.ContainsKey(spawnerColor))
        {
            GameManager.Singleton.Spawn(spawnerColor);
            Debug.Log("Set Flag instead");
            return;
        }

        
        Instantiate(enemyPrefab, transform.position, Quaternion.identity);

    }
}

