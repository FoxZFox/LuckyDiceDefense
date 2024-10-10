using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildCharacter : MonoBehaviour
{
    [SerializeField] private CharacterPool characterPool;
    private void Start()
    {
        GameManager.GetInstant().BuildManager.OnBuild += CreateCharacterOnCell;
    }

    private void CreateCharacterOnCell(Vector3 position, CharacterData characterData)
    {
        var init = characterPool.GetObject();
        var character = init.GetComponent<Character>();
        character.OnSell += ReturnObjectToPool;
        character.OnSell += GameManager.GetInstant().Drawmap.RemoveEmptyTile;
        character.SetUpData(characterData);
        init.transform.position = position;
        init.SetActive(true);
    }

    private void ReturnObjectToPool(Character character)
    {
        characterPool.ReturnPool(character.gameObject);
        character.OnSell -= ReturnObjectToPool;
        character.OnSell -= GameManager.GetInstant().Drawmap.RemoveEmptyTile;
    }
}
