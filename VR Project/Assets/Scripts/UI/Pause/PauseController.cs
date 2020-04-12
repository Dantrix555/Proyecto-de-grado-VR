using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseController : MonoBehaviour
{
    [SerializeField] private GameObject _pauseTitle;
    [SerializeField] private GameObject[] _pauseMenuOptions;

    private bool _isInPauseMenu = false;

    public bool IsInPauseMenu { get => _isInPauseMenu; set => _isInPauseMenu = value; }

    public void SetPauseFade(string fadeOption)
    {
        if(fadeOption == "In")
        {
            IsInPauseMenu = true;
            _pauseTitle.SetActive(true);
            _pauseTitle.GetComponent<TitleController>().SetFade("In");
        }
        else
        {
            IsInPauseMenu = false;
            _pauseTitle.GetComponent<TitleController>().SetFade("Out");
            _pauseTitle.SetActive(false);
        }

        foreach(GameObject pauseMenuButtons in _pauseMenuOptions)
        {
            if (fadeOption == "In")
            {
                pauseMenuButtons.GetComponent<ButtonController>().CanvasAnimator.SetBool("Fade", true);
                StartCoroutine(InGameManager.SetFade(InGameManager.FadeOperation.In, pauseMenuButtons));
                StartCoroutine(pauseMenuButtons.GetComponent<ButtonController>().SetInteractAtAble());
            }
            else
            {
                pauseMenuButtons.GetComponent<ButtonController>().IsInteractable = false;
                pauseMenuButtons.GetComponent<ButtonController>().CanvasAnimator.SetBool("Fade", false);
                StartCoroutine(InGameManager.SetFade(InGameManager.FadeOperation.Out, pauseMenuButtons));
            }
        }
    }
}