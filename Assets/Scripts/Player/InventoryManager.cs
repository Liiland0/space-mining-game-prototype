using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using TMPro;

public class InventoryManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject inventory;
    public InventorySlot[] inventorySlots = new InventorySlot[24];
    [SerializeField] private GraphicRaycaster raycaster;
    [SerializeField] private EventSystem eventSystem;

    [Header("GuiDescription")]
    [SerializeField] private GameObject GuiDescription;
    [SerializeField] private TextMeshProUGUI descriptionText;


    private GameObject drill;
    private List<RaycastResult> fireResults = new List<RaycastResult>();
    private List<RaycastResult> hoverResults = new List<RaycastResult>();

    private Vector2 mousePos;

    void Start()
    {
        drill = GetComponentInChildren<PlrLazerDrill>().gameObject;
        inventorySlots = inventory.GetComponentsInChildren<InventorySlot>();

    }

    public bool PickUp(ItemType itemType, int quantity)
    {
        foreach (InventorySlot slot in inventorySlots)
        {
            if (slot.itemType == itemType)
            {
                slot.Add(quantity);
                GameEvents.OnItemPickedUp?.Invoke(itemType);
                return true;
            }
        }

        foreach (InventorySlot slot in inventorySlots)
        {
            if (slot.itemType == null)
            {
                slot.SetItem(itemType, quantity);
                GameEvents.OnItemPickedUp?.Invoke(itemType);
                return true;
            }
        }

        return false;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Item"))
        {
            ItemType itemType = collision.gameObject.GetComponent<DroppedItem>().itemType;
            if (itemType != null)
            {
                if (PickUp(itemType, 1))
                {
                    Destroy(collision.gameObject);
                }
            }
        }
    }

    public void OnInventory()
    {
        inventory.SetActive(!inventory.activeSelf);
        drill.SetActive(!inventory.activeSelf);
    }

    public void OnFire1(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;

        if (!inventory.activeSelf)
            return;

        PointerEventData pointerData = new PointerEventData(eventSystem)
        {
            position = mousePos
        };

        fireResults.Clear();
        raycaster.Raycast(pointerData, fireResults);

        if (fireResults.Count > 0)
        {
            GameObject clickedObject = fireResults[0].gameObject;
            Debug.Log("Clicked: " + clickedObject.name);
        }
    }

    public void OnPoint(InputAction.CallbackContext context)
    {
        mousePos = context.ReadValue<Vector2>();

        PointerEventData pointerData = new PointerEventData(eventSystem)
        {
            position = mousePos
        };

        hoverResults.Clear();
        raycaster.Raycast(pointerData, hoverResults);
        GameObject hoveredObject = hoverResults.Count > 0 ? hoverResults[0].gameObject : null;

        if (hoveredObject == null)
        {
            GuiDescription.SetActive(false);
            return;
        }

        if (hoveredObject.TryGetComponent(out GUIDescription slot))
        {
            if (!string.IsNullOrEmpty(slot.title) && !string.IsNullOrEmpty(slot.description))
            {
                GuiDescription.SetActive(true);
                GuiDescription.transform.position = mousePos;
                descriptionText.text = $"{slot.title}:\n{slot.description}";
            }
            else
            {
                GuiDescription.SetActive(false);
            }
        }
        else
        {
            GuiDescription.SetActive(false);
        }
    }

    public void SetItem(int index, ItemType newItemType, int newQuantity)
    {
        inventorySlots[index].SetItem(newItemType, newQuantity);
    }
    public void AddQuantity(int index, int amount)
    {
        inventorySlots[index].Add(amount);
    }
    public void ClearSlot(int index)
    {
        inventorySlots[index].SetItem(null, 0);
    }
}