using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SpaceStation : MonoBehaviour
{
    [SerializeField] private QuestManager questManager;
    [SerializeField] private InventoryManager inventoryManager;
    [SerializeField] private PlrLazerDrill drill;
    private List<Quest> completedQuests = new List<Quest>();

    public void CheckQuests()
    {
        foreach (Quest quest in questManager.quests)
        {
            if (quest == null || quest.holder == null)
                continue;

            if (!quest.holder.isCompleted || quest.holder.rewardClaimed)
                continue;

            quest.holder.rewardClaimed = true;
            completedQuests.Add(quest);

            Debug.Log($"{quest.name} is completed");

            if (quest.rewardIsUpgrade)
            {
                switch (quest.rewardUpgradeType.upgradeType)
                {
                    case UpgradeType.UpgradeTypeEnum.DrillPower:
                        drill.UpgradeDrillPower((int)quest.rewardUpgradeType.upgradeValue);
                        break;
                }
            }
            else
            {
                inventoryManager.PickUp(quest.rewardItemType, quest.rewardAmount);
            }
        }

        foreach (Quest quest in completedQuests)
        {
            questManager.CompleteQuest(quest);
        }

        completedQuests.Clear();
    }
}
