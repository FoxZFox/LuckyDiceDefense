using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class LoadOutUiController : MonoBehaviour
{
    [BoxGroup("Setting")]
    [SerializeField] private DataSortType dataSortType;
    [BoxGroup("Container")]
    [SerializeField] private GameObject loadOutPanel;
    [BoxGroup("Container")]
    [SerializeField] private GameObject[] slotContainers;
    [BoxGroup("Container")]
    [SerializeField] private GameObject cardPrefab;
    [BoxGroup("Container")]
    [SerializeField] private GameObject content;
    [BoxGroup("Button")]
    [SerializeField] private Button loadOutButton;
    [BoxGroup("Button")]
    [SerializeField] private Button backButton;

    [BoxGroup("Poll-Card")]
    [SerializeField] private List<CharacterCard> loadOutCards;
    [BoxGroup("Poll-Card")]
    [SerializeField] private List<GameObject> loadOutPoll;
    [BoxGroup("Poll-Card")]
    [SerializeField] private List<CharacterCard> cards;
    [BoxGroup("Poll-Card")]
    [SerializeField] private List<int> cardIndex;
    [BoxGroup("Poll-Card")]
    [SerializeField] private List<GameObject> cardpoll;

    private void Awake()
    {
        loadOutButton.onClick.AddListener(LoadCard);
        backButton.onClick.AddListener(SaveDataToPlayer);
    }

    private void SaveDataToPlayer()
    {
        List<InventoryCharacter> data = new List<InventoryCharacter>();
        foreach (var item in loadOutCards)
        {
            if (item == null)
            {
                data.Add(null);
            }
            else
            {
                data.Add(item.GetInventory());
            }
        }
        PlayerData.Instant.UpdateLoadOut(data);
    }
    private void LoadCard()
    {
        loadOutCards.Clear();
        foreach (var item in loadOutPoll)
        {
            if (item != null)
                Destroy(item);
        }
        loadOutPoll.Clear();
        cards.Clear();
        cardIndex.Clear();
        for (int i = 0; i < 5; i++)
        {
            loadOutCards.Add(null);
            loadOutPoll.Add(null);
        }
        foreach (var item in cardpoll)
        {
            Destroy(item);
        }
        cardpoll.Clear();
        var datas = PlayerData.Instant.GetLoadOut();
        InstantLoadoutCard(datas);
        datas = InventoryManager.instant.InventoryCharacters;
        InstantContentCard(datas);
    }
    private void InstantLoadoutCard(List<InventoryCharacter> datas)
    {
        for (int i = 0; i < datas.Count; i++)
        {
            if (datas[i] == null || datas[i].CharacterID == 0)
            {
                continue;
            }
            var data = InstantCard(datas[i], loadOutPanel);
            loadOutCards[i] = data.GetComponent<CharacterCard>();
            loadOutPoll[i] = data;
            data.transform.position = slotContainers[i].transform.position;
        }
    }

    private void InstantContentCard(List<InventoryCharacter> datas)
    {
        var sortdatas = SortData(datas);
        for (int i = 0; i < sortdatas.Count; i++)
        {
            int tempIndex = i;
            var index = loadOutCards.FindIndex(j => j != null && j.GetInventory() == sortdatas[i]);
            if (index != -1)
            {
                cards.Add(loadOutCards[index]);
                cardIndex.Add(i);
                cardpoll.Add(loadOutPoll[index]);
                var button = loadOutPoll[index].gameObject.GetComponent<Button>();
                button.onClick.RemoveAllListeners();
                button.onClick.AddListener(() => UnSelectCard(loadOutCards[index], tempIndex, loadOutPoll[index], index));
            }
            else
            {
                var instant = InstantCard(sortdatas[i], content);
                cards.Add(instant.GetComponent<CharacterCard>());
                cardIndex.Add(i);
                cardpoll.Add(instant);
                var button = instant.GetComponent<Button>();
                button.onClick.RemoveAllListeners();
                button.onClick.AddListener(() => SelectCardToLoadOut(instant.GetComponent<CharacterCard>(), tempIndex, instant));
            }
        }
    }

    private void SelectCardToLoadOut(CharacterCard owner, int index, GameObject gameObject)
    {
        for (int i = 0; i < 5; i++)
        {
            int tempIndex = i;
            if (loadOutCards[i] != null)
            {
                continue;
            }
            loadOutCards[i] = owner;
            loadOutPoll[i] = gameObject;
            gameObject.transform.SetParent(loadOutPanel.transform);
            gameObject.transform.position = slotContainers[i].transform.position;
            var button = gameObject.GetComponent<Button>();
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() => UnSelectCard(owner, index, gameObject, tempIndex));
            break;
        }
    }

    private void UnSelectCard(CharacterCard owner, int oldindex, GameObject gameObject, int index)
    {
        loadOutCards[index] = null;
        loadOutPoll[index] = null;
        gameObject.transform.SetParent(content.transform);
        gameObject.transform.SetSiblingIndex(cardIndex[oldindex]);
        var button = gameObject.GetComponent<Button>();
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => SelectCardToLoadOut(gameObject.GetComponent<CharacterCard>(), oldindex, gameObject));
    }

    private GameObject InstantCard(InventoryCharacter data, GameObject container)
    {
        var instant = Instantiate(cardPrefab);
        var button = instant.GetComponent<Button>();
        var card = instant.GetComponent<CharacterCard>();
        button.onClick.RemoveAllListeners();
        card.SetUpCard(data);
        instant.transform.SetParent(container.transform);
        instant.SetActive(true);
        return instant;

    }

    private List<InventoryCharacter> SortData(List<InventoryCharacter> data)
    {
        switch (dataSortType)
        {
            case DataSortType.Level:
                data.Sort((x, y) => x.Level.CompareTo(y.Level));
                break;
            case DataSortType.Star:
                data.Sort((x, y) => x.Star.CompareTo(y.Star));
                break;
            case DataSortType.ID:
                data.Sort((x, y) => x.CharacterID.CompareTo(y.CharacterID));
                break;
        }
        return data;
    }
    void Update()
    {

    }

    private enum DataSortType
    {
        Level,
        Star,
        ID
    }
}
