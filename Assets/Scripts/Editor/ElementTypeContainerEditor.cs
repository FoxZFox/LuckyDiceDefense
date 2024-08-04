using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ElementTypeContainer))]
public class ElementTypeContainerEditor : Editor
{
    string newElementName;
    private void OnEnable()
    {
    }
    public override void OnInspectorGUI()
    {
        ElementTypeContainer elementTypeContainer = target as ElementTypeContainer;
        base.OnInspectorGUI();

        GUILayout.Space(10);
        EditorGUILayout.BeginVertical("box");
        EditorGUILayout.LabelField("Make New Element", EditorStyles.boldLabel);
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Elemnet Name", GUILayout.Width(120));
        newElementName = EditorGUILayout.TextField(newElementName);
        EditorGUILayout.EndHorizontal();
        if (GUILayout.Button("Make New Element"))
        {
            MakeNewElementType(elementTypeContainer);
            // Debug.Log(newElementName);
            newElementName = "";
            GUI.FocusControl(null);
        }
        EditorGUILayout.EndVertical();
        EditorUtility.SetDirty(elementTypeContainer);
    }

    private void MakeNewElementType(ElementTypeContainer container)
    {
        if (!container.FindElementByName(newElementName))
        {
            ElementType elementType = CreateInstance<ElementType>();
            elementType.name = newElementName;
            elementType.Initialise(newElementName, container);
            container.ElementTypes.Add(elementType);
            AssetDatabase.AddObjectToAsset(elementType, container);
            AssetDatabase.SaveAssets();
            EditorUtility.SetDirty(elementType);
        }
    }
}
