using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController2 : MonoBehaviour
{
    public GameObject inventoryPanel;
    public GameObject slotPrefab;
    public int slotCount;
    public GameObject[] itemPrefabs;
    void Start()
    {
        for(int i = 0;i< slotCount; i++)
        {
            
            Slot slot = Instantiate(slotPrefab,inventoryPanel.transform).GetComponent<Slot>();
            if(i < itemPrefabs.Length)
            {
                GameObject item = Instantiate(itemPrefabs[i], slot.transform);
                item.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                slot.currentItem = item;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
