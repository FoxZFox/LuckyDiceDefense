using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class DiceManager : MonoBehaviour
{
    [TabGroup("Dice-Info")]
    [SerializeField] private Sprite[] diceImage;
    [TabGroup("Dice-Info")]
    [SerializeField] private Image dice1obj;
    [TabGroup("Dice-Info")]
    [SerializeField] private Image dice2obj;
    [TabGroup("Dice-Info")]
    [SerializeField] private bool isRollDice = false;
    private Dictionary<Sprite, int> dicesValue = new Dictionary<Sprite, int>();
    [ShowInInspector] private CountDownTimer timer;

    [TabGroup("Dice-Setting")]
    [SerializeField] private float minDiceTime;
    [TabGroup("Dice-Setting")]
    [SerializeField] private float maxDiceTime;
    [TabGroup("Dice-Setting")]
    [SerializeField, Range(0f, 1f)] private float changeSpeed;
    int d1index, d2index;


    private void Start()
    {
        MapData();
        timer = new CountDownTimer(10);
    }

    private void Update()
    {
        if (isRollDice)
        {
            timer.Tick(Time.deltaTime);
        }
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
        if (isRollDice)
        {
            return;
        }
        isRollDice = true;
        timer.OnTimeStop += () =>
        {
            isRollDice = false;
        };
        timer.Start();
        timer.Reset(Random.Range(minDiceTime, maxDiceTime));
        d1index = Random.Range(0, 6);
        d2index = Random.Range(0, 6);
        Debug.Log($"Lock Dice1 : {d1index} Dice2: {d2index}");
        StartCoroutine(RollDiceAnimation());
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
    }
}
