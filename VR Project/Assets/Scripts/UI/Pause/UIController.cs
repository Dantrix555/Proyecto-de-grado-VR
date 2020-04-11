using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField] private PauseController _pauseController = default;
    [SerializeField] private ControlPanel _controlPanel = default;
    [SerializeField] private FactsController _factsController = default;

    public PauseController PauseController { get => _pauseController; }
    public ControlPanel ControlPanel { get => _controlPanel; }
    public FactsController FactsController { get => _factsController; }
}
