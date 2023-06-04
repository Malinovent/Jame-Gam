using System;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    [Header("Car properties")]
    //public AStar pathfinding;
    [SerializeField] int points = 1;
    public float speed = 5f;
    public float rotationSpeed = 5f;
    public ColorsEnum color;

    [SerializeField]
    private Node destination;

    private List<Node> path;
    private int pathIndex = 0;
    private Vector3 previousPosition;
    private Vector3 originalScale;

    [Header("Car death timer")]
    // Additions for stop timer
    private float stopTimer = 0f; // to keep track of time car is stopped
    [SerializeField] private float stopThreshold = 5f; // maximum allowed stop time

    public static List<CarController> Cars = new List<CarController>();

    public int PathIndex { 
        get => pathIndex;
        set 
        {
            pathIndex = value;
            //Debug.Log(pathIndex);
        }
    }

    #region Overrides
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
        destination = GameManager.flagNodes[color];
        originalScale = transform.localScale;
        //to do, set destination if game doesn't have target node
        Cars.Add(this);

        if (destination != null)
        {
            SetDestination(destination);
        }
    
    }

    void Update()
    {
        // If a valid path is available, move towards the target
        if (path != null && PathIndex < path.Count)
        {
            Node targetNode = path[PathIndex];

            CarController carInFront = CarInFront(15);
            if (carInFront != null)
            {
                // Car in front, stop moving
                return;
            }

            previousPosition = this.transform.position;

            // Move towards the target node
            transform.position = Vector3.MoveTowards(transform.position, targetNode.worldPosition, speed * Time.deltaTime);

            SetRotation();

            // Check if we've reached the target node
            if (Vector3.Distance(transform.position, targetNode.worldPosition) < 0.01f)
            {
                PathIndex++;

                if (targetNode == destination)
                {
                    ArrivedAtDestination();
                }
            }

            //resets every frame, not efficient but just getting it to work
            stopTimer = 0f;
            transform.localScale = originalScale;
        }
        else
        {
            //Debug.Log(this.gameObject.name + " is Not moving!");
            stopTimer += Time.deltaTime;
            OscillateSize();
            if (stopTimer >= stopThreshold)
            {
                CarCrashed();
            }
        }
    }
    #endregion

    private void OscillateSize()
    {
        float scale = 1f + Mathf.Sin(stopTimer * 2f * Mathf.PI) * 0.1f; // you can adjust the multiplier (0.1f here) to get the desired amplitude
        transform.localScale = originalScale * scale;
    }


    private void CarCrashed()
    {
        // Implement what should happen when the car 'crashes' due to being stopped for too long.
        // For example, decrease the player's lives count and destroy the car object.
        //PlayerLivesManager.DecreaseLives();
        LivesTracker.LoseLife();
        Cars.Remove(this);
        Destroy(gameObject);
    }


    private void ArrivedAtDestination()
    {
        // Add the code you want to execute when the car arrives at its destination
        // For example, you can remove this car from the Cars list and destroy the car GameObject
        Cars.Remove(this);
        ScoreTracker.Score += points;
        Destroy(gameObject);
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
    
    public void SetRotation()
    {
        Vector3 directionOfMovement = transform.position - previousPosition;
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

    public CarController CarInFront(float checkDistance)
    {
        for (int i = 1; i <= checkDistance; i++)
        {
            int checkIndex = PathIndex + i;

            if (checkIndex >= path.Count)
            {
                // We've reached the end of the path
                break;
            }

            Node checkNode = path[checkIndex];

            foreach (CarController car in Cars)
            {

                if (car != this && car.transform.position == checkNode.worldPosition)
                {
                    // There's a car in front on the path
                    Debug.Log("There is a car!");
                    return car;
                }
            }
        }

        // No car in front on the path
        return null;
    }

}
