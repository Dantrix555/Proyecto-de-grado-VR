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
        Next,
        Resume,
        Confirm,
        ConfirmControls,
        ConfirmTutorial,
        GoMenu,
        ExitGame
    }

    [SerializeField] private Option _buttonOption;
    [SerializeField] private Animator _canvasAnimator;
    [SerializeField] private GameObject _buttonToShow;
    [SerializeField] private GameObject _titleToShow;
    [SerializeField] private GameObject _objectToShow;

    [Space(5)]
    [Header("InteractableObject")]
    [SerializeField]private bool _isInteractable;
    public bool IsInteractable { get => _isInteractable; set => _isInteractable = value; }

    public Animator CanvasAnimator { get => _canvasAnimator; }

    public void CheckOption()
    {
        IsInteractable = false;
        CanvasAnimator.SetBool("Fade", false);
        SoundManager.LoadSoundEffect(SoundManager.SFX.ButtonClic);

        switch (_buttonOption)
        {
            case Option.Start:
                InGameManager.CanUseGameControls = false;
                StartCoroutine(InGameManager.LoadLevel(InGameManager.Level.Level_01));
                break;

            case Option.Controls:
                StartCoroutine(InGameManager.SetFade(InGameManager.FadeOperation.Out, gameObject));
                _titleToShow.SetActive(true);
                _titleToShow.GetComponent<TitleController>().SetFade("In");
                StartCoroutine(_buttonToShow.GetComponent<ButtonController>().SetInteractAtAble());
                StartCoroutine(InGameManager.SetFade(InGameManager.FadeOperation.In, _buttonToShow));
                _buttonToShow.GetComponent<ButtonController>().CanvasAnimator.SetBool("Fade", true);
                StartCoroutine(InGameManager.SetFade(InGameManager.FadeOperation.In, _objectToShow));
                break;

            case Option.GameControls:
                InGameManager.GameUI.PauseController.SetPauseFade("Out");
                _titleToShow.SetActive(true);
                _titleToShow.GetComponent<TitleController>().SetFade("In");
                StartCoroutine(_buttonToShow.GetComponent<ButtonController>().SetInteractAtAble());
                StartCoroutine(InGameManager.SetFade(InGameManager.FadeOperation.In, _buttonToShow));
                _buttonToShow.GetComponent<ButtonController>().CanvasAnimator.SetBool("Fade", true);
                StartCoroutine(InGameManager.SetFade(InGameManager.FadeOperation.In, _objectToShow));
                break;

            case Option.Credits:
                StartCoroutine(InGameManager.SetFade(InGameManager.FadeOperation.Out, gameObject));
                _titleToShow.SetActive(true);
                _titleToShow.GetComponent<TitleController>().SetFade("In");
                StartCoroutine(_buttonToShow.GetComponent<ButtonController>().SetInteractAtAble());
                StartCoroutine(InGameManager.SetFade(InGameManager.FadeOperation.In, _buttonToShow));
                _buttonToShow.GetComponent<ButtonController>().CanvasAnimator.SetBool("Fade", true);
                StartCoroutine(InGameManager.SetFade(InGameManager.FadeOperation.In, _objectToShow));
                break;

            case Option.Next:
                InGameManager.GameUI.TutorialController.LoadTutorialNextPart();
                break;

            case Option.Resume:
                InGameManager.ResumeGame();
                break;

            case Option.Confirm:
                StartCoroutine(InGameManager.SetFade(InGameManager.FadeOperation.Out, gameObject));

                if(_titleToShow != null)
                {
                    _titleToShow.GetComponent<TitleController>().SetFade("Out");
                    _titleToShow.SetActive(false);
                }
                else
                {
                    InGameManager.ActivateDescription(false);
                    InGameManager.GameUI.FactsController.HideFact();
                }

                if (_buttonToShow != null)
                {
                    StartCoroutine(_buttonToShow.GetComponent<ButtonController>().SetInteractAtAble());
                    StartCoroutine(InGameManager.SetFade(InGameManager.FadeOperation.In, _buttonToShow));
                    _buttonToShow.GetComponent<ButtonController>().CanvasAnimator.SetBool("Fade", true);
                }

                if (_objectToShow != null)
                {
                    StartCoroutine(_objectToShow.GetComponent<ImageController>().DeactivateImage());
                    StartCoroutine(InGameManager.SetFade(InGameManager.FadeOperation.Out, _objectToShow));
                }

                break;

            case Option.ConfirmControls:
                StartCoroutine(InGameManager.SetFade(InGameManager.FadeOperation.Out, gameObject));
                _titleToShow.GetComponent<TitleController>().SetFade("Out");
                _titleToShow.SetActive(false);
                StartCoroutine(_objectToShow.GetComponent<ImageController>().DeactivateImage());
                StartCoroutine(InGameManager.SetFade(InGameManager.FadeOperation.Out, _objectToShow));
                InGameManager.GameUI.PauseController.SetPauseFade("In");
                break;

            case Option.ConfirmTutorial:
                InGameManager.EndTutorial();
                break;

            case Option.GoMenu:
                InGameManager.CanUseGameControls = false;
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
        yield return new WaitForSeconds(1.1f);
        _isInteractable = true;
    }

}
