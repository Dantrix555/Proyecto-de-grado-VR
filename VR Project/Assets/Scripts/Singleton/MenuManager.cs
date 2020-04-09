using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : CHEMSingleton<MenuManager>
{

    protected MenuManager(){}

    [SerializeField] private CanvasManager _fadeCanvas;
    public static CanvasManager FadeCanvas { get => Instance._fadeCanvas; }

}
