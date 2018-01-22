/*
 * Author: Shon Verch
 * File Name: PlayerHealthDisplay.cs
 * Project Name: TheDungeonMaster
 * Creation Date: 01/21/2018
 * Modified Date: 01/21/2018
 * Description: Displays the player health in a text component.
 */

using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Displays the player health in a text component.
/// </summary>
[RequireComponent(typeof(Text))]
public class PlayerHealthDisplay : MonoBehaviour
{
    [SerializeField]
    private string format = "{0} HP";

    private Text textComponent;
    private EncounterController encounterController;

    private void Start()
    {
        encounterController = ControllerDatabase.Get<EncounterController>();
        textComponent = GetComponent<Text>();
    }

    private void Update()
    {
        textComponent.text = string.Format(format, encounterController.PlayerHealth);
    }
}