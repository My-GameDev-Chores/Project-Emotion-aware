using UnityEngine;
using TMPro;

public class ObjectiveUI : MonoBehaviour
{
    public TextMeshProUGUI objectiveText;

    public int currentTwigs;
    public int currentLeaves;
    public int currentGrass;

    public int requiredTwigs;
    public int requiredLeaves;
    public int requiredGrass;

    void Start()
    {
        UpdateUI();
    }

    public void UpdateProgress(CollectibleItem.ItemType type)
    {
        switch (type)
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
        UpdateUI();
    }

    void UpdateUI()
    {
        objectiveText.text = "Collect Materials:\n" +
                             "Twigs: " + currentTwigs + "/" + requiredTwigs + "\n" +
                             "Leaves: " + currentLeaves + "/" + requiredLeaves + "\n" +
                             "Dry Grass: " + currentGrass + "/" + requiredGrass;
    }
}
