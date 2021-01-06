using UnityEngine;

public class Crosshair : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    private Camera _mainCamera;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _mainCamera = Camera.main;
    }

    private void Update()
    {
        var mousePos = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f;

        transform.position = mousePos;
    }

    public void Show()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
        _spriteRenderer.enabled = true;
    }

    public void Hide()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        _spriteRenderer.enabled = false;
    }
}
