using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;
public class InputManager : MonoBehaviour
{
    [BoxGroup("Button UI")]
    [SerializeField] private Button cha1Button;
    [BoxGroup("Button UI")]
    [SerializeField] private Button cha2Button;
    [BoxGroup("Button UI")]
    [SerializeField] private Button cha3Button;
    [BoxGroup("Button UI")]
    [SerializeField] private Button cha4Button;
    [BoxGroup("Button UI")]
    [SerializeField] private Button cha5Button;
    private BuildManager buildManager;

    private void Start()
    {
        buildManager = GetComponent<BuildManager>();
        cha1Button.onClick.AddListener(() => OnCardInput(0));
        cha2Button.onClick.AddListener(() => OnCardInput(1));
        cha3Button.onClick.AddListener(() => OnCardInput(2));
        cha4Button.onClick.AddListener(() => OnCardInput(3));
        cha5Button.onClick.AddListener(() => OnCardInput(4));
    }

    private void OnCardInput(int index)
    {
        CharacterData data = PlayerData.Instant.GetLoadOutData(index);
        if (data == null)
        {
            return;
        }
        if (data.costToBuild > GameManager.GetInstant().DicePoint)
        {
            return;
        }
        buildManager.StartDrawBuildShadow(data);
        if (buildManager.InBuild)
        {
            DisableButton();
        }
    }

    private void DisableButton()
    {
        cha1Button.interactable = false;
        cha2Button.interactable = false;
        cha3Button.interactable = false;
        cha4Button.interactable = false;
        cha5Button.interactable = false;
    }

    public void ActiveButton()
    {
        cha1Button.interactable = true;
        cha2Button.interactable = true;
        cha3Button.interactable = true;
        cha4Button.interactable = true;
        cha5Button.interactable = true;
    }
}
