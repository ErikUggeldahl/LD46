using UnityEngine;

public class Survivor : MonoBehaviour
{
    [SerializeField]
    public bool survivor = false;

    [SerializeField]
    Animator animator = null;

    [SerializeField]
    SkinnedMeshRenderer[] shirtRenderers = null;

    [SerializeField]
    Material survivorShirt = null;

    [SerializeField]
    Material[] humanShirts = null;

    const int HUMAN_LAYER = 9;

    const float RESISTANCE_PERIOD = 1f;
    float resistance_timer = 0f;

    const float RESIST_DISTANCE = 4f;

    void Start()
    {
        if (survivor)
        {
            tag = "Untagged";
            gameObject.layer = 0;

            GetComponent<Human>().enabled = false;

            animator.SetTrigger("Sit");

            foreach (var shirtRenderer in shirtRenderers)
                shirtRenderer.material = survivorShirt;
        }
    }

    void Update()
    {
        if (survivor)
        {
            resistance_timer += Time.deltaTime;
            if (resistance_timer >= RESISTANCE_PERIOD)
            {
                resistance_timer -= RESISTANCE_PERIOD;

                var humans = GameObject.FindGameObjectsWithTag("Human");
                var thisPosition = transform.position;

                foreach (var human in humans)
                {
                    var distance = Vector3.Distance(human.transform.position, thisPosition);
                    if (distance < RESIST_DISTANCE)
                        Resist();
                }
            }
        }
        else
        {
            enabled = false;
        }
    }

    void Resist()
    {
        survivor = false;
        tag = "Human";
        gameObject.layer = HUMAN_LAYER;

        var human = GetComponent<Human>();
        human.enabled = true;
        human.Stop();

        animator.SetTrigger("Resist");

        var randomShirt = Random.Range(0, humanShirts.Length);
        foreach (var shirtRenderer in shirtRenderers)
            shirtRenderer.material = humanShirts[randomShirt];
    }
}
