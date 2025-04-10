using Inventory.Model;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.SceneManagement;

[System.Serializable]
public class SaveFileData
{
    public string[] saveLocations;
    public string lastSaveLocation;
    public string lastFileName;
    public int lastSlotIndex;
}

public class SaveController : MonoBehaviour
{
    [SerializeField] private Image[] PressedBorder;
    [SerializeField] private Image[] PressedIcon;
    [SerializeField] private List<TMP_InputField> inputFields;
    public SaveFileSO saveFileSO;
    [SerializeField] private string lastSaveLocation;
    [SerializeField] private string currentFileNameInput;
    [SerializeField] private int currentSlot_index;
    [SerializeField] private InventorySaver inventorySaver;

    private string saveFileSOPath => Path.Combine(Application.persistentDataPath, "saveFileSO.json");

    private void Start()
    {
        LoadSaveFileSO();

        for (int i = 0; i < inputFields.Count; i++)
        {
            AddEventTrigger(inputFields[i], i);
        }

        lastSaveLocation = saveFileSO.lastSaveLocation;
        LoadExsitFileName();
        LoadGame();
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            LoadFileSave();
            LoadFileSave();
        }
    }

    void AddEventTrigger(TMP_InputField inputField, int index)
    {
        EventTrigger trigger = inputField.GetComponent<EventTrigger>() ?? inputField.gameObject.AddComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry { eventID = EventTriggerType.Select };
        entry.callback.AddListener((data) => OnInputFieldSelect(index));
        trigger.triggers.Add(entry);
    }

    void OnInputFieldSelect(int index)
    {
        currentSlot_index = index;
        var placeholder = inputFields[currentSlot_index].placeholder as TextMeshProUGUI;
        string fileName = Path.GetFileNameWithoutExtension(saveFileSO.saveLocations[currentSlot_index]);
        placeholder.text = fileName;

        foreach (Image image in PressedIcon) image.enabled = false;
        foreach (Image image in PressedBorder) image.enabled = false;
        PressedIcon[currentSlot_index].enabled = true;
        PressedBorder[currentSlot_index].enabled = true;
        currentFileNameInput = Path.GetFileNameWithoutExtension(saveFileSO.saveLocations[currentSlot_index]);
        inventorySaver.inventoryData_index = currentSlot_index;
    }

    public void ReadFileNameInput(string input) => currentFileNameInput = input;

    public void SaveGame()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        Transform hurtBox = player.transform.Find("HurtBox");

        if (hurtBox == null) { Debug.LogError("Không tìm thấy HurtBox trong Player!"); return; }

        Health health = hurtBox.GetComponent<Health>();
        Stamina stamina = hurtBox.GetComponent<Stamina>();

        SaveData saveData = new SaveData
        {
            playerPosition = player.transform.position,
            currentHealth = health.currentHealth,
            maxHealth = health.maxHealth,
            currentStamina = stamina.currentStamina,
            maxStamina = stamina.maxStamina
        };

        string oldPath = saveFileSO.saveLocations[currentSlot_index];
        if (!string.IsNullOrEmpty(oldPath) && File.Exists(oldPath)) File.Delete(oldPath);

        string newSaveLocation = Path.Combine(Application.persistentDataPath, currentFileNameInput + ".json");
        saveFileSO.saveLocations[currentSlot_index] = newSaveLocation;
        saveFileSO.lastSlotIndex = currentSlot_index;
        saveFileSO.lastFileName = currentFileNameInput;
        saveFileSO.lastSaveLocation = newSaveLocation;

        File.WriteAllText(newSaveLocation, JsonUtility.ToJson(saveData));
        SaveSaveFileSO();
        inventorySaver.SaveInventory();
        LoadExsitFileName();
    }

    public void LoadExsitFileName()
    {
        for (int index = 0; index < inputFields.Count; index++)
        {
            var placeholder = inputFields[index].placeholder as TextMeshProUGUI;
            if (index >= saveFileSO.saveLocations.Length || string.IsNullOrEmpty(saveFileSO.saveLocations[index]))
            {
                placeholder.text = null;
            }
            else
            {
                string fileName = Path.GetFileNameWithoutExtension(saveFileSO.saveLocations[index]);
                placeholder.text = fileName;
            }
        }
    }

    public void LoadFileSave()
    {
        lastSaveLocation = saveFileSO.saveLocations[currentSlot_index];
        saveFileSO.lastSlotIndex = currentSlot_index;
        saveFileSO.lastSaveLocation = lastSaveLocation;

        if (!File.Exists(lastSaveLocation)) { Debug.Log("Find no file save"); return; }

        SaveData saveData = JsonUtility.FromJson<SaveData>(File.ReadAllText(lastSaveLocation));
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            player.transform.position = saveData.playerPosition;
            Transform hurtBox = player.transform.Find("HurtBox");
            if (hurtBox != null)
            {
                Health health = hurtBox.GetComponent<Health>();
                Stamina stamina = hurtBox.GetComponent<Stamina>();

                if (health != null)
                {
                    health.currentHealth = saveData.currentHealth;
                    health.maxHealth = saveData.maxHealth;
                    health.healthBar.SetValue((int)health.currentHealth);
                }

                if (stamina != null)
                {
                    stamina.currentStamina = saveData.currentStamina;
                    stamina.maxStamina = saveData.maxStamina;
                    stamina.staminaBar.SetValue(stamina.currentStamina);
                }
            }
            else
            {
                Debug.LogWarning("Không tìm thấy HurtBox trong Player!");
            }
        }

        for (int i = 0; i < saveFileSO.saveLocations.Length; i++)
        {
            if (saveFileSO.saveLocations[i] == lastSaveLocation)
            {
                currentFileNameInput = Path.GetFileNameWithoutExtension(saveFileSO.saveLocations[i]);
                saveFileSO.lastFileName = currentFileNameInput;
            }
        }

        inventorySaver.inventoryData_index = currentSlot_index;
        inventorySaver.LoadInventory();
    }

    public void LoadGame()
    {
        if (!File.Exists(lastSaveLocation)) { Debug.Log("Find no file save"); return; }

        SaveData saveData = JsonUtility.FromJson<SaveData>(File.ReadAllText(lastSaveLocation));
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            player.transform.position = saveData.playerPosition;
            Transform hurtBox = player.transform.Find("HurtBox");
            if (hurtBox != null)
            {
                Health health = hurtBox.GetComponent<Health>();
                Stamina stamina = hurtBox.GetComponent<Stamina>();

                if (health != null)
                {
                    health.currentHealth = saveData.currentHealth;
                    health.maxHealth = saveData.maxHealth;
                    health.healthBar.SetValue((int)health.currentHealth);
                }

                if (stamina != null)
                {
                    stamina.currentStamina = saveData.currentStamina;
                    stamina.maxStamina = saveData.maxStamina;
                    stamina.staminaBar.SetValue(stamina.currentStamina);
                }
            }
            else
            {
                Debug.LogWarning("Không tìm thấy HurtBox trong Player!");
            }

            for (int i = 0; i < saveFileSO.saveLocations.Length; i++)
            {
                if (saveFileSO.saveLocations[i] == lastSaveLocation)
                {
                    currentSlot_index = i;
                    currentFileNameInput = Path.GetFileNameWithoutExtension(saveFileSO.saveLocations[i]);
                    if (PressedIcon.Length == 0) return;
                    PressedIcon[currentSlot_index].enabled = true;
                    PressedBorder[currentSlot_index].enabled = true;
                    inventorySaver.inventoryData_index = currentSlot_index;
                }
            }
        }
        else
        {
            Debug.LogWarning("Không tìm thấy Player trong scene!");
        }
    }

    private void SaveSaveFileSO()
    {
        SaveFileData data = ConvertSOToData();
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(saveFileSOPath, json);
    }

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
            CreateDefaultSaveFileSO();
        }
    }

    private void CreateDefaultSaveFileSO()
    {
        saveFileSO.saveLocations = new string[3];
        saveFileSO.lastSaveLocation = "";
        saveFileSO.lastFileName = "NewGame";
        saveFileSO.lastSlotIndex = 0;
        SaveSaveFileSO();
    }

    private SaveFileData ConvertSOToData() => new SaveFileData
    {
        saveLocations = saveFileSO.saveLocations,
        lastFileName = saveFileSO.lastFileName,
        lastSaveLocation = saveFileSO.lastSaveLocation,
        lastSlotIndex = saveFileSO.lastSlotIndex
    };

    private void ApplyDataToSO(SaveFileData data)
    {
        saveFileSO.saveLocations = data.saveLocations;
        saveFileSO.lastFileName = data.lastFileName;
        saveFileSO.lastSaveLocation = data.lastSaveLocation;
        saveFileSO.lastSlotIndex = data.lastSlotIndex;
    }

    public void DeleteAllSaves()
    {
        if (File.Exists(saveFileSOPath)) File.Delete(saveFileSOPath);

        foreach (string savePath in saveFileSO.saveLocations)
        {
            if (!string.IsNullOrEmpty(savePath) && File.Exists(savePath)) File.Delete(savePath);
        }

        saveFileSO.saveLocations = new string[3];
        saveFileSO.lastSaveLocation = string.Empty;
        saveFileSO.lastFileName = "NewGame";
        saveFileSO.lastSlotIndex = 0;

        Debug.Log("Đã xóa toàn bộ dữ liệu save!");
    }
}
