/*
 * Author: Shon Verch
 * File Name: AudioController.cs
 * Project Name: TheDungeonMaster
 * Creation Date: 01/16/2018
 * Modified Date: 01/18/2018
 * Description: The top-level manager for all audio within the game.
 */

using UnityEngine;

/// <summary>
/// The top-level manager for all audio within the game.
/// </summary>
public class AudioController : ControllerBehaviour
{
    [SerializeField]
    private GameSettings gameSettings;

    [Tooltip("The audio source that footsteps should come from.")]
    [SerializeField]
    private AudioSource footstepAudioSource;
    [SerializeField]
    private AudioSource soundEffectAudioSource;
    [SerializeField]
    private AudioSource musicAudioSource;

    // Cache the original values of the audio sources so we can scale
    // the volume by it.
    private float footstepOriginalVolume;
    private float soundEffectOriginalVolume;
    private float musicOriginalVolume;

    private ConsecutiveAccessCollection<AudioClip> footstepAudioClips;
    private PlayerController playerController;

    /// <summary>
    /// Called when the component is created and placed into the world.
    /// </summary>
    private void Start()
    {
        // Load all audio clips in the Footsteps resource folder into a consecutive access collection.
        footstepAudioClips = new ConsecutiveAccessCollection<AudioClip>(Resources.LoadAll<AudioClip>("Audio/Footsteps"));

        // Cache the player controller so that we don't need to keep getting it from the database.
        // Doing this has negligible performance increase but it is still good to get rid of redudant code.
        playerController = ControllerDatabase.Get<PlayerController>();
        playerController.FootstepTriggered += OnFootstepTriggered;

        footstepOriginalVolume = footstepAudioSource.volume;
        soundEffectOriginalVolume = soundEffectAudioSource.volume;
        musicOriginalVolume = musicAudioSource.volume;
    }

    /// <summary>
    /// Called every frame.
    /// </summary>
    private void Update()
    {
        // Update the volumes
        soundEffectAudioSource.volume = gameSettings.MasterVolume * gameSettings.SoundEffectVolume * soundEffectOriginalVolume;
        footstepAudioSource.volume = gameSettings.MasterVolume * gameSettings.SoundEffectVolume * footstepOriginalVolume;
        musicAudioSource.volume = gameSettings.MasterVolume * gameSettings.MusicVolume * musicOriginalVolume;
    }

    /// <summary>
    /// Handle the player footstep-triggered event.
    /// Plays a footstep sound at the player controller position.
    /// </summary>
    private void OnFootstepTriggered(object sender, PlayerControllerEventArgs args)
    {
        // Sometimes this event may be triggered when the player isn't actually moving (due to the animation).
        // In this case, we don't want to play the sound effect. To handle this, we need to ask the 
        // player controller whether it is currently moving.
        if (!args.PlayerController.IsMoving) return;
        footstepAudioSource.PlayOneShot(footstepAudioClips.GetNext());
    }

    /// <summary>
    /// Plays a sound effect.
    /// </summary>
    public void PlaySoundEffect(AudioClip clip)
    {
        soundEffectAudioSource.PlayOneShot(clip);
    }
}
