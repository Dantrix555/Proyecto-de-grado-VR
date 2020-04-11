using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InGameManager : CHEMSingleton<InGameManager>
{

    protected InGameManager() { }

    public enum FadeOperation
    {
        In,
        Out
    }

    //The enum reference of the level has the same name of the level scene
    public enum Level
    {
        MainMenu,
        Level_01
    }

    [Header("Game Managers")]
    [SerializeField] private CanvasManager _mainCanvas;
    [SerializeField] private SpawnerController[] _spawnerControllers;

    [Header("Parent Objects")]
    [SerializeField] private UIController _gameUi;

    public static CanvasManager MainCanvas { get => Instance._mainCanvas; }
    public static SpawnerController[] SpawnerControllers { get => Instance._spawnerControllers; }
    public static UIController GameUI { get => Instance._gameUi; }

    private bool _isInGameScene = default;
    private bool _isGamePaused = false;

    public static bool IsGameScene { get => Instance._isInGameScene; set => Instance._isInGameScene = value; }
    public static bool IsGamePaused { get => Instance._isGamePaused; set => Instance._isGamePaused = value; }

    void Start()
    {
        //Set the components as a child of their respective spawner
        for (int i = 0; i < _spawnerControllers.Length; i++)
        {
            _spawnerControllers[i].SpawnComponent();
        }
    }

    //Operation is a variable that takes 2 values in or out, to determine the fade effect
    public static IEnumerator SetFade(FadeOperation operation, GameObject objectToFade)
    {
        float i;
        Color actualMaterialColor = objectToFade.GetComponent<MeshRenderer>().material.color;

        if (operation == FadeOperation.In)
        {
            objectToFade.SetActive(true);
            for (i = 0; i <= 1; i += 0.05f)
            {
                actualMaterialColor.a = i;
                objectToFade.GetComponent<MeshRenderer>().material.color = actualMaterialColor;
                yield return new WaitForSeconds(0.05f);
            }
        }
        else if (operation == FadeOperation.Out)
        {
            for (i = 1; i >= 0; i -= 0.05f)
            {
                actualMaterialColor.a = i;
                objectToFade.GetComponent<MeshRenderer>().material.color = actualMaterialColor;
                yield return new WaitForSeconds(0.05f);
            }
            objectToFade.SetActive(false);
        }
    }

    public static IEnumerator LoadLevel(Level levelToLoad)
    {
        MainCanvas.FadePanelWindow.SetFadeAnimation();
        yield return new WaitForSeconds(0.4f);
        SceneManager.LoadScene(levelToLoad.ToString());
    }

    public static IEnumerator TeleportPlayer(Vector3 teleportPosition, GameObject player)
    {
        IsGamePaused = true;
        yield return new WaitForSeconds(0.5f);
        player.transform.position = teleportPosition;
        yield return new WaitForSeconds(0.5f);
        IsGamePaused = false;
    }

    public static void SetPause()
    {
        MainCanvas.FadePanelWindow.SetPauseFade(true);
        GameUI.PauseController.SetPauseFade("In");
        IsGamePaused = true;
    }

    public static void ResumeGame()
    {
        MainCanvas.FadePanelWindow.SetPauseFade(false);
        if (GameUI.PauseController.IsInPauseMenu)
            GameUI.PauseController.SetPauseFade("Out");
        else
            GameUI.ControlPanel.FadeOutControlPanel();
        IsGamePaused = false;
    }
}
