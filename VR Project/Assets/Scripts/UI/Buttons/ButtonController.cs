using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonController : MonoBehaviour
{
    private enum Option
    {
        Start,
        Controls,
        GameControls,
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
        switch(_buttonOption)
        {
            case Option.Start:
                StartCoroutine(InGameManager.LoadLevel(InGameManager.Level.Level_01));
                break;

            case Option.Controls:
                GetComponent<ButtonController>().IsInteractable = false;
                GetComponent<ButtonController>().CanvasAnimator.SetBool("Fade", false);
                StartCoroutine(InGameManager.SetFade(InGameManager.FadeOperation.Out, gameObject));
                _titleToShow.SetActive(true);
                _titleToShow.GetComponent<TitleController>().SetFade("In");
                StartCoroutine(InGameManager.SetFade(InGameManager.FadeOperation.In, _buttonToShow));
                StartCoroutine(_buttonToShow.GetComponent<ButtonController>().SetInteractAtAble());
                _buttonToShow.GetComponent<ButtonController>().CanvasAnimator.SetBool("Fade", true);
                StartCoroutine(InGameManager.SetFade(InGameManager.FadeOperation.In, _objectToShow));
                break;

            case Option.GameControls:
                InGameManager.GameUI.PauseController.SetPauseFade("Out");
                _titleToShow.SetActive(true);
                _titleToShow.GetComponent<TitleController>().SetFade("In");
                StartCoroutine(InGameManager.SetFade(InGameManager.FadeOperation.In, _buttonToShow));
                StartCoroutine(_buttonToShow.GetComponent<ButtonController>().SetInteractAtAble());
                _buttonToShow.GetComponent<ButtonController>().CanvasAnimator.SetBool("Fade", true);
                StartCoroutine(InGameManager.SetFade(InGameManager.FadeOperation.In, _objectToShow));
                break;

            case Option.Credits:
                GetComponent<ButtonController>().IsInteractable = false;
                GetComponent<ButtonController>().CanvasAnimator.SetBool("Fade", false);
                StartCoroutine(InGameManager.SetFade(InGameManager.FadeOperation.Out, gameObject));
                _titleToShow.SetActive(true);
                _titleToShow.GetComponent<TitleController>().SetFade("In");
                StartCoroutine(InGameManager.SetFade(InGameManager.FadeOperation.In, _buttonToShow));
                StartCoroutine(_buttonToShow.GetComponent<ButtonController>().SetInteractAtAble());
                _buttonToShow.GetComponent<ButtonController>().CanvasAnimator.SetBool("Fade", true);
                StartCoroutine(InGameManager.SetFade(InGameManager.FadeOperation.In, _objectToShow));
                break;

            case Option.Resume:
                InGameManager.ResumeGame();
                break;

            case Option.Confirm:
                IsInteractable = false;
                CanvasAnimator.SetBool("Fade", false);
                StartCoroutine(InGameManager.SetFade(InGameManager.FadeOperation.Out, gameObject));
                _titleToShow.GetComponent<TitleController>().SetFade("Out");
                _titleToShow.SetActive(false);

                if (_buttonToShow != null)
                {
                    StartCoroutine(InGameManager.SetFade(InGameManager.FadeOperation.In, _buttonToShow));
                    StartCoroutine(_buttonToShow.GetComponent<ButtonController>().SetInteractAtAble());
                    _buttonToShow.GetComponent<ButtonController>().CanvasAnimator.SetBool("Fade", true);
                }

                if (_objectToShow != null)
                {
                    StartCoroutine(_objectToShow.GetComponent<ImageController>().DeactivateImage());
                    StartCoroutine(InGameManager.SetFade(InGameManager.FadeOperation.Out, _objectToShow));
                }

                if (InGameManager.IsInDescription)
                {
                    InGameManager.GameUI.FactsController.AnimateFactText(false);
                    InGameManager.IsInDescription = false;
                    //Fade pause panel
                }
                break;

            case Option.ConfirmControls:
                IsInteractable = false;
                CanvasAnimator.SetBool("Fade", false);
                StartCoroutine(InGameManager.SetFade(InGameManager.FadeOperation.Out, gameObject));
                _titleToShow.GetComponent<TitleController>().SetFade("Out");
                _titleToShow.SetActive(false);
                StartCoroutine(_objectToShow.GetComponent<ImageController>().DeactivateImage());
                StartCoroutine(InGameManager.SetFade(InGameManager.FadeOperation.Out, _objectToShow));
                InGameManager.GameUI.PauseController.SetPauseFade("In");
                break;

            case Option.GoMenu:
                InGameManager.IsGameScene = false;
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

    public IEnumerator SetInteractAtAble()
    {
        yield return new WaitForSeconds(1);
        this._isInteractable = true;
    }

}
