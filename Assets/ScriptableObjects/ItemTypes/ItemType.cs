using NUnit.Framework;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/ItemType")]
public class ItemType : ScriptableObject
{
    [Header("Item Properties")]
    public string itemName, itemDesc;
    public Sprite invIcon;
}
