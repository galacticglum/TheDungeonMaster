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
    [SerializeField]
    private Texture2D cursorNormal;
    [SerializeField]
    private Texture2D cursorClosed;

    private GameController gameController;
    private GamePauseMenuController gamePauseMenuController;
    private LootWindowController lootWindowController;
    private EncounterController encounterController;

    private void Start()
    {
        gameController = ControllerDatabase.Get<GameController>();
        gamePauseMenuController = ControllerDatabase.Get<GamePauseMenuController>();
        lootWindowController = ControllerDatabase.Get<LootWindowController>(); 
        encounterController = ControllerDatabase.Get<EncounterController>();
    }

    private void Update()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = gameController.IsPaused || encounterController.IsPlayerInsideEncounter || lootWindowController.IsOpen;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            gamePauseMenuController.HideActiveMenu();
            if (gamePauseMenuController.MenuStackCount == 0)
            {
                gameController.TogglePause();
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            Cursor.SetCursor(cursorClosed, Vector2.zero, CursorMode.Auto);
        }

        if (Input.GetMouseButtonUp(0))
        {
            Cursor.SetCursor(cursorNormal, Vector2.zero, CursorMode.Auto);
        }
    }
}