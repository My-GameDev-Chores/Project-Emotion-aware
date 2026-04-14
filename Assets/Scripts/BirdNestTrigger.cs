using UnityEngine;

public class BirdNestTrigger : MonoBehaviour
{
    public Transform player;
    public CompanionController companion;

    public float detectionDistance = 12f;

    private bool hasTriggered = false;

    void Update()
    {
        if (hasTriggered) return;

        float distance = Vector3.Distance(player.position, transform.position);

        if (distance <= detectionDistance)
        {
            hasTriggered = true;

            Debug.Log("Nest detected from distance!");

            companion.Investigate(transform);
        }
    }
}
