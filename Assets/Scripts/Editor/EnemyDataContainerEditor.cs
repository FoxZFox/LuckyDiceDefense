using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(EnemyDataContainer))]
public class EnemyDataContainerEditor : Editor
{
    private string enemyName;
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        EnemyDataContainer enemyDataContainer = target as EnemyDataContainer;
        GUILayout.Space(10);
        EditorGUILayout.BeginVertical("box");
        EditorGUILayout.LabelField("New EnemyData", EditorStyles.boldLabel);
        EditorGUILayout.BeginHorizontal(EditorStyles.helpBox);
        EditorGUILayout.LabelField("EnemyName", GUILayout.Width(75));
        enemyName = EditorGUILayout.TextField(enemyName);
        EditorGUILayout.EndHorizontal();
        GUI.enabled = !string.IsNullOrEmpty(enemyName);
        if (GUILayout.Button("New Enemy", EditorStyles.toolbarButton))
        {
            MakeNewCardData(enemyDataContainer);
            enemyName = null;
            GUI.FocusControl(null);
        }
        EditorGUILayout.EndVertical();
    }

    private void MakeNewCardData(EnemyDataContainer container)
    {
        EnemyData enemyData = CreateInstance<EnemyData>();
        enemyData.Initialise(enemyName, container);
        container.EnemyDatas.Add(enemyData);
        AssetDatabase.AddObjectToAsset(enemyData, container);
        AssetDatabase.SaveAssets();
        EditorUtility.SetDirty(enemyData);
    }
}