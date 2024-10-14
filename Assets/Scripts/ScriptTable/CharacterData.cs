using System;
using System.Security.Cryptography;
using System.Text;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "New CharacterData", menuName = "CreateTableObject/CharacterData")]
public class CharacterData : ScriptableObject
{
    [Header("Stat Growth Curves")]
    public AnimationCurve DamageGrowthCurve;
    public AnimationCurve RatioGrowthCurve;
    [Header("Common")]
    public int CharacterID = 1;
    [Range(1, 5)] public int Star = 1;
    public ElementType elementType;
    public int CardNeed = 1;
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

    [Button()]
    public void GenerateID()
    {
        string dataToHash = elementType.ToString() + cardData.ToString() + placeHolderSpitre.ToString() + animatorController.ToString() + Star.ToString();
        using SHA256 hash = SHA256.Create();
        byte[] bytes = hash.ComputeHash(Encoding.UTF8.GetBytes(dataToHash));
        CharacterID = BitConverter.ToInt32(bytes);
    }

    public float GetAttackDamageWithGrowth(int level)
    {
        float damage = attackDamage * DamageGrowthCurve.Evaluate(level / 100f);
        return Mathf.Round(damage * 100f) / 100f;
    }

    public float GetAttackRaioWithGrowth(int level)
    {
        float ratio = attackRatio * RatioGrowthCurve.Evaluate(level / 100f);
        return Mathf.Round(ratio * 100f) / 100f;
    }
}
