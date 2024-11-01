using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Levels levels;

    public static GameManager Instance { get; private set; }

    public bool GameOver { get; private set; }

    public Level CurrentLevel
    {
        get
        {
            int currentLevelIndex = PlayerPrefs.GetInt("CurrentLevelIndex", 0);
            if (currentLevelIndex >= levels.Length)
                currentLevelIndex = levels.Length - 1;

            return levels.GetLevel(currentLevelIndex);
        }
    }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        StartCoroutine(StartLevel());
    }

    private IEnumerator StartLevel()
    {
        yield return new WaitForSeconds(1f);

        GameOver = false;

        LevelManager.Instance.SpawnLevel(CurrentLevel);
        Timer.Instance.StartTimer(CurrentLevel.timeLimit, OnLevelComplete);
        AudioManager.Instance.PlayRandomLevelMusic();
    }

    public void OnLevelComplete()
    {
        Timer.Instance.StopTimer();
        GameOver = true;

        AudioManager.Instance.StopAllSounds();

        if (LevelManager.Instance.AllCardsMatched())
            AudioManager.Instance.PlaySFX(AudioManager.Instance.winSFX);
        else
            AudioManager.Instance.PlaySFX(AudioManager.Instance.loseSFX);

        int currentLevelIndex = PlayerPrefs.GetInt("CurrentLevelIndex", 0);
        currentLevelIndex++;
        if (currentLevelIndex >= levels.Length)
            currentLevelIndex = 0;

        PlayerPrefs.SetInt("CurrentLevelIndex", currentLevelIndex);
        
        StartCoroutine(StartLevel());
    }
}
