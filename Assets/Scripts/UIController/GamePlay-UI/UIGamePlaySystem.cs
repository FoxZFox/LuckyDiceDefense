using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using DG.Tweening;
using UnityEngine;
using System.Threading.Tasks;
using TMPro;
using System;

public class UIGamePlaySystem : MonoBehaviour
{
    [TabGroup("UIObject")]
    [SerializeField] private GameObject dicePanel;
    [TabGroup("UIObject")]
    [SerializeField] private GameObject cardPanel;
    [TabGroup("UIObject")]
    [SerializeField] private GameObject diceRollCountLeft;
    [TabGroup("UIObject")]
    [SerializeField] private GameObject diceRollCountRight;
    [TabGroup("UIObject")]
    [SerializeField] private GameObject diceRoll;
    [TabGroup("UIObject")]
    [SerializeField] private TMP_Text dicePointTxt;
    [TabGroup("UIObject")]
    [SerializeField] private TMP_Text diceRollTxt;

    public Action OnFadeInDiceCount;

    private GameManager gameManager;

    private void OnEnable()
    {
        gameManager = GameManager.GetInstant();
        gameManager.DiceManager.OnDiceRollEnd += OnDicerollComplete;
        gameManager.BuildManager.OnBuildCharacter += OnBuildCharacterComplete;
        gameManager.DiceManager.OnDiceCountEnd += OnDicerollComplete;
    }

    private void OnDisable()
    {
        gameManager.DiceManager.OnDiceRollEnd -= OnDicerollComplete;
        gameManager.BuildManager.OnBuildCharacter -= OnBuildCharacterComplete;
        gameManager.DiceManager.OnDiceCountEnd -= OnDicerollComplete;
    }
    [ButtonGroup()]
    public async void FadeInUI()
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Append(dicePanel.transform.DOMoveX(11f, 0.5f).SetEase(Ease.OutBack))
            .Join(cardPanel.transform.DOMoveY(65f, 0.5f).SetEase(Ease.OutBack));
        await sequence.AsyncWaitForCompletion();
        Debug.Log("Done");
    }

    [ButtonGroup()]
    public async void FadeOutUI()
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Append(dicePanel.transform.DOMoveX(-280f, 0.5f).SetEase(Ease.OutBack))
            .Join(cardPanel.transform.DOMoveY(-120f, 0.5f).SetEase(Ease.OutBack));
        await sequence.AsyncWaitForCompletion();
        Debug.Log("Done");
    }
    public async void FadeInDiceRollCount()
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Append(diceRollCountLeft.transform.DOLocalMoveX(-320f, 1f))
        .Join(diceRollCountRight.transform.DOLocalMoveX(320f, 1f))
        .Append(diceRoll.transform.DOLocalMoveX(0f, 1f).SetEase(Ease.OutBack, 3f));
        await sequence.AsyncWaitForCompletion();
        OnFadeInDiceCount?.Invoke();
    }
    public async void FadeOutDiceRollCount()
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Append(diceRoll.transform.DOLocalMoveX(-700f, 1f).SetEase(Ease.InBack))
        .Append(diceRollCountRight.transform.DOLocalMoveX(1000f, 1f))
        .Join(diceRollCountLeft.transform.DOLocalMoveX(-1000f, 1f));
        await sequence.AsyncWaitForCompletion();
        gameManager.SetStage(StageType.Prepare);
        FadeInUI();
    }
    public void FadeOutRollDiceUI()
    {
        dicePanel.transform.DOMoveX(-280f, 0.5f).SetDelay(0.5f);

    }
    private void OnDicerollComplete(int _)
    {
        UpdateDataInfo();
    }

    private void OnBuildCharacterComplete(Vector3 _, CharacterData __)
    {
        UpdateDataInfo();
    }

    private void UpdateDataInfo()
    {
        dicePointTxt.text = $"{gameManager.DicePoint}";
        diceRollTxt.text = $"{gameManager.DiceRollCount}";
    }

#if UNITY_EDITOR
    public void SetUIPosition()
    {
        dicePanel.transform.position = new Vector3(-280f, 8f);
        cardPanel.transform.position = new Vector3(640, -120f);
    }
#endif
}
