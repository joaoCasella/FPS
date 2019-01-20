using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneTransitions : MonoBehaviour {

    public Animator transitionAnimator;
    public GameObject mainMenuPanel, startMenuPanel, resultsMenuPanel;
    public InputField usernameInput; 
    public string sceneName;

    IEnumerator LoadScene()
    {
        transitionAnimator.SetTrigger("End");
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene(sceneName);
    }

    public void StartClick()
    {
        mainMenuPanel.SetActive(false);
        startMenuPanel.SetActive(true);
    }

    public void RegisterNameAndBeginNewGame()
    {
        WinnersTable.CurrentPlayer = new PlayerIdentifier(0, usernameInput.text);
        StartCoroutine(LoadScene());
    }

    public void PlayerDeathAndScreenChange(int pontuation)
    {
        WinnersTable.CurrentPlayer = new PlayerIdentifier(pontuation, WinnersTable.CurrentPlayer.name);
        StartCoroutine(SetupTransition());
    }

    IEnumerator SetupTransition()
    {
        yield return StartCoroutine(LoadScene());
        yield return StartCoroutine(SetupCursor());
    }

    IEnumerator SetupCursor()
    {
        GameController.showCursor(true);
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
