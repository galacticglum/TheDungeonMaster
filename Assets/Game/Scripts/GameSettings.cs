/*
 * Author: Shon Verch
 * File Name: GameSettings.cs
 * Project Name: TheDungeonMaster
 * Creation Date: 01/21/2018
 * Modified Date: 01/21/2018
 * Description: The container for all game settings.
 */

using UnityEngine;

/// <summary>
/// The container for all game settings.
/// </summary>
[CreateAssetMenu]
public class GameSettings : ScriptableObject
{
    [SerializeField]
    private float masterVolume = 1;
    [SerializeField]
    private float musicVolume = 1;
    [SerializeField]
    private float soundEffectVolume = 1;

    [SerializeField]
    private int graphicsQuality = QualitySettings.GetQualityLevel();
    [SerializeField]
    private bool isFullscreen = true;
}