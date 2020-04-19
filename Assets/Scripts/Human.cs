using UnityEngine;
using UnityEngine.AI;

public class Human : MonoBehaviour
{
    [SerializeField]
    Animator animator = null;

    [SerializeField]
    NavMeshAgent agent = null;

    [SerializeField]
    LineRenderer attackLine = null;

    enum State
    {
        Idle = 0,
        Moving = 1,
        Attacking = 2,
    }
    State state;


    const float ATTACK_DISTANCE = 8f;
    const float ENEMY_SCAN_PERIOD = 0.5f;
    float enemyScanTimer = 0f;
    GameObject targetEnemy = null;

    void Start()
    {
    }

    void Update()
    {
        if (state == State.Moving && agent.remainingDistance <= agent.stoppingDistance)
        {
            Stop();
        }
        else if (state == State.Idle && targetEnemy == null)
        {
            FindEnemy();
        }
        else if (state == State.Attacking)
        {
            Attack(targetEnemy);
        }
    }

    public void Stop()
    {
        state = State.Idle;
        targetEnemy = null;

        agent.destination = transform.position;
        animator.SetInteger("State", (int)State.Idle);
        attackLine.gameObject.SetActive(false);
    }

    public void MoveTo(Vector3 target)
    {
        state = State.Moving;
        targetEnemy = null;

        agent.SetDestination(target);
        animator.SetInteger("State", (int)State.Moving);
        attackLine.gameObject.SetActive(false);
    }

    void FindEnemy()
    {
        enemyScanTimer += Time.deltaTime;
        if (enemyScanTimer >= ENEMY_SCAN_PERIOD)
        {
            enemyScanTimer -= ENEMY_SCAN_PERIOD;

            var enemies = GameObject.FindGameObjectsWithTag("Enemy");

            var closestDistance = float.PositiveInfinity;
            GameObject closestEnemy = null;

            var thisPosition = transform.position;

            foreach (var enemy in enemies)
            {
                var distance = Vector3.Distance(thisPosition, enemy.transform.position);

                if (distance < ATTACK_DISTANCE && distance < closestDistance)
                {
                    closestDistance = distance;
                    closestEnemy = enemy;
                }
            }

            Attack(closestEnemy);
        }
    }

    public void Attack(GameObject enemy)
    {
        if (!enemy)
        {
            state = State.Idle;
            return;
        }

        if (enemy != targetEnemy)
        {
            state = State.Attacking;
            targetEnemy = enemy;
            enemyScanTimer = 0f;

            attackLine.gameObject.SetActive(true);
        }

        transform.LookAt(enemy.transform);
        attackLine.SetPosition(1, attackLine.transform.InverseTransformPoint(enemy.transform.position));

        if (Vector3.Distance(transform.position, enemy.transform.position) > ATTACK_DISTANCE)
        {
            animator.SetInteger("State", (int)State.Moving);

            enemyScanTimer += Time.deltaTime;
            if (enemyScanTimer > ENEMY_SCAN_PERIOD)
            {
                enemyScanTimer -= ENEMY_SCAN_PERIOD;
                agent.SetDestination(enemy.transform.position);
            }
        }
        else
        {
            agent.SetDestination(transform.position);
            animator.SetInteger("State", (int)State.Attacking);
        }
    }
}
