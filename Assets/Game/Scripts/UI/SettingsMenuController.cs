/*
 * Author: Shon Verch
 * File Name: SettingsMenuController.cs
 * Project Name: TheDungeonMaster
 * Creation Date: 01/21/2018
 * Modified Date: 01/21/2018
 * Description: The top-level manager for the settings menu.
 */

using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// The top-level manager for the settings menu.
/// </summary>
public class SettingsMenuController : ControllerBehaviour
{
    [SerializeField]
    private GameSettings gameSettings;

    [SerializeField]
    private Slider masterVolumeSlider;
    [SerializeField]
    private Slider musicVolumeSlider;
    [SerializeField]
    private Slider soundEffectVolumeSlider;

    [SerializeField]
    private Toggle lowGraphicsToggle;
    [SerializeField]
    private Toggle mediumGraphicsToggle;
    [SerializeField]
    private Toggle highGraphicsToggle;
    [SerializeField]
    private Toggle fullscreenToggle;

    /// <summary>
    /// A small "hack" to make it so the OnValueChanged callback
    /// isn't called when we modify toggles in code.
    /// </summary>
    private bool supressToggleEvents;

    /// <summary>
    /// Called when this object is created and placed into the world.
    /// </summary>
    private void Start()
    {
        // Initialize sliders to settings
        masterVolumeSlider.value = gameSettings.MasterVolume;
        musicVolumeSlider.value = gameSettings.MusicVolume;
        soundEffectVolumeSlider.value = gameSettings.SoundEffectVolume;

        // Quality index ranges from 0 to 2: low (0), medium (1), high(2)
        int quality = QualitySettings.GetQualityLevel();

        lowGraphicsToggle.isOn = quality == 0;
        mediumGraphicsToggle.isOn = quality == 1;
        highGraphicsToggle.isOn = quality == 2;
        fullscreenToggle.isOn = Screen.fullScreen;
    }

    /// <summary>
    /// Called when the master volume slider value changes.
    /// </summary>
    public void MasterVolumeSliderValueChanged() => gameSettings.MasterVolume = masterVolumeSlider.value;

    /// <summary>
    /// Called when the music volume slider changes.
    /// </summary>
    public void MusicVolumeSliderValueChanged() => gameSettings.MusicVolume = musicVolumeSlider.value;

    /// <summary>
    /// Called when the sound effect volume slider changes.
    /// </summary>
    public void SoundEffectVolumeSliderValueChanged() => gameSettings.SoundEffectVolume = soundEffectVolumeSlider.value;

    /// <summary>
    /// Called when the low graphics toggle value is changed.
    /// </summary>
    public void LowGraphicsToggleValueChanged()
    {
        if (supressToggleEvents) return;

        supressToggleEvents = true;
        mediumGraphicsToggle.isOn = false;
        highGraphicsToggle.isOn = false;
        supressToggleEvents = false;

        QualitySettings.SetQualityLevel(0);
    }

    /// <summary>
    /// Called when the medium graphics toggle value is changed.
    /// </summary>
    public void MediumGraphicsToggleValueChanged()
    {
        if (supressToggleEvents) return;

        supressToggleEvents = true;
        lowGraphicsToggle.isOn = false;
        highGraphicsToggle.isOn = false;
        supressToggleEvents = false;

        QualitySettings.SetQualityLevel(1);
    }

    /// <summary>
    /// Called when the high graphics toggle value is changed.
    /// </summary>
    public void HighGraphicsToggleValueChanged()
    {
        if (supressToggleEvents) return;

        supressToggleEvents = true;
        lowGraphicsToggle.isOn = false;
        mediumGraphicsToggle.isOn = false;
        supressToggleEvents = false;

        QualitySettings.SetQualityLevel(2);
    }

    /// <summary>
    /// Called when the fullscreen toggle value changes.
    /// </summary>
    public void FullscreenToggleValueChanged() => Screen.fullScreen = fullscreenToggle.isOn;
}
