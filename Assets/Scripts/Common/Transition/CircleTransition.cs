using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CircleTransition : SceneTransition
{
    [SerializeField] private Image circle;
    public override IEnumerator AnimationTransitionIn()
    {
        circle.rectTransform.anchoredPosition = new Vector2(-1650f, 0f);
        var tween = circle.rectTransform.DOAnchorPosX(0f, 1f);
        yield return tween.WaitForCompletion();
    }

    public override IEnumerator AnimationTransitionOut()
    {
        var tween = circle.rectTransform.DOAnchorPosX(1650f, 1f);
        yield return tween.WaitForCompletion();
    }
}
