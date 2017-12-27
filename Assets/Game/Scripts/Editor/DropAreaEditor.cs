/*
 * Author: Shon Verch
 * File Name: DropAreaEditor.cs
 * Project Name: TheDungeonMaster
 * Creation Date: 12/27/2017
 * Modified Date: 12/27/2017
 * Description: DropArea component inspector.
 */

using UnityEditor;

/// <summary>
/// <see cref="DropArea"/> component inspector.
/// </summary>
[CustomEditor(typeof(DropArea))]
public class DropAreaEditor : Editor
{
    protected override bool ShouldHideOpenButton()
    {
        return true;
    }

    public override void OnInspectorGUI()
    {
        // This is empty for a reason, we don't want to render any inspector options.
    }
}
