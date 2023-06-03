using UnityEngine;
using System.Collections.Generic;

public class SpawnerController : MonoBehaviour
{
    public List<Spawner> spawners; // The individual spawners.
    public float minSpawnInterval = 1.0f; // Minimum time between spawns.
    public float maxSpawnInterval = 5.0f; // Maximum time between spawns.

    private float nextSpawnTime;

    private void Start()
    {
        SetNextSpawnTime();
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
        nextSpawnTime = Time.time + Random.Range(minSpawnInterval, maxSpawnInterval);
    }

    private void SpawnCar()
    {
        // Choose a random spawner from the list.
        Spawner spawner = spawners[Random.Range(0, spawners.Count)];

        // Tell the chosen spawner to spawn an enemy.
        spawner.SpawnCar();
    }
}
