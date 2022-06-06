using System.Collections;
using UnityEngine;

public class EnvironmentController : MonoBehaviour
{
    [SerializeField]
    private Transform _background;
    [SerializeField]
    private Transform _backgroundDistant;
    [SerializeField]
    private SpriteRenderer _tint;

    [SerializeField]
    private Color _tintColor = new Color(0, 0, 0, 0.7f);
    [SerializeField]
    private float _parallaxTransitionTime = 2f;
    [SerializeField]
    private float _tintTransitionTime = 0.4f;

    private Vector3 _backgroundPositionDefault;
    private Vector3 _backgroundDistantPositionDefault;

    private void Awake()
    {
        _backgroundPositionDefault = _background.position;
        _backgroundDistantPositionDefault = _backgroundDistant.position;
    }

    private void OnSetFocusZoomedInCallback(Vector3 focusPoint)
    {
        StopAllCoroutines();
        StartCoroutine(ParallaxTransition(focusPoint));
        StartCoroutine(TintTransition(_tintColor));
    }

    private void OnSetFocusZoomedOutCallback(Vector3 focusPoint)
    {
        StopAllCoroutines();
        StartCoroutine(ParallaxTransition(focusPoint));
        StartCoroutine(TintTransition(new Color(0, 0, 0, 0)));
    }

    // 'OutQuart' easing.
    private float EaseTransition(float value)
    {
        return -(Mathf.Pow((value - 1), 4) - 1);
    }

    private IEnumerator ParallaxTransition(Vector3 focusPoint)
    {
        Vector3 backgroundPositionCurrent = _background.position;
        Vector3 backgroundPositionNew = _backgroundPositionDefault + (focusPoint.x * Vector3.right / 4);
        Vector3 backgroundDistantPositionCurrent = _backgroundDistant.position;
        Vector3 backgroundDistantPositionNew = _backgroundDistantPositionDefault + (focusPoint.x * Vector3.right / 2);
        for (float progress = 0; progress <= 1; progress += (Time.deltaTime / _parallaxTransitionTime))
        {
            _background.position = Vector3.Lerp(backgroundPositionCurrent, backgroundPositionNew, EaseTransition(progress));
            _backgroundDistant.position = Vector3.Lerp(backgroundDistantPositionCurrent, backgroundDistantPositionNew, EaseTransition(progress));
            yield return null;
        }
    }

    private IEnumerator TintTransition(Color targetTintColor)
    {
        Color currentTintColor = _tint.color;
        for (float progress = 0; progress <= 1; progress += (Time.deltaTime / _tintTransitionTime))
        {
            _tint.color = Color.Lerp(currentTintColor, targetTintColor, progress);
            yield return null;
        }
    }

    private void OnEnable()
    {
        EventManager.OnSetFocusZoomedIn += OnSetFocusZoomedInCallback;
        EventManager.OnSetFocusZoomedOut += OnSetFocusZoomedOutCallback;
    }

    private void OnDisable()
    {
        EventManager.OnSetFocusZoomedIn -= OnSetFocusZoomedInCallback;
        EventManager.OnSetFocusZoomedOut -= OnSetFocusZoomedOutCallback;
    }
}