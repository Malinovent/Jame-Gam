using System;
using UnityEngine;



public class PaintBrush : MonoBehaviour
{ 
    public NodeGrid nodeGrid;

    public static event Action OnGridChanged;
    public static event Action<ColorsEnum> OnColorChanged;

    [Header("Brush Settings")]
    public static ColorsEnum CurrentColor = ColorsEnum.RED;
    [SerializeField] private GameObject strokePrefab;

    [Header("Paint Materials")]
    [SerializeField] Material redMaterial;
    [SerializeField] Material blueMaterial;
    [SerializeField] Material greenMaterial;
    [SerializeField] Material yellowMaterial;

    private LineRenderer currentStroke;
    private Material currentPaintMaterial;

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

    private void Start()
    {
        InputManager.OnStartPainting += PaintBrushStrokes;
        ChangeBrushColor(CurrentColor);
        currentPaintMaterial = redMaterial;
    }

    // Update is called once per frame
    void Update()
    {
        ChangeBrushColorAlphaInput();

        switch(InputManager.CurrentBrushState) 
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
            currentPaintMaterial = redMaterial;
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ChangeBrushColor(ColorsEnum.GREEN);
            currentPaintMaterial = greenMaterial;
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            ChangeBrushColor(ColorsEnum.BLUE);
            currentPaintMaterial = blueMaterial;
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            ChangeBrushColor(ColorsEnum.YELLOW);
            currentPaintMaterial = yellowMaterial;
        }
    }

    public void ChangeMaterial(ColorsEnum newColor)
    {
        switch (newColor)
        { 
            case ColorsEnum.RED:
                ChangeBrushColor(ColorsEnum.RED);
                currentPaintMaterial = redMaterial;
                break;
            case ColorsEnum.GREEN:
                ChangeBrushColor(ColorsEnum.GREEN);
                currentPaintMaterial = greenMaterial;
                break;
            case ColorsEnum.BLUE:
                ChangeBrushColor(ColorsEnum.BLUE);
                currentPaintMaterial = blueMaterial;
                break;
            case ColorsEnum.YELLOW:
                ChangeBrushColor(ColorsEnum.YELLOW);
                currentPaintMaterial = yellowMaterial;
                break;
        }
    }

    public static void ChangeBrushColor(ColorsEnum newColor)
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
            node.ChangeColor(CurrentColor);   
        
            OnGridChanged?.Invoke();
        }

        if (!node.isMutable)
        {
            InputManager.CurrentBrushState = BrushStates.NONE;
            return;
        }

        AddPointToLineRenderer(GetMouseWorldPosition());
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
}
