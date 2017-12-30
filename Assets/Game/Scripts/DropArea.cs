/*
 * Author: Shon Verch
 * File Name: DropArea.cs
 * Project Name: TheDungeonMaster
 * Creation Date: 12/27/2017
 * Modified Date: 12/29/2017
 * Description: A drag and drop recipient area.
 */

using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// A drag and drop recipient area.
/// This derives from <see cref="Image"/> so that the drop area is visible by the event system.
/// </summary>
public class DropArea : Image
{
    /// <summary>
    /// Called when this component is enabled.
    /// </summary>
    protected override void OnEnable()
    {
        base.OnEnable();
        color = new Color(1, 1, 1, 0);
    }
}
