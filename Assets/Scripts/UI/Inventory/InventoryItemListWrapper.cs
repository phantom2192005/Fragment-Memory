using Inventory.Model;
using System.Collections.Generic;

[System.Serializable]
public class InventoryItemListWrapper
{
    public List<InventoryItem> inventoryItems;
    public int size; // Thêm trường size
}
