using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CardDataContainer))]
public class CardDataContainerEditor : Editor
{
    string cardName;
    int cardID = 0;
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        CardDataContainer cardDataContainer = target as CardDataContainer;
        GUILayout.Space(10);
        EditorGUILayout.BeginVertical("box");
        EditorGUILayout.LabelField("New CardData", EditorStyles.boldLabel);
        EditorGUILayout.BeginHorizontal(EditorStyles.helpBox);
        EditorGUILayout.LabelField("CardName", GUILayout.Width(70));
        cardName = EditorGUILayout.TextField(cardName);
        EditorGUILayout.LabelField("CardID", GUILayout.Width(50));
        cardID = EditorGUILayout.IntField(cardID);
        EditorGUILayout.EndHorizontal();
        GUI.enabled = !string.IsNullOrEmpty(cardName) && (cardID != 0);
        if (GUILayout.Button("New Card", EditorStyles.toolbarButton))
        {
            MakeNewCardData(cardDataContainer);
            cardID = 0;
            cardName = null;
            GUI.FocusControl(null);
        }
        EditorGUILayout.EndVertical();
    }

    private void MakeNewCardData(CardDataContainer container)
    {
        CardData cardData = CreateInstance<CardData>();
        cardData.Initialise(cardName, cardID, container);
        container.CardDatas.Add(cardData);
        AssetDatabase.AddObjectToAsset(cardData, container);
        AssetDatabase.SaveAssets();
        EditorUtility.SetDirty(cardData);
    }

}