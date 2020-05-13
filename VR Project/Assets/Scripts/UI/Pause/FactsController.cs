using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FactsController : MonoBehaviour
{
    [SerializeField] private GameObject _factTitle;
    [SerializeField] private GameObject _factBackground = default;
    [SerializeField] private GameObject _useFactBackground = default;
    [SerializeField] private GameObject _useImageBackground = default;
    [SerializeField] private GameObject _acceptButton = default;
    [SerializeField] private Animator _useFactAnimator = default;
    [SerializeField] private Animator _factAnimator = default;
    [SerializeField] private Text _factText = default;
    [SerializeField] private Text _useFactText = default;
    
    public void SetFact(GetAtAbleComponent componentData)
    {
        _factText.text = componentData.Description;
        _useFactText.text = componentData.UseDescription;
        _useImageBackground.GetComponent<MeshRenderer>().material = componentData.ImageDescription;
    }

    public void ShowFact()
    {
        _factTitle.SetActive(true);
        _factTitle.GetComponent<TitleController>().SetFade("In");
        StartCoroutine(InGameManager.SetFade(InGameManager.FadeOperation.In, _factBackground));
        StartCoroutine(InGameManager.SetFade(InGameManager.FadeOperation.In, _useFactBackground));
        StartCoroutine(InGameManager.SetFade(InGameManager.FadeOperation.In, _useImageBackground));
        StartCoroutine(InGameManager.SetFade(InGameManager.FadeOperation.In, _acceptButton));
        StartCoroutine(_acceptButton.GetComponent<ButtonController>().SetInteractAtAble());
    }

    public void HideFact()
    {
        _factTitle.SetActive(false);
        _factTitle.GetComponent<TitleController>().SetFade("Out");
        StartCoroutine(InGameManager.SetFade(InGameManager.FadeOperation.Out, _factBackground));
        StartCoroutine(InGameManager.SetFade(InGameManager.FadeOperation.Out, _useFactBackground));
        StartCoroutine(InGameManager.SetFade(InGameManager.FadeOperation.Out, _useImageBackground));
        StartCoroutine(InGameManager.SetFade(InGameManager.FadeOperation.Out, _acceptButton));
    }

    public void AnimateFactText(bool animationState)
    {
        _useFactAnimator.SetBool("Fade", animationState);
        _factAnimator.SetBool("Fade", animationState);
    }
}
