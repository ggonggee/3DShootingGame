using UnityEngine;

public class MinimapCamera : MonoBehaviour
{
    public Transform Target;
    public float YOffect = 10f;
    private Camera _camera;
    private float _maxZoomSize = 20f;
    private float _minZoomSize = 2f;

    private void Start()
    {
        _camera = GetComponent<Camera>();
    }


    private void LateUpdate()
    {

        Vector3 newPosition = Target.position;
        newPosition.y += YOffect;

        transform.position = newPosition;

        //플레이어가 Y축 회전한만큼 미니맵 
        Vector3 newEulerAngles = Target.eulerAngles;
        newEulerAngles.x = 90;
        newEulerAngles.z = 0;
        transform.eulerAngles = newEulerAngles;
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.KeypadPlus))
        {
            _camera.orthographicSize -= 1;
            _camera.orthographicSize = Mathf.Clamp(_camera.orthographicSize, _minZoomSize, _maxZoomSize);

        }

        if (Input.GetKeyDown(KeyCode.KeypadMinus))
        {
            _camera.orthographicSize += 1;
            _camera.orthographicSize = Mathf.Clamp(_camera.orthographicSize, _minZoomSize, _maxZoomSize);
        }

    }


}
