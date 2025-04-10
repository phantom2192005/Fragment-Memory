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
        AudioSource.PlayClipAtPoint(SFXManager.Instance.SFX["Open Menu"], Camera.main.transform.position);
        Time.timeScale = 0f; // Dừng thời gian
        isPaused = true;
    }

    public void Hide()
    {
        InGameMenu.SetActive(false);
        Time.timeScale = 1f; // Tiếp tục thời gian
        isPaused = false;
        AudioSource.PlayClipAtPoint(SFXManager.Instance.SFX["Close Menu"], Camera.main.transform.position);
    }

    private void Start()
    {
        if(InGameMenu != null)
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

