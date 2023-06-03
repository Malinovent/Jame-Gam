using System;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

public class CarController : MonoBehaviour
{
    //public AStar pathfinding;
    public float speed = 5f;
    public float rotationSpeed = 5f;
    public ColorsEnum color;

    [SerializeField]
    private Node destination;

    private List<Node> path;
    private int pathIndex = 0;
    private Vector3 lastPosition;

    public static List<CarController> Cars = new List<CarController>();

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
        //pathfinding = AStar.Singleton;
        destination = GameManager.targetNodes[color];
        //to do, set destination if game doesn't have target node
        Cars.Add(this);

        if (destination != null)
        {
            SetDestination(destination);
        }
    
    }

    // Sets the destination of the car
    public void SetDestination(Node newDestination)
    {
        List<Node> newPath = AStar.Singleton.FindPath(this.transform.position, newDestination.transform.position, color);

        if (newPath != null && newPath.Count > 0)
        {
            destination = newDestination;
            path = newPath;
            PathIndex = 0;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // If a valid path is available, move towards the target
        if (path != null && PathIndex < path.Count)
        {
            Node targetNode = path[PathIndex];

            lastPosition = this.transform.position;

            // Move towards the target node
            transform.position = Vector3.MoveTowards(transform.position, targetNode.worldPosition, speed * Time.deltaTime);

            SetRotation();

            // Check if we've reached the target node
            if (Vector3.Distance(transform.position, targetNode.worldPosition) < 0.01f)
            {
                PathIndex++;
            }
        }
    }

    public void SetRotation()
    {
        Vector3 directionOfMovement = transform.position - lastPosition;
        if (directionOfMovement != Vector3.zero)  // Avoid setting rotation if there's no movement
        {
            float angle = Mathf.Atan2(directionOfMovement.y, directionOfMovement.x) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.Euler(0, 0, angle);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }
    }

    void UpdatePath()
    {
        if (destination == null) { return; }
        SetDestination(destination);
    }

    public static bool AreCarsOfColorInScene(ColorsEnum randomColor)
    {
        foreach (CarController car in Cars)
        {
            if (car.color == randomColor)
            {
                Debug.Log(randomColor + " car already exists!");
                return false;
            }
        }

        return true;
    }
}
