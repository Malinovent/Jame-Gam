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
    public GameObject nodePrefab;
    public NodeRow[] grid;


    public void CreateGrid()
    {
        grid = new NodeRow[height];

        for (int y = 0; y < height; y++)
        {
            grid[y] = new NodeRow();
            grid[y].row = new Node[width];

            for (int x = 0; x < width; x++)
            {
                GameObject nodeObj = Instantiate(nodePrefab, new Vector3(x, y, 0), Quaternion.identity, this.transform);
                grid[y].row[x] = nodeObj.GetComponent<Node>();
                grid[y].row[x].Initialize(x, y, true);
            }
        }
    }

    public Node NodeFromWorldPoint(Vector3 worldPosition)
    {
        int x = Mathf.RoundToInt(worldPosition.x);
        int y = Mathf.RoundToInt(worldPosition.y);

        if (x < 0 || x >= width || y < 0 || y >= height)
        {
            Debug.Log("There is no valid node at this position");
            return null;
        }

        return grid[y].row[x];
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
                    //Gizmos.DrawCube(n.worldPosition, Vector3.one * 0.5f);

                    List<Node> neighbours = GetNeighbours(n);
                    foreach (Node neighbour in neighbours)
                    {
                        Gizmos.color = new Color(1, 1, 1, 0.1f);
                        Gizmos.DrawLine(n.transform.position, neighbour.transform.position);
                        //SceneView.RepaintAll();
                    }
                }
            }
        }
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
}