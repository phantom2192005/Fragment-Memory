using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabController : MonoBehaviour
{
    public GameObject[] pages;

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
        }
    }
}
