using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using DG.Tweening;
using UnityEngine;
using Unity.VisualScripting;
using System.Threading.Tasks;

public class UIGamePlaySystem : MonoBehaviour
{
    [TabGroup("UIObject")]
    [SerializeField] private GameObject dicePanel;
    [TabGroup("UIObject")]
    [SerializeField] private GameObject cardPanel;
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
            cardPanel.transform.DOMoveY(-65f, 1f).SetEase(Ease.InOutBack).AsyncWaitForCompletion()
        };
        await Task.WhenAll(tasks);
        Debug.Log("Done");
    }

#if UNITY_EDITOR
    public void SetUIPosition()
    {
        dicePanel.transform.position = new Vector3(-280f, 8f);
        cardPanel.transform.position = new Vector3(640, -65f);
    }
#endif
}
