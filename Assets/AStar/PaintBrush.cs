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
    public NodeGrid nodeGrid;

    public static event Action OnGridChanged;
    public static event Action<ColorsEnum> OnColorChanged;

    [Header("Brush Settings")]
    public ColorsEnum CurrentColor = ColorsEnum.RED;
    [SerializeField] private GameObject strokePrefab;

    private LineRenderer currentStroke;

    private void Start()
    {
        ChangeBrushColor(CurrentColor);
    }

    // Update is called once per frame
    void Update()
    {
        SetPaintState();
        ChangeBrushColorAlphaInput();

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

    public void ChangeBrushColorAlphaInput()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ChangeBrushColor(ColorsEnum.RED);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ChangeBrushColor(ColorsEnum.GREEN);     
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            ChangeBrushColor(ColorsEnum.BLUE);           
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            ChangeBrushColor(ColorsEnum.YELLOW);           
        }
    }

    public void ChangeBrushColor(ColorsEnum newColor)
    {
        CurrentColor = newColor;
        OnColorChanged?.Invoke(CurrentColor);
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

        //Design - Should this paint on node?
        AddPointToLineRenderer(node.worldPosition);
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
        //Paint Mode
        if(Input.GetMouseButtonDown(0)) 
        {
            currentStates = BrushStates.PAINTING;

          
            GameObject lineObject = Instantiate(strokePrefab);
            currentStroke = lineObject.GetComponent<LineRenderer>();
            currentStroke.positionCount = 0;
        }

        //No mode
        if (Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1))
        {
            currentStates = BrushStates.NONE;
        }

        //Erase mode
        if (Input.GetMouseButtonDown(1))
        {
            currentStates = BrushStates.ERASING;
        }
    }

    void AddPointToLineRenderer(Vector3 point)
    {
        currentStroke.positionCount++;
        currentStroke.SetPosition(currentStroke.positionCount - 1, point);
    }
}
