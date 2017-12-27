/*
 * Author: Shon Verch
 * File Name: DropArea.cs
 * Project Name: TheDungeonMaster
 * Creation Date: 12/27/2017
 * Modified Date: 12/27/2017
 * Description: A drag and drop recipient area.
 */

using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// A drag and drop recipient area.
/// </summary>
public class DropArea : Image
{
    protected override void OnEnable()
    {
        base.OnEnable();
        color = new Color(1, 1, 1, 0);
    }
}
