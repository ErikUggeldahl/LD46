using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField]
    int maxHealth = 5;

    [SerializeField]
    GameObject healthBarObj = null;

    int health;

    HealthBar healthBar;

    void Start()
    {
        health = maxHealth;

        var canvas = GameObject.Find("/HealthBarCanvas").transform;
        var healthBarInst = Instantiate(healthBarObj, canvas);
        healthBar = healthBarInst.GetComponent<HealthBar>();
        healthBar.SetTracking(transform);
        healthBar.SetDisplay(1f);
    }

    public int Damage(int amount)
    {
        health = Mathf.Clamp(health - amount, 0, maxHealth);

        healthBar.SetDisplay((float)health / (float)maxHealth);

        if (health == 0)
        {
            BroadcastMessage("OnDie");
            enabled = false;
        }

        return health;
    }
}
