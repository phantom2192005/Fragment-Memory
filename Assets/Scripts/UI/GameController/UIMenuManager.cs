using Inventory.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMenuManager : MonoBehaviour
{
    [SerializeField]
    private GameObject InGameMenu;
    public void Show()
    {
        InGameMenu.SetActive(true);
    }

    public void Hide()
    {
        InGameMenu.SetActive(false);
    }
    private void Start()
    {
        InGameMenu.SetActive(false);
    }
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            if (!InGameMenu.activeSelf)
            {
                Show();
                
            }
            else
            {
                Hide();
            }
        }
    }
}
