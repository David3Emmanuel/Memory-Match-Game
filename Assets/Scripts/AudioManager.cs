using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [SerializeField] private AudioClip mainMenuBackgroundMusic;
    [SerializeField] private AudioClip[] levelBackgroundMusic;

    public AudioClip winSFX, loseSFX, clickSFX, matchSFX;

    private AudioSource audioSource;
    [SerializeField] private AudioSource sfxAudioSource;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        else if (Instance != this)
            Destroy(gameObject);
    }

    public void PlayMainMenuMusic()
    {
        audioSource.clip = mainMenuBackgroundMusic;
        audioSource.Play();
    }

    public void PlayRandomLevelMusic()
    {
        int randomIndex = Random.Range(0, levelBackgroundMusic.Length);
        audioSource.clip = levelBackgroundMusic[randomIndex];
        audioSource.Play();
    }

    public void StopAllSounds()
    {
        audioSource.Stop();
    }

    public void PlaySFX(AudioClip clip)
    {
        sfxAudioSource.clip = clip;
        sfxAudioSource.Play();
    }
}