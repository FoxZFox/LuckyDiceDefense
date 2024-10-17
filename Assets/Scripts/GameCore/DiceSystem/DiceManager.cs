using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class DiceManager : MonoBehaviour
{
    [TabGroup("Dice-Info")]
    [SerializeField] private Sprite[] diceImage;
    [TabGroup("Dice-Info")]
    [SerializeField] private Image dice1obj;
    [TabGroup("Dice-Info")]
    [SerializeField] private Image dice2obj;
    [TabGroup("Dice-Info")]
    [SerializeField] private Image diceCountObj;
    [TabGroup("Dice-Info")]
    [SerializeField, ReadOnly] private bool isRollDice = false;
    [TabGroup("Dice-Info")]
    [SerializeField, Required] private Button rollButton;
    private Dictionary<Sprite, int> dicesValue = new Dictionary<Sprite, int>();
    [ShowInInspector] private CountDownTimer timer;

    [TabGroup("Dice-Setting")]
    [SerializeField] private float minDiceTime;
    [TabGroup("Dice-Setting")]
    [SerializeField] private float maxDiceTime;
    [TabGroup("Dice-Setting")]
    [SerializeField, Range(0f, 1f)] private float changeSpeed;
    int d1index, d2index;
    public Action OnRiceRollStart;
    public Action<int> OnDiceRollEnd;

    public Action<int> OnDiceCountEnd;
    private GameManager gameManager;

    private void Awake()
    {
        gameManager = GameManager.GetInstant();
        MapData();
        rollButton.onClick.AddListener(OnClickRoll);
    }

    private void OnEnable()
    {
        gameManager.UiSystem.OnFadeInDiceCount += OnFadeDiceCountComeplete;
    }

    private void OnDisable()
    {
        gameManager.UiSystem.OnFadeInDiceCount -= OnFadeDiceCountComeplete;
    }

    private void Update()
    {
        if (isRollDice)
        {
            timer.Tick(Time.deltaTime);
        }
    }

    private void OnClickRoll()
    {
        if (gameManager.DiceRollCount < 1)
        {
            return;
        }
        rollButton.interactable = false;
        OnRiceRollStart?.Invoke();
        RollDice();
    }

    private void MapData()
    {
        for (int i = 0; i < 6; i++)
        {
            dicesValue.Add(diceImage[i], i + 1);
        }
    }
    [Button()]
    public void RollDice()
    {
        if (isRollDice || gameManager.StageType != StageType.Prepare)
        {
            return;
        }
        timer = new CountDownTimer(Random.Range(minDiceTime, maxDiceTime));
        isRollDice = true;
        timer.OnTimeStop += () =>
        {
            isRollDice = false;
        };
        timer.Start();
        d1index = Random.Range(0, 6);
        d2index = Random.Range(0, 6);
        Debug.Log($"Lock Dice1 : {d1index} Dice2: {d2index}");
        SoundManager.Instant.PlayAudioOneShot(SoundType.RollDice, transform);
        StartCoroutine(RollDiceAnimation());
    }

    private void OnFadeDiceCountComeplete()
    {
        if (isRollDice)
        {
            return;
        }
        timer = new CountDownTimer(Random.Range(minDiceTime, maxDiceTime));
        isRollDice = true;
        timer.OnTimeStop += () =>
        {
            isRollDice = false;
        };
        timer.Start();
        StartCoroutine(RollDiceCountAnimation());
    }

    private IEnumerator RollDiceCountAnimation()
    {
        SoundManager.Instant.PlayAudioOneShot(SoundType.ThrowDice, transform);
        while (isRollDice)
        {
            int diceIndex = Random.Range(0, 6);
            diceCountObj.sprite = diceImage[diceIndex];
            yield return new WaitForSeconds(changeSpeed);
        }
        yield return new WaitForSeconds(0.5f);
        OnDiceCountEnd?.Invoke(dicesValue[diceCountObj.sprite]);
        UpdateRollButton();
    }

    private IEnumerator RollDiceAnimation()
    {
        while (isRollDice)
        {
            int dice1 = Random.Range(0, 6);
            int dice2 = Random.Range(0, 6);
            yield return new WaitForSeconds(changeSpeed);
            if (!isRollDice)
            {
                break;
            }
            dice1obj.sprite = diceImage[dice1];
            dice2obj.sprite = diceImage[dice2];
        }
        dice1obj.sprite = diceImage[d1index];
        dice2obj.sprite = diceImage[d2index];
        Debug.Log($"Dice1 : {dicesValue[dice1obj.sprite]} Dice2 : {dicesValue[dice2obj.sprite]}");
        UpdateRollButton();
        OnDiceRollEnd?.Invoke(dicesValue[dice1obj.sprite] + dicesValue[dice2obj.sprite]);
    }

    private void UpdateRollButton()
    {
        if (gameManager.DiceRollCount > 0)
        {
            rollButton.interactable = true;
        }
        else
        {
            rollButton.interactable = false;
            gameManager.UiSystem.FadeOutRollDiceUI();
        }

    }
}
