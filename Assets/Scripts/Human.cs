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

    [SerializeField]
    Animation muzzleFlash = null;

    [SerializeField]
    GameObject laserObject = null;

    [SerializeField]
    Transform muzzlePoint = null;

    enum State
    {
        Idle = 0,
        Moving = 1,
        Attacking = 2,
        Dead = 3,
    }
    State state = State.Idle;

    const float ATTACK_DISTANCE = 8f;
    const float ENEMY_SCAN_PERIOD = 0.5f;
    float enemyScanTimer = 0f;
    GameObject targetEnemy = null;

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
        print(name + " Move to " + target);
        state = State.Moving;
        targetEnemy = null;

        var success = agent.SetDestination(target);
        print("Pathing successful: " + success);
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
            var thisPosition = transform.position;

            var closestDistance = float.PositiveInfinity;
            GameObject closestEnemy = null;

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
        if (!enemy || enemy.tag != "Enemy")
        {
            Stop();
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

    void GunFired()
    {
        muzzleFlash.Play();
        var laser = Instantiate(laserObject, muzzlePoint.position, Quaternion.identity);
        if (targetEnemy)
        {
            laser.GetComponent<Laser>().SetTarget(targetEnemy.transform.position);
            targetEnemy.GetComponent<Health>().Damage(1);
        }
    }

    public void OnDie()
    {
        state = State.Dead;

        tag = "Untagged";
        gameObject.layer = 0;

        animator.SetInteger("State", (int)State.Dead);

        agent.enabled = false;
        attackLine.enabled = false;
        GetComponent<Selectable>().enabled = false;
        enabled = false;
    }
}
