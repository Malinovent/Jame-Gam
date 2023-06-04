using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PaintAmountTextUI : MonoBehaviour
{
    [SerializeField] private ColorsEnum paintColor;
    private TMP_Text paintAmountText;

    private void OnEnable()
    {
        PaintBrush.OnPaintChanged += UpdateUI;
    }

    private void OnDisable()
    {
        PaintBrush.OnPaintChanged -= UpdateUI;
    }

    // Start is called before the first frame update
    void Start()
    {
        paintAmountText = GetComponent<TMP_Text>();
    }

    private void UpdateUI(ColorsEnum color, int amount)
    { 
        if(color != paintColor) { return; }

        paintAmountText.text = amount.ToString();
    }
}
