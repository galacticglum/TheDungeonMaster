/*
 * Author: Shon Verch
 * File Name: MenuController.cs
 * Project Name: TheDungeonMaster
 * Creation Date: 01/21/2018
 * Modified Date: 01/21/2018
 * Description: The top-level manager for all menus.
 */

using UnityEngine;

/// <summary>
/// The top-level manager for all menus.
/// </summary>
public class MenuController : ControllerBehaviour
{
    [SerializeField]
    private RectTransform pauseMenuRoot;

    private void Start()
    {
        pauseMenuRoot.gameObject.SetActive(false);
        ControllerDatabase.Get<GameController>().PauseStateChanged += (sender, args) => pauseMenuRoot.gameObject.SetActive(args.IsPaused);
    }
}
