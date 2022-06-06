using UnityEngine;

public class CharacterHUD : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer _focusIndicator;
    [SerializeField]
    private Color _focusIndicatorActiveColor;
    [SerializeField]
    private Color _focusIndicatorInactiveColor;

    private void Awake()
    {
        SetFocused(false);
    }

    public void SetFocused(bool status)
    {
        _focusIndicator.color = status ? _focusIndicatorActiveColor : _focusIndicatorInactiveColor;
    }
}