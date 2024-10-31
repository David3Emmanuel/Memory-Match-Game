using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Level[] levels;

    public static GameManager Instance { get; private set; }

    public bool GameOver { get; private set; }

    public Level CurrentLevel
    {
        get
        {
            int currentLevelIndex = PlayerPrefs.GetInt("CurrentLevelIndex", 0);
            if (currentLevelIndex >= levels.Length)
                currentLevelIndex = levels.Length - 1;

            return levels[currentLevelIndex];
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
        LevelManager.Instance.SpawnLevel(CurrentLevel);
        Timer.Instance.StartTimer(CurrentLevel.timeLimit, OnLevelComplete);
    }

    public void OnLevelComplete()
    {
        Timer.Instance.StopTimer();
        GameOver = true;

        int currentLevelIndex = PlayerPrefs.GetInt("CurrentLevelIndex", 0);
        currentLevelIndex++;
        if (currentLevelIndex >= levels.Length)
            currentLevelIndex = 0;

        PlayerPrefs.SetInt("CurrentLevelIndex", currentLevelIndex);
        LevelManager.Instance.SpawnLevel(levels[currentLevelIndex]);
        Timer.Instance.StartTimer(CurrentLevel.timeLimit, OnLevelComplete);
        GameOver = false;
    }
}
