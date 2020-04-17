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

    public Text debugText;
    public static Text DebugText { get => Instance.debugText; }
    //public GameObject button;
    //private int x;

    [SerializeField] private bool _canUseGameControls;
    private bool _isGamePaused = false;
    private bool _isInDescription = false;

    public GameObject _arrow;
    public GameObject _player;

    public static bool CanUseGameControls { get => Instance._canUseGameControls; set => Instance._canUseGameControls = value; }
    public static bool IsGamePaused { get => Instance._isGamePaused; set => Instance._isGamePaused = value; }
    public static bool IsInDescription { get => Instance._isInDescription; set => Instance._isInDescription = value; }

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
        //Set the components as a child of their respective spawner
    }

    private void Update()
    {
        DebugText.text = "A: " + _arrow.transform.eulerAngles.y.ToString() + " - P: " + _player.transform.eulerAngles.y.ToString();
    }

    //Operation is a variable that takes 2 values in or out, to determine the fade effect
    public static IEnumerator SetFade(FadeOperation operation, GameObject objectToFade)
    {
        float i;
        Color actualMaterialColor = objectToFade.GetComponent<MeshRenderer>().material.color;
        
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

    public static IEnumerator TeleportPlayer(Vector3 teleportPosition, Vector3 playerNewRotation, GameObject player)
    {
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
    }

    public static void ActivateDescription(bool activationState)
    {
        IsInDescription = activationState;
        GameUI.FactsController.AnimateFactText(activationState);
        SetScenarioActive(!activationState);
        MainCanvas.FadePanelWindow.SetPauseFade(activationState);
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
}
