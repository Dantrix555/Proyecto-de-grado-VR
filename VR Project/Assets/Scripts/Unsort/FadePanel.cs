using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadePanel : MonoBehaviour
{
    [SerializeField] private Animator _panelAnimator = default;

    public void SetFadeAnimation()
    {
        _panelAnimator.SetTrigger("FadeTeleport");
    }
}
