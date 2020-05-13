using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialPart : MonoBehaviour
{
    [SerializeField] private Animator _tutorialTextAnimator = default;
    [SerializeField] private GameObject[] _tutorialElements = default;
    [SerializeField] private GameObject _acceptButton = default;

    public void SetTutorial(bool tutorialState)
    {

        _tutorialTextAnimator.SetBool("Fade", tutorialState);
        
        foreach(GameObject tutorialElement in _tutorialElements)
        {
            if(tutorialState)
            {
                StartCoroutine(InGameManager.SetFade(InGameManager.FadeOperation.In, tutorialElement));
            }
            else
            {
                StartCoroutine(InGameManager.SetFade(InGameManager.FadeOperation.Out, tutorialElement));
            }
        }
        if(tutorialState)
        {
            StartCoroutine(InGameManager.SetFade(InGameManager.FadeOperation.In, _acceptButton));
            StartCoroutine(_acceptButton.GetComponent<ButtonController>().SetInteractAtAble());
        }
        else
        {
            StartCoroutine(InGameManager.SetFade(InGameManager.FadeOperation.Out, _acceptButton));
        }
    }
}
