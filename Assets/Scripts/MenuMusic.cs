using UnityEngine;

public class MenuMusic : MonoBehaviour
{
    private static MenuMusic instance;
    private AudioSource audioSource;

    [Header("Music Settings")]
    public AudioClip menuMusic;
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

        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = menuMusic;
        audioSource.loop = true;
        audioSource.volume = volume;
        audioSource.playOnAwake = false;

        if (menuMusic != null)
            audioSource.Play();
    }
}
