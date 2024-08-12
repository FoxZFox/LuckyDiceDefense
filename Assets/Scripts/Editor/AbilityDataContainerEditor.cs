using UnityEngine;
using UnityEditor;
using UnityEditor.Playables;

[CustomEditor(typeof(AbilityDataContainer))]
public class AbilityDataContainerEditor : Editor
{
    private enum AbilityType { Slow, Buff }
    private AbilityType at;
    private string abilityName = "";
    bool deleteSide = false;
    public override void OnInspectorGUI()
    {
        AbilityDataContainer abilityDataContainer = target as AbilityDataContainer;
        base.OnInspectorGUI();

        GUILayout.Space(10);
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Create", EditorStyles.toolbarButton))
        {
            deleteSide = false;
        }
        if (GUILayout.Button("Delete", EditorStyles.toolbarButton))
        {
            deleteSide = true;
        }
        EditorGUILayout.EndHorizontal();
        if (!deleteSide)
        {
            DrawCreate(abilityDataContainer);
        }
        else
        {
            DrawDelete(abilityDataContainer);
        }
    }

    private void DrawCreate(AbilityDataContainer container)
    {
        EditorGUILayout.BeginVertical("box");
        EditorGUILayout.BeginHorizontal(EditorStyles.helpBox);
        GUILayout.Space(10);
        EditorGUILayout.Foldout(true, "New Ability");
        GUILayout.FlexibleSpace();
        GUI.enabled = !string.IsNullOrEmpty(abilityName);
        if (GUILayout.Button("CreateAbility", EditorStyles.toolbarButton))
        {
            CreateAbility(container);
            abilityName = "";
            GUI.FocusControl(null);
        }
        EditorGUILayout.EndHorizontal();
        GUI.enabled = true;
        EditorGUILayout.BeginHorizontal(EditorStyles.helpBox);
        EditorGUILayout.LabelField("Name", GUILayout.Width(40));
        abilityName = EditorGUILayout.TextField(abilityName);
        EditorGUILayout.LabelField("Type", GUILayout.Width(40));
        at = (AbilityType)EditorGUILayout.EnumPopup(at);
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.EndVertical();
    }

    private void DrawDelete(AbilityDataContainer container)
    {
        SerializedProperty deleteAbility = serializedObject.FindProperty("deleteAbility");
        EditorGUILayout.BeginVertical("box");
        EditorGUILayout.PropertyField(deleteAbility);
        if (GUILayout.Button("Delete"))
        {
            container.AbilityDatas.Remove(container.deleteAbility);
            Undo.DestroyObjectImmediate(container.deleteAbility);
            container.deleteAbility = null;
            AssetDatabase.SaveAssets();
        }
        serializedObject.ApplyModifiedProperties();
        EditorGUILayout.EndVertical();
    }

    private void CreateAbility(AbilityDataContainer container)
    {
        AbilityData itemData = null;
        switch (at)
        {
            case AbilityType.Slow:
                SlowAbility slowAbility = CreateInstance<SlowAbility>();
                slowAbility.Initialise(abilityName);
                itemData = slowAbility;
                break;
        }
        container.AbilityDatas.Add(itemData);
        AssetDatabase.AddObjectToAsset(itemData, container);
        AssetDatabase.SaveAssets();
        EditorUtility.SetDirty(itemData);
    }
}