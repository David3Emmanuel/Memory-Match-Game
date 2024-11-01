using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public static MainMenu Instance { get; private set; }

    [SerializeField] private Levels levels;
    [SerializeField] private GameObject levelIconPrefab;
    [SerializeField] private float verticalSpacing = 1.5f;
    [SerializeField] private float horizontalOffset = 1f;
    [SerializeField] private float moveSpeed = 5f;

    private Vector2 targetPosition;

    private void Awake()
    {
        Instance = this;
        targetPosition = transform.localPosition;

        int currentLevelIndex = PlayerPrefs.GetInt("CurrentLevelIndex", 0);

        for (int i = 0; i < levels.Length; i++)
        {
            // Calculate position
            float spawnX = (i % 3 - 1) * horizontalOffset;
            float  spawnY = (i - currentLevelIndex) * verticalSpacing;
            Vector2 position = new Vector2(spawnX, spawnY);

            // Spawn level icon
            GameObject iconObject = Instantiate(levelIconPrefab, transform);
            iconObject.transform.localPosition = position;

            // Set level info
            LevelIcon icon = iconObject.GetComponent<LevelIcon>();
            Level level = levels.GetLevel(i);
            icon.SetLevel(level, i, i == currentLevelIndex);
        }
    }

    private void Update()
    {
        // Lerp to the targetPosition
        transform.localPosition = Vector2.Lerp(transform.localPosition, targetPosition, Time.deltaTime * moveSpeed);
    }

    public void OnPlay()
    {
        SceneManager.LoadScene("Game");
    }

    public void SetCurrentLevel(int levelIndex)
    {
        int previousLevelIndex = PlayerPrefs.GetInt("CurrentLevelIndex", 0);
        PlayerPrefs.SetInt("CurrentLevelIndex", levelIndex);
        int levelChange = levelIndex - previousLevelIndex;

        float newY = targetPosition.y - levelChange * verticalSpacing;
        targetPosition = new Vector2(targetPosition.x, newY);

        foreach (Transform child in transform)
        {
            LevelIcon icon = child.GetComponent<LevelIcon>();
            if (icon != null)
                icon.UpdateCurrentLevel(levelIndex);
        }
    }
}
