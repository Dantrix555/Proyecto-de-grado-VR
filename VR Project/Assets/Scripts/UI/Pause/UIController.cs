using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField] private TutorialController _tutorialController = default;
    [SerializeField] private PauseController _pauseController = default;
    [SerializeField] private ControlPanel _controlPanel = default;
    [SerializeField] private FactsController _factsController = default;
    [SerializeField] private GameOverController _gameOverController = default;
    [SerializeField] private GameOverController _finishedGameController = default;


    public TutorialController TutorialController { get => _tutorialController; }
    public PauseController PauseController { get => _pauseController; }
    public ControlPanel ControlPanel { get => _controlPanel; }
    public FactsController FactsController { get => _factsController; }
    public GameOverController GameOverController { get => _gameOverController; }
    public GameOverController FinishedGameController { get => _finishedGameController; }
}
