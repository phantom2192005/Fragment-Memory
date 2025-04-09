using Inventory.Model;
using System;
using System.Collections.Generic;

[Serializable]
public class InventoryItemListWrapper
{
    public int size; // Kích thước của inventory (số slot)
    public List<InventoryItem> inventoryItems; // Danh sách các item
}
