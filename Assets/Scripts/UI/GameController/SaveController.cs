using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using Inventory.Model;

public class SaveController : MonoBehaviour
{
    [SerializeField]
    private Image[] PressedBorder;
    [SerializeField]
    private Image[] PressedIcon;
    [SerializeField]
    private List<TMP_InputField> inputFields;
    public SaveFileSO saveFileSO;
    [SerializeField]
    private string lastSaveLocation;
    [SerializeField]
    private string currentFileNameInput;
    [SerializeField]
    private int currentSlot_index;
    [SerializeField]
    private InventorySaver inventorySaver;

    private string saveFileSOPath => Path.Combine(Application.persistentDataPath, "saveFileSO.json");

    private void Start()
    {
        LoadSaveFileSO(); // Tải dữ liệu SaveFileSO từ file JSON khi game bắt đầu

        for (int i = 0; i < inputFields.Count; i++)
        {
            AddEventTrigger(inputFields[i], i);
        }

        lastSaveLocation = saveFileSO.lastSaveLocation;
        LoadExsitFileName();
        LoadGame();
        inventorySaver.inventoryData_index = currentSlot_index;
        inventorySaver.LoadInventory();
    }

    void AddEventTrigger(TMP_InputField inputField, int index)
    {
        EventTrigger trigger = inputField.GetComponent<EventTrigger>();
        if (trigger == null)
        {
            trigger = inputField.gameObject.AddComponent<EventTrigger>();
        }

        EventTrigger.Entry entry = new EventTrigger.Entry
        {
            eventID = EventTriggerType.Select
        };
        entry.callback.AddListener((data) => OnInputFieldSelect(index));

        trigger.triggers.Add(entry);
    }

    void OnInputFieldSelect(int index)
    {
        currentSlot_index = index;
        var placeholder = inputFields[currentSlot_index].placeholder as TextMeshProUGUI;
        string fileName = Path.GetFileNameWithoutExtension(saveFileSO.saveLocations[currentSlot_index]);
        placeholder.text = fileName;

        Debug.Log("Chọn ô input: " + currentSlot_index);
        foreach (Image image in PressedIcon)
        {
            image.enabled = false;
        }
        foreach (Image image in PressedBorder)
        {
            image.enabled = false;
        }
        PressedIcon[currentSlot_index].enabled = true;
        PressedBorder[currentSlot_index].enabled = true;
        currentFileNameInput = Path.GetFileNameWithoutExtension(saveFileSO.saveLocations[currentSlot_index]);
        inventorySaver.inventoryData_index = currentSlot_index;
    }

    public void ReadFileNameInput(string input)
    {
        currentFileNameInput = input;
    }

    public void SaveGame()
    {
        SaveData saveData = new SaveData
        {
            playerPosition = GameObject.FindGameObjectWithTag("Player").transform.position,
        };

        string oldPath = saveFileSO.saveLocations[currentSlot_index];
        if (!string.IsNullOrEmpty(oldPath) && File.Exists(oldPath))
        {
            File.Delete(oldPath);
        }

        string newSaveLocation = Path.Combine(Application.persistentDataPath, currentFileNameInput + ".json");
        saveFileSO.saveLocations[currentSlot_index] = newSaveLocation;
        saveFileSO.lastSlotIndex = currentSlot_index;
        saveFileSO.lastFileName = currentFileNameInput;
        saveFileSO.lastSaveLocation = newSaveLocation;

        File.WriteAllText(newSaveLocation, JsonUtility.ToJson(saveData));
        SaveSaveFileSO();  // Lưu SaveFileSO vào file JSON sau khi lưu game
        inventorySaver.SaveInventory();

        LoadExsitFileName();
    }

    public void LoadExsitFileName()
    {
        int index = 0;
        foreach (string save_Location in saveFileSO.saveLocations)
        {
            if (index >= inputFields.Count) break; // Ngăn lỗi vượt quá chỉ số

            var placeholder = inputFields[index].placeholder as TextMeshProUGUI;

            if (save_Location == null)
            {
                placeholder.text = null;
            }
            else
            {
                string fileName = Path.GetFileNameWithoutExtension(save_Location);
                placeholder.text = fileName;
            }

            index++;
        }
    }

