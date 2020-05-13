using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialController : MonoBehaviour
{
    private int _part = 0;
    [SerializeField] private TutorialPart[] _tutorialPart = default;

    public TutorialPart[] TutorialPart { get => _tutorialPart; set => _tutorialPart = value; }

    public void StartTutorial()
    {
        TutorialPart[_part].SetTutorial(true);
    }

    public void LoadTutorialNextPart()
    {
        //Close the previous part of the tutorial
        TutorialPart[_part].SetTutorial(false);
        _part++;
        //Load the next part of the tutorial
        TutorialPart[_part].SetTutorial(true);
    }

    public void EndTutorial()
    {
        TutorialPart[_part].SetTutorial(false);
    }
}
