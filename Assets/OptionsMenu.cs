using UnityEngine;

public class OptionsMenu : MonoBehaviour
{
    public StartMenu startMenu; // Reference to StartMenu script

    public void ToggleConfirmExit(bool enabled)
    {
        startMenu.SetConfirmExit(enabled);
    }

    public void BackToMainMenu()
    {
        gameObject.SetActive(false); // Disable Options Menu
        startMenu.gameObject.SetActive(true); // Enable Start Menu
    }
}
