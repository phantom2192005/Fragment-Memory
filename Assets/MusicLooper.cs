using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicLooper : MonoBehaviour
{
    public static MusicLooper Instance { get; private set; }

    [Header("Music Settings")]
    [SerializeField] private AudioClip mainMenuMusic;
    [SerializeField] private AudioClip mainGameMusic;
    [Range(0f, 1f)] public float volume = 0.2f;
    [SerializeField] private float delayBetweenLoops = 30f;

    private AudioSource audioSource;
    private Coroutine musicLoopCoroutine;
    private string currentSceneName;

    void Awake()
    {
        // Singleton implementation
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        InitializeAudioSource();
    }

    void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        HandleSceneChange(SceneManager.GetActiveScene());
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void InitializeAudioSource()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

        audioSource.volume = volume;
        audioSource.loop = false;
        audioSource.playOnAwake = false;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        HandleSceneChange(scene);
    }

    private void HandleSceneChange(Scene scene)
    {
        if (currentSceneName == scene.name) return;

        currentSceneName = scene.name;
        AudioClip newClip = GetMusicForScene(scene.name);

        if (newClip != audioSource.clip)
        {
            UpdateMusic(newClip);
        }
    }

    private AudioClip GetMusicForScene(string sceneName)
    {
        switch (sceneName)
        {
            case "MainMenu":
                return mainMenuMusic;
            case "MainGame":
                return mainGameMusic;
            default:
                return null;
        }
    }

    private void UpdateMusic(AudioClip newClip)
    {
        // Stop current playback
        if (musicLoopCoroutine != null)
        {
            StopCoroutine(musicLoopCoroutine);
        }

        audioSource.Stop();
        audioSource.clip = newClip;

        // Start new playback if valid clip
        if (newClip != null)
        {
            musicLoopCoroutine = StartCoroutine(MusicLoopRoutine());
        }
    }

    private IEnumerator MusicLoopRoutine()
    {
        while (true)
        {
            if (audioSource.clip == null)
            {
                Debug.LogWarning("No music clip assigned!");
                yield break;
            }

            audioSource.Play();
            yield return new WaitForSeconds(audioSource.clip.length + delayBetweenLoops);
        }
    }

    // Public method for manual music control
    public void ChangeMusic(AudioClip newClip)
    {
        UpdateMusic(newClip);
    }
}