/*
 * Author: Shon Verch
 * File Name: AnimatedText.cs
 * Project Name: TheDungeonMaster
 * Creation Date: 01/21/18
 * Modified Date: 01/21/18
 * Description: Animates a text component iterating through phases every x seconds. 
 */
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Animates a text component iterating through phases every x seconds. 
/// </summary>
[RequireComponent(typeof(Text))]
public class AnimatedText : MonoBehaviour
{
    [SerializeField]
    private string[] phases;
    [SerializeField]
    private float phasesPerSecond;

    private Text textComponent;
    private int phaseIndex;

    /// <summary>
    /// Called when the component is placed into the world.
    /// </summary>
    private void Start()
    {
        textComponent = GetComponent<Text>();

        float repeatRate = 1 / phasesPerSecond;
        InvokeRepeating(nameof(NextPhase), 0, repeatRate);
    }

    /// <summary>
    /// Transition to the next phase of the animation..
    /// </summary>
    private void NextPhase()
    {
        textComponent.text = phases[phaseIndex++ % phases.Length];
    }
}
