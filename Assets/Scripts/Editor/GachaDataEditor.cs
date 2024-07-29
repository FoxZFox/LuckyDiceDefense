using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.VersionControl;
using UnityEngine;

[CustomEditor(typeof(GachaData))]
public class GachaDataEditor : Editor
{
    int fiveStarSelect = 0;
    List<CharacterData> characterDatas;
    List<CharacterData> fiveStarDatas;
    List<CharacterData> fourStarDatas;
    string characterDataPath = "Assets/TableObject/Character";
    string[] fiveStarName;
    string[] fourStarName;
    bool showFourStarEdit = false;
    bool showBelowFourStarEdit = false;
    private void OnEnable()
    {
        characterDatas = new List<CharacterData>();
        fiveStarDatas = new List<CharacterData>();
        fourStarDatas = new List<CharacterData>();
        string[] assetGuids = AssetDatabase.FindAssets("t:CharacterData", new[] { characterDataPath });
        for (int i = 0; i < assetGuids.Length; i++)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(assetGuids[i]);
            characterDatas.Add(AssetDatabase.LoadAssetAtPath<CharacterData>(assetPath));
        }

        foreach (var item in characterDatas)
        {
            int star = item.Star;
            if (star == 5)
            {
                fiveStarDatas.Add(item);
            }
            else if (star == 4)
            {
                fourStarDatas.Add(item);
            }
        }
        fiveStarName = new string[fiveStarDatas.Count];
        fourStarName = new string[fourStarDatas.Count];
        for (int i = 0; i < fiveStarDatas.Count; i++)
        {
            fiveStarName[i] = fiveStarDatas[i].name;
        }
        for (int i = 0; i < fourStarDatas.Count; i++)
        {
            fourStarName[i] = fourStarDatas[i].name;
        }
    }
    public override void OnInspectorGUI()
    {
        // base.OnInspectorGUI();
        serializedObject.Update();
        GachaData gachaData = (GachaData)target;
        if (fiveStarDatas.Count > 0 && fourStarDatas.Count > 0)
        {
            fiveStarSelect = EditorGUILayout.Popup("Select Five Star", fiveStarSelect, fiveStarName);
            gachaData.fiveStarCard = fiveStarDatas[fiveStarSelect];
            showFourStarEdit = EditorGUILayout.Foldout(showFourStarEdit, "FourStarEdit");
            if (showFourStarEdit)
            {
                SerializedProperty fourStarCardProperty = serializedObject.FindProperty("fourStarCard");
                // EditorGUILayout.PropertyField(fourStarCardProperty, new GUIContent("Four Star Card"), true);
                EditorGUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                EditorGUILayout.LabelField($"Current Element:{fourStarCardProperty.arraySize}", EditorStyles.boldLabel, GUILayout.Width(120));
                GUILayout.FlexibleSpace();
                EditorGUILayout.EndHorizontal();
                for (int i = 0; i < fourStarCardProperty.arraySize; i++)
                {
                    SerializedProperty element = fourStarCardProperty.GetArrayElementAtIndex(i);
                    CharacterData currentCharacter = (CharacterData)element.objectReferenceValue;
                    int selectIndex = 0;
                    for (int j = 0; j < fourStarDatas.Count; j++)
                    {
                        if (currentCharacter == fourStarDatas[j])
                        {
                            selectIndex = j;
                        }
                    }
                    EditorGUILayout.BeginHorizontal();
                    GUILayout.FlexibleSpace();
                    EditorGUILayout.LabelField($"Select Four Star[{i}]", EditorStyles.boldLabel, GUILayout.Width(120));
                    EditorGUILayout.PropertyField(element, GUIContent.none);
                    selectIndex = EditorGUILayout.Popup(selectIndex, fourStarName);
                    GUILayout.FlexibleSpace();
                    EditorGUILayout.EndHorizontal();
                    element.objectReferenceValue = fourStarDatas[selectIndex];
                }
                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button("Add Elemnet"))
                {
                    int newsize = fourStarCardProperty.arraySize + 1;
                    fourStarCardProperty.InsertArrayElementAtIndex(newsize - 1);
                    SerializedProperty newelement = fourStarCardProperty.GetArrayElementAtIndex(newsize - 1);
                    newelement.objectReferenceValue = null;
                }
                if (GUILayout.Button("Delete Elemnet"))
                {
                    int lastindex = fourStarCardProperty.arraySize - 1;
                    fourStarCardProperty.DeleteArrayElementAtIndex(lastindex);
                }
                EditorGUILayout.EndHorizontal();
            }
            showBelowFourStarEdit = EditorGUILayout.Foldout(showBelowFourStarEdit, "BelowFourstar");
            if (showBelowFourStarEdit)
            {
                SerializedProperty belowFourStar = serializedObject.FindProperty("belowFourStar");
                EditorGUILayout.PropertyField(belowFourStar, true);

            }
        }
        else
        {
            EditorGUILayout.LabelField("Plese add fourstar character and fivestar character to game data");
        }
        serializedObject.ApplyModifiedProperties();
        EditorUtility.SetDirty(gachaData);
    }
}
