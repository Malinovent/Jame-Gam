using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    [SerializeField] private float minSpawnTime = 60;
    [SerializeField] private float maxSpawnTime = 120;
    private float actualSpawnTime = 0;

    [SerializeField] private GameObject REDTargetSpritePrefab;
    [SerializeField] private GameObject BLUETargetSpritePrefab;
    [SerializeField] private GameObject GREENTargetSpritePrefab;
    [SerializeField] private GameObject YELLOWTargetSpritePrefab;
    private NodeGrid nodeGrid;

    // Store the current target nodes
    public static Dictionary<ColorsEnum, Node> targetNodes = new Dictionary<ColorsEnum, Node>();

    private GameObject[] flags = new GameObject[4];

    public static GameManager Singleton;

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

    void Start()
    {
        nodeGrid = FindObjectOfType<NodeGrid>();
        Spawn();
    }

    public float ReturnRandomCooldown()
    { 
        return Random.Range(minSpawnTime, maxSpawnTime);
    }

    public Node SetTargetColor(ColorsEnum color)
    {
        //TO DO: Set a while loop until a new color popsup

        // If there is already a target node of this color
        if (targetNodes.ContainsKey(color))
        {
            // Remove the target node, and make it mutable again
            targetNodes[color].isMutable = true;

            // Remove the target node from the dictionary
            targetNodes.Remove(color);
        }

        // Get a random mutable node from the grid
        Node newTargetNode = nodeGrid.GetRandomMutableNode();

        // If there are no mutable nodes, return
        if (newTargetNode == null)
        {
            Debug.LogWarning("No mutable nodes available");
            return null;
        }

        // Make it a target node, and make it immutable
        newTargetNode.isMutable = false;
        newTargetNode.CurrentColor = color;

        // Add the new target node to the dictionary
        targetNodes.Add(color, newTargetNode);

        return newTargetNode;
    }

    public void Spawn()
    {
        int randomColor = Random.Range(0, SpawnerController.currentNumOfSpawners - 1);
        bool shouldChangePosition = false;

        if(targetNodes.ContainsKey((ColorsEnum)randomColor))
        {
            shouldChangePosition = true;
        }

        Node newNode = SetTargetColor((ColorsEnum)randomColor);
        actualSpawnTime = ReturnRandomCooldown();
        StartCoroutine(SetNewTargetRoutine());

        if(newNode == null) { return; }
        if(shouldChangePosition) 
        {
            ChangeFlag(newNode.CurrentColor, newNode.worldPosition);
        }
        else
        {
            InstantiateTargetFlagObject(newNode);
        }    
    }

    public void Spawn(ColorsEnum color)
    {
        bool shouldChangePosition = false;
        if (targetNodes.ContainsKey(color))
        {
            shouldChangePosition = true;
        }
        Node newNode = SetTargetColor(color);
        actualSpawnTime = ReturnRandomCooldown();
        StartCoroutine(SetNewTargetRoutine());

        if(newNode == null) { return; }

        if (shouldChangePosition)
        {
            ChangeFlag(newNode.CurrentColor, newNode.worldPosition);
        }
        else
        {
            InstantiateTargetFlagObject(newNode);
        }
    }

    //Spawning cooldown before the next target
    IEnumerator SetNewTargetRoutine()
    { 
        yield return new WaitForSeconds(actualSpawnTime);
        Spawn();
    }

    void ChangeFlag(ColorsEnum color, Vector3 newPosition)
    {
        switch (color)
        {
            case ColorsEnum.RED:
                flags[0].transform.position = newPosition;
                break;
            case ColorsEnum.GREEN:
                flags[1].transform.position = newPosition;
                break;
            case ColorsEnum.BLUE:
                flags[2].transform.position = newPosition;
                break;
            case ColorsEnum.YELLOW:
                flags[3].transform.position = newPosition;
                break;
        }
    }

    void InstantiateTargetFlagObject(Node node)
    { 
        if(node == null) { return; }
        switch (node.CurrentColor)
        {
            case ColorsEnum.RED:
                flags[0] = Instantiate(REDTargetSpritePrefab, node.worldPosition, Quaternion.identity);
                break;
            case ColorsEnum.GREEN:
                flags[1] = Instantiate(GREENTargetSpritePrefab, node.worldPosition, Quaternion.identity);
                break;
            case ColorsEnum.BLUE:
                flags[2] = Instantiate(BLUETargetSpritePrefab, node.worldPosition, Quaternion.identity);
                break;
            case ColorsEnum.YELLOW:
                flags[3] = Instantiate(YELLOWTargetSpritePrefab, node.worldPosition, Quaternion.identity);
                break;
        }
    }
   
    
}
