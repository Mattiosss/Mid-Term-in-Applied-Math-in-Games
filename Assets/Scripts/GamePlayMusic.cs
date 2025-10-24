using UnityEngine;

public class GameplayMusic : MonoBehaviour
{
    private static GameplayMusic instance;
    private AudioSource audioSource;

    [Header("Music Settings")]
    public AudioClip gameplayMusic;
    [Range(0f, 1f)] public float volume = 0.5f;

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        MenuMusic menuMusic = FindObjectOfType<MenuMusic>();
        if (menuMusic != null)
            Destroy(menuMusic.gameObject);

        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = gameplayMusic;
        audioSource.loop = true;
        audioSource.volume = volume;
        audioSource.playOnAwake = false;

        if (gameplayMusic != null)
            audioSource.Play();
    }
}
