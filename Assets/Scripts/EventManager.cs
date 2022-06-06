using System;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static event Action<Character> OnCharacterSelected;
    public static event Action<Vector3> OnSetFocusZoomedIn;
    public static event Action<Vector3> OnSetFocusZoomedOut;

    public static void BroadcastCharacterSelected(Character character)
    {
        OnCharacterSelected?.Invoke(character);
    }

    public static void BroadcastSetFocusZoomedIn(Vector3 focusPoint)
    {
        OnSetFocusZoomedIn?.Invoke(focusPoint);
    }

    public static void BroadcastSetFocusZoomedOut(Vector3 focusPoint)
    {
        OnSetFocusZoomedOut?.Invoke(focusPoint);
    }

    private void Start()
    {
        DontDestroyOnLoad(this);
    }
}