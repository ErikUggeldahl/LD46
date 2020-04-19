using UnityEngine;

public class HumanRandomization : MonoBehaviour
{
    [SerializeField]
    SkinnedMeshRenderer skinRenderer = null;

    [SerializeField]
    SkinnedMeshRenderer eyesRenderer = null;

    [SerializeField]
    SkinnedMeshRenderer[] shirtRenderers = null;

    [SerializeField]
    SkinnedMeshRenderer pantsRenderer = null;

    [SerializeField]
    Material[] skin = null;

    [SerializeField]
    Material[] eyes = null;

    [SerializeField]
    Material[] shirts = null;

    [SerializeField]
    Material[] pants = null;

    void Start()
    {
        var randomSkin = Random.Range(0, skin.Length);
        var randomEyes = Random.Range(0, eyes.Length);
        var randomShirt = Random.Range(0, shirts.Length);
        var randomPants = Random.Range(0, pants.Length);

        skinRenderer.material = skin[randomSkin];
        eyesRenderer.material = eyes[randomEyes];
        pantsRenderer.material = pants[randomPants];
        foreach (var shirtRenderer in shirtRenderers)
            shirtRenderer.material = shirts[randomShirt];
    }
}
