/*
 * Author: Shon Verch
 * File Name: InputController.cs
 * Project Name: TheDungeonMaster
 * Creation Date: 01/21/2018
 * Modified Date: 01/21/2018
 * Description: The manager for any input logic.
 */

using UnityEngine;

/// <summary>
/// The manager for any input logic.
/// </summary>
public class InputController : ControllerBehaviour
{
    private GameController gameController;
    private GamePauseMenuController gamePauseMenuController;
    private EncounterController encounterController;

    private void Start()
    {
        gameController = ControllerDatabase.Get<GameController>();
        gamePauseMenuController = ControllerDatabase.Get<GamePauseMenuController>();
        encounterController = ControllerDatabase.Get<EncounterController>();

        Cursor.lockState = CursorLockMode.None;
    }

    private void Update()
    {
        Cursor.visible = gameController.IsPaused || encounterController.IsPlayerInsideEncounter;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            gamePauseMenuController.HideActiveMenu();
            if (gamePauseMenuController.MenuStackCount == 0)
            {
                gameController.TogglePause();
            }

        }
    }
}