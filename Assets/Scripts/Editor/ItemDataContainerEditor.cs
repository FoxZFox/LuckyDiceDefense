using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ItemDataContainer))]
public class ItemDataContainerEditor : Editor
{
    ItemDataContainer container;
    string itemName = "";
    int itemID = 0;
    public override void OnInspectorGUI()
    {
        container = target as ItemDataContainer;
        base.OnInspectorGUI();

        EditorGUILayout.BeginVertical("box");
        EditorGUILayout.BeginHorizontal(EditorStyles.helpBox);
        GUILayout.Space(10);
        EditorGUILayout.Foldout(true, "New Item");
        GUILayout.FlexibleSpace();
        GUI.enabled = !string.IsNullOrEmpty(itemName) && itemID != 0;
        if (GUILayout.Button("CreateItem", EditorStyles.toolbarButton))
        {
            CreateNewItem();
            itemName = "";
            itemID = 0;
            GUI.FocusControl(null);
        }
        EditorGUILayout.EndHorizontal();
        GUI.enabled = true;
        EditorGUILayout.BeginHorizontal(EditorStyles.helpBox);
        EditorGUILayout.LabelField("Name", GUILayout.Width(40));
        itemName = EditorGUILayout.TextField(itemName);
        EditorGUILayout.LabelField("ID", GUILayout.Width(20));
        itemID = EditorGUILayout.IntField(itemID);
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.EndVertical();
    }

    private void CreateNewItem()
    {
        ItemData itemData = CreateInstance<ItemData>();
        itemData.Initialise(itemName, itemID, container);
        container.ItemDatas.Add(itemData);
        AssetDatabase.AddObjectToAsset(itemData, container);
        AssetDatabase.SaveAssets();
        EditorUtility.SetDirty(itemData);
    }
}