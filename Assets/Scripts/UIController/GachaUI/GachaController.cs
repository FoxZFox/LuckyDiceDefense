using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;

public class GachaController : MonoBehaviour
{
    public static GachaController instant;
    [SerializeField] private GameObject cardPrefabs;
    [SerializeField] private GameObject cardSpawnParent;
    [SerializeField] private TMP_Text cardRemainText;
    [SerializeField] private Button confirmButton;
    [SerializeField] private Vector3[] cardSpawnPoints;
    [SerializeField] private GameObject panel;
    [SerializeField] private int cardSpawnCount = 8;
    [SerializeField] private int cardCanSelect = 0;
    [Header("Gacha For Test")]
    [SerializeField] private GachaData gachaTableObject;
    [SerializeField] private GameObject cardStar;
    [SerializeField] private bool debugCardId;
    private List<GameObject> debugGameobjects = new List<GameObject>();
    private List<GameObject> cardpool = new List<GameObject>();
    private float tweenDuration = 2f;

    bool onGacha = false;

    private void Start()
    {
        if (instant == null)
        {
            instant = this;
        }
        InstantCard();
        panel.SetActive(false);
    }

    private void OnDestroy()
    {
        instant = null;
    }

    private void Update()
    {
        if (onGacha)
            SetCardRemainText();
    }

    private void OnDisable()
    {
        instant = null;
    }

    private void InstantCard()
    {
        for (int i = 0; i < cardSpawnCount; i++)
        {
            var instant = Instantiate(cardPrefabs, new Vector3(0, 0, 0), Quaternion.identity);
            instant.transform.SetParent(cardSpawnParent.transform);
            instant.transform.localPosition = cardSpawnPoints[i];
            instant.transform.localScale = Vector3.zero;
            instant.SetActive(false);
            cardpool.Add(instant);
        }
    }

    [ContextMenu("DrawCard")]
    private void DrawCard()
    {
        foreach (var item in debugGameobjects)
        {
            Destroy(item);
        }
        debugGameobjects.Clear();
        var cardsid = gachaTableObject.GetDeckList();
        for (int i = 0; i < cardpool.Count; i++)
        {
            cardpool[i].transform.localScale = Vector3.zero;
            int cardAmount = UnityEngine.Random.Range(1, 17);
            cardpool[i].GetComponent<Card>().SetCardID(cardsid[i], cardAmount);
        }
        panel.SetActive(true);
        StartCoroutine(DrawAnimation());
    }

    public void SpawnGacha(GachaData gachaData, int cardSelect)
    {
        foreach (var item in debugGameobjects)
        {
            Destroy(item);
        }
        debugGameobjects.Clear();

        List<CharacterData> chaData = gachaData.GetDeckList();
        for (int i = 0; i < cardpool.Count; i++)
        {
            cardpool[i].transform.localScale = Vector3.zero;
            int cardAmount = UnityEngine.Random.Range(1, 17);
            cardpool[i].GetComponent<Card>().SetCardID(chaData[i], cardAmount);
        }
        cardCanSelect = cardSelect;
        panel.SetActive(true);
        StartCoroutine(DrawAnimation());
    }

    private IEnumerator DrawAnimation()
    {
        for (int i = 0; i < cardpool.Count; i++)
        {
            cardpool[i].SetActive(true);
            if (i == cardpool.Count - 1)
            {
                cardpool[i].transform.DOScale(1f, tweenDuration).SetEase(Ease.OutBounce).OnComplete(SetCardInteractable);
            }
            else
            {
                cardpool[i].transform.DOScale(1f, tweenDuration).SetEase(Ease.OutBounce);
            }
            yield return new WaitForSeconds(0.2f);
        }
    }
    private void SetCardInteractable()
    {

        foreach (var item in cardpool)
        {
            item.GetComponent<Button>().enabled = true;
            //Debug
            if (debugCardId)
            {
                var instant = Instantiate(cardStar, cardSpawnParent.transform);
                debugGameobjects.Add(instant);
                var text = instant.GetComponent<TextMeshProUGUI>();
                int star = item.GetComponent<Card>().CardStar;
                text.text = $"{star}Star";
                instant.transform.localPosition = item.transform.localPosition;
                text.color = Color.green;
            }
        }
        onGacha = true;
    }


    private void SetCardRemainText()
    {
        if (cardCanSelect > 0 && !cardRemainText.gameObject.activeInHierarchy)
        {
            cardRemainText.gameObject.SetActive(true);
            confirmButton.gameObject.SetActive(false);
        }
        else if (cardCanSelect < 1 && cardRemainText.gameObject.activeInHierarchy)
        {
            cardRemainText.gameObject.SetActive(false);
            confirmButton.gameObject.SetActive(true);
        }
        cardRemainText.text = $"Card Remain {cardCanSelect}";
    }

    [SerializeField] private List<GameObject> cardSelected = new List<GameObject>();
    public bool PickupCard(GameObject cardObject, int cardStar, bool selected = false)
    {
        if (selected)
        {
            cardCanSelect++;
            if (cardSelected.Contains(cardObject))
                cardSelected.Remove(cardObject);
            return false;
        }
        if (cardCanSelect > 0)
        {
            cardCanSelect--;
        }
        else
        {
            Debug.Log("Cant Select did card");
            return false;
        }
        Debug.Log($"Select {cardStar}Star");
        cardSelected.Add(cardObject);
        return true;
    }

    public void ConfirmCardSelect()
    {
        foreach (var item in cardSelected)
        {
            if (item.TryGetComponent<Card>(out Card card))
            {
                InventoryManager.instant.AddCard(InventoryManager.instant.GetCardData(card.characterData), card.Amount);
            }
        }
        cardSelected.Clear();
        panel.SetActive(false);
    }

    // #if UNITY_EDITOR
    //     private void OnDrawGizmos()
    //     {
    //         Gizmos.color = Color.green;
    //         if (cardSpawnPoints.Length > 0)
    //         {
    //             foreach (var item in cardSpawnPoints)
    //             {
    //                 Gizmos.DrawSphere(Camera.main.transform.TransformPoint(cardSpawnParent.transform.position + item), 40f);
    //             }
    //         }

    //     }
    // #endif
}
