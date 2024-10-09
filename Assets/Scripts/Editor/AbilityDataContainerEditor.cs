using UnityEngine;
using UnityEditor;
using UnityEditor.Playables;
using System;
using System.Linq;

[CustomEditor(typeof(AbilityDataContainer))]
public class AbilityDataContainerEditor : Editor
{
    private string abilityName = "";
    bool deleteSide = false;
    AbilityType at;
    private Type[] abilityClass;
    private string[] abilityClassName;
    int abilitySelect = -1;
    private void OnEnable()
    {
        LoadAbilityData();
    }

    private void LoadAbilityData()
    {
        abilityClass = AppDomain.CurrentDomain.GetAssemblies().SelectMany(assembly => assembly.GetTypes()).Where(type => type.IsSubclassOf(typeof(AbilityData))).ToArray();
        abilityClassName = new string[abilityClass.Length];
        for (int i = 0; i < abilityClass.Length; i++)
        {
            abilityClassName[i] = abilityClass[i].Name;
        }
    }
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
        EditorGUILayout.LabelField("Class", GUILayout.Width(40));
        abilitySelect = EditorGUILayout.Popup(abilitySelect, abilityClassName);
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
        AbilityData itemData = (AbilityData)CreateInstance(abilityClass[abilitySelect]);
        itemData.Initialise(abilityName, at);
        container.AbilityDatas.Add(itemData);
        AssetDatabase.AddObjectToAsset(itemData, container);
        AssetDatabase.SaveAssets();
        EditorUtility.SetDirty(itemData);
    }
}