using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonController : MonoBehaviour
{
    private enum Option
    {
        Start,
        Controls,
        Credits,
        Resume,
        Confirm,
        GoMenu,
        ExitGame
    }

    [SerializeField] private Option _buttonOption;
    [SerializeField] private Animator _buttonAnimator;

    public void CheckOption()
    {
        switch(_buttonOption)
        {
            case Option.Start:
                InGameManager.MainCanvas.FadePanelWindow.SetFadeAnimation();
                Invoke("LoadLevel", 0.3f);
                break;
            case Option.Controls:
                break;
            case Option.Credits:
                break;
            case Option.Resume:
                //Set resume 
                break;
            case Option.Confirm:
                //Close canvas (set active false)
                break;
            case Option.GoMenu:
                SceneManager.LoadScene("MainMenu");
                break;
            case Option.ExitGame:
                Application.Quit();
                break;
        }
    }

    public string GetOptionName()
    {
        return _buttonOption.ToString();
    }

    private void LoadLevel()
    {
        SceneManager.LoadScene("Level_01");
    }
}
