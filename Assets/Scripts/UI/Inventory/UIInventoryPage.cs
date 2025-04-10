using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Inventory.UI
{
    public class UIInventoryPage : MonoBehaviour
    {
        [SerializeField]
        private UIInventoryItem itemPrefab;

        [SerializeField]
        private RectTransform contentPanel;

        [SerializeField]
        private UIInventoryDescription itemDescription;

        [SerializeField]
        private MouseFollower mouseFollower;

        public List<UIInventoryItem> listOfUIItems = new List<UIInventoryItem>();

        private int currentlyDraggedItemIndex = -1;

        public event Action<int> OnDescriptionRequested,
            OnItemActionRequested,
            OnStartDragging;

        public event Action<int, int> OnSwapItems;

        [SerializeField]
        private ItemActionPanel actionPanel;


        public void Awake()
        {
            //Hide();
            mouseFollower.Toggle(false);
            itemDescription.ResetDescription();
        }
        public void InitializeIventoryUI(int inventorySize)
        {
            for (int i = 0; i < inventorySize; i++)
            {
                UIInventoryItem uiItem = Instantiate(itemPrefab, Vector3.zero, Quaternion.identity);
                uiItem.transform.SetParent(contentPanel.transform);
                listOfUIItems.Add(uiItem);

                uiItem.OnItemClicked += HandleItemSelection;
                uiItem.OnItemBeginDrag += HandleBeginDrag;
                uiItem.OnItemDroppedOn += HandleSwap;
                uiItem.OnItemEndDrag += HandleEndDrag;
                uiItem.OnLeftMouseBtnClick += HandleShowItemActions;
            }
        }
        internal void UpdateDescription(int itemIndex, Sprite itemImage, string name, string description)
        {
            itemDescription.SetDescription(itemImage, name, description);
            DeselectAllItems();
            listOfUIItems[itemIndex].Select();


        }
        public void UpdateData(int itemIndex, Sprite itemImage, int itemQuantity)
        {
            if (listOfUIItems.Count > itemIndex)
            {
                listOfUIItems[itemIndex].SetData(itemImage, itemQuantity);
            }
        }

        private void HandleShowItemActions(UIInventoryItem inventoryItemUI)
        {
            int index = listOfUIItems.IndexOf(inventoryItemUI);
            if (index == -1) return;
            OnItemActionRequested?.Invoke(index);
        }

        private void HandleEndDrag(UIInventoryItem inventoryItemUI)
        {

            ResetDragItem();
        }

        private void HandleSwap(UIInventoryItem inventoryItemUI)
        {
            int index = listOfUIItems.IndexOf(inventoryItemUI);
            if (currentlyDraggedItemIndex == -1) {return;}
                
            OnSwapItems?.Invoke(currentlyDraggedItemIndex, index);
            HandleItemSelection(inventoryItemUI);
        }

        private void ResetDragItem()
        {
            mouseFollower.Toggle(false);
            currentlyDraggedItemIndex = -1;
        }

        private void HandleBeginDrag(UIInventoryItem inventoryItemUI)
        {
            int index = listOfUIItems.IndexOf(inventoryItemUI);
            if (index == -1) return;
            currentlyDraggedItemIndex = index;
            HandleItemSelection(inventoryItemUI);
            OnStartDragging?.Invoke(index);
        }

        public void CreateDraggedItem(Sprite spirte, int quantity)
        {
            mouseFollower.Toggle(true);
            mouseFollower.SetData(spirte, quantity);
        }

        private void HandleItemSelection(UIInventoryItem inventoryItemUI)
        {
            int index = listOfUIItems.IndexOf(inventoryItemUI);
            if (index == -1) return;
            OnDescriptionRequested?.Invoke(index);
        }

        public void Show()
        {
            gameObject.SetActive(true);
            ResetSelection();
        }

        public void ResetSelection()
        {
            itemDescription.ResetDescription();
            DeselectAllItems();
        }

        public void ShowItemAction(int itemIndex)
        {
            actionPanel.Toggle(true);
           // actionPanel.transform.position = listOfUIItems[itemIndex].transform.position; 
        }

        public void AddAction(string actionName, Action onPointerDown, Action onPointerUp, Action performAction)
        {
            actionPanel.AddButton(actionName, onPointerDown, onPointerUp, performAction);
        }

        void DeselectAllItems()
        {
            foreach (UIInventoryItem item in listOfUIItems)
            {
                item.Deselect();
            }
            actionPanel.Toggle(false);
        }

        public void Hide()
        {
            actionPanel.Toggle(false);
            gameObject.SetActive(false);
            ResetDragItem();
        }

        public void ResetAllItems()
        {
            foreach (var item in listOfUIItems)
            {
                if (item != null) // Thêm kiểm tra null
                {
                    item.ResetData();
                    item.Deselect();
                }
            }
        }

        public void ClearAllItems()
        {
            // Thay đổi cách xử lý - không destroy items
            foreach (var item in listOfUIItems)
            {
                if (item != null)
                {
                    item.ResetData();
                    item.Deselect();
                }
            }
        }

        public void InitializeInventoryUI(int inventorySize)
        {
            // Xóa các items null trước khi khởi tạo
            listOfUIItems.RemoveAll(item => item == null);

            if (listOfUIItems.Count >= inventorySize) return;

            int itemsToCreate = inventorySize - listOfUIItems.Count;
            for (int i = 0; i < itemsToCreate; i++)
            {
                UIInventoryItem uiItem = Instantiate(itemPrefab, contentPanel.transform);
                uiItem.gameObject.SetActive(true); // Đảm bảo item được kích hoạt
                listOfUIItems.Add(uiItem);
                // ... các event handlers
            }
        }

    }
}