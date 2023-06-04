using System;
using System.Collections.Generic;
using UnityEngine;

public class PaintBrush : MonoBehaviour
{ 
    public NodeGrid nodeGrid;

    public static event Action OnGridChanged;
    public static event Action<ColorsEnum, int> OnPaintChanged;


    [Header("Brush Settings")]
    [SerializeField] private GameObject strokePrefab;

    [Header("Paint Materials")]
    [SerializeField] Material redMaterial;
    [SerializeField] Material blueMaterial;
    [SerializeField] Material greenMaterial;
    [SerializeField] Material yellowMaterial;

    private LineRenderer currentStroke;
    private Material currentPaintMaterial;

    private Dictionary<ColorsEnum, int> colorCounts = new Dictionary<ColorsEnum, int>()
    {
        { ColorsEnum.RED, 150 },
        { ColorsEnum.BLUE, 150 },
        { ColorsEnum.GREEN, 150 },
        { ColorsEnum.YELLOW, 150 }
    };


    private Dictionary<ColorsEnum, int> maxColorNodes  = new Dictionary<ColorsEnum, int>()
    {
        { ColorsEnum.RED, 150 },
        { ColorsEnum.BLUE, 150 },
        { ColorsEnum.GREEN, 150 },
        { ColorsEnum.YELLOW, 150 }
    };

    public static PaintBrush Singleton;

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

    private void OnEnable()
    {
        BrushManager.OnColorChanged += ChangeMaterial;
    }

    private void OnDisable()
    {
        BrushManager.OnColorChanged -= ChangeMaterial;
    }

    private void Start()
    {
        InputManager.OnStartPainting += PaintBrushStrokes;
        currentPaintMaterial = redMaterial;
        colorCounts[ColorsEnum.RED] = maxColorNodes[ColorsEnum.RED];
        colorCounts[ColorsEnum.GREEN] = maxColorNodes[ColorsEnum.GREEN];
        colorCounts[ColorsEnum.BLUE] = maxColorNodes[ColorsEnum.BLUE];
        colorCounts[ColorsEnum.YELLOW] = maxColorNodes[ColorsEnum.YELLOW];
    }

    // Update is called once per frame
    void Update()
    {
        switch(BrushManager.CurrentBrushState) 
        { 
            case BrushStates.PAINTING:
                if (CanPaint())
                {
                    DoPaint();
                }
                break;
            case BrushStates.ERASING:
                DoErase();
                break;
        }
    }

    private bool CanPaint()
    {
        if (!colorCounts.ContainsKey(BrushManager.CurrentBrushColor))
        {
            colorCounts[BrushManager.CurrentBrushColor] = maxColorNodes[BrushManager.CurrentBrushColor];
        }

        return colorCounts[BrushManager.CurrentBrushColor] > 0;
    }

    public void ChangeMaterial(ColorsEnum newColor)
    {
        switch (newColor)
        { 
            case ColorsEnum.RED:
                currentPaintMaterial = redMaterial;             
                break;
            case ColorsEnum.GREEN:
                currentPaintMaterial = greenMaterial;
                break;
            case ColorsEnum.BLUE:
                currentPaintMaterial = blueMaterial;
                break;
            case ColorsEnum.YELLOW:
                currentPaintMaterial = yellowMaterial;
                break;
        }
    }


    void DoPaint()
    {
        Node node = GetNodeFromMousePosition();
        if (node)
        {
            if (node.CurrentColor != ColorsEnum.NONE)
            {
                IncreaseColorCount(node.CurrentColor); // Decrease the count for the color being painted over
            }

            // Debug.Log("Found node at grid location: " + node.gridX + ", " + node.gridY);
            node.ChangeColor(BrushManager.CurrentBrushColor);

            DecreaseColorCount(BrushManager.CurrentBrushColor);
            OnGridChanged?.Invoke();
        }

        if (!node.isMutable)
        {
            BrushManager.SetBrushState(BrushStates.NONE);
            return;
        }

        AddPointToLineRenderer(GetMouseWorldPosition());
    }

    void DoErase()
    {
        Node node = GetNodeFromMousePosition();
        if (node)
        {
            if (node.CurrentColor != ColorsEnum.NONE)
            {
                IncreaseColorCount(node.CurrentColor); // Decrease the count for the color being erased
            }

            // Debug.Log("Found node at grid location: " + node.gridX + ", " + node.gridY);
            node.CurrentColor = ColorsEnum.NONE;
            node.UpdateSpriteColor();

            OnGridChanged?.Invoke();
        }
    }

    public Vector3 GetMouseWorldPosition()
    {
        Vector2 mousePos = Input.mousePosition;
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
        worldPos.z = 1;

        return worldPos;
    }

    public Node GetNodeFromMousePosition()
    {

        Vector3 worldPos = GetMouseWorldPosition();

        //Debug.Log("Mouse pos is " + mousePos + " and world pos is " + worldPos);

        Node node = nodeGrid.NodeFromWorldPoint(worldPos);
        return node;
    }

    private int lineRendererSortingOrder = 0;

    private void PaintBrushStrokes()
    {
        GameObject lineObject = Instantiate(strokePrefab);
        currentStroke = lineObject.GetComponent<LineRenderer>();
        currentStroke.material = currentPaintMaterial;
        lineRendererSortingOrder += 1;
        currentStroke.sortingOrder = lineRendererSortingOrder;
        currentStroke.positionCount = 0;
    }

    void AddPointToLineRenderer(Vector3 point)
    {
        currentStroke.positionCount++;
        currentStroke.SetPosition(currentStroke.positionCount - 1, point);
    }

    public void IncreaseColorCount(ColorsEnum color)
    {
        if (colorCounts[color] < maxColorNodes[color])
        {
            colorCounts[color]++;
            OnPaintChanged(color, colorCounts[color]);

            if (colorCounts[color] > maxColorNodes[color]) { colorCounts[color] = maxColorNodes[color]; }
        }
        else
        {
            // you've hit the limit for this color
            // handle this case however you like (e.g. play a sound, show a message, etc.)
        }
    }

    public void DecreaseColorCount(ColorsEnum color)
    {
        if (colorCounts[color] > 0)
        {
            colorCounts[color]--;
            OnPaintChanged(color, colorCounts[color]);
        }
        else
        { 
            // you've run out for this color
            // handle this case however you like (e.g. play a sound, show a message, etc.)
        }
    }
}
