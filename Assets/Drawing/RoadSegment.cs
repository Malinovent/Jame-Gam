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

   
}
