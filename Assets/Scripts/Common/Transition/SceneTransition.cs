using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SceneTransition : MonoBehaviour
{
    public TransitionType Type;
    public abstract IEnumerator AnimationTransitionIn();
    public abstract IEnumerator AnimationTransitionOut();
}
