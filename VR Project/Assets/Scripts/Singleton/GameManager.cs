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
    [Space(5)]
    [SerializeField] private SpawnerController[] _spawnerControllers; 

    public static CanvasManager MainCanvas { get => Instance._mainCanvas; }
    public static SpawnerController[] SpawnerControllers { get => Instance._spawnerControllers; }

    void Start()
    {
        //Set the components as a child of their respective spawner
        for(int i = 0; i < _spawnerControllers.Length; i++)
        {
            _spawnerControllers[i].SpawnComponent();
        }
    }

}
