using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Level[] levels;

    public static GameManager Instance { get; private set; }

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
    }

    public void OnLevelComplete()
    {
        int currentLevelIndex = PlayerPrefs.GetInt("CurrentLevelIndex", 0);
        currentLevelIndex++;
        if (currentLevelIndex < levels.Length)
        {
            PlayerPrefs.SetInt("CurrentLevelIndex", currentLevelIndex);
            LevelManager.Instance.SpawnLevel(levels[currentLevelIndex]);
        }
        else
        {
            Debug.Log("All levels completed!");
        }
    }
}
