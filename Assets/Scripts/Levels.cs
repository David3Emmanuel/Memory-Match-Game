using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Levels : ScriptableObject
{
    [SerializeField] private Level[] levels;

    public int Length => levels.Length;

    public Level GetLevel(int index)
    {
        if (index < 0 || index >= levels.Length)
            throw new System.IndexOutOfRangeException("Invalid level index");

        return levels[index];
    }
}
