using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverController : MonoBehaviour
{
    [SerializeField] private GameObject _gameOverTitle;
    [SerializeField] private GameObject _restartGameButton;
    [SerializeField] private GameObject _goBackMenuButton;
    [SerializeField] private GameObject _exitGameButton;

    public void SetGameOver()
    {
        _gameOverTitle.SetActive(true);
        _gameOverTitle.GetComponent<TitleController>().SetFade("In");
        StartCoroutine(InGameManager.SetFade(InGameManager.FadeOperation.In, _restartGameButton));
        StartCoroutine(_restartGameButton.GetComponent<ButtonController>().SetInteractAtAble());
        StartCoroutine(InGameManager.SetFade(InGameManager.FadeOperation.In, _goBackMenuButton));
        StartCoroutine(_goBackMenuButton.GetComponent<ButtonController>().SetInteractAtAble());
        StartCoroutine(InGameManager.SetFade(InGameManager.FadeOperation.In, _exitGameButton));
        StartCoroutine(_exitGameButton.GetComponent<ButtonController>().SetInteractAtAble());
    }
}
