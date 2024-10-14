using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

// [CustomEditor(typeof(CharacterData))]
public class CharacterDataEditor : Editor
{
    string cardContainerPath = "Assets/TableObject/Container-Card-Data.asset";
    List<CardData> cardDatas;
    string[] cardName;
    int selectCardData = -1;
    SerializedProperty cardData;
    private void OnEnable()
    {
        LoadCardData();
        cardData = serializedObject.FindProperty("cardData");
        MatchCardDataAndIndex();
    }

    private void LoadCardData()
    {
        cardDatas = new List<CardData>();
        var container = AssetDatabase.LoadAssetAtPath<CardDataContainer>(cardContainerPath);
        cardName = new string[container.CardDatas.Count];
        for (int i = 0; i < container.CardDatas.Count; i++)
        {
            cardDatas.Add(container.CardDatas[i]);
            cardName[i] = $"ID:{container.CardDatas[i].CardID} ({container.CardDatas[i].name})";
        }
    }
    public override void OnInspectorGUI()
    {
        CharacterData characterData = target as CharacterData;
        base.OnInspectorGUI();
        selectCardData = EditorGUILayout.Popup("Card", selectCardData, cardName);
        if (selectCardData >= 0)
            cardData.objectReferenceValue = cardDatas[selectCardData];
        serializedObject.ApplyModifiedProperties();
        EditorUtility.SetDirty(characterData);
    }

    private void MatchCardDataAndIndex()
    {
        CardData element = cardData.objectReferenceValue as CardData;
        for (int i = 0; i < cardDatas.Count; i++)
        {
            if (cardDatas[i] == element)
                selectCardData = i;
        }
    }
}