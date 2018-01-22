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
    /// <summary>
    /// The master volume multiplier.
    /// Applies to music and sound effects.
    /// </summary>
    public float MasterVolume
    {
        get { return masterVolume; }
        set { masterVolume = value; }
    }

    /// <summary>
    /// The music volume multiplier.
    /// </summary>
    public float MusicVolume
    {
        get { return musicVolume; }
        set { musicVolume = value; }
    }

    /// <summary>
    /// The sound effect volume multiplier;
    /// </summary>
    public float SoundEffectVolume
    {
        get { return soundEffectVolume; }
        set { soundEffectVolume = value; }
    }

    [SerializeField]
    private float masterVolume = 1;
    [SerializeField]
    private float musicVolume = 1;
    [SerializeField]
    private float soundEffectVolume = 1;
}