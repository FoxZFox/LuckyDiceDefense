using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardSlot : MonoBehaviour
{
    [SerializeField] private Image slotcardPlaceHolder;
    [SerializeField] private TMP_Text costText;
    public void SetupData(int index)
    {
        var data = PlayerData.Instant.GetLoadOutData(index);
        if (data == null)
        {
            gameObject.SetActive(false);
            return;
        }
        slotcardPlaceHolder.sprite = data.characterData.placeHolderSpitre;
        costText.text = data.characterData.costToBuild.ToString();
    }

}
