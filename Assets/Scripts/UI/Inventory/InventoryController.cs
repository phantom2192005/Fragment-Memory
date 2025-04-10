﻿using Inventory.Model;
using Inventory.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Inventory
{
    public class InventoryController : MonoBehaviour
    {
        [SerializeField]
        public UIInventoryPage inventoryUI;

        [SerializeField]
        public InventorySO inventorySO;
        
        [SerializeField]
        private GameObject HurtBox;

        public List<InventoryItem> initalItems = new List<InventoryItem>();

        [SerializeField]
        private AudioClip dropClip;

        [SerializeField]
        private AudioSource audioSource;

        [SerializeField]
        private GameObject DropItemPrefab;


        private void Start()
        {
            PrepareInventory();
        }

        public void PrepareInventory()
        {
            PrepareUI();
            PrepareInventoryData();
        }

        public void PrepareInventoryData()
        { 
            inventorySO.Initialize();
            inventorySO.OnInventoryUpdated += UpdateInventoryUI;
        }

        private void UpdateInventoryUI(Dictionary<int, InventoryItem> inventoryState)
        {
            inventoryUI.ResetAllItems();
            foreach (var item in inventoryState)
            {
                inventoryUI.UpdateData(item.Key, item.Value.item.ItemImage, item.Value.quantity);
            }

        }

        public void PrepareUI()
        {
            inventoryUI.InitializeIventoryUI(inventorySO.Size);
            inventoryUI.OnDescriptionRequested += HandleDescriptionRequest;
            inventoryUI.OnSwapItems += HandleSwapItems;
            inventoryUI.OnStartDragging += HandleDragging;
            inventoryUI.OnItemActionRequested += HandleItemActionRequest;
        }

        private void HandleItemActionRequest(int itemIndex)
        {
            InventoryItem inventoryItem = inventorySO.GetItemAt(itemIndex);
            if (inventoryItem.IsEmpty)
                return;

            IItemAction itemAction = inventoryItem.item as IItemAction;
            if (itemAction != null)
            {
                inventoryUI.ShowItemAction(itemIndex);

                // Chuyển hành động vào UI
                inventoryUI.AddAction(
                    itemAction.ActionName,
                    () => Debug.Log("Pointer Down"), // có thể bật hiệu ứng tại đây
                    () => Debug.Log("Pointer Up"),   // có thể tắt hiệu ứng tại đây
                    () => PerformAction(itemIndex)   // hành động chính
                );
            }

            IDestroyableItem destroyableItem = inventoryItem.item as IDestroyableItem;
            if (destroyableItem != null)
            {
                inventoryUI.AddAction(
                    "Drop",
                    () => Debug.Log("Pointer Down Drop"),
                    () => Debug.Log("Pointer Up Drop"),
                    () => DropItem(itemIndex, inventoryItem.quantity)
                );
            }
        }


        private void DropItem(int itemIndex, int quantity)
        {
            InventoryItem inventoryItem = inventorySO.GetItemAt(itemIndex);
            Item itemWorldSpace = DropItemPrefab.GetComponent<Item>();

            itemWorldSpace.itemState = inventoryItem.itemState;
            itemWorldSpace.Quantity = inventoryItem.quantity;
            itemWorldSpace.InventoryItem = inventoryItem.item;
            

            Instantiate(DropItemPrefab,transform.position + new Vector3(4,0,0), Quaternion.identity);
            inventorySO.RemoveItem(itemIndex, quantity);
            inventoryUI.ResetSelection();
            audioSource.PlayOneShot(dropClip);
        }

        public void PerformAction(int itemIndex)
        {
            InventoryItem inventoryItem = inventorySO.GetItemAt(itemIndex);
            if (inventoryItem.IsEmpty)
            {
                return;
            }
            IDestroyableItem destroyableItem = inventoryItem.item as IDestroyableItem;
            if (destroyableItem != null)
            {
                inventorySO.RemoveItem(itemIndex, 1);
            }
            IItemAction itemAction = inventoryItem.item as IItemAction;
            if (itemAction != null)
            {
                itemAction.PerformAction(HurtBox, inventoryItem.itemState);
                audioSource.PlayOneShot(itemAction.actionSFX);
                if (inventorySO.GetItemAt(itemIndex).IsEmpty)
                {
                    inventoryUI.ResetSelection();
                }
                if(inventorySO.GetItemAt(itemIndex).item is EquippableItemSO)
                {
                    inventoryUI.ResetSelection();
                }
            }
        }

        private void HandleDragging(int itemIndex)
        {
            InventoryItem inventoryItem = inventorySO.GetItemAt(itemIndex);
            if (inventoryItem.IsEmpty)
            {
                return;
            }
            inventoryUI.CreateDraggedItem(inventoryItem.item.ItemImage, inventoryItem.quantity);
        }

        private void HandleSwapItems(int itemIndex_1, int itemIndex_2)
        {
            inventorySO.SwapItems(itemIndex_1, itemIndex_2);
        }

        private void HandleDescriptionRequest(int itemIndex)
        {
            InventoryItem inventoryItem = inventorySO.GetItemAt(itemIndex);
            if (inventoryItem.IsEmpty)
            {
                inventoryUI.ResetSelection();
                return;
            }
            ItemSO item = inventoryItem.item;
            string description = PrepareDescription(inventoryItem);
            inventoryUI.UpdateDescription(itemIndex, item.ItemImage,
                item.name, description);
        }

        private string PrepareDescription(InventoryItem inventoryItem)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(inventoryItem.item.Description);
            sb.AppendLine();
            for (int i = 0; i < inventoryItem.itemState.Count; i++)
            {
                sb.Append($"{inventoryItem.itemState[i].itemParameter.ParameterName} " +
                    $": {inventoryItem.itemState[i].value} / " +
                    $"{inventoryItem.item.DefaultParametersList[i].value}");
                sb.AppendLine();
            }
            return sb.ToString();
        }

        private void Update()
        {
            /*
            if (Input.GetKeyUp(KeyCode.I))
            {
                if (inventoryUI.isActiveAndEnabled == false)
                {
                    inventoryUI.Show();
                    foreach (var item in inventoryData.GetCurrentInventoryState())
                    {
                        inventoryUI.UpdateData(item.Key,
                            item.Value.item.ItemImage,
                            item.Value.quantity);
                    }
                }
                else
                {
                    inventoryUI.Hide();
                }
            }*/
        }

        internal UIInventoryPage GetUIInventoryPage()
        {
            return inventoryUI;
        }

    }
}