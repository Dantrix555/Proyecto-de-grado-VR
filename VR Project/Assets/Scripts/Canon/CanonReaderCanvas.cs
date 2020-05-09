using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanonReaderCanvas : MonoBehaviour
{
    [SerializeField] private Text _readerText = default;

    public void SetReaderText(string readerText)
    {
        _readerText.text = readerText;
    }
}
