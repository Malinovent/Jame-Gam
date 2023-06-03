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
    private void OnEnable()
    {
        BrushManager.OnColorChanged += ChangeMaterial;
        InputManager.OnStartPainting += PaintBrushStrokes;
    }

    private void OnDisable()
    {
        BrushManager.OnColorChanged -= ChangeMaterial;
        InputManager.OnStartPainting -= PaintBrushStrokes;
    }


    private void Start()
    {
        
        currentPaintMaterial = redMaterial;
    }

    void Update()
    {
        //Debug.Log("Updating " + InputManager);
        switch (BrushManager.CurrentBrushState)
        {
            case BrushStates.PAINTING:
                DoPaint();
                break;
            case BrushStates.ERASING:
                DoErase();
                break;
        }
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
        AddPointToLineRenderer(GetMouseWorldPosition());
    }

    void DoErase()
    {
        
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
