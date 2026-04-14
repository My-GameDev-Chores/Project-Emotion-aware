using UnityEngine;

public class NestSystem : MonoBehaviour
{
    public int requiredTwigs = 5;
    public int requiredLeaves = 3;
    public int requiredGrass = 2;

    private int currentTwigs;
    private int currentLeaves;
    private int currentGrass;

    public ObjectiveUI objectiveUI;
    public GameObject completionUI;
    public CompanionController companion;

    private bool isCompleted = false;

    public void AddItem(CollectibleItem item)
    {
        if (isCompleted) return;

        switch (item.itemType)
        {
            case CollectibleItem.ItemType.Twig:
                currentTwigs++;
                break;

            case CollectibleItem.ItemType.Leaf:
                currentLeaves++;
                break;

            case CollectibleItem.ItemType.DryGrass:
                currentGrass++;
                break;
        }

        // UI update
        if (objectiveUI != null)
            objectiveUI.UpdateProgress(item.itemType);

        // Nest grows visually
        transform.localScale += new Vector3(0.05f, 0.02f, 0.05f);

        CheckCompletion();
    }

    void CheckCompletion()
    {
        if (currentTwigs >= requiredTwigs &&
            currentLeaves >= requiredLeaves &&
            currentGrass >= requiredGrass)
        {
            CompleteNest();
        }
    }

    void CompleteNest()
    {
        isCompleted = true;

        Debug.Log("Nest Completed!");

        if (completionUI != null)
            completionUI.SetActive(true);

        if (companion != null)
        {
            companion.SendMessage("ShowDialogue",
                "Great job! The nest is complete. The bird will be safe now.");
        }

        // Slow motion effect
        Time.timeScale = 0.8f;
    }
}