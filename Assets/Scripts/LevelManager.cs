using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public GameObject[] cardPrefabs;
    public Level testLevel;

    public static LevelManager Instance { get; private set; }

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        if (testLevel != null)
            SpawnLevel(testLevel);
    }

    public void SpawnLevel(Level level)
    {
        ClearLevel();
        List<GameObject> filteredCardPrefabs = FilterCardPrefabs(level.cardTypes);

        float offsetX = (level.Columns - 1) / 2f;
        float offsetY = (level.Rows - 1) / 2f;

        for (int i = 0; i < level.Rows; i++)
            for (int j = 0; j < level.Columns; j++)
                if (!level.layout[i, j])
                {
                    int index = Random.Range(0, filteredCardPrefabs.Count);
                    GameObject cardPrefab = filteredCardPrefabs[index];
                    GameObject cardInstance = Instantiate(cardPrefab, transform);
                    cardInstance.transform.localPosition = new Vector2(j - offsetX, -i + offsetY);
                }
    }

    void ClearLevel()
    {
        foreach (Transform child in transform)
            Destroy(child.gameObject);
    }

    List<GameObject> FilterCardPrefabs(CardType[] cardTypes)
    {
        if (cardTypes == null || cardTypes.Length == 0)
            return new List<GameObject>(cardPrefabs);

        List<GameObject> filteredCardPrefabs = new List<GameObject>();

        foreach (var prefab in cardPrefabs)
        {
            Card card = prefab.GetComponent<Card>();
            if (card != null && System.Array.Exists(cardTypes, type => type == card.Type))
                filteredCardPrefabs.Add(prefab);
        }

        return filteredCardPrefabs;
    }
}
