using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PaintBrushCursor : MonoBehaviour
{
    public static PaintBrushCursor Singleton;

    [SerializeField] Image spriteRenderer;
    [SerializeField] Sprite idleRed;
    [SerializeField] Sprite idleGreen;
    [SerializeField] Sprite idleBlue;
    [SerializeField] Sprite idleYellow;
    [SerializeField] Sprite paintingRed;
    [SerializeField] Sprite paintingGreen;
    [SerializeField] Sprite paintingBlue;
    [SerializeField] Sprite paintingYellow;
    [SerializeField] Animator animator;

    //private RectTransform transform;
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
        BrushManager.OnColorChanged += UpdateAnimation;
    }

    private void OnDisable()
    {
        BrushManager.OnBrushStateChanged -= UpdateAnimation;
        BrushManager.OnColorChanged -= UpdateAnimation;
    }

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;

        
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 cursorPos = Input.mousePosition;
        transform.position = cursorPos;
    }

    private void UpdateAnimation(BrushStates brushState)
    {
        //Debug.Log("Updating animation");
        if (brushState == BrushStates.PAINTING)
        {
            if (animator) animator.gameObject.SetActive(true);
            switch (BrushManager.CurrentBrushColor)
            {
                case ColorsEnum.RED:
                    spriteRenderer.sprite = paintingRed;
                    if(animator) animator.Play("Animation_RedSplash");
                    break;
                case ColorsEnum.BLUE:
                    spriteRenderer.sprite = paintingBlue;
                    if (animator) animator.Play("Animation_BlueSplash");
                    break;
                case ColorsEnum.GREEN:
                    spriteRenderer.sprite = paintingGreen;
                    if (animator) animator.Play("Animation_GreenSplash");
                    break;
                case ColorsEnum.YELLOW:
                    spriteRenderer.sprite = paintingYellow;
                    if (animator) animator.Play("Animation_YellowSplash");
                    break;
            }
        }
        else
        {
            if (animator) animator.gameObject.SetActive(false);
            switch (BrushManager.CurrentBrushColor)
            {
                case ColorsEnum.RED:
                    spriteRenderer.sprite = idleRed;
                    break;
                case ColorsEnum.BLUE:
                    spriteRenderer.sprite = idleBlue;
                    break;
                case ColorsEnum.GREEN:
                    spriteRenderer.sprite = idleGreen;
                    break;
                case ColorsEnum.YELLOW:
                    spriteRenderer.sprite = idleYellow;
                    break;
            }
        }
    }

    private void UpdateAnimation(ColorsEnum color)
    {
        if (BrushManager.CurrentBrushState == BrushStates.PAINTING)
        {
            switch (BrushManager.CurrentBrushColor)
            {
                case ColorsEnum.RED:
                    spriteRenderer.sprite = paintingRed;
                    break;
                case ColorsEnum.BLUE:
                    spriteRenderer.sprite = paintingBlue;
                    break;
                case ColorsEnum.GREEN:
                    spriteRenderer.sprite = paintingGreen;
                    break;
                case ColorsEnum.YELLOW:
                    spriteRenderer.sprite = paintingYellow;
                    break;
            }
        }
        else
        {
            switch (BrushManager.CurrentBrushColor)
            {
                case ColorsEnum.RED:
                    spriteRenderer.sprite = idleRed;
                    break;
                case ColorsEnum.BLUE:
                    spriteRenderer.sprite = idleBlue;
                    break;
                case ColorsEnum.GREEN:
                    spriteRenderer.sprite = idleGreen;
                    break;
                case ColorsEnum.YELLOW:
                    spriteRenderer.sprite = idleYellow;
                    break;
            }
        }
    }
}
