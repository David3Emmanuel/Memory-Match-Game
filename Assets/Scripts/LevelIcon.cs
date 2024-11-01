using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class LevelIcon : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private GameObject currentLevelIndicator;

    private Level level;
    private int levelIndex;
    private bool isCurrentLevel;

    public bool IsCurrentLevel
    {
        get { return isCurrentLevel; }
        set
        {
            isCurrentLevel = value;
            currentLevelIndicator.SetActive(isCurrentLevel);
        }
    }

    public void SetLevel(Level level, int levelIndex, bool isCurrentLevel)
    {
        this.level = level;
        this.levelIndex = levelIndex;
        IsCurrentLevel = isCurrentLevel;
        levelText.text = (levelIndex + 1).ToString();
    }

    private void OnMouseDown()
    {
        MainMenu.Instance.SetCurrentLevel(levelIndex);
    }

    public void UpdateCurrentLevel(int currentLevelIndex)
    {
        IsCurrentLevel = levelIndex == currentLevelIndex;
    }
}
