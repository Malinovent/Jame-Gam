using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Node : MonoBehaviour
{
    public bool isMutable = true;
    public bool walkable = true;
    public Vector3 worldPosition;
    public int gridX, gridY;
    public int gCost, hCost;
    public Node parent;
    public ColorsEnum CurrentColor;

    [SerializeField] SpriteRenderer spriteRenderer;

    public int fCost
    {
        get
        {
            return gCost + hCost;
        }
    }

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Initialize(int _gridX, int _gridY, Vector3 worldPosition, bool _walkable)
    {
        gridX = _gridX;
        gridY = _gridY;
        walkable = _walkable;
        this.worldPosition = worldPosition;
    }

    public void ChangeColor(ColorsEnum newColor)
    {
        if (isMutable)
        {
            CurrentColor = newColor;
            UpdateSpriteColor();
        }
    }

    public Color GetColorFromEnum()
    {
        Color color;
        switch (CurrentColor)
        {
            case ColorsEnum.NONE:
                color = Color.white;
                break;
            case ColorsEnum.RED:
                color = Color.red;
                break;
            case ColorsEnum.GREEN:
                color = Color.green;
                break;
            case ColorsEnum.BLUE:
                color = Color.blue;
                break;
            case ColorsEnum.YELLOW:
                color = Color.yellow;
                break;
            default:
                color = Color.white;
                break;
        }

        //Debug.Log("Returning enum: " + CurrentColor + " to actual color: " + color);
        return color;
    }

    public void UpdateSpriteColor()
    {
        //To do delete this line when ready for build
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer != null)
        {
            spriteRenderer.color = GetColorFromEnum();
        }
        else
        {
            Debug.LogWarning("No SpriteRenderer found on Node object.");
        }
    }

    
}