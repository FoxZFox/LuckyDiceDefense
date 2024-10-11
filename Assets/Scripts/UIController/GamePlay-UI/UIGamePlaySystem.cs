using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using DG.Tweening;
using UnityEngine;
using System.Threading.Tasks;
using TMPro;

public class UIGamePlaySystem : MonoBehaviour
{
    [TabGroup("UIObject")]
    [SerializeField] private GameObject dicePanel;
    [TabGroup("UIObject")]
    [SerializeField] private GameObject cardPanel;
    [TabGroup("UIObject")]
    [SerializeField] private TMP_Text dicePointTxt;
    [TabGroup("UIObject")]
    [SerializeField] private TMP_Text diceRollTxt;

    private GameManager gameManager;

    private void OnEnable()
    {
        gameManager = GameManager.GetInstant();
        gameManager.DiceManager.OnDiceRollEnd += OnDicerollComplete;
        gameManager.BuildManager.OnBuildCharacter += OnBuildCharacterComplete;
    }

    private void OnDisable()
    {
        gameManager.DiceManager.OnDiceRollEnd -= OnDicerollComplete;
        gameManager.BuildManager.OnBuildCharacter -= OnBuildCharacterComplete;
    }
    [ButtonGroup()]
    public async void FadeInUI()
    {
        List<Task> tasks = new List<Task>
        {
            dicePanel.transform.DOMoveX(11f, 1f).SetEase(Ease.InOutBack).AsyncWaitForCompletion(),
            cardPanel.transform.DOMoveY(65f, 1f).SetEase(Ease.InOutBack).AsyncWaitForCompletion()
        };
        await Task.WhenAll(tasks);
        Debug.Log("Done");
    }

    [ButtonGroup()]
    public async void FadeOutUI()
    {
        List<Task> tasks = new List<Task>
        {
            dicePanel.transform.DOMoveX(-280f, 1f).SetEase(Ease.InOutBack).AsyncWaitForCompletion(),
            cardPanel.transform.DOMoveY(-120f, 1f).SetEase(Ease.InOutBack).AsyncWaitForCompletion()
        };
        await Task.WhenAll(tasks);
        Debug.Log("Done");
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
