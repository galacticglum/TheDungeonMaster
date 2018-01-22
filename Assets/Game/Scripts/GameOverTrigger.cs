/*
 * Author: Shon Verch
 * File Name: GameOverTrigger.cs
 * Project Name: TheDungeonMaster
 * Creation Date: 01/21/2018
 * Modified Date: 01/21/2018
 * Description: Triggers the win screen.
 */

using UnityEngine;

/// <summary>
/// Triggers the win screen.
/// </summary>
public class GameOverTrigger : MonoBehaviour
{
    [SerializeField]
    private CanvasGroup winScreenCanvasGroup;
    [SerializeField]
    private float fadeDuration = 0.5f;

    private LerpInformation<float> fadeLerpInformation;

    /// <summary>
    /// Triggers when the player collides with the collider attached to this <see cref="GameObject"/>.
    /// </summary>
    private void OnTriggerEnter(Collider other)
    {
        fadeLerpInformation = new LerpInformation<float>(0, 1, fadeDuration, Mathf.Lerp, null, (sender, args) => fadeLerpInformation = null);
    }

    /// <summary>
    /// Called every frame.
    /// </summary>
    private void Update()
    {
        if (fadeLerpInformation == null) return;
        winScreenCanvasGroup.alpha = fadeLerpInformation.Step(Time.deltaTime);
    }
}