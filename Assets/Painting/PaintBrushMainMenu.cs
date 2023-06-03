using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintBrushMainMenu : MonoBehaviour
{

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
    private int lineRendererSortingOrder = 0;

    private void Start()
    {
        ChangeBrushColor(CurrentColor);
        InputManager.OnStartPainting += PaintBrushStrokes;
        currentPaintMaterial = redMaterial;
    }

    void Update()
    {
        ChangeBrushColorAlphaInput();

        switch (InputManager.CurrentBrushState)
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

    public static void ChangeBrushColor(ColorsEnum newColor)
    {
        CurrentColor = newColor;
        //OnColorChanged?.Invoke(CurrentColor); // If you have a static event for when color changes, uncomment this line.
    }

    void DoPaint()
    {
        AddPointToLineRenderer(GetMouseWorldPosition());
    }

    void DoErase()
    {
        // You'll need to add your own code here to handle erasing, if you want that functionality.
        // For example, you might do a Physics.Raycast to find the closest stroke and remove it.
    }

    public Vector3 GetMouseWorldPosition()
    {
        Vector2 mousePos = Input.mousePosition;
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
        worldPos.z = 0;

        return worldPos;
    }

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
