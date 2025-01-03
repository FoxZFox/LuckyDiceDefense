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
    [BoxGroup("Data")]
    [SerializeField] private GameObject target;
    [BoxGroup("Data")]
    [SerializeField] private Tilemap cantBuildMap;

    [BoxGroup("DataForTest")]
    [SerializeField] private Grid gridmap;
    [SerializeField] private Transform targetTransform;
    [SerializeField] private SpriteRenderer targetSpriteRenderer;
    private bool inBuild = false;
    private bool canBuild = false;
    public bool InBuild => inBuild;
    public Action<Vector3, InventoryCharacter> OnBuildCharacter;
    [SerializeField] private InventoryCharacter data;
    private GameManager gameManager;

    private void Awake()
    {
        gameManager = GameManager.GetInstant();
    }


    private void OnEnable()
    {
        gameManager.DrawMap.OnDrawMap += OnDrawMapComPlete;
    }

    private void OnDisable()
    {
        gameManager.DrawMap.OnDrawMap -= OnDrawMapComPlete;
    }

    private void OnDrawMapComPlete(Tilemap tilemap)
    {
        cantBuildMap = tilemap;
    }

    public void StartDrawBuildShadow(InventoryCharacter value)
    {
        data = value;
        inBuild = true;
        targetSpriteRenderer.sprite = data.characterData.placeHolderSpitre;
        target.SetActive(true);
    }
    void Update()
    {
        if (inBuild)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                ResetBuild();
                return;
            }
            if (!CheckBoundAndEvent())
            {
                return;
            }

            MovePosition();
            if (Input.GetMouseButtonDown(0) && canBuild)
            {
                Vector3 position = targetTransform.position;
                OnBuildCharacter?.Invoke(position, data);
                ResetBuild();
            }
        }
    }

    private void ResetBuild()
    {
        inBuild = false;
        canBuild = false;
        target.SetActive(false);
        targetTransform.position = new Vector3(0, 10f, 0);
        GetComponent<InputManager>().ActiveButton();
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
        var tilebase = cantBuildMap.GetTile(cell);
        if (tilebase != cantBuildTile)
        {
            target.GetComponent<SpriteRenderer>().color = Color.green;
            canBuild = true;
        }
        Debug.Log(tilebase);
        if (tilebase == cantBuildTile)
        {
            target.GetComponent<SpriteRenderer>().color = Color.red;
            canBuild = false;
        }
    }
}
