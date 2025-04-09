using UnityEngine;

[CreateAssetMenu(fileName = "SaveFileData", menuName = "Save System/Save File Data")]
public class SaveFileSO : ScriptableObject
{
    public string lastSaveLocation;
    public string lastFileName;
    public int lastSlotIndex;
    public string[] saveLocations;
}
