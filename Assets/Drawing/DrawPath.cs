using UnityEngine;
using System.Collections.Generic;

public class DrawPath : MonoBehaviour
{
    public GameObject roadSegmentPrefab;

    private List<Vector2> points;
    private List<RoadSegment> paths;

    private bool isDrawing = false;

    private RoadSegment currentRoadSegment;

    void Awake()
    {
        points = new List<Vector2>();
        paths = new List<RoadSegment>();
    }

    void Update()
    {
        // Check if we've started drawing
        if (Input.GetMouseButtonDown(0))
        {
            isDrawing = true;

            // Create a new road segment
            GameObject roadSegment = Instantiate(roadSegmentPrefab, Vector3.zero, Quaternion.identity);
            currentRoadSegment = roadSegment.GetComponent<RoadSegment>();
            paths.Add(currentRoadSegment);

            // Clear the list of points for the new path
            points.Clear();
        }

        // Check if we've stopped drawing
        if (Input.GetMouseButtonUp(0))
        {
            isDrawing = false;
            currentRoadSegment = null;
        }

        // Check if we're currently drawing
        if (isDrawing)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (!points.Contains(mousePos))
            {
                points.Add(mousePos);

                LineRenderer line = currentRoadSegment.lineRenderer;
                line.positionCount = points.Count;
                line.SetPosition(points.Count - 1, mousePos);

                EdgeCollider2D edgeCollider = currentRoadSegment.edgeCollider2D;
                edgeCollider.points = points.ToArray();
            }
        }
    }
}

