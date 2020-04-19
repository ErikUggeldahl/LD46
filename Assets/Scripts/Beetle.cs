using UnityEngine;
using UnityEngine.AI;

public class Beetle : MonoBehaviour
{
    [SerializeField]
    NavMeshAgent agent = null;

    Human target = null;

    float targetAdjustmentTimer = 0f;
    const float TARGET_ADJUSTMENT_PERIOD = 0.5f;

    void Start()
    {
        FindTarget();
    }

    void Update()
    {
        ChaseTarget();
    }

    void FindTarget()
    {
        var humans = GameObject.FindGameObjectsWithTag("Human");

        var closestDistance = float.PositiveInfinity;

        var thisPosition = transform.position;

        foreach (var human in humans)
        {
            var distance = Vector3.Distance(thisPosition, human.transform.position);

            if (distance < closestDistance)
            {
                closestDistance = distance;
                target = human.GetComponent<Human>();

                agent.SetDestination(target.transform.position);
            }
        }
    }

    void ChaseTarget()
    {
        if (target)
        {
            targetAdjustmentTimer += Time.deltaTime;

            if (targetAdjustmentTimer >= TARGET_ADJUSTMENT_PERIOD)
            {
                targetAdjustmentTimer -= TARGET_ADJUSTMENT_PERIOD;
                agent.SetDestination(target.transform.position);
            }
        }
    }
}
