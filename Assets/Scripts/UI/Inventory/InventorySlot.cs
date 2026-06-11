using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    [Header("Slot Properties")]
    public ItemType itemType;
    public GUIDescription itemDescription;
    [SerializeField] private int quantity;

    [Header("UI Elements")]
    [SerializeField] private Image displayIcon;
    [SerializeField] private TextMeshProUGUI quantityText;

    void Awake()
    {
        displayIcon = GetComponent<Image>();
        quantityText = GetComponentInChildren<TextMeshProUGUI>();
        RefreshUI();
    }

    public void SetItem(ItemType newItemType, int newQuantity)
    {
        itemType = newItemType;
        quantity = newQuantity;

        if (itemType != null && itemDescription != null)
        {
            itemDescription.title = itemType.itemName;
            itemDescription.description = itemType.itemDesc;
        }
        else
        {
            itemDescription.title = "";
            itemDescription.description = "";
        }

        RefreshUI();
    }

    public void Add(int amount)
    {
        quantity += amount;
        RefreshUI();
    }

    private void RefreshUI()
    {
        if (quantityText != null)
        {
            if (quantity == 0)
                quantityText.text = "";
            else
                quantityText.text = quantity.ToString();
        }

        if (itemType != null && displayIcon != null)
        {
            if (itemType.invIcon != null)
                displayIcon.sprite = itemType.invIcon;
        }
    }
}