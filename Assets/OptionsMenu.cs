using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Linq; // Required for LINQ

public class OptionsMenu : MonoBehaviour
{
    [Header("Volume Sliders")]
    public Slider masterVolumeSlider;
    public Slider musicVolumeSlider;
    public Slider sfxVolumeSlider;

    [Header("Display Settings")]
    public TMP_Dropdown resolutionDropdown; // TMP_Dropdown for resolutions
    public TMP_Dropdown displayModeDropdown; // TMP_Dropdown for display modes
    public Toggle confirmExitToggle;

    private Resolution[] availableResolutions;
    private bool isInitialized = false; // Tracks if UI has been initialized

    [Header("Start Menu Script")]
    public StartMenu startMenu; // Reference to StartMenu script for confirm exit toggle
    [Header("Start Menu Closing")]
    public GameObject StartMenu; // Reference to StartMenu 


    private float masterVolume;
    private float musicVolume;
    private float sfxVolume;
    private int resolutionIndex;
    private int displayModeIndex;
    private bool confirmExit;

    private void OnEnable()
    {
        if (!isInitialized)
        {
            LoadSettings();
            InitializeUI();
            isInitialized = true;
        }
    }

    private void LoadSettings()
    {
        // Load PlayerPrefs settings or use defaults
        masterVolume = PlayerPrefs.GetFloat("MasterVolume", 1f);
        musicVolume = PlayerPrefs.GetFloat("MusicVolume", 1f);
        sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 1f);
        resolutionIndex = PlayerPrefs.GetInt("ResolutionIndex", 0); // Default to first resolution in sorted list
        displayModeIndex = PlayerPrefs.GetInt("DisplayMode", 0);
        confirmExit = PlayerPrefs.GetInt("ConfirmExit", 1) == 1;
    }

    private void InitializeUI()
    {
        // Set sliders
        masterVolumeSlider.value = masterVolume;
        musicVolumeSlider.value = musicVolume;
        sfxVolumeSlider.value = sfxVolume;

        // Populate resolution dropdown
        availableResolutions = Screen.resolutions.OrderByDescending(r => r.width * r.height).ToArray(); // Sort by resolution (width * height)
        resolutionDropdown.ClearOptions();
        foreach (Resolution res in availableResolutions)
        {
            resolutionDropdown.options.Add(new TMP_Dropdown.OptionData(res.width + "x" + res.height));
        }
        resolutionDropdown.value = resolutionIndex;
        resolutionDropdown.RefreshShownValue();

        // Set display mode dropdown
        displayModeDropdown.options = new System.Collections.Generic.List<TMP_Dropdown.OptionData>
        {
            new TMP_Dropdown.OptionData("Windowed"),
            new TMP_Dropdown.OptionData("Fullscreen"),
            new TMP_Dropdown.OptionData("Windowed Fullscreen")
        };
        displayModeDropdown.value = displayModeIndex;
        displayModeDropdown.RefreshShownValue();

        // Set confirm exit toggle
        confirmExitToggle.isOn = confirmExit;
    }

    public void OpenOptionsMenu()
    {
        gameObject.SetActive(true); // Show Options Menu
    }

    public void CloseOptionsMenu()
    {
        SaveSettings();             // Save changes when exiting the menu
        gameObject.SetActive(false); // Hide Options Menu
        if (startMenu != null)
        {
            StartMenu.SetActive(true); // Show Start Menu
        }
    }

    private void SaveSettings()
    {
        // Save Volume
        masterVolume = masterVolumeSlider.value;
        musicVolume = musicVolumeSlider.value;
        sfxVolume = sfxVolumeSlider.value;
        PlayerPrefs.SetFloat("MasterVolume", masterVolume);
        PlayerPrefs.SetFloat("MusicVolume", musicVolume);
        PlayerPrefs.SetFloat("SFXVolume", sfxVolume);

        // Save Resolution
        resolutionIndex = resolutionDropdown.value;
        Resolution selectedResolution = availableResolutions[resolutionIndex];
        Screen.SetResolution(selectedResolution.width, selectedResolution.height, Screen.fullScreenMode);
        PlayerPrefs.SetInt("ResolutionIndex", resolutionIndex);

        // Save Display Mode
        displayModeIndex = displayModeDropdown.value;
        FullScreenMode mode = FullScreenMode.Windowed;
        switch (displayModeIndex)
        {
            case 0: mode = FullScreenMode.Windowed; break;
            case 1: mode = FullScreenMode.FullScreenWindow; break;
            case 2: mode = FullScreenMode.MaximizedWindow; break;
        }
        Screen.fullScreenMode = mode;
        PlayerPrefs.SetInt("DisplayMode", displayModeIndex);

        // Save Confirm Exit
        confirmExit = confirmExitToggle.isOn;
        PlayerPrefs.SetInt("ConfirmExit", confirmExit ? 1 : 0);
        startMenu.SetConfirmExit(confirmExit);

        PlayerPrefs.Save(); // Ensure changes are written to disk
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CloseOptionsMenu();
        }
    }
}
