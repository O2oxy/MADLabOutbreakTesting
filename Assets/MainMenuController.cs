using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    [Header("Menu References")]
    public GameObject startMenu;
    public GameObject optionsMenu;

    private void Start()
    {
        // Ensure only the Start Menu is active at the beginning
        startMenu.SetActive(true);
        optionsMenu.SetActive(false);
    }

    public void StartGame()
    {
        Debug.Log("Starting Game...");
        startMenu.SetActive(false);
        Time.timeScale = 1f; // Unfreeze game
    }

    public void OpenOptionsMenu()
    {
        Debug.Log("Opening Options Menu...");
        startMenu.SetActive(false);
        optionsMenu.SetActive(true);
    }

    public void CloseOptionsMenu()
    {
        Debug.Log("Closing Options Menu...");
        optionsMenu.SetActive(false);
        startMenu.SetActive(true);
    }

    public void ExitGame()
    {
        Debug.Log("Exiting Game...");
        Application.Quit();
    }
}
