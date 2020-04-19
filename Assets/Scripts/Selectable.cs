using UnityEngine;

public class Selectable : MonoBehaviour
{
    [SerializeField]
    GameObject selector = null;

    [SerializeField]
    GameObject selectorBroken = null;

    bool highlighted = false;

    public void ToggleSelected(bool selected)
    {
        selector.SetActive(selected);
    }

    public void ToggleHighlighted(bool highlighted)
    {
        this.highlighted = highlighted;
        selectorBroken.SetActive(highlighted);
    }

    void OnMouseEnter()
    {
        if (!highlighted)
        {
            selectorBroken.SetActive(true);
        }
    }

    void OnMouseExit()
    {
        if (!highlighted)
        {
            selectorBroken.SetActive(false);
        }
    }
}
