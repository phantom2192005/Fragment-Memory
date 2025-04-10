using UnityEngine;
using System.Collections.Generic;
public class SFXManager : MonoBehaviour
{
    public static SFXManager Instance; // Thêm dòng này

    [System.Serializable]
    public class SFXPair
    {
        public string key;
        public AudioClip clip;
    }

    [SerializeField] private List<SFXPair> sfxList = new List<SFXPair>();
    public Dictionary<string, AudioClip> SFX = new Dictionary<string, AudioClip>();

    private void Awake()
    {
        // Singleton setup
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Tuỳ chọn: Giữ lại khi chuyển scene
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Khởi tạo Dictionary
        foreach (var pair in sfxList)
        {
            if (!SFX.ContainsKey(pair.key))
            {
                SFX.Add(pair.key, pair.clip);
            }
        }
    }
}