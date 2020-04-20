using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField]
    GameObject enemy = null;

    [SerializeField]
    GameObject fastEnemy = null;

    [SerializeField]
    GameObject largeEnemy = null;

    [SerializeField]
    Animator animator = null;

    const float SPAWN_PERIOD = 20f;
    float spawnTimer = 0f;

    void Update()
    {
        var humansAlive = GameObject.FindGameObjectsWithTag("Human").Length;

        spawnTimer += Time.deltaTime;
        if (spawnTimer >= SPAWN_PERIOD * (4f / humansAlive))
        {
            spawnTimer -= SPAWN_PERIOD;

            Instantiate(RandomEnemy(humansAlive), transform.position, Quaternion.identity);
        }
    }

    GameObject RandomEnemy(int living)
    {
        var random = Random.Range(0, 16 - living);
        if (random == 0)
            return largeEnemy;
        else if (random <= 3)
            return fastEnemy;
        else
            return enemy;
    }

    void OnDie()
    {
        tag = "Untagged";
        gameObject.layer = 0;

        animator.SetTrigger("Close");

        GetComponent<Selectable>().enabled = false;
        enabled = false;
    }
}
