using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintBrushCursor : MonoBehaviour
{
    public static PaintBrushCursor Singleton;

    private void Awake()
    {
        if (Singleton == null)
        {
            Singleton = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void OnEnable()
    {
        BrushManager.OnBrushStateChanged += UpdateAnimation;
    }

    private void OnDisable()
    {
        BrushManager.OnBrushStateChanged -= UpdateAnimation;
    }

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 cursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        this.transform.position = new Vector3(cursorPos.x, cursorPos.y, -1); // Change Z value as per your requirement
    }

    private void UpdateAnimation(BrushStates brushState)
    { 
        if(brushState == BrushStates.PAINTING)
        {
            
        }
    }
}
