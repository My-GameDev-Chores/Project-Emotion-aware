using UnityEngine;

public class CollectibleItem : MonoBehaviour
{
    public enum ItemType
    {
        Twig,
        Leaf,
        DryGrass
    }

    public ItemType itemType;

    // ✅ IMPORTANT (THIS WAS MISSING)
    private Renderer itemRenderer;
    private Color originalColor;

    void Start()
    {
        itemRenderer = GetComponent<Renderer>();

        if (itemRenderer != null)
        {
            originalColor = itemRenderer.material.color;
        }
    }

    // ✅ Highlight ON
    public void Highlight()
    {
        if (itemRenderer != null)
        {
            itemRenderer.material.color = Color.yellow;
        }
    }

    // ✅ Highlight OFF
    public void RemoveHighlight()
    {
        if (itemRenderer != null)
        {
            itemRenderer.material.color = originalColor;
        }
    }

    // ✅ Pickup
    public void PickUp()
    {
        Debug.Log(itemType + " picked");

        transform.localScale *= 1.2f;

        gameObject.SetActive(false);
    }
}