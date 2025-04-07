using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabController : MonoBehaviour
{
    public GameObject[] pages;
    public GameObject actionButton;

    private void Start()
    {
        // Ẩn tất cả các page
        for (int i = 0; i < pages.Length; i++)
        {
            pages[i].SetActive(false);
        }

        // Hiện page thứ 2 (index = 1)
        if (pages.Length > 1)
        {
            pages[1].SetActive(true);
        }

        // Gọi thử hàm xóa con
        DeleteAllActionButton();
    }

    public void ActivateTab(int tab_index)
    {
        // Ẩn tất cả page
        for (int i = 0; i < pages.Length; i++)
        {
            pages[i].SetActive(false);
        }

        // Hiện page được chọn
        if (tab_index >= 0 && tab_index < pages.Length)
        {
            pages[tab_index].SetActive(true);
            DeleteAllActionButton();
        }
    }

    // Hàm xóa tất cả object con trực tiếp của actionButton
    public void DeleteAllActionButton()
    {
        if (actionButton == null) return;

        // Tạo danh sách tạm để tránh lỗi khi xóa trong lúc lặp
        List<Transform> children = new List<Transform>();
        foreach (Transform child in actionButton.transform)
        {
            children.Add(child);
        }

        // Xóa từng object con
        foreach (Transform child in children)
        {
            Destroy(child.gameObject);
        }
    }
}
