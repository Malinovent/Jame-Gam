using System.Collections.Generic;
using UnityEngine;

public class Hazard : MonoBehaviour
{
    [SerializeField] private int width = 3;
    [SerializeField] private int height = 3;
    [SerializeField] private float lifetime = 60f;
    private List<Node> occupiedNodes = new List<Node>();

    public int Width { get { return width; } }
    public int Height { get { return height; } }

    [SerializeField] private AudioData plopSound;

    private void Start()
    {
        Invoke("DestroyHazard", lifetime);
        AudioManager_Mali.Instance.PlayAudioDataOnce(plopSound, this.transform.position);
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
                    //node.ChangeColor(ColorsEnum.RED);
                    //PaintBrush.Singleton.TryIncreaseColorCount(node);

                    node.isMutable = false;
                    node.isWalkable = false;
                    occupiedNodes.Add(node);
                }
            }
        }
    }

    private void ClearNodes()
    {
        foreach (Node node in occupiedNodes)
        {
            node.isMutable = true;
            node.isWalkable = true;
        }
    }

    private void DestroyHazard()
    {
        ClearNodes();
        Destroy(gameObject);
    }
}
