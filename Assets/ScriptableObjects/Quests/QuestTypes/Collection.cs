using UnityEngine;

[CreateAssetMenu(fileName = "Collection", menuName = "Scriptable Objects/Quests/Collection")]
public class Collection : Quest
{
    [Header("Collection Info")]
    public ItemType itemType;
}
