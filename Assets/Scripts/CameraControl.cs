using UnityEngine;

public class CameraControl : MonoBehaviour
{
    [SerializeField]
    Transform worldCamera = null;

    Camera worldCameraCamera;
    float defaultCameraOrthoSize;

    const float CAMERA_SPEED = 8f;

    const float EDGE_THRESHOLD = 0.05f;

    Vector3 UP = new Vector3(1f, 0f, 1f);
    Vector3 RIGHT = new Vector3(1f, 0f, -1f);

    void Start()
    {
        worldCameraCamera = worldCamera.GetComponent<Camera>();
        defaultCameraOrthoSize = worldCameraCamera.orthographicSize;
    }

    void Update()
    {
        var x = Input.GetAxis("Horizontal");
        var y = Input.GetAxis("Vertical");

        var mousePos = Input.mousePosition;
        var mousePosX = Mathf.Clamp(mousePos.x, 0, Screen.width);
        var mousePosY = Mathf.Clamp(mousePos.y, 0, Screen.height);

        var leftEdge = Screen.width * EDGE_THRESHOLD;
        var rightEdge = Screen.width * (1f - EDGE_THRESHOLD);
        var bottomEdge = Screen.height * EDGE_THRESHOLD;
        var topEdge = Screen.height * (1f - EDGE_THRESHOLD);

        //if (mousePosX < leftEdge)
        //    x = (mousePosX / Screen.width / EDGE_THRESHOLD - 1f);
        //else if (mousePos.x > rightEdge)
        //    x = ((mousePosX / Screen.width) - (1f - EDGE_THRESHOLD)) / EDGE_THRESHOLD;

        //if (mousePosY < bottomEdge)
        //    y = (mousePosY / Screen.height / EDGE_THRESHOLD - 1f);
        //else if (mousePosY > topEdge)
        //    y = ((mousePosY / Screen.height) - (1f - EDGE_THRESHOLD)) / EDGE_THRESHOLD;

        var zoomSpeedFactor = worldCameraCamera.orthographicSize / defaultCameraOrthoSize;

        var movement = (new Vector3(x, 0f, -x) + new Vector3(y, 0f, y)) * CAMERA_SPEED * zoomSpeedFactor * Time.deltaTime;

        worldCamera.Translate(movement, Space.World);

        var scroll = Input.mouseScrollDelta.y;
        worldCameraCamera.orthographicSize = Mathf.Clamp(worldCameraCamera.orthographicSize - scroll * 0.5f, 3f, 10f);
    }
}
