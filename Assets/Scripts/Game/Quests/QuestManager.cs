using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    [Header("Quests")]
    public Quest testQuest;
    public List<Quest> quests = new List<Quest>();

    [Header("References")]
    [SerializeField] private GameObject QuestHolderHolder;
    [SerializeField] private GameObject QuestPrefab;

    void Start()
    {
        AddQuest(testQuest);
    }

    public void OnPickup(ItemType itemType)
    {
        if (quests.Count == 0)
            return;
            
        foreach (Quest quest in quests)
        {
            if (quest is Collection collectionQuest && collectionQuest.itemType == itemType)
            {
                quest.holder.ProgressQuest(1);
            }
        }
    }

    public void AddQuest(Quest quest)
    {
        quests.Add(quest);

        QuestHolder newHolder = Instantiate(QuestPrefab, QuestHolderHolder.transform).GetComponent<QuestHolder>();
        newHolder.quest = quest;
    }

    public void CompleteQuest(Quest quest)
    {
        if (!quest.holder.isCompleted)
            return;

        quests.Remove(quest);
        Destroy(quest.holder.gameObject);
    }

    void OnEnable()
    {
        GameEvents.OnItemPickedUp += OnPickup;
    }

    void OnDisable()
    {
        GameEvents.OnItemPickedUp -= OnPickup;
    }
}
