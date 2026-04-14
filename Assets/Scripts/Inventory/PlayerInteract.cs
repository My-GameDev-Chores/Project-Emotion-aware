using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    [Header("Interaction Settings")]
    public float interactRadius = 3f;

    [Header("UI")]
    public GameObject interactButton;

    [Header("Hold System")]
    public Transform holdPoint;

    private CollectibleItem heldItem;
    private GameObject heldItemObject;

    private CollectibleItem nearestItem;
    private CollectibleItem lastItem;

    void Update()
    {
        FindNearestItem();
        HandleHighlight();
        HandleUI();
        AutoRotateToItem();

        // PC Input
        if (Input.GetKeyDown(KeyCode.E))
        {
            Interact();
        }
    }

    // 🔍 FIND NEAREST ITEM
    void FindNearestItem()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, interactRadius);

        float closestDistance = Mathf.Infinity;
        nearestItem = null;

        foreach (Collider col in hits)
        {
            CollectibleItem item = col.GetComponent<CollectibleItem>();

            if (item != null && item.gameObject.activeSelf)
            {
                float dist = Vector3.Distance(transform.position, col.transform.position);

                if (dist < closestDistance)
                {
                    closestDistance = dist;
                    nearestItem = item;
                }
            }
        }
    }

    // 🎨 HIGHLIGHT SYSTEM
    void HandleHighlight()
    {
        if (nearestItem != lastItem)
        {
            if (lastItem != null)
                lastItem.RemoveHighlight();

            if (nearestItem != null)
                nearestItem.Highlight();

            lastItem = nearestItem;
        }
    }

    // 🎮 SMART BUTTON SYSTEM
    void HandleUI()
    {
        if (interactButton == null) return;

        if (nearestItem != null || heldItem != null)
            interactButton.SetActive(true);
        else
            interactButton.SetActive(false);
    }

    // 🎥 AUTO ROTATE PLAYER
    void AutoRotateToItem()
    {
        if (nearestItem == null) return;

        Vector3 direction = (nearestItem.transform.position - transform.position).normalized;
        direction.y = 0;

        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
        }
    }

    // 🎯 MAIN INTERACTION FUNCTION
    public void Interact()
    {
        // PICKUP
        if (nearestItem != null && heldItem == null)
        {
            heldItem = nearestItem;

            // CREATE VISUAL COPY
            heldItemObject = Instantiate(nearestItem.gameObject, holdPoint.position, Quaternion.identity);
            heldItemObject.transform.SetParent(holdPoint);

            // DISABLE COLLIDER (IMPORTANT)
            Collider col = heldItemObject.GetComponent<Collider>();
            if (col != null) col.enabled = false;

            nearestItem.PickUp();

            return;
        }

        // DROP TO NEST
        if (heldItem != null)
        {
            Collider[] hits = Physics.OverlapSphere(transform.position, interactRadius);

            foreach (Collider col in hits)
            {
                NestSystem nest = col.GetComponent<NestSystem>();

                if (nest != null)
                {
                    nest.AddItem(heldItem);

                    // DESTROY VISUAL
                    if (heldItemObject != null)
                        Destroy(heldItemObject);

                    heldItem = null;

                    return;
                }
            }
        }
    }

    // 🟢 DEBUG RANGE
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, interactRadius);
    }
}