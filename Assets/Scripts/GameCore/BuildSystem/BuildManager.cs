using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.EventSystems;

public class BuildManager : MonoBehaviour
{
    [BoxGroup("Data")]
    [SerializeField] private TileBase cantBuildTile;
    [BoxGroup("DataForTest")]
    [SerializeField] private GameObject target;
    [BoxGroup("DataForTest")]
    [SerializeField] private Grid gridmap;
    [SerializeField] private Transform targetTransform;
    [SerializeField] private SpriteRenderer targetSpriteRenderer;
    private bool inBuild = false;
    private bool canBuild = false;
    public bool InBuild => inBuild;
    public Action<Vector3, CharacterData> OnBuildCharacter;
    [SerializeField] private CharacterData data;
    public void StartDrawBuildShadow(CharacterData characterData)
    {
        data = characterData;
        inBuild = true;
        targetSpriteRenderer.sprite = data.placeHolderSpitre;
        target.SetActive(true);
    }
    void Update()
    {
        if (inBuild)
        {
            if (!CheckBoundAndEvent())
            {
                return;
            }
            MovePosition();
            if (Input.GetMouseButtonDown(0) && canBuild)
            {
                Vector3 position = targetTransform.position;
                OnBuildCharacter?.Invoke(position, data);
                inBuild = false;
                canBuild = false;
                target.SetActive(false);
                targetTransform.position = new Vector3(0, 10f, 0);
                GetComponent<InputManager>().ActiveButton();
            }
        }
    }

    private bool CheckBoundAndEvent()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return false;
        }
        var mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Vector3 minBounds = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, Camera.main.nearClipPlane));
        Vector3 maxBounds = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, Camera.main.nearClipPlane));

        if (mousePosition.x < minBounds.x || mousePosition.x > maxBounds.x ||
            mousePosition.y < minBounds.y || mousePosition.y > maxBounds.y)
        {
            return false;
        }
        return true;
    }

    private void MovePosition()
    {
        var mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;

        var cellPosstion = gridmap.WorldToCell(mousePosition);
        Vector3 newPosition = new Vector3(cellPosstion.x + 0.5f, cellPosstion.y + 0.5f);
        CheckTileCanBuild(cellPosstion);
        targetTransform.position = newPosition;
    }

    private void CheckTileCanBuild(Vector3Int cell)
    {
        var tiles = gridmap.GetComponentsInChildren<Tilemap>();
        foreach (var tile in tiles)
        {
            var tilebase = tile.GetTile(cell);
            if (tilebase != cantBuildTile)
            {
                target.GetComponent<SpriteRenderer>().color = Color.green;
                canBuild = true;
                continue;
            }
            Debug.Log(tilebase);
            if (tilebase == cantBuildTile)
            {
                target.GetComponent<SpriteRenderer>().color = Color.red;
                canBuild = false;
                return;
            }
        }
    }
}
