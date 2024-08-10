using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementType : ScriptableObject
{
    [SerializeField] private ElementTypeContainer container;
    [SerializeField] private ElementType bestElement;
    [SerializeField] private ElementType worthElement;
    [SerializeField] private string elementName;

    public ElementType Bestelement { get => bestElement; }
    public ElementType WorthElement { get => worthElement; }
    public string ElementName { get => elementName; }

#if UNITY_EDITOR
    public void Initialise(string name, ElementTypeContainer elementTypeContainer)
    {
        elementName = name;
        container = elementTypeContainer;
    }

    public void SetElementName(string s)
    {
        elementName = s;
    }
#endif
}