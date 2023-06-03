using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ColorPickerUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private ColorsEnum pickerColor;
    [SerializeField] private float scaleFactor = 1.2f;
    [SerializeField] private float scaleSpeed = 1f;

    private Vector3 originalScale = Vector3.one;
    private bool isActive = false;
    private bool isHovering = false;

    private void OnEnable()
    {
        PaintBrush.OnColorChanged += TryActivateColor;
    }

    private void OnDisable()
    {
        PaintBrush.OnColorChanged -= TryActivateColor;
    }

    public void PickThisColor()
    {
        PaintBrush.ChangeBrushColor(pickerColor);
        PaintBrush.Singleton.ChangeMaterial(pickerColor);
        isActive = true;
        if (isHovering)
        {
            StopAllCoroutines();
            StartCoroutine(ScaleUp());
        }
    }

    public void TryActivateColor(ColorsEnum newColor)
    {
        if (isActive && newColor == pickerColor)
        {
            //If this is the same color and already active, then don't change anything
            Debug.Log("This is already the color!");
            return;
        }    

        if (newColor == pickerColor && !isActive)
        {
            StartCoroutine(ScaleUp());
            isActive = true;
        }
        else
        {
            if (isActive)
            {
                StartCoroutine(ScaleDown());
            }

            isActive = false;
        }
    }

    private IEnumerator ScaleUp()
    {
        float time = 0;
        while (time < 1)
        {
            transform.localScale = Vector3.Lerp(originalScale, originalScale * scaleFactor, time);
            time += Time.deltaTime * scaleSpeed;
            yield return null;
        }
        transform.localScale = originalScale * scaleFactor;
    }

    private IEnumerator ScaleDown()
    {
        float time = 0;
        while (time < 1)
        {
            transform.localScale = Vector3.Lerp(originalScale * scaleFactor, originalScale, time);
            time += Time.deltaTime * scaleSpeed;
            yield return null;
        }
        transform.localScale = originalScale;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovering = true;
        StopAllCoroutines();
        StartCoroutine(ScaleUp());
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isHovering = false;
        if (!isActive)
        {
            StopAllCoroutines();
            StartCoroutine(ScaleDown());
        }
    }
}
