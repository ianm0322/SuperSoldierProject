using UnityEngine;
using UnityEngine.UI;

public class Crosshair : MonoBehaviour
{
    [SerializeField]
    private Color _hookableColor = Color.cyan;
    [SerializeField]
    private Color _nonHookableColor = Color.red;

    [SerializeField]
    private Image _crosshairImage;

    public bool IsHookable { get; set; }

    private void Update()
    {
        if (IsHookable)
        {
            _crosshairImage.color = _hookableColor;
        }
        else
        {
            _crosshairImage.color = _nonHookableColor;
        }
    }
}
