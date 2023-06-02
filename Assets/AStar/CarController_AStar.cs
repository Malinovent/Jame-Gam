using System.Collections.Generic;
using UnityEngine;

public class CarController_AStar : MonoBehaviour
{
    public AStar pathfinding;
    public float speed = 5f;
    public ColorsEnum color;

    [SerializeField]
    private Node destination;

    private List<Node> path;
    private int pathIndex = 0;

    public int PathIndex { 
        get => pathIndex;
        set 
        {
            pathIndex = value;
            //Debug.Log(pathIndex);
        }
    }
    private void OnEnable()
    {
        PaintBrush.OnGridChanged += UpdatePath;
    }

    private void OnDisable()
    {
        PaintBrush.OnGridChanged -= UpdatePath;
    }

    private void Start()
    {
        if (destination != null)
        {
            SetDestination(destination);
        }
    }

    // Sets the destination of the car
    public void SetDestination(Node newDestination)
    {
        List<Node> newPath = pathfinding.FindPath(this.transform.position, newDestination.transform.position, color);

        if (newPath != null && newPath.Count > 0)
        {
            destination = newDestination;
            path = newPath;
            PathIndex = 0;
        }
        else
        {
            // If no valid path was found, move along the current path
            //MoveAlongCurrentPath();
        }
    }

    private void MoveAlongCurrentPath()
    {
        // If the car is already at the end of the path, do nothing
        if (path == null || PathIndex >= path.Count) return;

        // Move towards the next node in the path
        Node nextNode = path[PathIndex];
        transform.position = Vector3.MoveTowards(transform.position, nextNode.worldPosition, speed * Time.deltaTime);

        // Check if the car has reached the next node
        if (Vector3.Distance(transform.position, nextNode.worldPosition) < 0.01f)
        {
            // Go to the next node in the path
            PathIndex++;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // If a valid path is available, move towards the target
        if (path != null && PathIndex < path.Count)
        {
            Node targetNode = path[PathIndex];

            // Move towards the target node
            transform.position = Vector3.MoveTowards(transform.position, targetNode.worldPosition, speed * Time.deltaTime);

            // Check if we've reached the target node
            if (Vector3.Distance(transform.position, targetNode.worldPosition) < 0.01f)
            {
                PathIndex++;
            }
        }
        else
        {
           // MoveAlongCurrentPath();
        }
    }

    void UpdatePath()
    {
        SetDestination(destination);
    }
}
