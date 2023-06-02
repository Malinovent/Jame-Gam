using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RoadSegment : MonoBehaviour
{
    public ColorsEnum color;
    public LineRenderer lineRenderer;
    public EdgeCollider2D edgeCollider2D;

    // Start is called before the first frame update
    void Awake()
    {
        if (!lineRenderer) { GetComponent<LineRenderer>(); }
        if(!edgeCollider2D) { GetComponent<EdgeCollider2D>(); }

        //SetEdgeColliders();
    }

    private void SetEdgeColliders()
    {

        // Get the positions from the LineRenderer
        Vector3[] positions = new Vector3[lineRenderer.positionCount];
        lineRenderer.GetPositions(positions);

        // Convert the positions to 2D
        Vector2[] positions2D = positions.Select(p => new Vector2(p.x, p.y)).ToArray();

        // Update the EdgeCollider2D with the new positions
        edgeCollider2D.points = positions2D;
    }
}
