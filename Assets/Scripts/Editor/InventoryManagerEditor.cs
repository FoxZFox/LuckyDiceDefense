using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

[CustomEditor(typeof(InventoryManager))]
public class InventoryManagerEditor : Editor
{
    static string itemDataPath = "Assets/TableObject/Item";
    static List<ItemData> itemDatas = new List<ItemData>();
    static List<string> names = new List<string>();
    static List<int> datas = new List<int>();
    string characterDataPath = "Assets/TableObject/Character";
    List<CharacterData> chaDatas;
    int itemID = 0;

    private void OnEnable()
    {
        LoadItemData();
        LoadChaData();
    }
    private void LoadChaData()
    {
        chaDatas = new List<CharacterData>();
        string[] assetGuids = AssetDatabase.FindAssets("t:CharacterData", new[] { characterDataPath });
        string assetPath;
        foreach (var item in assetGuids)
        {
            assetPath = AssetDatabase.GUIDToAssetPath(item);
            chaDatas.Add(AssetDatabase.LoadAssetAtPath<CharacterData>(assetPath));
        }
    }
    [MenuItem("DeBugger/LoadItemdata")]
    [RuntimeInitializeOnLoadMethod]
    private static void LoadItemData()
    {
        string[] assetguids = AssetDatabase.FindAssets("t:ItemData", new[] { itemDataPath });
        itemDatas.Clear();
        names.Clear();
        datas.Clear();
        for (int i = 0; i < assetguids.Length; i++)
        {
            string assetpath = AssetDatabase.GUIDToAssetPath(assetguids[i]);
            itemDatas.Add(AssetDatabase.LoadAssetAtPath<ItemData>(assetpath));
            datas.Add(itemDatas[i].ItemID);
            names.Add(itemDatas[i].ItemName);
        }
    }
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        InventoryManager ic = (InventoryManager)target;
        EditorGUILayout.BeginHorizontal();
        itemID = EditorGUILayout.IntPopup("ItemID", itemID, names.ToArray(), datas.ToArray());
        if (GUILayout.Button("AddItem"))
        {
            // Debug.Log($"{itemID}");
            ic.AddItem(itemID);
        }
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("UpdateItemData&ChaData"))
        {
            LoadItemDataToProperty();
        }
        if (GUILayout.Button("DeleteInventoryData"))
        {
            DeleteInventoryData();
        }
        EditorGUILayout.EndHorizontal();
        serializedObject.ApplyModifiedProperties();
    }

    private void LoadItemDataToProperty()
    {
        SerializedProperty itemDataProperty = serializedObject.FindProperty("itemDatas");
        SerializedProperty characterDataProperty = serializedObject.FindProperty("characterDatas");
        itemDataProperty.ClearArray();
        characterDataProperty.ClearArray();
        foreach (var item in itemDatas)
        {
            int newSize = itemDataProperty.arraySize + 1;
            itemDataProperty.InsertArrayElementAtIndex(newSize - 1);
            itemDataProperty.GetArrayElementAtIndex(newSize - 1).objectReferenceValue = item;
        }

        foreach (var item in chaDatas)
        {
            int newSize = characterDataProperty.arraySize + 1;
            characterDataProperty.InsertArrayElementAtIndex(newSize - 1);
            characterDataProperty.GetArrayElementAtIndex(newSize - 1).objectReferenceValue = item;
        }
    }

    private void DeleteInventoryData()
    {
        SerializedProperty invenItem = serializedObject.FindProperty("inventoryItems");
        SerializedProperty invenCha = serializedObject.FindProperty("inventoryCharacters");
        invenItem.ClearArray();
        invenCha.ClearArray();
    }
}