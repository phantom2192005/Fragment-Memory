using Inventory.Model;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Inventory;

public class InventorySaver : MonoBehaviour
{
    public InventoryController InventoryController;
    public InventorySO InventoryData;
    public int inventoryData_index;

    // Hàm lưu danh sách InventoryItem vào file JSON
    public void SaveInventory()
    {
        InventoryItemListWrapper wrapper = new InventoryItemListWrapper();
        wrapper.inventoryItems = InventoryData.inventoryItems;
        wrapper.size = InventoryData.Size; // Gán size vào wrapper

        string json = JsonUtility.ToJson(wrapper, true);
        string path = Application.persistentDataPath + "/inventory" + inventoryData_index + ".json";
        File.WriteAllText(path, json);

        Debug.Log("Inventory saved to " + path);
    }

    // Hàm tải danh sách InventoryItem từ file JSON
    public List<InventoryItem> LoadInventory()
    {
        string path = Application.persistentDataPath + "/inventory" + inventoryData_index + ".json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            InventoryItemListWrapper wrapper = JsonUtility.FromJson<InventoryItemListWrapper>(json);

            InventoryController.GetUIInventoryPage().ClearAllItems();
            InventoryController.PrepareInventory();
            InventoryData.Size = wrapper.size; // Load lại size từ file
            InventoryData.inventoryItems = wrapper.inventoryItems;
            InventoryData.InformAboutChange();
            Debug.Log("Inventory loaded from " + path);
            return wrapper.inventoryItems;
        }
        else
        {
            Debug.LogWarning("No inventory file found at " + path);
            SaveInventory();
            return new List<InventoryItem>();
        }
    }
    private string GetInventoryFilePath()
    {
        return Path.Combine(Application.persistentDataPath, $"inventory_slot{inventoryData_index}.json");
    }

}