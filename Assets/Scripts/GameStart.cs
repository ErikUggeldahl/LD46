using UnityEngine;

public class GameStart : MonoBehaviour
{
    [SerializeField]
    GameObject[] initiallyDisabled = null;

    [SerializeField]
    Human[] humans = null;

    [SerializeField]
    Selection selection = null;

    [SerializeField]
    CameraControl cameraControl = null;

    [SerializeField]
    Animation fadeAnimation = null;

    public bool gameStarted = false;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;

        ToggleEnabled(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            ToggleEnabled(true);

            fadeAnimation.Play();
            gameStarted = true;

            enabled = false;
        }
    }

    void ToggleEnabled(bool toggle)
    {
        foreach (var disabled in initiallyDisabled)
            disabled.SetActive(toggle);

        foreach (var human in humans)
            human.enabled = toggle;

        selection.enabled = toggle;
        cameraControl.enabled = toggle;
    }
}
