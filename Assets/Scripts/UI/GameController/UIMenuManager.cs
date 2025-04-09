using Inventory.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMenuManager : MonoBehaviour
{
    [SerializeField]
    private GameObject InGameMenu;

    private bool isPaused = false;

    public void Show()
    {
        InGameMenu.SetActive(true);
        Time.timeScale = 0f; // Dừng thời gian
        isPaused = true;
    }

    public void Hide()
    {
        InGameMenu.SetActive(false);
        Time.timeScale = 1f; // Tiếp tục thời gian
        isPaused = false;
    }

    private void Start()
    {
        InGameMenu.SetActive(false);
        Time.timeScale = 1f; // Đảm bảo thời gian chạy bình thường lúc khởi động
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            if (!isPaused)
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

