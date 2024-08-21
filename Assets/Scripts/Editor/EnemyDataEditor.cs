using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(EnemyData))]
public class EnemyDataEditor : Editor
{
    bool deleteSidel = false;
    EnemyData enemyData;
    string newEnemyName = "";
    public override void OnInspectorGUI()
    {
        enemyData = target as EnemyData;
        base.OnInspectorGUI();
        EditorGUILayout.BeginHorizontal("box");
        if (GUILayout.Button("Edit", EditorStyles.toolbarButton))
        {
            deleteSidel = false;
            newEnemyName = enemyData.enemyName;
        }
        if (GUILayout.Button("Delete", EditorStyles.toolbarButton))
        {
            deleteSidel = true;
        }
        EditorGUILayout.EndHorizontal();
        if (!deleteSidel)
        {
            DrawEditor();
        }
        else
        {
            DrawDelete();
        }
    }

    private void DrawEditor()
    {
        EditorGUILayout.BeginVertical("box");
        EditorGUILayout.BeginVertical(EditorStyles.helpBox);
        EditorGUILayout.BeginHorizontal();
        GUILayout.Space(10);
        EditorGUILayout.Foldout(true, "Edit");
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("Invoke"))
        {
            enemyData.EditName(newEnemyName);
            newEnemyName = "";
            AssetDatabase.SaveAssets();
        }
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Name", GUILayout.Width(40));
        newEnemyName = EditorGUILayout.TextField(newEnemyName);
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.EndVertical();
        EditorGUILayout.EndVertical();

    }

    private void DrawDelete()
    {
        EditorGUILayout.BeginVertical("box");
        if (GUILayout.Button("Delete"))
        {
            enemyData.container.EnemyDatas.Remove(enemyData);
            Undo.DestroyObjectImmediate(enemyData);
            AssetDatabase.SaveAssets();
        }
        EditorGUILayout.EndVertical();
    }
}