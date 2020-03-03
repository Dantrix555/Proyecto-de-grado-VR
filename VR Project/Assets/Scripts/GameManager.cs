using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : CHEMSingleton<GameManager>
{

    protected GameManager()
    {

    }

    [Header("Game Managers")]
    [SerializeField] private CanvasManager _mainCanvas;

    public static CanvasManager MainCanvas { get => Instance._mainCanvas; }

}
