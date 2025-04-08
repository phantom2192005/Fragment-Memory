using Inventory.Model;
using System.Collections.Generic;

[System.Serializable]
public class SerializableInventoryItem
{
    public int itemID;
    public int quantity;
    public List<ItemParameter> itemState;
}
