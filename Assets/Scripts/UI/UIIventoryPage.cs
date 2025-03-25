using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIInventoryPage : MonoBehaviour
{
    [SerializeField]
    private UIIventoryItem itemPrefab;

    [SerializeField]
    private RectTransform contentPanel;

    List<UIIventoryItem> listOfUIItems = new List<UIIventoryItem>();

    public void InitializeIventoryUI(int inventorySize)
    {
        for (int i = 0; i < inventorySize; i++)
        {
            UIIventoryItem uiItem = Instantiate(itemPrefab, Vector3.zero, Quaternion.identity);
            uiItem.transform.SetParent(contentPanel.transform);
            listOfUIItems.Add(uiItem);
        }
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide() 
    {
        gameObject.SetActive(false);
    }
}
