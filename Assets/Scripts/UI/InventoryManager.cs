using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public int maxStackedItems = 9999;
    public InventorySlot[] inventorySlots;
    public GameObject inventoryItemPrefab;

    int selectedSlot = -1;
    public bool isHoldingCheese = false;
    private void Start()
    {
        ChangeSelectedSlot(0);
    }

    private void Update()
    {
        if (Input.inputString != null)
        {
            bool isNumber = int.TryParse(Input.inputString, out int number);
            if (isNumber && number > 0 && number < 8)
            {
                ChangeSelectedSlot(number - 1);
            }
        }
        UpdateIsHoldingCheese();
    }

    void ChangeSelectedSlot(int newValue)
    {
        if (selectedSlot >= 0)
        {
            inventorySlots[selectedSlot].Deselect();
        }

        inventorySlots[newValue].Select();
        selectedSlot = newValue;
        UpdateIsHoldingCheese();
    }

    public bool AddItem(Item item)
    {
        // Check if any slot has the same item with count lower than max
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            InventorySlot slot = inventorySlots[i];
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            if (itemInSlot != null &&
                itemInSlot.item == item &&
                itemInSlot.count < maxStackedItems &&
                itemInSlot.item.stackable == true)
            {

                itemInSlot.count++;
                itemInSlot.RefreshCount();
                UpdateIsHoldingCheese();
                return true;
            }
        }

        // Find any empty slot
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            InventorySlot slot = inventorySlots[i];
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            if (itemInSlot == null)
            {
                SpawnNewItem(item, slot);
                UpdateIsHoldingCheese();
                return true;
            }
        }

        return false;
    }

    void SpawnNewItem(Item item, InventorySlot slot)
    {
        GameObject newItemGo = Instantiate(inventoryItemPrefab, slot.transform);
        InventoryItem inventoryItem = newItemGo.GetComponent<InventoryItem>();
        inventoryItem.InitialiseItem(item);
        UpdateIsHoldingCheese();
    }

    public Item GetSelectedItem(bool use)
    {
        InventorySlot slot = inventorySlots[selectedSlot];
        InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
        if (itemInSlot != null)
        {
            Item item = itemInSlot.item;
            if (use == true)
            {
                itemInSlot.count--;
                if (itemInSlot.count <= 0)
                {
                    Destroy(itemInSlot.gameObject);
                }
                else
                {
                    itemInSlot.RefreshCount();
                }
                UpdateIsHoldingCheese();
            }

            return item;
        }
        UpdateIsHoldingCheese();
        return null;
    }

    public int GetItemCount(string itemName)
    {
        int count = 0;
        foreach (var slot in inventorySlots)
        {
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            if (itemInSlot != null && itemInSlot.item.name == itemName)
            {
                count += itemInSlot.count;
            }
        }
        return count;
    }
    public void RemoveItem(string itemName, int amount)
    {
        foreach (var slot in inventorySlots)
        {
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            if (itemInSlot != null && itemInSlot.item.name == itemName)
            {
                if (itemInSlot.count >= amount)
                {
                    itemInSlot.count -= amount;
                    if (itemInSlot.count <= 0)
                    {
                        Destroy(itemInSlot.gameObject);
                    }
                    else
                    {
                        itemInSlot.RefreshCount();
                    }
                    UpdateIsHoldingCheese();
                    break;
                }
            }
        }
    }

    public void ClearInventory()
    {
        foreach (var slot in inventorySlots)
        {
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            if (itemInSlot != null)
            {
                Destroy(itemInSlot.gameObject);
            }
        }
        UpdateIsHoldingCheese();
    }

    void UpdateIsHoldingCheese()
    {
        InventorySlot slot = inventorySlots[selectedSlot];
        InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
        if (itemInSlot != null)
        {
            isHoldingCheese = itemInSlot.item.name == "Cheese" || itemInSlot.item.name == "SpicyCheese";
        }
        else
        {
            isHoldingCheese = false;
        }
    }
}
