using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using DG.Tweening;
using UnityEngine;
using System.Threading.Tasks;
using TMPro;
using System;
using UnityEngine.UI;

public class UIGamePlaySystem : MonoBehaviour
{
    [TabGroup("RollDicePanel")]
    [SerializeField] private GameObject dicePanel;

    [TabGroup("CardPanel")]
    [SerializeField] private GameObject cardPanel;
    [TabGroup("CardPanel")]
    [SerializeField] private TMP_Text dicePointTxt;
    [TabGroup("CardPanel")]
    [SerializeField] private TMP_Text diceRollTxt;
    [TabGroup("CardPanel")]
    [SerializeField] private TMP_Text goldRewardTxt;
    [TabGroup("CardPanel")]
    [SerializeField] private TMP_Text gemRewardTxt;

    [TabGroup("MiddlePanel")]
    [SerializeField] private GameObject diceRollCountLeft;
    [TabGroup("MiddlePanel")]
    [SerializeField] private GameObject diceRollCountRight;
    [TabGroup("MiddlePanel")]
    [SerializeField] private GameObject diceRoll;
    [TabGroup("MiddlePanel")]
    [SerializeField] private TMP_Text stageStatus;

    [TabGroup("TopPanel")]
    [SerializeField] private GameObject waveInfoContaner;
    [TabGroup("TopPanel")]
    [SerializeField] private GameObject pauseButton;
    [TabGroup("TopPanel")]
    [SerializeField] private TMP_Text remainTxt;
    [TabGroup("TopPanel")]
    [SerializeField] private TMP_Text waveTxt;

    [TabGroup("PausePanel")]
    [SerializeField] private GameObject pausePanel;
    [TabGroup("PausePanel")]
    [SerializeField] private Button resumeButton;
    [TabGroup("PausePanel")]
    [SerializeField] private Button exitButton;
    [TabGroup("PausePanel")]
    [SerializeField] private GameObject ConfirmContainer;
    [TabGroup("PausePanel")]
    [SerializeField] private Button confirmButton;
    [TabGroup("PausePanel")]
    [SerializeField] private Button cancelButton;


    public Action OnFadeInDiceCount;

    public Action OnWaveStart;

    private GameManager gameManager;

    private void OnEnable()
    {
        gameManager = GameManager.GetInstant();
        gameManager.DiceManager.OnDiceRollEnd += OnDicerollComplete;
        gameManager.BuildManager.OnBuildCharacter += OnBuildCharacterComplete;
        gameManager.DiceManager.OnDiceCountEnd += OnDicerollComplete;
        gameManager.GameSpawn.OnWaveEnd += OnWaveEnd;
    }

    private void OnDisable()
    {
        gameManager.DiceManager.OnDiceRollEnd -= OnDicerollComplete;
        gameManager.BuildManager.OnBuildCharacter -= OnBuildCharacterComplete;
        gameManager.DiceManager.OnDiceCountEnd -= OnDicerollComplete;
        gameManager.GameSpawn.OnWaveEnd -= OnWaveEnd;
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
        stageStatus.transform.localPosition = new Vector3(-800f, 0, 0);
        stageStatus.text = "Prepare";
        Sequence sequence = DOTween.Sequence();
        sequence.Append(diceRollCountLeft.transform.DOLocalMoveX(-320f, 1f))
        .Join(diceRollCountRight.transform.DOLocalMoveX(320f, 1f))
        .Append(stageStatus.transform.DOLocalMoveX(0f, 1f).SetEase(Ease.OutQuint))
        .Append(stageStatus.transform.DOLocalMoveX(-800f, 0.5f).SetDelay(1f))
        .Join(diceRoll.transform.DOLocalMoveX(0f, 1f).SetEase(Ease.OutBack, 3f));
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

    public async void OnStageStart()
    {
        FadeOutUI();
        await PlayStartIntro();
        await PlayStartOutro();
        PlayWaveInfoIntro();
        OnWaveStart?.Invoke();
    }

    private async Task PlayStartIntro()
    {
        int wave = gameManager.GameSpawn.GetCurrentWave();
        if (wave != -1)
        {
            stageStatus.text = $"Wave {wave}";
            waveTxt.text = $"{wave}";
        }
        else
        {
            stageStatus.text = $"Last Wave!!!";
            waveTxt.text = "last";
        }
        stageStatus.transform.localPosition = new Vector3(-800f, 0, 0);
        Sequence sequence = DOTween.Sequence();
        sequence.Append(diceRollCountLeft.transform.DOLocalMoveX(-320f, 1f))
        .Join(diceRollCountRight.transform.DOLocalMoveX(320f, 1f))
        .Append(stageStatus.transform.DOLocalMoveX(0f, 1f).SetEase(Ease.OutQuint));
        await sequence.AsyncWaitForCompletion();
    }
    private async Task PlayStartOutro()
    {
        Sequence sequence = DOTween.Sequence();
        sequence.SetDelay(0.5f).Append(stageStatus.transform.DOLocalMoveX(800f, 1f))
        .Append(diceRollCountRight.transform.DOLocalMoveX(1000f, 1f))
        .Join(diceRollCountLeft.transform.DOLocalMoveX(-1000f, 1f));
        await sequence.AsyncWaitForCompletion();
    }

    private void PlayWaveInfoIntro()
    {
        float speed = 0.5f;
        Sequence sequence = DOTween.Sequence();
        sequence.Append(waveInfoContaner.transform.DOLocalMoveX(525f, speed))
        .Join(pauseButton.transform.DOLocalMoveY(30f, speed));
    }

    private void PlayWaveInfoOutro()
    {
        float speed = 0.5f;
        Sequence sequence = DOTween.Sequence();
        sequence.Append(waveInfoContaner.transform.DOLocalMoveX(760f, speed))
        .Join(pauseButton.transform.DOLocalMoveY(100f, speed));
    }

    private void PlayPauseMenuIntro()
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Append(pausePanel.transform.DOLocalMoveY(0f, 0.5f));
    }
    private void PlayPauseMenuOutro()
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Append(pausePanel.transform.DOLocalMoveY(500f, 0.5f));
    }
    public void UpdateEnemyRemain(int value)
    {
        remainTxt.text = $"{value}";
    }
    private void OnDicerollComplete(int _)
    {
        UpdateDataInfo();
    }

    private void OnWaveEnd(int _, int __)
    {
        PlayWaveInfoOutro();
        FadeInDiceRollCount();
    }

    private void OnPause()
    {

    }

    private void OnBuildCharacterComplete(Vector3 _, InventoryCharacter __)
    {
        UpdateDataInfo();
    }

    private void UpdateDataInfo()
    {
        dicePointTxt.text = $"{gameManager.DicePoint}";
        diceRollTxt.text = $"{gameManager.DiceRollCount}";
        goldRewardTxt.text = $"{gameManager.GoldReward}";
        gemRewardTxt.text = $"{gameManager.GemReward}";
    }
    public void SetUIPosition()
    {
        dicePanel.transform.position = new Vector3(-280f, 8f);
        cardPanel.transform.position = new Vector3(640, -120f);
    }


}
