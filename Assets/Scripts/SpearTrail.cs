using System;
using UnityEngine;

public class SpearTrail : MonoBehaviour
{
    private LineRenderer _lineRenderer;
    private bool _fadingOut;
    private const float FadeOutTimeout = 0.2f;
    private float _fadeOutElapsed;
    private float _originalWidth;

    private Vector3 _start;
    private Vector3 _end;

    private void Awake()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        _lineRenderer.enabled = false;
        _originalWidth = _lineRenderer.startWidth;
    }

    private void Update()
    {
        if (_fadingOut)
        {
            _fadeOutElapsed += Time.deltaTime;
            if (_fadeOutElapsed >= FadeOutTimeout)
            {
                _fadingOut = false;
                _fadeOutElapsed = 0f;
                _lineRenderer.enabled = false;
            }
            else
            {
                var scale = _fadeOutElapsed / FadeOutTimeout;
                var newStartPos = Vector3.Lerp(_start, _end, scale);
                _lineRenderer.SetPosition(0, newStartPos);

                var negativeScale = -(_fadeOutElapsed / FadeOutTimeout) + 1f;
                _lineRenderer.startWidth = _lineRenderer.endWidth = negativeScale * _originalWidth;
            }
        }
    }

    public void ShowTrail(Vector3 start, Vector3 end)
    {
        _start = start;
        _end = end;
        
        _lineRenderer.enabled = true;
        _lineRenderer.SetPosition(0, start);
        _lineRenderer.SetPosition(1, end);
        _fadingOut = true;
    }
}