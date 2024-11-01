using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
[System.Serializable]
public class Level : ScriptableObject, ISerializationCallbackReceiver
{
    // Level info
    public string levelName;
    public Difficulty difficulty;
    public float timeLimit = 60.0f;
    public CardType[] cardTypes;

    // Grid
    private int rows = 4;
    private int columns = 4;
    public bool[,] layout;
    public SerializableLayout serializedLayout;
    
    // Extras
    public AudioClip backgroundMusic;

    public int Rows
    {
        get => rows;
        set
        {
            if (rows != value)
            {
                rows = value;
                InitializeLayout();
            }
        }
    }

    public int Columns
    {
        get => columns;
        set
        {
            if (columns != value)
            {
                columns = value;
                InitializeLayout();
            }
        }
    }

    private void InitializeLayout() {
        if (layout == null)
        {
            layout = new bool[rows, columns];
            return;
        }
        
        int currentRows = layout.GetLength(0);
        int currentColumns = layout.GetLength(1);

        if (rows == currentRows && columns == currentColumns)
            return;

        bool[,] newLayout = new bool[rows, columns];
        for (int i = 0; i < rows; i++)
        {
            if (i >= currentRows) continue;
            for (int j = 0; j < columns; j++)
            {
                if (j >= currentColumns) continue;
                else newLayout[i, j] = layout[i, j];
            }
        }

        layout = newLayout;
    }

    public void OnBeforeSerialize()
    {
        InitializeLayout();

        serializedLayout = new SerializableLayout
        {
            rows = new SerializableLayoutRow[rows]
        };

        for (int i = 0; i < rows; i++)
            serializedLayout.rows[i] = new SerializableLayoutRow
            {
                row = new bool[columns]
            };
        
        for (int i = 0; i < rows; i++)
            for (int j = 0; j < columns; j++)
                serializedLayout.rows[i].row[j] = layout[i, j];
    }

    public void OnAfterDeserialize()
    {
        InitializeLayout();
        if (serializedLayout.rows == null)
            return;

        for (int i = 0; i < rows; i++)
            for (int j = 0; j < columns; j++)
            {
                SerializableLayoutRow row = serializedLayout.rows[i];
                bool value = row.row[j];
                layout[i, j] = serializedLayout.rows[i].row[j];
            }
    }
}

[System.Serializable]
public struct SerializableLayout
{
    public SerializableLayoutRow[] rows;
}

[System.Serializable]
public struct SerializableLayoutRow
{
    public bool[] row;
}

public enum Difficulty
{
    EASY,
    MEDIUM,
    HARD,
}
