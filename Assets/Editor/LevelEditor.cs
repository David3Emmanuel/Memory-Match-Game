using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Level))]
public class LevelEditor : Editor
{
    public override void OnInspectorGUI()
    {
        Level level = (Level)target;

        DrawDefaultInspector();

        // Custom layout grid editor
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Grid Layout", EditorStyles.boldLabel);

        level.Rows = EditorGUILayout.IntField("Rows", level.Rows);
        level.Columns = EditorGUILayout.IntField("Columns", level.Columns);

        for (int i = 0; i < level.Rows; i++)
        {
            EditorGUILayout.BeginHorizontal();
            for (int j = 0; j < level.Columns; j++)
            {
                level.layout[i, j] = EditorGUILayout.Toggle(level.layout[i, j], GUILayout.Width(20));
            }
            EditorGUILayout.EndHorizontal();
        }

        if (GUI.changed)
            EditorUtility.SetDirty(level);
    }
}