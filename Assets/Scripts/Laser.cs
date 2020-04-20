using UnityEngine;

public class Laser : MonoBehaviour
{
    Vector3 target;

    const float SPEED = 30f;

    void Update()
    {
        transform.LookAt(target);
        transform.Translate(Vector3.forward * SPEED * Time.deltaTime, Space.Self);

        if (Vector3.Distance(transform.position, target) < 0.1f)
            Destroy(gameObject);
    }

    public void SetTarget(Vector3 target)
    {
        this.target = target + new Vector3(0f, 0.5f, 0f);
    }
}
