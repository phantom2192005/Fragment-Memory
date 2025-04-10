using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingPage : MonoBehaviour
{
    [SerializeField] private GameObject settingPage;


    public void ToggleSettingPage(bool state)
    {
        if (settingPage != null)
            settingPage.SetActive(state);
    }

    public void Home()
    {
        if (SceneManager.GetActiveScene().name != "MainMenu")
        {
            SceneManager.LoadScene("MainMenu");

        }
    }

    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // Cho Editor
#else
        Application.Quit(); // Cho build thật
#endif
    }
}
