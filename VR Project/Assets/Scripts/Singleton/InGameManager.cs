using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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
    [SerializeField] private GameObject[] _scenarioParentObjects;

    public static CanvasManager MainCanvas { get => Instance._mainCanvas; }
    public static SpawnerController[] SpawnerControllers { get => Instance._spawnerControllers; }
    public static UIController GameUI { get => Instance._gameUi; }

    [SerializeField] private bool _canUseGameControls;
    private bool _isGamePaused = false;
    private bool _isInDescription = false;
    private bool _playerHasKey = false;
    private bool _playerIsDamaged = false;
    
    public static bool CanUseGameControls { get => Instance._canUseGameControls; set => Instance._canUseGameControls = value; }
    public static bool IsGamePaused { get => Instance._isGamePaused; set => Instance._isGamePaused = value; }
    public static bool IsInDescription { get => Instance._isInDescription; set => Instance._isInDescription = value; }
    public static bool PlayerHasKey { get => Instance._playerHasKey; set => Instance._playerHasKey = value; }
    public static bool PlayerIsDamaged { get => Instance._playerIsDamaged; set => Instance._playerIsDamaged = value; }

    private HashSet<int> _componentDex = new HashSet<int>();

    void Start()
    {
        if(_spawnerControllers.Length > 0)
        {
            for (int i = 0; i < _spawnerControllers.Length; i++)
            {
                _spawnerControllers[i].SpawnComponent();
            }
        }

        //Set the game tutorial if player is in the first level
        if (SceneManager.GetActiveScene().name == Level.Level_01.ToString())
            SetTutorial();
        //Set the menu music (because the project has only the menu scene and first level scene)
        else
            SoundManager.SetBackgroundMusic(SoundManager.Music.Menu);
    }

    //Operation is a variable that takes 2 values in or out, to determine the fade effect
    public static IEnumerator SetFade(FadeOperation operation, GameObject objectToFade)
    {
        float i;
        Color actualMaterialColor = objectToFade.GetComponent<MeshRenderer>().material.color;
        
        //Set full opacity to the object (visible)
        if (operation == FadeOperation.In)
        {
            objectToFade.SetActive(true);
            for (i = 0; i < 1; i += 0.05f)
            {
                actualMaterialColor.a = i;
                objectToFade.GetComponent<MeshRenderer>().material.color = actualMaterialColor;
                yield return new WaitForSeconds(0.05f);
            }
        }
        //Set no opacity to the object (invisible)
        else if (operation == FadeOperation.Out)
        {
            for (i = 1; i > 0; i -= 0.05f)
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
        yield return new WaitForSeconds(0.3f);
        SceneManager.LoadScene(levelToLoad.ToString());
    }

    public static IEnumerator TeleportPlayerToPortal(Vector3 teleportPosition, GameObject player)
    {
        SoundManager.SetBackgroundMusic(SoundManager.Music.Finished);
        CanUseGameControls = false;
        yield return new WaitForSeconds(0.5f);
        player.transform.position = teleportPosition;
        yield return new WaitForSeconds(0.25f);
        SetFinishedGame();
    }

    public static IEnumerator TeleportPlayer(Vector3 teleportPosition, Vector3 playerNewRotation, GameObject player)
    {
        SoundManager.LoadSoundEffect(SoundManager.SFX.Teletransportation);
        CanUseGameControls = false;
        yield return new WaitForSeconds(0.5f);
        player.transform.position = teleportPosition;
        player.transform.eulerAngles = playerNewRotation;
        yield return new WaitForSeconds(0.5f);
        CanUseGameControls = true;
    }

    public static void SetPause()
    {
        SetScenarioActive(false);
        MainCanvas.FadePanelWindow.SetPauseFade(true);
        GameUI.PauseController.SetPauseFade("In");
        IsGamePaused = true;
        SoundManager.SetBackgroundMusic(SoundManager.Music.Pause);
    }

    public static void ResumeGame()
    {
        MainCanvas.FadePanelWindow.SetPauseFade(false);
        if (GameUI.PauseController.IsInPauseMenu)
            GameUI.PauseController.SetPauseFade("Out");
        else
            GameUI.ControlPanel.FadeOutControlPanel();
        SetScenarioActive(true);
        IsGamePaused = false;
        SoundManager.SetBackgroundMusic(SoundManager.Music.InGame);
    }

    public static void ActivateDescription(bool activationState)
    {
        //Set that player is in desription and scenario disapear during description
        IsInDescription = activationState;
        GameUI.FactsController.AnimateFactText(activationState);
        SetScenarioActive(!activationState);
        MainCanvas.FadePanelWindow.SetPauseFade(activationState);
    }

    public static void SetGameOver()
    {
        IsGamePaused = true;
        CanUseGameControls = false;
        SetScenarioActive(false);
        GameUI.GameOverController.SetGameOver();
        MainCanvas.FadePanelWindow.SetGameOverFade();
        SoundManager.SetBackgroundMusic(SoundManager.Music.GameOver);
    }

    public static void SetFinishedGame()
    {
        IsGamePaused = true;
        SetScenarioActive(false);
        GameUI.FinishedGameController.SetGameOver();
        MainCanvas.FadePanelWindow.SetFinishedGameFade();
    }

    public static bool ComponentIsInDex(int componentId)
    {
        if(Instance._componentDex.Contains(componentId))
        {
            return true;
        }
        Instance._componentDex.Add(componentId);
        return false;
    }

    public static void SetScenarioActive(bool activationState)
    {
        int i;
        for(i = 0; i < Instance._scenarioParentObjects.Length; i++)
        {
            Instance._scenarioParentObjects[i].SetActive(activationState);
        }
    }

    private void SetTutorial()
    {
        //Deactivate scenario and set tutorial description
        _canUseGameControls = false;
        _isInDescription = true;
        SetScenarioActive(false);
        _gameUi.TutorialController.StartTutorial();
        MainCanvas.FadePanelWindow.SetPauseFade(true);
        SoundManager.SetBackgroundMusic(SoundManager.Music.Pause);
    }

    public static void EndTutorial()
    {
        GameUI.TutorialController.EndTutorial();
        CanUseGameControls = true;
        IsInDescription = false;
        SetScenarioActive(true);
        MainCanvas.FadePanelWindow.SetPauseFade(false);
        SoundManager.SetBackgroundMusic(SoundManager.Music.InGame);
    }
}
