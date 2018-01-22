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

    /// <summary>
    /// Called when this componnet is created in the world.
    /// </summary>
    private void Start()
    {
        // Cache any controllers that we need.
        gameController = ControllerDatabase.Get<GameController>();
        gamePauseMenuController = ControllerDatabase.Get<GamePauseMenuController>();
        lootWindowController = ControllerDatabase.Get<LootWindowController>(); 
        encounterController = ControllerDatabase.Get<EncounterController>();
    }

    /// <summary>
    /// Called every frame.
    /// </summary>
    private void Update()
    {
        if (gameController.HasWon || encounterController.HasLost) return;

        // Are cursor is visible when the game is either paused, in combat, or when a loot window is open.
        Cursor.visible = gameController.IsPaused || encounterController.IsPlayerInsideEncounter || lootWindowController.IsOpen;

        // If our cursor is visible, we unlock our cursor, otherwise we lock it.
        Cursor.lockState = Cursor.visible ? CursorLockMode.None : CursorLockMode.Locked;

        // When we press escape and we are not in the ending game screen, pause the game.
        if (Input.GetKeyDown(KeyCode.Escape) && !gameController.HasWon && !encounterController.HasLost)
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