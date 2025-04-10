using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Inventory.UI
{
    public class UIInventoryItem : MonoBehaviour, IPointerClickHandler
        , IBeginDragHandler, IEndDragHandler, IDropHandler, IDragHandler
    {
        [SerializeField]
        private Image itemImage;

        [SerializeField]
        private TMP_Text quantityTxt;

        [SerializeField]
        private Image borderImage;

        public event Action<UIInventoryItem> OnItemClicked,
            OnItemDroppedOn, OnItemBeginDrag, OnItemEndDrag
            , OnLeftMouseBtnClick;

        private bool empty = true;

        public void Awake()
        {
            ResetData();
            Deselect();
        }

        public void ResetData()
        {
            if (itemImage != null) // Thêm kiểm tra null
            {
                itemImage.gameObject.SetActive(false);
            }
            if (quantityTxt != null) // Thêm kiểm tra null
            {
                quantityTxt.text = "";
            }
            empty = true;
        }

        public void Deselect()
        {
            if (borderImage != null) // Thêm kiểm tra null
            {
                borderImage.enabled = false;
            }
        }

        public void SetData(Sprite sprite, int quantity)
        {
            try
            {
                if (itemImage != null)
                {
                    itemImage.gameObject.SetActive(true);
                    itemImage.sprite = sprite;
                }

                if (quantityTxt != null)
                {
                    quantityTxt.text = "x" + quantity;
                    quantityTxt.gameObject.SetActive(quantity > 1);
                }

                empty = false;
            }
            catch (MissingReferenceException)
            {
                // Xử lý trường hợp component bị destroy
                ReinitializeComponents();
            }
        }

        private void ReinitializeComponents()
        {
            itemImage = GetComponentInChildren<Image>();
            quantityTxt = GetComponentInChildren<TMP_Text>();
            borderImage = GetComponent<Image>();

            if (itemImage != null) itemImage.gameObject.SetActive(false);
            if (quantityTxt != null) quantityTxt.text = "";
        }

        public void Select()
        {
            borderImage.enabled = true;
        }

        public void OnPointerClick(PointerEventData pointerEventData)
        {
            if (pointerEventData.button == PointerEventData.InputButton.Left)
            { 
                OnItemClicked?.Invoke(this);
                OnLeftMouseBtnClick?.Invoke(this);
               
            }
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (empty) { return; }
            OnItemBeginDrag?.Invoke(this);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            OnItemEndDrag?.Invoke(this);
        }

        public void OnDrop(PointerEventData eventData)
        {
            OnItemDroppedOn?.Invoke(this);
        }

        public void OnDrag(PointerEventData eventData)
        {

        }
    }
}