using UnityEngine;
using System.Collections.Generic;
using System;

public class SpawnerController : MonoBehaviour
{
    public List<Spawner> spawners; // The individual spawners.
    public float minSpawnInterval = 1.0f; // Minimum time between spawns.
    public float maxSpawnInterval = 5.0f; // Maximum time between spawns.

    private float nextSpawnTime;

    private GameObject parent;
    public static int currentNumOfSpawners = 1;

    public static Action<ColorsEnum> OnSpawnerAdded;


    public void UnlockNextSpawner()
    {
        if(currentNumOfSpawners >= spawners.Count) { return; }
        Spawner spawner = spawners[currentNumOfSpawners];
        spawner.gameObject.SetActive(true);
        currentNumOfSpawners++;

        OnSpawnerAdded?.Invoke(spawner.spawnerColor);
    }

    private void Start()
    {
        SetNextSpawnTime();
        parent = new GameObject("Spawned Cars");
    }

    private void Update()
    {
        if (Time.time >= nextSpawnTime && spawners.Count > 0)
        {
            SpawnCar();
            SetNextSpawnTime();
        }
    }

    private void SetNextSpawnTime()
    {
        nextSpawnTime = Time.time + UnityEngine.Random.Range(minSpawnInterval, maxSpawnInterval);
    }

    private void SpawnCar()
    {
        // Choose a random spawner from the list.
        Spawner spawner = spawners[UnityEngine.Random.Range(0, currentNumOfSpawners - 1)];

        // Tell the chosen spawner to spawn an enemy.
        spawner.SpawnCar(parent.transform);
    }
}
