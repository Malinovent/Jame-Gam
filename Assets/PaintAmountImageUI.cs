using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PaintAmountImageUI : MonoBehaviour
{
    [SerializeField] private ColorsEnum paintColor;
    private Image fillImage;

    private void OnEnable()
    {
        PaintBrush.OnPaintChanged += UpdateUI;
    }

    private void OnDisable()
    {
        PaintBrush.OnPaintChanged -= UpdateUI;
    }

    private void Start()
    {
        fillImage = GetComponent<Image>();
    }

    void UpdateUI(ColorsEnum color, int amount)
    { 
        if(color != paintColor) { return; }

        fillImage.fillAmount = (float)amount / (float)PaintBrush.Singleton.maxColorNodes[color];
    }
}
