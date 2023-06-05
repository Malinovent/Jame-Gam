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

    private bool isBrushStroking = false;

    private Dictionary<ColorsEnum, int> colorCounts = new Dictionary<ColorsEnum, int>()
    {
        { ColorsEnum.RED, 200 },
        { ColorsEnum.BLUE, 200 },
        { ColorsEnum.GREEN, 200 },
        { ColorsEnum.YELLOW, 200 }
    };


    public Dictionary<ColorsEnum, int> maxColorNodes  = new Dictionary<ColorsEnum, int>()
    {
        { ColorsEnum.RED, 200 },
        { ColorsEnum.BLUE, 200 },
        { ColorsEnum.GREEN, 200 },
        { ColorsEnum.YELLOW, 200 }
    };

    public static PaintBrush Singleton;

    [SerializeField] private AudioData switchColorSound;

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
        InputManager.OnStartPainting += PaintBrushStrokes;
    }

    private void OnDisable()
    {
        BrushManager.OnColorChanged -= ChangeMaterial;
        InputManager.OnStartPainting -= PaintBrushStrokes;
    }

    private void Start()
    {
        BrushManager.SetBrushColor(ColorsEnum.RED);
        currentPaintMaterial = redMaterial;
        colorCounts[ColorsEnum.RED] = maxColorNodes[ColorsEnum.RED];
        colorCounts[ColorsEnum.GREEN] = maxColorNodes[ColorsEnum.GREEN];
        colorCounts[ColorsEnum.BLUE] = maxColorNodes[ColorsEnum.BLUE];
        colorCounts[ColorsEnum.YELLOW] = maxColorNodes[ColorsEnum.YELLOW];

        lastMousePosition = GetMouseWorldPosition();
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
                //DoErase();
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

        AudioManager_Mali.Instance.PlayAudioDataOnce(switchColorSound, this.transform.position);
    }

    private Vector3 lastMousePosition;
    void DoPaint()
    {
        Vector3 currentPosition = GetMouseWorldPosition();

        foreach (var position in GetPositionsBetween(lastMousePosition, currentPosition))
        {
            Node node = GetNodeFromPosition(position);
            if (node)
            {
                PaintNode(node);
            }          
        }

        AddPointToLineRenderer(GetMouseWorldPosition());
        lastMousePosition = currentPosition;    
    }

    Node GetNodeFromPosition(Vector3 position)
    {
        return nodeGrid.NodeFromWorldPoint(position);
    }

    IEnumerable<Vector3> GetPositionsBetween(Vector3 start, Vector3 end)
    {
        for (float t = 0; t < 1; t += 0.01f)
        {
            yield return Vector3.Lerp(start, end, t);
        }
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

    void PaintNode(Node node)
    {
        if (!node.isMutable)
        {
            //Keep the state to painting so that the brush can continue painting but return from this statement
            //To not paint this node.
            //However, we need to stop the linerender
            //BrushManager.SetBrushState(BrushStates.NONE);
            isBrushStroking = false;
            return;
        }

        if (isBrushStroking == false)
        {
            PaintBrushStrokes();
        }

        if (node.CurrentColor != ColorsEnum.NONE)
        {
            IncreaseColorCount(node.CurrentColor); // Decrease the count for the color being painted over
        }

        // Debug.Log("Found node at grid location: " + node.gridX + ", " + node.gridY);
        node.ChangeColor(BrushManager.CurrentBrushColor);

        DecreaseColorCount(BrushManager.CurrentBrushColor);
        OnGridChanged?.Invoke();
    }

    public void PaintNodes()
    {
        Vector3 worldPos = GetMouseWorldPosition();

        //Debug.Log("Mouse pos is " + mousePos + " and world pos is " + worldPos);
        List<Node> nodes;
        nodes = nodeGrid.NodesFromWorldPoint(worldPos, 0.1f);

        foreach(Node node in nodes) 
        {
            if (!node.isMutable)
            {
                //BrushManager.SetBrushState(BrushStates.NONE);
                isBrushStroking = false;
                continue;
            }

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
        }
    }

    private int lineRendererSortingOrder = 0;

    private void PaintBrushStrokes()
    {
        isBrushStroking = true;
        GameObject lineObject = Instantiate(strokePrefab);
        currentStroke = lineObject.GetComponent<LineRenderer>();
        currentStroke.material = currentPaintMaterial;
        lineRendererSortingOrder += 1;
        currentStroke.sortingOrder = lineRendererSortingOrder;
        currentStroke.positionCount = 0;
        lastMousePosition = GetMouseWorldPosition();
        AddPointToLineRenderer(GetMouseWorldPosition());
    }

    void AddPointToLineRenderer(Vector3 point)
    {
        if(isBrushStroking == false) { return; }
        currentStroke.positionCount++;
        currentStroke.SetPosition(currentStroke.positionCount - 1, point);
    }

    public void IncreaseColorCount(ColorsEnum color)
    {
        if (Time.timeScale == 0) { return; }
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
        if(Time.timeScale == 0) { return; }
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

    public void TryIncreaseColorCount(Node node)
    {
        if (node.CurrentColor != ColorsEnum.NONE)
        {
            IncreaseColorCount(node.CurrentColor); // Decrease the count for the color being painted over
            Debug.Log("This color node has reverted from " + node.CurrentColor);
            node.CurrentColor = ColorsEnum.NONE; 
        }
    }
}
