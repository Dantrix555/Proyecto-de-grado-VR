using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasManager : MonoBehaviour
{
    [Header("Canvas Panels")]
    [SerializeField] private FadePanel _fadePanelWindow = default;
    
    public FadePanel FadePanelWindow { get => _fadePanelWindow; }
}
