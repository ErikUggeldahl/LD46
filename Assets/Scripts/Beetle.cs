using UnityEngine;
using UnityEngine.AI;

public class Beetle : MonoBehaviour
{
    [SerializeField]
    Animator animator = null;

    [SerializeField]
    NavMeshAgent agent = null;

    Human target = null;

    const float TARGET_ADJUSTMENT_PERIOD = 0.5f;
    float targetAdjustmentTimer = 0f;

    const float ATTACK_RADIUS = 2f;
    const float ATTACK_PERIOD = 1f;
    float attackTimer = 0f;

    void Start()
    {
        FindTarget();
    }

    void Update()
    {
        if (!target)
        {
            FindTarget();
        }
        else
        {
            ChaseTarget();
            Attack();
        }
    }

    void FindTarget()
    {
        var humans = GameObject.FindGameObjectsWithTag("Human");

        var closestDistance = float.PositiveInfinity;

        var thisPosition = transform.position;

        foreach (var human in humans)
        {
            NavMeshPath path = new NavMeshPath();
            agent.CalculatePath(human.transform.position, path);
            var distance = PathLength(path);

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

                if (target.tag == "Human")
                    agent.SetDestination(target.transform.position);
                else
                    target = null;
            }
        }
    }

    void Attack()
    {
        if (target && Vector3.Distance(target.transform.position, transform.position) < ATTACK_RADIUS)
        {
            attackTimer += Time.deltaTime;
            if (attackTimer >= ATTACK_PERIOD)
            {
                attackTimer -= ATTACK_PERIOD;

                var remainingHealth = target.GetComponent<Health>().Damage(1);
                if (remainingHealth == 0)
                {
                    target = null;
                }
            }
        }
    }

    void OnDie()
    {
        tag = "Untagged";
        gameObject.layer = 0;

        animator.SetTrigger("Death");

        agent.enabled = false;
        GetComponent<Selectable>().enabled = false;
        enabled = false;
    }

    // https://forum.unity.com/threads/getting-the-distance-in-nav-mesh.315846/#post-2052142
    static float PathLength(NavMeshPath path)
    {
        float length = 0f;

        if (path.status != NavMeshPathStatus.PathInvalid && path.corners.Length > 1)
            for (int i = 1; i < path.corners.Length; i++)
                length += Vector3.Distance(path.corners[i - 1], path.corners[i]);

        return length;
    }
}
