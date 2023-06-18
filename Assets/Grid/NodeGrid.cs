using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
using System;

[System.Serializable]
public class NodeRow
{
    public Node[] row;
}

public class NodeGrid : MonoBehaviour
{
    public int width, height;
    public float nodeSpacing = 1f;
    public GameObject nodePrefab;
    public NodeRow[] grid;

    [SerializeField] int spawningPaddingInNodes_WIDTH = 3;
    [SerializeField] int spawningPaddingInNodes_HEIGHT = 3;

    public static NodeGrid Singleton;

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


    public void CreateGrid()
    {
        grid = new NodeRow[height];

        for (int y = 0; y < height; y++)
        {
            grid[y] = new NodeRow();
            grid[y].row = new Node[width];

            for (int x = 0; x < width; x++)
            {
                Vector3 nodeWorldPosition = new Vector3(x * nodeSpacing, y * nodeSpacing, 0);
                GameObject nodeObj = Instantiate(nodePrefab, nodeWorldPosition, Quaternion.identity, this.transform);
                
                grid[y].row[x] = nodeObj.GetComponent<Node>();
                grid[y].row[x].Initialize(x, y, nodeWorldPosition, true);
            }
        }
    }

    public Node GetRandomMutableNode()
    {
        List<Node> mutableNodes = new List<Node>();
        foreach (NodeRow nodeRow in grid)
        {
            foreach (Node node in nodeRow.row)
            {
                if (node.isMutable)
                {
                    mutableNodes.Add(node);
                }
            }
        }
        return mutableNodes[UnityEngine.Random.Range(0, mutableNodes.Count)];
    }

    public Node NodeFromWorldPoint(Vector3 worldPosition)
    {
        float x = worldPosition.x / nodeSpacing;
        float y = worldPosition.y / nodeSpacing;

        int xInt = Mathf.Clamp(Mathf.RoundToInt(x), 0, width - 1);
        int yInt = Mathf.Clamp(Mathf.RoundToInt(y), 0, height - 1);

        return grid[yInt].row[xInt];
    }

    public List<Node> NodesFromWorldPoint(Vector3 worldPosition, float detectionRadius)
    {
        List<Node> detectedNodes = new List<Node>();

        int startX = Mathf.Clamp(Mathf.RoundToInt((worldPosition.x - detectionRadius) / nodeSpacing), 0, width - 1);
        int endX = Mathf.Clamp(Mathf.RoundToInt((worldPosition.x + detectionRadius) / nodeSpacing), 0, width - 1);

        int startY = Mathf.Clamp(Mathf.RoundToInt((worldPosition.y - detectionRadius) / nodeSpacing), 0, height - 1);
        int endY = Mathf.Clamp(Mathf.RoundToInt((worldPosition.y + detectionRadius) / nodeSpacing), 0, height - 1);

        for (int y = startY; y <= endY; y++)
        {
            for (int x = startX; x <= endX; x++)
            {
                detectedNodes.Add(grid[y].row[x]);
            }
        }

        return detectedNodes;
    }

    public List<Node> GetNeighbours(Node node)
    {
        List<Node> neighbours = new List<Node>();

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0)
                    continue;

                int checkX = node.gridX + x;
                int checkY = node.gridY + y;

                if (checkX >= 0 && checkX < width && checkY >= 0 && checkY < height)
                {
                    neighbours.Add(grid[checkY].row[checkX]);
                }
            }
        }

        return neighbours;
    }


    public void UpdateAllNodeColors()
    {
        foreach (NodeRow nodeRow in grid)
        {
            foreach (Node n in nodeRow.row)
            {
                n.UpdateSpriteColor();
            }
        }
    }

    private bool SpriteRenderersOn = true;
    public void ToggleAllNodeSpriteRenderers()
    {
        SpriteRenderersOn = !SpriteRenderersOn;
        foreach (NodeRow nodeRow in grid)
        {
            foreach (Node n in nodeRow.row)
            {         
                n.GetComponent<SpriteRenderer>().enabled = SpriteRenderersOn;
            }
        }
    }

    public void DestroyAllNodes()
    {
        foreach (NodeRow nodeRow in grid)
        {
            foreach (Node n in nodeRow.row)
            {
                DestroyImmediate(n);
            }
        }

        grid = null;
    }



    public bool CanFit(Node startNode, int width, int height)
    {
        // Expand the area by one in each direction
        int startX = Math.Max(startNode.gridX - 1, 0);
        int startY = Math.Max(startNode.gridY - 1, 0);
        int endX = Math.Min(startNode.gridX + width, this.width - 1);
        int endY = Math.Min(startNode.gridY + height, this.height - 1);

        // Check if the area fits within the grid boundaries
        if (startNode.gridX + width > this.width || startNode.gridY + height > this.height)
        {
            return false;
        }

        // Check if all nodes in the expanded area are mutable or a flag
        for (int x = startX; x <= endX; x++)
        {
            for (int y = startY; y <= endY; y++)
            {
                Node node = grid[y].row[x];
                if (!node.isMutable || IsFlagNode(node))
                {
                    return false;
                }
            }
        }

        // The area fits and all nodes are mutable
        return true;
    }

    // Add a new method to check if a node is a flag
    public bool IsFlagNode(Node node)
    {
        // Check if the node is in the list of flag nodes
        //Debug.Log("This node is a flag! ");
        return GameManager.flagNodes.ContainsValue(node);
    }


    void OnDrawGizmos()
    {
        if (grid != null)
        {
            //Debug.Log("Updating Gizmos");
            foreach (NodeRow nodeRow in grid)
            {
                foreach (Node n in nodeRow.row)
                {
                    //Gizmos.color = (n.walkable) ? n.GetColorFromEnum() : Color.white;
                    n.UpdateSpriteColor();
                    Gizmos.DrawCube(n.worldPosition, Vector3.one * 0.5f);

                    List<Node> neighbours = GetNeighbours(n);
                    foreach (Node neighbour in neighbours)
                    {
                        Gizmos.color = new Color(1, 1, 1, 0.1f);
                        Gizmos.DrawLine(n.worldPosition, neighbour.worldPosition);
                        //SceneView.RepaintAll();
                    }
                }
            }
        }
    }
}