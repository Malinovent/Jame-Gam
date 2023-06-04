using UnityEngine;
using System.Collections.Generic;
using System;

public class SpawnerController : MonoBehaviour
{
    //public List<Spawner> spawners; // The individual spawners.
    public float minSpawnInterval = 1.0f; // Minimum time between spawns.
    public float maxSpawnInterval = 5.0f; // Maximum time between spawns.

    private float nextSpawnTime;

    private GameObject parent;

    [SerializeField] private Spawner[] spawnerPrefabs;
    [HideInInspector]public List<Spawner> instantiatedSpawner = new List<Spawner>();

    public static Action<ColorsEnum> OnSpawnerAdded;

    public static SpawnerController Singleton;

    private void Awake()
    {
        if (Singleton == null)
        {
            Singleton = this;
        }
        else
        {
            Destroy(this);
        }
    }

    public void UnlockNextSpawner(int color)
    {
        //Debug.Log("Passed in colors int : " + color + " upcasted enum: " + (ColorsEnum)color);
        SpawnASpawner((ColorsEnum)color);
        OnSpawnerAdded?.Invoke((ColorsEnum)color);

        switch (color)
        {
            case 2:
                InputManager.greenUnlocked = true;
                break;
            case 3:
                InputManager.blueUnlocked = true;
                break;
            case 4:
                InputManager.yellowUnlocked = true;
                break;
        }
    }


    public void MoveRandomSpawnerSubscriber(int amount)
    { 
        if(amount >= 20 && amount % 30 == 0)
        {
            MoveRandomSpawner();
            Debug.Log("Moving a random spawner!");
        }
    }

    public void MoveRandomSpawner()
    {
        Spawner spawnerToMove;

        int randomSpawnIndex = UnityEngine.Random.Range(0, instantiatedSpawner.Count - 1);
        spawnerToMove = instantiatedSpawner[randomSpawnIndex];

        MoveSpawner(spawnerToMove);
    }

    public void MoveSpawner(Spawner spawner)
    {
        Node spawnNode = GetValidSpawnNode();
        if (spawnNode)
        {
            spawner.transform.position = spawnNode.worldPosition;
            Obstacle obstacleComponent = spawner.GetComponent<Obstacle>();
            obstacleComponent.ClearNodes();
            obstacleComponent.OccupyNodes(NodeGrid.Singleton, spawnNode);
        }
        else
        {
            Debug.LogWarning("This not find a valid node to move the spanwer");
        }
    }

    public void DecreaseSpawnTime(int scoreAmount)
    {
        if (scoreAmount % 20 == 0)
        {
            minSpawnInterval -= minSpawnInterval * 0.05f;
            maxSpawnInterval -= maxSpawnInterval * 0.05f;
        }
    }
    

    private void Start()
    {
        SpawnASpawner(ColorsEnum.RED);
        SetNextSpawnTime();
        parent = new GameObject("Spawned Cars");
    }

    private void Update()
    {
        if (Time.time >= nextSpawnTime && instantiatedSpawner.Count > 0)
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
        Spawner spawner = instantiatedSpawner[UnityEngine.Random.Range(0, instantiatedSpawner.Count - 1)];

        // Tell the chosen spawner to spawn an enemy.
        spawner.SpawnCar(parent.transform);
    }


    
    public void SpawnASpawner(ColorsEnum color)
    {
        //Spawner chosenSpawner = spawnerPrefabs[UnityEngine.Random.Range(0, spawnerPrefabs.Length)];
        Spawner chosenSpawner = ChooseSpawnerFromColorsEnum(color);
        Node spawnNode = GetValidSpawnNode();

        if (spawnNode != null)
        {
            Spawner spawnerInstance = Instantiate(chosenSpawner, spawnNode.worldPosition, Quaternion.identity);
            spawnerInstance.transform.parent = this.transform;
            //Debug.Log("Spawning a new spawner!");
            instantiatedSpawner.Add(spawnerInstance);
            
            //Spawner.OccupyNodes(nodeGrid, spawnNode);
            //spawnerInstance.GetComponent<Obstacle>().OccupyNodes(NodeGrid.Singleton, spawnNode);
        }
        else
        {
            Debug.LogWarning("No valid spawn node found for hazard!");
        }
    }

    private Spawner ChooseSpawnerFromColorsEnum(ColorsEnum color)
    {
        switch (color)
        {
            case ColorsEnum.RED:
                return spawnerPrefabs[0];
            case ColorsEnum.GREEN:
                return spawnerPrefabs[1];
            case ColorsEnum.BLUE:
                return spawnerPrefabs[2];
            case ColorsEnum.YELLOW:
                return spawnerPrefabs[3];
        }

        return spawnerPrefabs[0];
    }

    private Node GetValidSpawnNode()
    {
        // Convert 2D grid array to a list
        List<Node> allNodes = new List<Node>();
        foreach (NodeRow nodeRow in NodeGrid.Singleton.grid)
        {
            foreach (Node node in nodeRow.row)
            {
                allNodes.Add(node);
            }
        }

        // Shuffle the list
        for (int i = 0; i < allNodes.Count; i++)
        {
            Node temp = allNodes[i];
            int randomIndex = UnityEngine.Random.Range(i, allNodes.Count);
            allNodes[i] = allNodes[randomIndex];
            allNodes[randomIndex] = temp;
        }

        // Check each node until one fits the criteria
        foreach (Node node in allNodes)
        {
            if (NodeGrid.Singleton.CanFit(node, 2, 2))
            {
                return node;
            }
        }

        // Return null if no node fits the criteria
        return null;
    }


    private void OnEnable()
    {
        ScoreTracker.onScoreChanged += MoveRandomSpawnerSubscriber;
        ScoreTracker.onScoreChanged += DecreaseSpawnTime;
    }

    private void OnDisable()
    {
        ScoreTracker.onScoreChanged -= MoveRandomSpawnerSubscriber;
        ScoreTracker.onScoreChanged -= DecreaseSpawnTime;
    }

}
