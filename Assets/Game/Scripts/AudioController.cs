/*
 * Author: Shon Verch
 * File Name: AudioController.cs
 * Project Name: TheDungeonMaster
 * Creation Date: 01/16/2018
 * Modified Date: 01/16/2018
 * Description: The top-level manager for all audio within the game.
 */

using UnityEngine;

/// <summary>
/// The top-level manager for all audio within the game.
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class AudioController : ControllerBehaviour
{
    [SerializeField]
    private AudioSource footstepAudioSource;
    private AudioSource mainAudioSource;

    private ConsecutiveAccessCollection<AudioClip> footstepAudioClips;
    private PlayerController playerController;

    private void Start()
    {
        mainAudioSource = GetComponent<AudioSource>();
        footstepAudioClips = new ConsecutiveAccessCollection<AudioClip>(Resources.LoadAll<AudioClip>("Audio/Footsteps"));

        playerController = ControllerDatabase.Get<PlayerController>();
        playerController.FootstepTriggered += OnFootstepTriggered;
    }

    /// <summary>
    /// Handle the player footstep-triggered event.
    /// Plays a footstep sound at a specified position.
    /// </summary>
    private void OnFootstepTriggered(object sender, PlayerControllerEventArgs args)
    {
        if (!args.PlayerController.IsMoving) return;
        footstepAudioSource.PlayOneShot(footstepAudioClips.GetNext());
    }
}
