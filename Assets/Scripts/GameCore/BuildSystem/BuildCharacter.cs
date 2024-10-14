using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildCharacter : MonoBehaviour
{
    [SerializeField] private CharacterPool characterPool;
    private GameManager gameManager;
    private void Start()
    {
        gameManager = GameManager.GetInstant();
        gameManager.BuildManager.OnBuildCharacter += CreateCharacterOnCell;
    }

    private void CreateCharacterOnCell(Vector3 position, InventoryCharacter data)
    {
        var init = characterPool.GetObject();
        var character = init.GetComponent<Character>();
        character.OnSell += ReturnObjectToPool;
        character.OnSell += gameManager.DrawMap.RemoveEmptyTile;
        character.SetUpData(data);
        init.transform.position = position;
        init.SetActive(true);
    }

    private void ReturnObjectToPool(Character character)
    {
        characterPool.ReturnPool(character.gameObject);
        character.OnSell -= ReturnObjectToPool;
        character.OnSell -= gameManager.DrawMap.RemoveEmptyTile;
    }
}
