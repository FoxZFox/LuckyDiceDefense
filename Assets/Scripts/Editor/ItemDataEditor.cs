using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ItemData))]
public class ItemDataEditor : Editor
{
    bool deleteSidel = true;
    ItemData itemData;
    string newItemName = "";
    int newItemID = 0;
    public override void OnInspectorGUI()
    {
        itemData = target as ItemData;
        base.OnInspectorGUI();
        EditorGUILayout.BeginHorizontal("box");
        if (GUILayout.Button("Edit", EditorStyles.toolbarButton))
        {
            deleteSidel = false;
            newItemName = itemData.ItemName;
            newItemID = itemData.ItemID;
        }
        if (GUILayout.Button("Delete", EditorStyles.toolbarButton))
        {
            deleteSidel = true;
        }
        EditorGUILayout.EndHorizontal();
        if (!deleteSidel)
        {
            DrawEditor();
        }
        else
        {
            DrawDelete();
        }
    }

    private void DrawEditor()
    {
        EditorGUILayout.BeginVertical("box");
        EditorGUILayout.BeginVertical(EditorStyles.helpBox);
        EditorGUILayout.BeginHorizontal();
        GUILayout.Space(10);
        EditorGUILayout.Foldout(true, "Edit");
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("Invoke"))
        {
            itemData.EditData(newItemName, newItemID);
            newItemName = "";
            newItemID = 0;
            AssetDatabase.SaveAssets();
        }
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Name", GUILayout.Width(40));
        newItemName = EditorGUILayout.TextField(newItemName);
        EditorGUILayout.LabelField("ID", GUILayout.Width(20));
        newItemID = EditorGUILayout.IntField(newItemID);
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.EndVertical();
        EditorGUILayout.EndVertical();

    }

    private void DrawDelete()
    {
        EditorGUILayout.BeginVertical("box");
        if (GUILayout.Button("Delete"))
        {
            itemData.Container.ItemDatas.Remove(itemData);
            Undo.DestroyObjectImmediate(itemData);
            AssetDatabase.SaveAssets();
        }
        EditorGUILayout.EndVertical();
    }

}