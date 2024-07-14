using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using Unity.VisualScripting;

public class GachaController : MonoBehaviour
{
    public static GachaController instant;
    [SerializeField] private GameObject cardPrefabs;
    [SerializeField] private GameObject cardSpawnParent;
    [SerializeField] private Vector3[] cardSpawnPoints;
    [SerializeField] private int cardSpawnCount = 8;
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
        }
    }

    [ContextMenu("DrawCard")]
    private void DrawCard()
    {
        foreach (var item in cards)
        {
            item.transform.localScale = Vector3.zero;
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

    public void PickupCard()
    {
        Debug.Log("CardPickup");
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        if (cardSpawnPoints.Length > 0)
        {
            foreach (var item in cardSpawnPoints)
            {
                Gizmos.DrawSphere(Camera.main.transform.TransformPoint(cardSpawnParent.transform.position + item), 40f);
            }
        }

    }
#endif
}
