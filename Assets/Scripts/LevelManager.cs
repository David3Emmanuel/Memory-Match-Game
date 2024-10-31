using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    const int MATCH_COUNT = 2;

    [SerializeField] private GameObject[] cardPrefabs;
    [SerializeField] private Level testLevel;

    public static LevelManager Instance { get; private set; }
    
    private List<Card> openCards = new List<Card>();

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
        yield return new WaitForSeconds(1.0f);

        if (AreCardsMatching())
            foreach (var openCard in openCards)
                openCard.DestroyCard();
        else
            foreach (var openCard in openCards)
                openCard.IsShowing = false;

        openCards.Clear();
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
