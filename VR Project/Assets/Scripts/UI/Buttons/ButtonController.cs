using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonController : MonoBehaviour
{
    private enum Option
    {
        Start,
        Controls,
        Credits,
        Resume,
        Confirm,
        ConfirmControls,
        GoMenu,
        ExitGame
    }

    [SerializeField] private Option _buttonOption;
    [SerializeField] private Animator _canvasAnimator;
    [SerializeField] private GameObject _buttonToShow;
    [SerializeField] private GameObject _titleToShow;
    [SerializeField] private GameObject _objectToShow;

    private bool _isInteractable = true;
    public bool IsInteractable { get => _isInteractable; set => _isInteractable = value; }

    public Animator CanvasAnimator { get => _canvasAnimator; }

    public void CheckOption()
    {
        _canvasAnimator.SetBool("Fade", false);
        _isInteractable = false;
        switch(_buttonOption)
        {
            case Option.Start:
                StartCoroutine(InGameManager.LoadLevel(InGameManager.Level.Level_01));
                break;

            case Option.Controls:
                StartCoroutine(InGameManager.SetFade(InGameManager.FadeOperation.Out, gameObject));
                _titleToShow.SetActive(true);
                _titleToShow.GetComponent<TitleController>().SetFade("In");
                StartCoroutine(InGameManager.SetFade(InGameManager.FadeOperation.In, _buttonToShow));
                _buttonToShow.GetComponent<ButtonController>().CanvasAnimator.SetBool("Fade", true);
                StartCoroutine(InGameManager.SetFade(InGameManager.FadeOperation.In, _objectToShow));
                _buttonToShow.GetComponent<ButtonController>().IsInteractable = true;
                break;

            case Option.Credits:
                StartCoroutine(InGameManager.SetFade(InGameManager.FadeOperation.Out, gameObject));
                _titleToShow.SetActive(true);
                _titleToShow.GetComponent<TitleController>().SetFade("In");
                StartCoroutine(InGameManager.SetFade(InGameManager.FadeOperation.In, _buttonToShow));
                _buttonToShow.GetComponent<ButtonController>().CanvasAnimator.SetBool("Fade", true);
                StartCoroutine(InGameManager.SetFade(InGameManager.FadeOperation.In, _objectToShow));
                _buttonToShow.GetComponent<ButtonController>().IsInteractable = true;
                break;

            case Option.Resume:
                InGameManager.ResumeGame();
                break;

            case Option.Confirm:
                StartCoroutine(InGameManager.SetFade(InGameManager.FadeOperation.Out, gameObject));
                _titleToShow.GetComponent<TitleController>().SetFade("Out");
                _titleToShow.SetActive(false);
                if (_buttonToShow != null)
                {
                    StartCoroutine(InGameManager.SetFade(InGameManager.FadeOperation.In, _buttonToShow));
                    _buttonToShow.GetComponent<ButtonController>().CanvasAnimator.SetBool("Fade", true);
                    _buttonToShow.GetComponent<ButtonController>().IsInteractable = true;
                }
                if(_objectToShow != null)
                {
                    StartCoroutine(InGameManager.SetFade(InGameManager.FadeOperation.Out, _objectToShow));
                }
                break;

            case Option.ConfirmControls:
                StartCoroutine(InGameManager.SetFade(InGameManager.FadeOperation.Out, gameObject));
                _titleToShow.GetComponent<TitleController>().SetFade("Out");
                _titleToShow.SetActive(false);
                StartCoroutine(InGameManager.SetFade(InGameManager.FadeOperation.Out, _objectToShow));
                InGameManager.GameUI.PauseController.SetPauseFade("In");
                break;

            case Option.GoMenu:
                StartCoroutine(InGameManager.LoadLevel(InGameManager.Level.MainMenu));
                break;

            case Option.ExitGame:
                Application.Quit();
                break;
        }
    }

    public string GetOptionName()
    {
        return _buttonOption.ToString();
    }

}
