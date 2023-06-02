using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    public RoadSegment currentPath;

    [Header("Car Settings")]
    public ColorsEnum carColor;
    public float speed = 5f;
    public float rotationSpeed = 5f;

    private int targetPointIndex = 0;
    private Vector3 lastPosition;

    void Update()
    {
        // If there's no path or the path is not the right color, stop the car
        if (currentPath == null || currentPath.color != carColor)
        {
            Debug.Log("Car is not on a path, or path is the wrong color!");
            return;
        }

        LineRenderer currentPathLine = currentPath.lineRenderer;
        // Remember the last position
        lastPosition = transform.position;

        // Move the car towards the next point in the path
        transform.position = Vector3.MoveTowards(transform.position, currentPathLine.GetPosition(targetPointIndex), speed * Time.deltaTime);

        Vector3 directionOfMovement = transform.position - lastPosition;
        if (directionOfMovement != Vector3.zero)  // Avoid setting rotation if there's no movement
        {
            float angle = Mathf.Atan2(directionOfMovement.y, directionOfMovement.x) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.Euler(0, 0, angle);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }

        // Check if the car has reached the current target point
        if (Vector3.Distance(transform.position, currentPathLine.GetPosition(targetPointIndex)) < 0.001f)
        {
            // Move to the next point, if there is one
            if (targetPointIndex < currentPathLine.positionCount - 1)
            {
                targetPointIndex++;
            }
            else
            {
                // Car has reached the end of the path, handle as needed
                Debug.Log("Car has reached the end of the path!");
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Triggered with " + collision.gameObject.name);
        // Check if the car has collided with a path
        if (collision.gameObject.GetComponent<RoadSegment>() != null)
        {
            // Set the current path to the collided path
            currentPath = collision.gameObject.GetComponent<RoadSegment>();
            targetPointIndex = 0;
            Debug.Log("Collided with a new road");
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        // Check if the car has left a path
        /*if (collision.gameObject.GetComponent<RoadSegment>() != null)
        {
            // Clear the current path
            currentPath = null;
        }*/
    }
}
