using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;
public class InputManager : MonoBehaviour
{
    [BoxGroup("Button UI")]
    [SerializeField] private List<Button> chaButtons;
    [BoxGroup("Button UI")]
    [SerializeField] private Button startButton;
    [BoxGroup("Button UI")]
    [SerializeField] private Button pauseButon;
    private BuildManager buildManager;
    private GameManager gameManager;
    public Action OnPause;

    private void Start()
    {
        gameManager = GameManager.GetInstant();
        buildManager = GetComponent<BuildManager>();
        for (int i = 0; i < 5; i++)
        {
            int index = i;
            chaButtons[index].onClick.AddListener(() => OnCardInput(index));
            chaButtons[index].GetComponent<CardSlot>().SetupData(index);
        }
        startButton.onClick.AddListener(OnStartInput);
        pauseButon.onClick.AddListener(() => OnPauseInput());
    }

    private void OnCardInput(int index)
    {
        Debug.Log("Click!");
        if (GameManager.GetInstant().StageType != StageType.Prepare) return;
        Debug.Log("Prepare!");
        InventoryCharacter data = PlayerData.Instant.GetLoadOutData(index);
        Debug.Log("GetLoadOutData!");
        if (data == null)
        {
            return;
        }
        Debug.Log("data!");
        if (data.characterData.costToBuild > GameManager.GetInstant().DicePoint)
        {
            return;
        }
        Debug.Log("DicePoint!");
        buildManager.StartDrawBuildShadow(data);
        if (buildManager.InBuild)
        {
            DisableButton();
        }
    }

    private void OnStartInput()
    {
        gameManager.UiSystem.OnStageStart();
    }


    private void OnPauseInput()
    {
        OnPause?.Invoke();
    }

    private void DisableButton()
    {
        for (int i = 0; i < 5; i++)
        {
            if (chaButtons[i].gameObject.activeInHierarchy)
                chaButtons[i].interactable = false;
        }
    }

    public void ActiveButton()
    {
        for (int i = 0; i < 5; i++)
        {
            if (chaButtons[i].gameObject.activeInHierarchy)
                chaButtons[i].interactable = true;
        }
    }
}
