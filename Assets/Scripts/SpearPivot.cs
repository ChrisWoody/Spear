using UnityEngine;

public class SpearPivot : MonoBehaviour
{
    public Transform spearChargeSlider; // lazy way to make sure slider doesn't flip

    private Transform _player;
    private Camera _mainCamera;

    // Start is called before the first frame update
    private void Awake()
    {
        _player = GameObject.FindGameObjectWithTag("Player").transform;
        _mainCamera = Camera.main;
    }

    // Update is called once per frame
    private void Update()
    {
        if (!GameController.IsGameRunning)
            return;
        
        var mousePos = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f;

        if (mousePos.x < _player.position.x)
        {
            _player.localScale = new Vector3(-1f, 1f, 1f);
            spearChargeSlider.localScale = new Vector3(-1f, 1f, 1f);
            var playerToMouse = (mousePos - _player.position).normalized;
            transform.right = -playerToMouse;
        }
        else
        {
            _player.localScale = Vector3.one;
            spearChargeSlider.localScale = Vector3.one;
            var playerToMouse = (mousePos - _player.position).normalized;
            transform.right = playerToMouse;
        }
    }
}