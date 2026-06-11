using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class QuestHolder : MonoBehaviour
{
    public Quest quest;
    public SpaceStation station;
    public TextMeshProUGUI questName, questDesc, questCounter, rewardText;
    public float progress;
    public bool isCompleted, rewardClaimed;

    public void Start()
    {
        quest.holder = this;
        questName.text = quest.questName;
        questDesc.text = quest.questDesc;
        questCounter.text = $"{quest.questProgressText}: {progress}/{quest.progressToComplete}";

        if (quest.rewardIsUpgrade)
        {
            if (quest.rewardUpgradeType.upgradeMath == UpgradeType.UpgradeMathEnum.Additive)
            {
                rewardText.text = $"Reward(s): +{quest.rewardUpgradeType.upgradeValue} {quest.rewardUpgradeType.upgradeType}";
            }
            else
            {
                rewardText.text = $"Reward(s): {quest.rewardUpgradeType.upgradeValue}x {quest.rewardUpgradeType.upgradeType}";
            }
        }
        else
        {
            rewardText.text = $"Reward(s): {quest.rewardAmount}x {quest.rewardItemType.itemName}";
        }

        station = FindAnyObjectByType<SpaceStation>();
    }


    public void ProgressQuest(float progress)
    {
        if (isCompleted)
            return;

        this.progress += progress;

        if (this.progress < quest.progressToComplete)
            questCounter.text = $"{quest.questProgressText}: {this.progress}/{quest.progressToComplete}";
        else
        {
            questCounter.color = Color.green;
            questCounter.text = "Completed!";

            questDesc.color = Color.yellow;
            questDesc.text = "Return to the ship";

            isCompleted = true;
        }

        station.CheckQuests();
    }
}
