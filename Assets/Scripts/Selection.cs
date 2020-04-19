using System.Collections.Generic;
using UnityEngine;

public class Selection : MonoBehaviour
{
    [SerializeField]
    RectTransform selectionBox = null;

    [SerializeField]
    Camera worldCamera = null;

    bool selecting = false;

    float originX = 0f;
    float originY = 0f;

    ISet<Selectable> highlighted = new HashSet<Selectable>();
    IList<GameObject> selected = new List<GameObject>();

    void Start()
    {
        selectionBox.gameObject.SetActive(false);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            selecting = true;
            selectionBox.gameObject.SetActive(true);

            originX = Input.mousePosition.x;
            originY = Input.mousePosition.y;

            selectionBox.position = new Vector3(originX, originY, 0f);

            selectionBox.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 0);
            selectionBox.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 0);
        }

        var mousePos = Input.mousePosition;
        var mousePosX = Mathf.Clamp(mousePos.x, 0, Screen.width);
        var mousePosY = Mathf.Clamp(mousePos.y, 0, Screen.height);

        if (selecting)
        {
            var scaleX = 1f;
            var scaleY = 1f;
            if (mousePosX >= originX)
            {
                selectionBox.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Input.mousePosition.x - originX);
            }
            else
            {
                scaleX = -1f;
                selectionBox.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, originX - Input.mousePosition.x);
            }
            if (mousePosY >= originY)
            {
                selectionBox.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, Input.mousePosition.y - originY);
            }
            else
            {
                scaleY = -1f;
                selectionBox.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, originY - Input.mousePosition.y);
            }

            selectionBox.localScale = new Vector3(scaleX, scaleY, 1f);

            var center = (new Vector3(originX, originY, 0f) + new Vector3(mousePosX, mousePosY)) / 2f;
            var halfExtents = selectionBox.sizeDelta / 2f;

            var worldCenter = worldCamera.ScreenToWorldPoint(center);
            float pixelsToWorld = 2f * worldCamera.orthographicSize / Screen.height; // https://forum.unity.com/threads/boxcastall-to-get-all-objects-within-rts-style-selection.450882/#post-3877612
            var worldHalfExtents = halfExtents * pixelsToWorld;

            var hits = Physics.BoxCastAll(worldCenter, worldHalfExtents, worldCamera.transform.forward, worldCamera.transform.rotation, 100f, LayerMask.GetMask("Human"));

            var newHighlighted = new HashSet<Selectable>();
            foreach (var collider in hits)
            {
                var selectable = collider.collider.GetComponentInParent<Selectable>();
                newHighlighted.Add(selectable);
                selectable.ToggleHighlighted(true);
            }

            highlighted.ExceptWith(newHighlighted);
            foreach (var selectable in highlighted)
                selectable.ToggleHighlighted(false);

            highlighted = newHighlighted;
        }

        if (Input.GetMouseButtonUp(0))
        {
            selecting = false;
            selectionBox.gameObject.SetActive(false);

            foreach (var selected in selected)
            {
                var selectable = selected.GetComponent<Selectable>();
                if (!highlighted.Contains(selectable))
                    selectable.ToggleSelected(false);
            }

            selected.Clear();
            foreach (var selectable in highlighted)
            {
                selectable.ToggleHighlighted(false);
                selectable.ToggleSelected(true);
                selected.Add(selectable.gameObject);
            }
            highlighted.Clear();
        }

        if (Input.GetMouseButtonDown(1))
        {
            RaycastHit hit;
            var isHit = Physics.Raycast(worldCamera.ScreenPointToRay(new Vector3(mousePosX, mousePosY, 0f)), out hit, 100f, LayerMask.GetMask("Ground", "Enemy"));
            if (!isHit)
                return;

            if (hit.collider.gameObject.tag == "Enemy")
                foreach (var selected in selected)
                    selected.GetComponent<Human>().Attack(hit.collider.gameObject);
            else
                foreach (var selected in selected)
                    selected.GetComponent<Human>().MoveTo(hit.point);
        }
    }
}
