using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ElementType))]
public class ElementTypeEditor : Editor
{
    SerializedProperty container;
    bool deleteSide = false;
    private void OnEnable()
    {
        container = serializedObject.FindProperty("container");
    }
    public override void OnInspectorGUI()
    {
        ElementType elementType = target as ElementType;
        base.OnInspectorGUI();
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Rename", EditorStyles.toolbarButton))
        {
            deleteSide = false;
        }
        if (GUILayout.Button("Delete", EditorStyles.toolbarButton))
        {
            deleteSide = true;
        }
        EditorGUILayout.EndHorizontal();
        if (deleteSide)
        {
            DrawDelteElement(elementType);
        }
        else
        {
            DrawRenameElement(elementType);
        }

    }

    string rename = "";
    private void DrawRenameElement(ElementType elementType)
    {
        EditorGUILayout.BeginVertical("box");
        EditorGUILayout.BeginHorizontal(EditorStyles.helpBox);
        GUILayout.Space(10);
        EditorGUILayout.Foldout(true, "Rename");
        GUILayout.FlexibleSpace();
        GUI.enabled = !string.IsNullOrEmpty(rename);
        if (GUILayout.Button("Invoke"))
        {
            elementType.SetElementName(rename);
            elementType.name = rename;
            AssetDatabase.SaveAssets();
            EditorUtility.SetDirty(elementType);
            rename = "";
            GUI.FocusControl(null);
        }
        GUI.enabled = true;
        EditorGUILayout.EndHorizontal();
        GUILayout.Space(-3.2f);
        EditorGUILayout.BeginHorizontal("helpbox");
        EditorGUILayout.LabelField("Value");
        rename = EditorGUILayout.TextField(rename);
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.EndVertical();
    }

    private void DrawDelteElement(ElementType elementType)
    {
        EditorGUILayout.BeginVertical("box");
        if (GUILayout.Button("Delete Element"))
        {
            ElementTypeContainer elementTypeContainer = container.objectReferenceValue as ElementTypeContainer;
            elementTypeContainer.ElementTypes.Remove(elementType);
            Undo.DestroyObjectImmediate(elementType);
            AssetDatabase.SaveAssets();
        }
        EditorGUILayout.EndVertical();

    }
}