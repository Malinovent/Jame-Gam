using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BrushStates
{ 
    NONE,
    PAINTING,
    ERASING
}

public class PaintBrush : MonoBehaviour
{
    public BrushStates currentStates;
    public ColorsEnum CurrentColor = ColorsEnum.RED;
    public NodeGrid nodeGrid;

    public static event Action OnGridChanged;


    // Update is called once per frame
    void Update()
    {
        SetPaintState();

        switch(currentStates) 
        { 
            case BrushStates.PAINTING:
                DoPaint();
                break;
            case BrushStates.ERASING:
                DoErase();
                break;
        }
    }

    void DoPaint()
    {
        Node node = GetNodeFromMousePosition();
        if (node)
        {
            
           // Debug.Log("Found node at grid location: " + node.gridX + ", " + node.gridY);
            node.CurrentColor = CurrentColor;
            node.UpdateSpriteColor();

            OnGridChanged?.Invoke();
        }
    }

    void DoErase()
    {
        Node node = GetNodeFromMousePosition();
        if (node)
        {
            // Debug.Log("Found node at grid location: " + node.gridX + ", " + node.gridY);
            node.CurrentColor = ColorsEnum.NONE;
            node.UpdateSpriteColor();

            OnGridChanged?.Invoke();
        }
    }

    public Node GetNodeFromMousePosition()
    {
        Vector2 mousePos = Input.mousePosition;
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);

        //Debug.Log("Mouse pos is " + mousePos + " and world pos is " + worldPos);

        Node node = nodeGrid.NodeFromWorldPoint(worldPos);
        return node;
    }

    void SetPaintState()
    { 
        if(Input.GetMouseButtonDown(0)) 
        {
            currentStates = BrushStates.PAINTING;
        }

        if (Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1))
        {
            currentStates = BrushStates.NONE;
        }

        if (Input.GetMouseButtonDown(1))
        {
            currentStates = BrushStates.ERASING;
        }

    }
}
