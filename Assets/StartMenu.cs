using UnityEngine;

public class StartMenu : MonoBehaviour
{
    public GameObject optionsMenu;      // Drag the "Options Menu" Canvas here
    public GameObject startMenu;        // Drag the "Start Menu" Canvas here
    public GameObject confirmExitMenu;  // Drag the "Confirm Exit" Canvas here
    public GameObject PlayerUI;
    private bool gameStarted = false;
    private bool menuActive = true;     // Tracks whether any menu is active
    private bool confirmExitEnabled = true;

    void Start()
    {
        PlayerUI.SetActive(false); //Hides playerUI at the start of the game
        // Freeze the game initially and show the cursor
        Time.timeScale = 0f;
        UpdateCursor(true);
    }

    void Update()
    {
        // Handle ESC key for toggling menus
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (optionsMenu.activeSelf)
            {
                BackToMainMenu();
            }
            else if (confirmExitMenu.activeSelf)
            {
                CancelConfirmExit();
            }
            else if (gameStarted)
            {
                ToggleMenu();
            }
        }
    }

    public void StartGame()
    {
        gameStarted = true;
        menuActive = false;
        PlayerUI.SetActive(true); // Shows playerUI
        startMenu.SetActive(false);
        Time.timeScale = 1f; // Resume the game
        UpdateCursor(false); // Lock and hide the cursor
    }

    public void OpenOptionsMenu()
    {
        optionsMenu.SetActive(true);
        startMenu.SetActive(false);
    }

    public void ExitGame()
    {
        if (gameStarted && confirmExitEnabled)
        {
            confirmExitMenu.SetActive(true);
        }
        else
        {
            Application.Quit(); // Exits the application
            Debug.Log("Exiting Game"); // For debugging in the editor
        }
    }

    public void ConfirmExit(bool confirm)
    {
        if (confirm)
        {
            Application.Quit(); // Exits the application
            Debug.Log("Exiting Game"); // For debugging in the editor
        }
        else
        {
            CancelConfirmExit();
        }
    }

    public void BackToMainMenu()
    {
        optionsMenu.SetActive(false);
        startMenu.SetActive(true);
    }

    public void CancelConfirmExit()
    {
        confirmExitMenu.SetActive(false);
        startMenu.SetActive(true);
    }

    private void ToggleMenu()
    {
        menuActive = !menuActive;
        startMenu.SetActive(menuActive);

        // Freeze or resume the game based on menu visibility
        Time.timeScale = menuActive ? 0f : 1f;

        // Update the cursor visibility and lock state
        UpdateCursor(menuActive);
    }

    public void SetConfirmExit(bool enabled)
    {
        confirmExitEnabled = enabled;
    }

    private void UpdateCursor(bool visible)
    {
        Cursor.visible = visible;
        Cursor.lockState = visible ? CursorLockMode.None : CursorLockMode.Locked;
    }
}
