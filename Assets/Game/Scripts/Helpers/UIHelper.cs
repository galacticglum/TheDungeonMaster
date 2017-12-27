/*
 * Author: Shon Verch
 * File Name: UIHelper.cs
 * Project Name: TheDungeonMaster
 * Creation Date: 12/26/2017
 * Modified Date: 12/26/2017
 * Description: Extension functionality relating to the user-interface.
 */

using UnityEngine;

/// <summary>
/// Extension functionality relating to the user-interface.
/// </summary>
public static class UIHelper
{
    /// <summary>
    /// Set the visibility of a <see cref="CanvasGroup"/>.
    /// </summary>
    /// <param name="canvasGroup">The <see cref="CanvasGroup"/> to modify.</param>
    /// <param name="visibility">A boolean representing the visibility of the <see cref="CanvasGroup"/>.</param>
    public static void SetVisibility(this CanvasGroup canvasGroup, bool visibility) => canvasGroup.alpha = visibility ? 1f : 0f;

    /// <summary>
    /// Toggle the block raycast functionality of a <see cref="CanvasGroup"/>.
    /// </summary>
    /// <param name="canvasGroup">The <see cref="CanvasGroup"/> to modify.</param>
    /// <param name="blockInput">A boolean representing the block raycast property of the <see cref="CanvasGroup"/>.</param>
    public static void SetBlockInput(this CanvasGroup canvasGroup, bool blockInput) => canvasGroup.blocksRaycasts = blockInput;
}
