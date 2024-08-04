using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


[CreateAssetMenu(fileName = "Container Element Type", menuName = "Stata/ElementContainer")]
public class ElementTypeContainer : ScriptableObject
{
    [SerializeField] private List<ElementType> elementTypes = new List<ElementType>();
    public List<ElementType> ElementTypes { get => elementTypes; set => elementTypes = value; }
#if UNITY_EDITOR
    public bool FindElementByName(string s)
    {
        return elementTypes.Any(i => i.ElementName == s);
    }
#endif
}
