using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FactsController : MonoBehaviour
{
    [SerializeField] private GameObject _factTitle;
    [SerializeField] private GameObject _factBackground = default;
    [SerializeField] private Animator _factAnimator = default;
    [SerializeField] private GameObject _acceptButton = default;
    [SerializeField] private Text _factText = default;

    public void SetText(string factText)
    {
        _factText.text = factText;
    }

    public void ShowFact()
    {
        _factTitle.SetActive(true);
        _factTitle.GetComponent<TitleController>().SetFade("In");
        InGameManager.SetFade(InGameManager.FadeOperation.In, _factBackground);
        InGameManager.SetFade(InGameManager.FadeOperation.In, _acceptButton);
        _factAnimator.SetTrigger("Fade");
    }
}
