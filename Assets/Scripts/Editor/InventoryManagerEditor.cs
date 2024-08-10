using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

[CustomEditor(typeof(InventoryManager))]
public class InventoryManagerEditor : Editor
{
    string itemDataPath = "Assets/TableObject/Container-Item-Data.asset";
    List<ItemData> itemDatas = new List<ItemData>();
    List<string> names = new List<string>();
    List<int> datas = new List<int>();
    string characterDataPath = "Assets/TableObject/Character";
    string cardDataPath = "Assets/TableObject/Container-Card-Data.asset";
    List<CharacterData> chaDatas;
    List<CardData> cardDatas;
    int itemID = -1;
    int cardSelect = -1;
    string[] cardName;

    private void OnEnable()
    {
        LoadItemData();
        LoadChaData();
        LoadCardData();
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
    [RuntimeInitializeOnLoadMethod]
    private void LoadItemData()
    {
        ItemDataContainer container = AssetDatabase.LoadAssetAtPath<ItemDataContainer>(itemDataPath);
        itemDatas.Clear();
        names.Clear();
        datas.Clear();
        for (int i = 0; i < container.ItemDatas.Count; i++)
        {
            itemDatas.Add(container.ItemDatas[i]);
            datas.Add(itemDatas[i].ItemID);
            names.Add(itemDatas[i].ItemName);
        }
    }

    private void LoadCardData()
    {
        cardDatas = new List<CardData>();
        var container = AssetDatabase.LoadAssetAtPath<CardDataContainer>(cardDataPath);
        cardName = new string[container.CardDatas.Count];
        for (int i = 0; i < container.CardDatas.Count; i++)
        {
            cardDatas.Add(container.CardDatas[i]);
            cardName[i] = container.CardDatas[i].name;
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
        cardSelect = EditorGUILayout.Popup("CardData", cardSelect, cardName);
        if (GUILayout.Button("AddCard"))
        {
            ic.AddCard(cardDatas[cardSelect]);
        }
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("UpdateData"))
        {
            UpdateData();
        }
        if (GUILayout.Button("DeleteInventoryData"))
        {
            DeleteInventoryData();
        }
        EditorGUILayout.EndHorizontal();
        serializedObject.ApplyModifiedProperties();
    }

    private void UpdateData()
    {
        SerializedProperty itemDataProperty = serializedObject.FindProperty("itemDatas");
        SerializedProperty characterDataProperty = serializedObject.FindProperty("characterDatas");
        SerializedProperty cardDataProperty = serializedObject.FindProperty("cardDatas");
        itemDataProperty.ClearArray();
        characterDataProperty.ClearArray();
        cardDataProperty.ClearArray();
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
        foreach (var item in cardDatas)
        {
            int newSize = cardDataProperty.arraySize + 1;
            cardDataProperty.InsertArrayElementAtIndex(newSize - 1);
            cardDataProperty.GetArrayElementAtIndex(newSize - 1).objectReferenceValue = item;
        }
    }

    private void DeleteInventoryData()
    {
        SerializedProperty invenItem = serializedObject.FindProperty("inventoryItems");
        SerializedProperty invenCha = serializedObject.FindProperty("inventoryCharacters");
        SerializedProperty invenCard = serializedObject.FindProperty("inventoryCards");
        invenItem.ClearArray();
        invenCha.ClearArray();
        invenCard.ClearArray();
    }
}