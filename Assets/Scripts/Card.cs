using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    [SerializeField] private CardType type;

    private Animator animator;
    private bool isShowing;

    public CardType Type => type;

    public bool IsShowing
    {
        get => isShowing;
        set {
            animator.SetTrigger(value ? "Show" : "Hide");
            isShowing = value;
        }
    }

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void OnMouseDown()
    {
        LevelManager.Instance.Select(this);
    }

    public void DestroyCard()
    {
        Destroy(gameObject);
    }
}

public enum CardType
{
    NONE,
    Red,
    Orange,
    Yellow,
    Green,
    Blue,
    Purple,
}
