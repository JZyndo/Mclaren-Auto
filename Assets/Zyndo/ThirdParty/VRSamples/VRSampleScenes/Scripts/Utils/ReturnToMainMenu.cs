using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

namespace VRStandardAssets.Utils
{
    // This class simply allows the user to return to the main menu.
    public class ReturnToMainMenu : MonoBehaviour
    {
		[SerializeField] private string m_MenuSceneName = "ExampleMenu";   // The name of the main menu scene.
        [SerializeField] private VRInput m_VRInput;                     // Reference to the VRInput in order to know when Cancel is pressed.
        [SerializeField] private VRCameraFade m_VRCameraFade;           // Reference to the script that fades the scene to black.

        [SerializeField] bool inMenu = false;

        private void OnEnable ()
        {
            m_VRInput.OnCancel += HandleCancel;
        }


        private void OnDisable ()
        {
            m_VRInput.OnCancel -= HandleCancel;
        }


        private void HandleCancel ()
        {
            if(!inMenu)
                StartCoroutine (FadeToMenu ());
            else
                StartCoroutine(FadeBackToScene());
        }


        private IEnumerator FadeToMenu()
        {
            if (m_MenuSceneName == "ExampleMenu")
            {
                Debug.LogError("You are using the default ExampleMenu scene. You are highly encouraged to copy the scene to create a project specific version.");
            }

            PlayerPrefs.SetInt("LastVisitedPage", PageEventsManager.currentPage.gameObject.transform.GetSiblingIndex());
            PlayerPrefs.SetString("LastSceneLoaded", SceneManager.GetActiveScene().name);

            // Wait for the screen to fade out.
            yield return StartCoroutine(m_VRCameraFade.BeginFadeOut(true));

            // Load the main menu by itself.
            SceneManager.LoadScene(m_MenuSceneName, LoadSceneMode.Single);
        }

        private IEnumerator FadeBackToScene()
        {
            // Wait for the screen to fade out.
            yield return StartCoroutine(m_VRCameraFade.BeginFadeOut(true));

            // Load the main menu by itself.
            SceneManager.LoadScene(PlayerPrefs.GetString("LastSceneLoaded"), LoadSceneMode.Single);
        }
    }
}