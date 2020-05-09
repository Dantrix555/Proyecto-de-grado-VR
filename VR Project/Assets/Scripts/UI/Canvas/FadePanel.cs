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

    public void SetPauseFade(bool fadeState)
    {
        _fadePanelAnimator.SetBool("FadePause", fadeState);
    }

    public void SetFinishedGameFade()
    {
        _fadePanelAnimator.SetTrigger("Finished");
    }

    public void SetGameOverFade()
    {
        _fadePanelAnimator.SetTrigger("GameOver");
    }

    public void SetDamageFade()
    {
        _fadePanelAnimator.SetTrigger("Damaged");
    }

    public void SetGotKeyAnimation()
    {
        _fadePanelAnimator.SetTrigger("Key");
    }

    public void SetGotComponentAnimation()
    {
        _fadePanelAnimator.SetTrigger("Component");
    }
}
