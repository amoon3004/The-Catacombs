using UnityEngine;
using UnityEngine.SceneManagement;

// Title screen with play, instructions, back, and exit buttons
public class MainMenu : MonoBehaviour
{
    public GameObject menuButtons;
    public GameObject instructionsPanel;
    public string gameSceneName = "Gameplay";

    void Start()
    {
        ShowMenu();
    }

    // Play button loads level 1
    public void PlayGame()
    {
        SceneManager.LoadScene(gameSceneName);
    }

    // Instructions button hides menu and shows the scroll
    public void ShowInstructions()
    {
        menuButtons.SetActive(false);
        instructionsPanel.SetActive(true);
    }

    // back button hides the scroll and shows menu again
    public void ShowMenu()
    {
        instructionsPanel.SetActive(false);
        menuButtons.SetActive(true);
    }

    // Exit button closes the game
    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // stops play mode in the editor
#else
        Application.Quit();// closes the real built game
#endif
    }
}
