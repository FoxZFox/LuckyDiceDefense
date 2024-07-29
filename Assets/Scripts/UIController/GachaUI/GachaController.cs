using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using Unity.VisualScripting;
using TMPro;

public class GachaController : MonoBehaviour
{
    public static GachaController instant;
    [SerializeField] private GameObject cardPrefabs;
    [SerializeField] private GameObject cardSpawnParent;
    [SerializeField] private Vector3[] cardSpawnPoints;
    [SerializeField] private GameObject panel;
    [SerializeField] private int cardSpawnCount = 8;
    [Header("Gacha For Test")]
    [SerializeField] private GachaData gachaTableObject;
    [SerializeField] private GameObject cardStar;
    [SerializeField] private bool debugCardId;
    private List<GameObject> debugGameobjects = new List<GameObject>();
    private List<GameObject> cards = new List<GameObject>();
    private float tweenDuration = 2f;

    private void Start()
    {
        if (instant == null)
        {
            instant = this;
        }
        else
        {
            Destroy(gameObject);
        }
        InstantCard();
        panel.SetActive(false);
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
            cards.Add(instant);
        }
    }
    private void SetCardInteractable()
    {

        foreach (var item in cards)
        {
            item.GetComponent<Button>().enabled = true;
            //need optimize 
            if (debugCardId)
            {
                var instant = Instantiate(cardStar, cardSpawnParent.transform);
                debugGameobjects.Add(instant);
                var text = instant.GetComponent<TextMeshProUGUI>();
                int id = item.GetComponent<Card>().CardID;
                if (id >= 50)
                {
                    text.text = "5Star";
                }
                else if (id >= 40 && id <= 49)
                {
                    text.text = "4Star";
                }
                else if (id >= 30 && id <= 39)
                {
                    text.text = "3Star";
                }
                else if (id >= 20 && id <= 29)
                {
                    text.text = "2Star";
                }
                else if (id >= 10 && id <= 19)
                {
                    text.text = "1Star";
                }
                instant.transform.localPosition = item.transform.localPosition;
                text.color = Color.green;
            }
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
        foreach (var item in cards)
        {

        }
        for (int i = 0; i < cards.Count; i++)
        {
            cards[i].transform.localScale = Vector3.zero;
            cards[i].GetComponent<Card>().SetCardID(cardsid[i]);
        }
        StartCoroutine(DrawAnimation());
    }

    private IEnumerator DrawAnimation()
    {
        for (int i = 0; i < cards.Count; i++)
        {
            cards[i].SetActive(true);
            if (i == cards.Count - 1)
            {
                cards[i].transform.DOScale(1f, tweenDuration).SetEase(Ease.OutBounce).OnComplete(SetCardInteractable);
            }
            else
            {
                cards[i].transform.DOScale(1f, tweenDuration).SetEase(Ease.OutBounce);
            }
            yield return new WaitForSeconds(0.2f);
        }
    }

    public void PickupCard(int cardID)
    {
        //need optimize
        if (cardID >= 50)
        {
            Debug.Log("5Star");
        }
        else if (cardID >= 40 && cardID <= 49)
        {
            Debug.Log("4Star");
        }
        else if (cardID >= 30 && cardID <= 39)
        {
            Debug.Log("3Star");
        }
        else if (cardID >= 20 && cardID <= 29)
        {
            Debug.Log("2Star");
        }
        else if (cardID >= 10 && cardID <= 19)
        {
            Debug.Log("1Star");
        }
        // Debug.Log($"Card ID = {cardID}");
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
