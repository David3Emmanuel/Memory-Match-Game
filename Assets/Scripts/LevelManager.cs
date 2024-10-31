using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class LevelManager : MonoBehaviour
{
    const int MATCH_COUNT = 2;

    [SerializeField] private GameObject[] cardPrefabs;

    public static LevelManager Instance { get; private set; }
    
    private List<Card> openCards = new List<Card>();

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void SpawnLevel(Level level)
    {
        ClearLevel();
        List<GameObject> filteredCardPrefabs = FilterCardPrefabs(level.cardTypes);
        int count = CountCardsToSpawn(level);

        List<GameObject> cardsToSpawn = GenerateCardsToSpawn(count, MATCH_COUNT, filteredCardPrefabs);

        float offsetX = (level.Columns - 1) / 2f;
        float offsetY = (level.Rows - 1) / 2f;

        int cardIndex = 0;
        for (int i = 0; i < level.Rows; i++)
            for (int j = 0; j < level.Columns; j++)
                if (!level.layout[i, j])
                {
                    GameObject cardInstance = Instantiate(cardsToSpawn[cardIndex], transform);
                    cardInstance.transform.localPosition = new Vector2(j - offsetX, -i + offsetY);
                    cardIndex++;
                }
    }

    List<GameObject> GenerateCardsToSpawn(int count, int matchCount, List<GameObject> filteredCardPrefabs)
    {
        Assert.IsTrue(count % MATCH_COUNT == 0, "The number of cards to spawn must be a multiple of MATCH_COUNT.");

        List<GameObject> cardsToSpawn = new List<GameObject>();
        for (int i = 0; i < count / matchCount; i++)
        {
            int index = Random.Range(0, filteredCardPrefabs.Count);
            GameObject cardPrefab = filteredCardPrefabs[index];
            for (int j = 0; j < matchCount; j++)
            {
                cardsToSpawn.Add(cardPrefab);
            }
        }

        // Shuffle the list
        for (int i = 0; i < cardsToSpawn.Count; i++)
        {
            int rnd = Random.Range(0, cardsToSpawn.Count);
            GameObject temp = cardsToSpawn[rnd];
            cardsToSpawn[rnd] = cardsToSpawn[i];
            cardsToSpawn[i] = temp;
        }

        return cardsToSpawn;
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

    int CountCardsToSpawn(Level level)
    {
        int count = 0;
        for (int i = 0; i < level.Rows; i++)
            for (int j = 0; j < level.Columns; j++)
                if (!level.layout[i, j])
                    count++;
        return count;
    }

    public void Select(Card card)
    {
        if (openCards.Count < MATCH_COUNT)
        {
            openCards.Add(card);
            card.IsShowing = true;

            if (openCards.Count == MATCH_COUNT)
                StartCoroutine(HideCards());
        }
    }

    private IEnumerator HideCards()
    {
        yield return new WaitForSeconds(0.1f);

        if (AreCardsMatching())
        {
            foreach (var openCard in openCards)
                openCard.HasMatched = true;
            
            if (AllCardsMatched())
                GameManager.Instance.OnLevelComplete();
        }
        else
        {
            foreach (var openCard in openCards)
                openCard.IsShowing = false;
        }

        openCards.Clear();
    }

    private bool AllCardsMatched()
    {
        foreach (Transform child in transform)
        {
            Card card = child.GetComponent<Card>();
            if (card != null && !card.HasMatched)
                return false;
        }
        return true;
    }

    private bool AreCardsMatching()
    {
        if (openCards.Count < MATCH_COUNT)
            return false;

        CardType firstType = openCards[0].Type;
        foreach (var card in openCards)
            if (card.Type != firstType)
                return false;
        
        return true;
    }
}
