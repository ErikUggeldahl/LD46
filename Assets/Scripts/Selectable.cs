using UnityEngine;

public class Selectable : MonoBehaviour
{
    [SerializeField]
    GameObject selector = null;

    [SerializeField]
    GameObject selectorBroken = null;

    public void ToggleSelected(bool selected)
    {
        selector.SetActive(selected);
    }

    public void ToggleHighlighted(bool highlighted)
    {
        selectorBroken.SetActive(highlighted);
    }
}
