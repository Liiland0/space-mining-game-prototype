using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Quests")]
public class Quest : ScriptableObject
{
    [Header("Quest Info")]
    public QuestHolder holder;
    public string questName;
    public string questDesc;
    public string questProgressText;
    public int progressToComplete;
    
    [Header("Reward Info")]
    public bool rewardIsUpgrade;
    public UpgradeType rewardUpgradeType;
    public ItemType rewardItemType;
    public int rewardAmount;
}
