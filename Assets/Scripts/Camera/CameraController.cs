using System.Collections;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private Camera _camera;
    [SerializeField]
    private float _zoomFactor = 0.8f;
    [SerializeField]
    private float _zoomTransitionTime = 0.4f;

    private float _originalOrthographicSize;

    private void Awake()
    {
        _originalOrthographicSize = _camera.orthographicSize;
    }

    private void OnSetFocusZoomedInCallback(Vector3 focusPoint)
    {
        StopAllCoroutines();
        StartCoroutine(PanSmooth(_originalOrthographicSize * _zoomFactor));
    }

    private void OnSetFocusZoomedOutCallback(Vector3 focusPoint)
    {
        StopAllCoroutines();
        StartCoroutine(PanSmooth(_originalOrthographicSize));
    }

    private IEnumerator PanSmooth(float newOrthographicSize)
    {
        float currentOrthographicSize = _camera.orthographicSize;
        for (float progress = 0; progress <= 1; progress += (Time.deltaTime / _zoomTransitionTime))
        {
            _camera.orthographicSize = Mathf.SmoothStep(currentOrthographicSize, newOrthographicSize, progress);
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