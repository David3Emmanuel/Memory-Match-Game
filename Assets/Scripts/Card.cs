using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    [SerializeField] private CardType type;
    
    public CardType Type => type;
    public bool IsShowing
    {
        get => isShowing;
        set {
            animator.SetTrigger(value ? "Show" : "Hide");
            isShowing = value;
        }
    }
    public bool HasMatched
    {
        get => hasMatched;
        set {
            if (value)
                animator.SetTrigger("Match");
            hasMatched = value;
        }
    }
    
    private Animator animator;
    private bool isShowing, hasMatched;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void OnMouseDown()
    {
        if (IsShowing || HasMatched) return;
        
        LevelManager.Instance.Select(this);
    }
}
