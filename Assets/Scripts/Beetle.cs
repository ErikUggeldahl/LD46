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
}
