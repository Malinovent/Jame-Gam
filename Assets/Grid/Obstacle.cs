using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField] private int width = 3;
    [SerializeField] private int height = 3;
    private List<Node> occupiedNodes = new List<Node>();

    public int Width { get { return width; } }
    public int Height { get { return height; } }

    // Start is called before the first frame update
    void Start()
    {
        OccupyNodes(NodeGrid.Singleton, NodeGrid.Singleton.NodeFromWorldPoint(this.transform.position));
    }

    public void OccupyNodes(NodeGrid nodeGrid, Node startNode)
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Node node = nodeGrid.NodeFromWorldPoint(new Vector3((startNode.gridX + x) * nodeGrid.nodeSpacing, (startNode.gridY + y) * nodeGrid.nodeSpacing, 0));
                if (node != null)
                {
                    node.ChangeColor(GetComponent<Spawner>().spawnerColor);
                    //Add any color back to the pool
                    node.isMutable = false;
                    occupiedNodes.Add(node);
                }
            }
        }
    }

    public void ClearNodes()
    {
        foreach (Node node in occupiedNodes)
        {
            node.isMutable = true;
        }
    }
}
