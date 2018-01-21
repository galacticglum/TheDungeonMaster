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

/// <summary>
/// The top-level manager for all menus during gameplay.
/// </summary>
[RequireComponent(typeof(RectTransform))]
public class GamePauseMenuController : ControllerBehaviour
{
    public int MenuStackCount => menuStack.Count;

    private RectTransform rectTransform;
    private Stack<RectTransform> menuStack;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        menuStack = new Stack<RectTransform>();

        gameObject.SetActive(false);

        ControllerDatabase.Get<GameController>().PauseStateChanged += OnPauseStateChanged;
    }

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

    public void ShowMenu(RectTransform rectTransform)
    {
        if (menuStack.Count > 0)
        {
            menuStack.Peek()?.gameObject.SetActive(false);
        }

        menuStack.Push(rectTransform);
        rectTransform.gameObject.SetActive(true);
    }

    public void HideActiveMenu()
    {
        if (menuStack.Count > 0)
        {
            menuStack.Pop()?.gameObject.SetActive(false);
        }

        if (menuStack.Count > 0)
        {
            menuStack.Peek()?.gameObject.SetActive(true);
        }
    }
}
