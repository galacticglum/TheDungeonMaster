/*
 * Author: Shon Verch
 * File Name: GameMenuController.cs
 * Project Name: TheDungeonMaster
 * Creation Date: 01/21/2018
 * Modified Date: 01/21/2018
 * Description: The top-level manager for all menus during gameplay.
 */

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// The top-level manager for all menus during gameplay.
/// </summary>
[RequireComponent(typeof(RectTransform))]
public class GamePauseMenuController : ControllerBehaviour
{
    /// <summary>
    /// The amount of menus in the stack.
    /// </summary>
    public int MenuStackCount => menuStack.Count;

    private RectTransform rectTransform;
    private Stack<RectTransform> menuStack;

    /// <summary>
    /// Called when this controller is created.
    /// </summary>
    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        menuStack = new Stack<RectTransform>();

        gameObject.SetActive(false);
        ControllerDatabase.Get<GameController>().PauseStateChanged += OnPauseStateChanged;
    }

    /// <summary>
    /// Called when the paused state changes.
    /// </summary>
    private void OnPauseStateChanged(object sender, PauseStateEventArgs args)
    {
        if (args.IsPaused)
        {
            ShowMenu(rectTransform);
        }
        else
        {
            while (menuStack.Count > 0)
            {
                HideActiveMenu();
            }
        }
    }

    /// <summary>
    /// Shows the menu.
    /// </summary>
    public void ShowMenu(RectTransform rectTransform)
    {
        if (menuStack.Count > 0)
        {
            menuStack.Peek()?.gameObject.SetActive(false);
        }

        menuStack.Push(rectTransform);
        rectTransform.gameObject.SetActive(true);
    }

    /// <summary>
    /// Hides the active menu.
    /// </summary>
    public void HideActiveMenu()
    {
        if (menuStack.Count > 0)
        {
            menuStack.Pop()?.gameObject.SetActive(false);
        }

        // We need to make sure that once we've popped a menu, that we still have another menu to peek at.
        if (menuStack.Count > 0)
        {
            menuStack.Peek()?.gameObject.SetActive(true);
        }
    }

    /// <summary>
    /// Quits to the main menu.
    /// </summary>
    public void QuitToMainMenu() => SceneManager.LoadSceneAsync(0);
}
