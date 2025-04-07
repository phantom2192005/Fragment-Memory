using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Inventory.UI
{
    public class UIInventoryDescription : MonoBehaviour
    {
        [SerializeField]
        private Image itemImage;
        [SerializeField]
        private TMP_Text title;
        [SerializeField]
        private TMP_Text description;
        private int currentPage = 1;



        public void Awake()
        {
            ResetDescription();
        }

        public void ResetDescription()
        {
            itemImage.gameObject.SetActive(false);
            title.text = "";
            description.text = "";
        }

        public void SetDescription(Sprite sprite, string itemName, string itemDescription)
        {
            itemImage.gameObject.SetActive(true);
            itemImage.sprite = sprite;
            title.text = itemName;
            description.text = itemDescription;

            currentPage = 1;
            description.pageToDisplay = currentPage;
        }


        public void TurnNextPageDescription()
        {
            if (currentPage < description.textInfo.pageCount)
            {
                currentPage++;
                description.pageToDisplay = currentPage;
            }
        }

        public void TurnPreviousPageDescription()
        {
            if (currentPage > 1)
            {
                currentPage--;
                description.pageToDisplay = currentPage;
            }
        }

    }
}