using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadePanel : MonoBehaviour
{
    [SerializeField] private Animator _fadePanelAnimator = default;

    public void SetFadeAnimation()
    {
        _fadePanelAnimator.SetTrigger("FadeTeleport");
    }
}
