using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class LevelEditorCamera : MonoBehaviour
{
    [SerializeField] private float _scrollSens;
    [SerializeField] private Transform _gridPlane;

    private bool _controllEnabled;
    private Vector2 _mouseWheelDownPosition;
    private Camera _camera;


    private void Start()
    {
        _camera = GetComponent<Camera>();
    }

    private void Update()
    {
        if (_controllEnabled)
        {
            if (Input.GetMouseButtonDown(2))
            {
                _mouseWheelDownPosition = _camera.ScreenToWorldPoint(Input.mousePosition);
            }
            if (Input.GetMouseButton(2))
            {
                Vector2 delta = _mouseWheelDownPosition - (Vector2)_camera.ScreenToWorldPoint(Input.mousePosition);
                transform.position = transform.position + (Vector3)delta;
                _mouseWheelDownPosition = _camera.ScreenToWorldPoint(Input.mousePosition);
            }

            _camera.orthographicSize -= Input.mouseScrollDelta.y * _scrollSens;
            _camera.orthographicSize = Mathf.Clamp(_camera.orthographicSize, 2, 30);
        }
        Vector2 gridSize = CalculateGridScale();
        _gridPlane.transform.localScale = new Vector3(gridSize.x, 1f, gridSize.y);
    }


    private Vector2 CalculateGridScale() 
    {
        float aspect = (float)Screen.width / (float)Screen.height;
        Vector2 size;
        size.y = _camera.orthographicSize / 5f;
        size.x = size.y * aspect;
        return size;
    }

    public void SetActiveControll(bool arg)
    {
        _controllEnabled = arg;
    }
}
