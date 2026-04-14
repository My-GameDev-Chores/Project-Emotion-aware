using UnityEngine;

public class EmotionSystem : MonoBehaviour
{
    public Transform player;
    public Transform nest;
    public CompanionController companion;

    public float detectionRadius = 4f;
    public float confusionTime = 5f;

    private float timer = 0f;
    private bool triggered = false;

    void Update()
    {
        if (player == null || nest == null) return;

        float distance = Vector3.Distance(player.position, nest.position);

        if (distance < detectionRadius)
        {
            timer += Time.deltaTime;

            if (timer >= confusionTime && !triggered)
            {
                triggered = true;
                companion.Investigate(nest);
            }
        }
        else
        {
            timer = 0f;
            triggered = false;
        }
    }
}