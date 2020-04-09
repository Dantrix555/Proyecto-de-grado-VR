using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanonReaderCanvas : MonoBehaviour
{
    [SerializeField] private Text _readerText = default;
    [SerializeField] private Animator _animator = default;

    public void SetReaderText(string readerText)
    {
        _animator.SetTrigger("Fade");
        _readerText.text = readerText;
    }
}
