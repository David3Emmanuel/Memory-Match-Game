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
        IsShowing = true;
        StartCoroutine(FlipBack());
    }

    IEnumerator FlipBack()
    {
        yield return new WaitForSeconds(1.0f);
        IsShowing = false;
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
