using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class MapSelectUi : MonoBehaviour, IBeginDragHandler, IEndDragHandler
{
    [SerializeField] private ScrollRect scrollRect;
    [SerializeField] private RectTransform contentPanel;
    [SerializeField] private RectTransform firstItemPanel;
    [SerializeField] private HorizontalLayoutGroup HLG;
    [SerializeField] private List<StageData> stageDatas;
    private bool isDrag = false;
    int index;
    int oldIndex = 0;
    private void Start()
    {
        SetIndexAndPosition();
    }

    private void SetIndexAndPosition()
    {
        var data = PlayerData.Instant.StageSelected;
        var i = stageDatas.FindIndex((r) => r.ID == data.ID);
        Debug.Log(i);
        if (i != -1)
        {
            oldIndex = i;
            index = i;
            contentPanel.localPosition = new Vector3(0 - (index * (firstItemPanel.rect.width + HLG.spacing)), contentPanel.localPosition.y, contentPanel.localPosition.z);
        }
    }
    public StageData GetStageData()
    {
        return stageDatas[index];
    }

    void Update()
    {
        index = Mathf.Clamp(Mathf.RoundToInt(0 - contentPanel.localPosition.x / (firstItemPanel.rect.width + HLG.spacing)), 0, stageDatas.Count - 1);
        if (oldIndex != index)
        {
            oldIndex = index;
            SoundManager.Instant.PlayAudioOneShot(SoundType.MapScorll, transform);
        }
        SnapScroll();

    }

    private void SnapScroll()
    {
        if (!isDrag)
        {
            scrollRect.velocity = Vector2.zero;
            contentPanel.localPosition = new Vector3(Mathf.Lerp(contentPanel.localPosition.x, 0 - (index * (firstItemPanel.rect.width + HLG.spacing)), 0.1f), contentPanel.localPosition.y, contentPanel.localPosition.z);
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        isDrag = true;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        isDrag = false;
    }
}
