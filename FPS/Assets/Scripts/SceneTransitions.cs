using Fps.Controller;
using Fps.UI;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneTransitions : MonoBehaviour {

    public Animator transitionAnimator;
    public GameObject mainMenuPanel, startMenuPanel, resultsMenuPanel;
    public InputField usernameInput;
    public GameObject usernameNotNullMessage;
    public float sceneTransitionTime = 1.5f;
    public string sceneName;

    IEnumerator LoadScene()
    {
        transitionAnimator.SetTrigger("End");
        yield return new WaitForSeconds(sceneTransitionTime);
        SceneManager.LoadScene(sceneName);
    }

    public void StartClick()
    {
        mainMenuPanel.SetActive(false);
        startMenuPanel.SetActive(true);
    }

    public void RegisterNameAndBeginNewGame()
    {
        if(string.IsNullOrEmpty(usernameInput.text))
        {
            usernameNotNullMessage.SetActive(true);
        } else
        {
            usernameNotNullMessage.SetActive(false);
            WinnersTable.RegisterCurrentPlayer(0, usernameInput.text);
            StartCoroutine(LoadScene());
        }
        
    }

    public void PlayerDeathAndScreenChange(int pontuation)
    {
        WinnersTable.UpdateCurrentPlayerPontuation(pontuation);
        StartCoroutine(SetupTransition());
    }

    IEnumerator SetupTransition()
    {
        yield return StartCoroutine(LoadScene());
        yield return StartCoroutine(SetupCursor());
    }

    IEnumerator SetupCursor()
    {
        GameController.ShowCursor(true);
        yield return null;
    }

    public void StartNewGameClick()
    {
        StartCoroutine(LoadScene());
    }

    public void ResultsClick()
    {
        mainMenuPanel.SetActive(false);
        resultsMenuPanel.SetActive(true);
    }

    public void BackClick()
    {
        resultsMenuPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }

    public void QuitClick()
    {
        Application.Quit();
    }
}
