using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CardData))]
public class CardDataEditor : Editor
{
    bool deleteSide = false;
    CardData cardData;
    SerializedProperty container;
    private void OnEnable()
    {
        container = serializedObject.FindProperty("container");
    }
    public override void OnInspectorGUI()
    {
        cardData = target as CardData;
        base.OnInspectorGUI();
        if (GUILayout.Button("GenerateID"))
        {
            cardData.GenerateID();
        }
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("EditName&ID", EditorStyles.toolbarButton))
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
            DrawEditData();
        }
        else
        {
            DrawDeleteSide();
        }
    }
    string cardName = "";
    int cardID = 0;
    private void DrawEditData()
    {
        EditorGUILayout.BeginVertical("box");
        EditorGUILayout.BeginHorizontal(EditorStyles.helpBox);
        GUILayout.Space(10);
        EditorGUILayout.Foldout(true, "Edit Name & ID");
        GUILayout.FlexibleSpace();
        GUI.enabled = !string.IsNullOrEmpty(cardName) || cardID != 0;
        if (GUILayout.Button("Edit"))
        {
            cardData.EditNameAndID(cardName, cardID);
            cardName = "";
            cardID = 0;
            GUI.FocusControl(null);
            AssetDatabase.SaveAssets();
            EditorUtility.SetDirty(cardData);
        }
        EditorGUILayout.EndHorizontal();
        GUI.enabled = true;
        GUILayout.Space(-3.2f);
        EditorGUILayout.BeginHorizontal("helpbox");
        EditorGUILayout.LabelField("CardName", GUILayout.Width(70));
        cardName = EditorGUILayout.TextField(cardName);
        EditorGUILayout.LabelField("CardID", GUILayout.Width(50));
        cardID = EditorGUILayout.IntField(cardID);
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.EndVertical();
    }
    private void DrawDeleteSide()
    {
        EditorGUILayout.BeginVertical("box");
        if (GUILayout.Button("Delete"))
        {
            CardDataContainer cardDataContainer = container.objectReferenceValue as CardDataContainer;
            cardDataContainer.CardDatas.Remove(cardData);
            Undo.DestroyObjectImmediate(cardData);
            AssetDatabase.SaveAssets();
        }
        EditorGUILayout.EndVertical();
    }
}