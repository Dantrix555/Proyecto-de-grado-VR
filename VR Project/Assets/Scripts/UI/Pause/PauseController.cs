using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseController : MonoBehaviour
{
    [SerializeField] private GameObject[] _pauseMenuOptions;

    private bool _isInPauseMenu = false;

    public bool IsInPauseMenu { get => _isInPauseMenu; set => _isInPauseMenu = value; }

    public void SetPauseFade(string fadeOption)
    {
        foreach(GameObject pauseMenuButtons in _pauseMenuOptions)
        {
            if (fadeOption == "In")
            {
                StartCoroutine(InGameManager.SetFade(InGameManager.FadeOperation.In, pauseMenuButtons));
                IsInPauseMenu = true;
            }
            else
            {
                StartCoroutine(InGameManager.SetFade(InGameManager.FadeOperation.Out, pauseMenuButtons));
                IsInPauseMenu = false;
            }
        }
    }
}