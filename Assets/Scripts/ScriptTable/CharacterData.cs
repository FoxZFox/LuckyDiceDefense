using UnityEngine;

[CreateAssetMenu(fileName = "New CharacterData", menuName = "CreateTableObject/CharacterData")]
public class CharacterData : ScriptableObject
{
    [Header("Common")]
    public GameObject CharacterPrefab;
    public int CharacterID = 1;
    [Range(1, 5)] public int Star = 1;
    public ElementType elementType;
    public int ItemNeded = 1;
    public CardData cardData;
    public Sprite placeHolderSpitre;
    public RuntimeAnimatorController animatorController;
    public float AttackDuretionAnimation = 0.267f;
    [Header("Units and Build")]
    public float attackDamage = 1f;
    public float attackRatio = 1f;
    public float attackRange = 3f;
    public bool longRange = false;
    public AbilityData ability = null;
    [Range(1f, 100f)] public float skillChange = 100f;
    [Min(1)] public int costToBuild = 100;
    public (CardData, CharacterData) MapData()
    {
        return (cardData, this);
    }
}
