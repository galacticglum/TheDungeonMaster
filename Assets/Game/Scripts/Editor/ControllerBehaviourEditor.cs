/*
 * Author: Shon Verch
 * File Name: ControllerBehaviourEditor.cs
 * Project Name: TheDungeonMaster
 * Creation Date: 12/29/2017
 * Modified Date: 1/13/2017
 * Description: The custom inspector for ControllerBehaviours.
 */

using UnityEditor;

/// <summary>
/// The custom inspector for ControllerBehaviours.
/// </summary>
[CustomEditor(typeof(ControllerBehaviour), true)]
public class ControllerBehaviourEditor : Editor
{
    /// <summary>
    /// Initialize the custom editor.
    /// </summary>
    private void OnEnable() => EditorApplication.update -= Update;

    /// <summary>
    /// Called every editor frame.
    /// </summary>
    private void Update()
    {
        ControllerBehaviour controllerBehaviour = (ControllerBehaviour)target;
        if (!controllerBehaviour.ShouldDestroy) return;

        EditorUtility.DisplayDialog("Can't add controller!", "Can't add controller because it already exists in the world.", "Okay");
        controllerBehaviour.ShouldDestroy = false;

        DestroyImmediate(controllerBehaviour);
    }

    /// <summary>
    /// Handle any destruction-related logic.
    /// Called when this <see cref="ControllerBehaviourEditor"/> is destroyed.
    /// </summary>
    private void OnDestroy() => EditorApplication.update -= Update;
}
