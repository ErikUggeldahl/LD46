using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField]
    RectTransform barRect = null;

    [SerializeField]
    Image background = null;
    [SerializeField]
    Image foreground = null;

    Camera worldCamera = null;
    RectTransform rTransform = null;

    Transform tracking = null;

    Vector3 offset = new Vector3(0f, 3f, 0f);

    void Start()
    {
        worldCamera = Camera.main;
        rTransform = GetComponent<RectTransform>();
    }

    void Update()
    {
        if (tracking)
        {
            rTransform.position = worldCamera.WorldToScreenPoint(tracking.position + offset);
        }
    }

    public void SetTracking(Transform tracking)
    {
        this.tracking = tracking;

        if (tracking.tag == "Enemy")
        {
            foreground.color = Color.red;
        }
    }

    public void SetDisplay(float percentValue)
    {
        barRect.localScale = new Vector3(percentValue, barRect.localScale.y, 1f);
        if (percentValue == 1f || percentValue == 0f)
        {
            background.enabled = false;
            foreground.enabled = false;
        }
        else
        {
            background.enabled = true;
            foreground.enabled = true;
        }
    }
}
