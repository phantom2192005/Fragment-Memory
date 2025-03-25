using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    [SerializeField]
    private UIInventoryPage inventoryUI;
    public int inventorySize = 10;


    private void Start()
    {
        inventoryUI.InitializeIventoryUI(inventorySize);
    }
    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.I))
        {
            Debug.Log("Input I is pressed");
            if (inventoryUI.isActiveAndEnabled == false)
            {
                inventoryUI.Show();
            }
            else
            {
                inventoryUI.Hide();
            }
        }
    }
}
