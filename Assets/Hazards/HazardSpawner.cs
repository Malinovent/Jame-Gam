using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class HazardSpawner : MonoBehaviour
{
    [SerializeField] private List<Hazard> hazards;
    [SerializeField] private NodeGrid nodeGrid;

    [SerializeField] private float minSpawnTime = 1f;
    [SerializeField] private float maxSpawnTime = 5f;


    [SerializeField] int spawningPaddingInNodes_WIDTH = 3;
    [SerializeField] int spawningPaddingInNodes_HEIGHT = 3;

    private void Start()
    {
        StartCoroutine(SpawnHazardRoutine());
    }

    private IEnumerator SpawnHazardRoutine()
    { 
        yield return new WaitForSeconds(Random.Range(minSpawnTime, maxSpawnTime));
        StartCoroutine(SpawnHazardRoutine());
        SpawnHazard();
    }

    public void SpawnHazard()
    {
        Hazard chosenHazard = hazards[Random.Range(0, hazards.Count)];
        Node spawnNode = GetValidSpawnNode(chosenHazard);

        if (spawnNode != null)
        {
            Hazard hazardInstance = Instantiate(chosenHazard, spawnNode.worldPosition, Quaternion.identity);
            hazardInstance.OccupyNodes(nodeGrid, spawnNode);
        }
        else
        {
            Debug.LogWarning("No valid spawn node found for hazard!");
        }
    }

    private Node GetValidSpawnNode(Hazard hazard)
    {
        // Convert 2D grid array to a list
        List<Node> allNodes = new List<Node>();
        foreach (NodeRow nodeRow in nodeGrid.grid)
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
            int randomIndex = Random.Range(i, allNodes.Count);
            allNodes[i] = allNodes[randomIndex];
            allNodes[randomIndex] = temp;
        }


        // Check each node until one fits the criteria
        foreach (Node node in allNodes)
        {
            if (nodeGrid.CanFit(node, hazard.Width + spawningPaddingInNodes_WIDTH, hazard.Height + spawningPaddingInNodes_HEIGHT))
            {
                return node;
            }
        }

        // Return null if no node fits the criteria
        return null;
    }
}