    public void LoadFileSave()
    {
        lastSaveLocation = saveFileSO.saveLocations[currentSlot_index];
        saveFileSO.lastSlotIndex = currentSlot_index;
        saveFileSO.lastSaveLocation = lastSaveLocation;

        if (File.Exists(lastSaveLocation))
        {
            SaveData saveData = JsonUtility.FromJson<SaveData>(File.ReadAllText(lastSaveLocation));
            GameObject.FindGameObjectWithTag("Player").transform.position = saveData.playerPosition;

            for (int i = 0; i < saveFileSO.saveLocations.Length; i++)
            {
                if (saveFileSO.saveLocations[i] == lastSaveLocation)
                {
                    currentFileNameInput = Path.GetFileNameWithoutExtension(saveFileSO.saveLocations[i]);
                    saveFileSO.lastFileName = currentFileNameInput;
                    inventorySaver.LoadInventory();
                }
            }
        }
        else
        {
            Debug.Log("Find no file save");
        }
    }

    public void LoadGame()
    {
        if (File.Exists(lastSaveLocation))
        {
            SaveData saveData = JsonUtility.FromJson<SaveData>(File.ReadAllText(lastSaveLocation));
            GameObject.FindGameObjectWithTag("Player").transform.position = saveData.playerPosition;

            for (int i = 0; i < saveFileSO.saveLocations.Length; i++)
            {
                if (saveFileSO.saveLocations[i] == lastSaveLocation)
                {
                    currentSlot_index = i;
                    currentFileNameInput = Path.GetFileNameWithoutExtension(saveFileSO.saveLocations[i]);
                    PressedIcon[currentSlot_index].enabled = true;
                    PressedBorder[currentSlot_index].enabled = true;
                }
            }
        }
        else
        {
            Debug.Log("Find no file save");
        }
    }

    // Lưu SaveFileSO vào file JSON
    private void SaveSaveFileSO()
    {
        SaveFileData data = ConvertSOToData();
        string json = JsonUtility.ToJson(data, true); // true để format dễ đọc
        File.WriteAllText(saveFileSOPath, json);
    }

    // Tải SaveFileSO từ file JSON
    private void LoadSaveFileSO()
    {
        if (File.Exists(saveFileSOPath))
        {
            string json = File.ReadAllText(saveFileSOPath);
            SaveFileData data = JsonUtility.FromJson<SaveFileData>(json);
            ApplyDataToSO(data);
        }
        else
        {
            Debug.Log("No save file found, creating a new one with default values.");

            // Nếu không có file, tạo dữ liệu mặc định và lưu vào file JSON
            CreateDefaultSaveFileSO();
        }
    }
    // Tạo file SaveFileSO mới với dữ liệu mặc định
    private void CreateDefaultSaveFileSO()
    {
        saveFileSO.saveLocations = new string[3];  // Ví dụ, bạn có 3 vị trí lưu game
        saveFileSO.lastSaveLocation = "";
        saveFileSO.lastFileName = "NewGame";
        saveFileSO.lastSlotIndex = 0;

        // Lưu dữ liệu mặc định vào file JSON
        SaveSaveFileSO();
    }

    // Chuyển SaveFileSO thành SaveFileData
    private SaveFileData ConvertSOToData()
    {
        return new SaveFileData
        {
            saveLocations = saveFileSO.saveLocations,
            lastFileName = saveFileSO.lastFileName,
            lastSaveLocation = saveFileSO.lastSaveLocation,
            lastSlotIndex = saveFileSO.lastSlotIndex
        };
    }

    // Áp dụng dữ liệu từ SaveFileData vào SaveFileSO
    private void ApplyDataToSO(SaveFileData data)
    {
        saveFileSO.saveLocations = data.saveLocations;
        saveFileSO.lastFileName = data.lastFileName;
        saveFileSO.lastSaveLocation = data.lastSaveLocation;
        saveFileSO.lastSlotIndex = data.lastSlotIndex;
    }
}

// Class để lưu trữ dữ liệu trong SaveFileSO
[System.Serializable]
public class SaveFileData
{
    public string[] saveLocations;
    public string lastSaveLocation;
    public string lastFileName;
    public int lastSlotIndex;
}