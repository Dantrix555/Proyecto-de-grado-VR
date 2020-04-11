﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlPanel : MonoBehaviour
{
    [SerializeField] private GameObject _titleControl;
    [SerializeField] private GameObject _imageControl;
    [SerializeField] private GameObject _acceptButton;
    
    public void FadeOutControlPanel()
    {
        _titleControl.GetComponent<TitleController>().SetFade("Out");
        _titleControl.SetActive(false);
        StartCoroutine(InGameManager.SetFade(InGameManager.FadeOperation.Out, _imageControl));
        StartCoroutine(InGameManager.SetFade(InGameManager.FadeOperation.Out, _acceptButton));
    }
}
