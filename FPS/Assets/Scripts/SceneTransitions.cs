using Fps.Controller;
using Fps.UI;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Fps.Manager
{
    public class SceneTransitions : MonoBehaviour
    {
        [field: SerializeField]
        private Animator TransitionAnimator { get; set; }

        [field: SerializeField]
        private GameObject MainMenuPanel { get; set; }

        [field: SerializeField]
        private GameObject StartMenuPanel { get; set; }

        [field: SerializeField]
        private GameObject ResultsMenuPanel { get; set; }

        [field: SerializeField]
        private InputField UsernameInput { get; set; }

        [field: SerializeField]
        private GameObject UsernameNotNullMessage { get; set; }

        [field: SerializeField]
        private float SceneTransitionTime { get; set; } = 1.5f;

        [field: SerializeField]
        private string SceneName { get; set; }

        private IEnumerator LoadScene()
        {
            TransitionAnimator.SetTrigger("End");
            yield return new WaitForSeconds(SceneTransitionTime);
            SceneManager.LoadScene(SceneName);
        }

        public void StartClick()
        {
            MainMenuPanel.SetActive(false);
            StartMenuPanel.SetActive(true);
        }

        public void RegisterNameAndBeginNewGame()
        {
            if (string.IsNullOrWhiteSpace(UsernameInput.text))
            {
                UsernameNotNullMessage.SetActive(true);
                return;
            }

            UsernameNotNullMessage.SetActive(false);
            WinnersTable.RegisterCurrentPlayer(0, UsernameInput.text);
            StartCoroutine(LoadScene());
        }

        public void PlayerDeathAndScreenChange(int pontuation)
        {
            WinnersTable.UpdateCurrentPlayerPontuation(pontuation);
            StartCoroutine(SetupTransition());
        }

        private IEnumerator SetupTransition()
        {
            yield return StartCoroutine(LoadScene());
            yield return StartCoroutine(SetupCursor());
        }

        private IEnumerator SetupCursor()
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
            MainMenuPanel.SetActive(false);
            ResultsMenuPanel.SetActive(true);
        }

        public void BackClick()
        {
            ResultsMenuPanel.SetActive(false);
            MainMenuPanel.SetActive(true);
        }

        public void QuitClick()
        {
            Application.Quit();
        }
    }
}