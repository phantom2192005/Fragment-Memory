using Inventory.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpSystem : MonoBehaviour
{
    [SerializeField]
    private InventorySO inventoryData;



    private void OnTriggerEnter2D(Collider2D trigger)
    {
        Item item = trigger.GetComponent<Item>();
        if (item != null)
        {
            int remainder = inventoryData.AddItem(item.InventoryItem, item.Quantity,item.itemState);
            if (remainder == 0)
            {
                item.canAbsorbAll = true;
                item.DestroyItem();
            }
            else
            {
                item.Quantity = remainder;
            }

        }
    }
    private void Update()
    {

    }
}