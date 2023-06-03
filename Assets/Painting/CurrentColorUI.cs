using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CurrentColorUI : MonoBehaviour
{
    Image image;

    [SerializeField] Color Red;
    [SerializeField] Color Green;
    [SerializeField] Color Blue;
    [SerializeField] Color Yellow;

    private void Awake()
    {
        image = GetComponent<Image>();
    }

    private void OnEnable()
    {
        BrushManager.OnColorChanged += UpdateUI;
    }

    private void OnDisable()
    {
        BrushManager.OnColorChanged -= UpdateUI;
    }

    void UpdateUI(ColorsEnum newColor)
    {
        Color col = Color.white;
        switch (newColor)
        {
            case ColorsEnum.NONE:
                //TO DO : Switch to eraser picture
                break;
                case ColorsEnum.RED:
                col = Red;
                break;
                case ColorsEnum.GREEN:
                col = Green;
                break;
                case ColorsEnum.BLUE:
                col = Blue;
                break;
                case ColorsEnum.YELLOW:
                col = Yellow;
                break;
        }

        image.color = col;
    }
}
