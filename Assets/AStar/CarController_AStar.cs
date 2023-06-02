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

    private void Start()
    {
        if (destination != null)
        {
            SetDestination(destination);
        }
    }

    // Sets the destination of the car
    public void SetDestination(Node destination)
    {
        path = pathfinding.FindPath(this.transform.position, destination.transform.position, color);
        if (path != null && path.Count > 0)
        {
            pathIndex = 0;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (path != null && pathIndex < path.Count)
        {
            Node targetNode = path[pathIndex];

            // Move towards the target node
            transform.position = Vector3.MoveTowards(transform.position, targetNode.worldPosition, speed * Time.deltaTime);

            // Check if we've reached the target node
            if (Vector3.Distance(transform.position, targetNode.worldPosition) < 0.01f)
            {
                pathIndex++;
            }
        }
    }
}
