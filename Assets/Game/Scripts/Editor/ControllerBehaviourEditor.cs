/*
 * Author: Shon Verch
 * File Name: ControllerBehaviourEditor.cs
 * Project Name: TheDungeonMaster
 * Creation Date: 12/29/2017
 * Modified Date: 1/15/2018
 * Description: The custom inspector for ControllerBehaviours.
 */

using UnityEditor;

/// <summary>
/// The custom inspector for ControllerBehaviours.
/// </summary>
[CustomEditor(typeof(ControllerBehaviour), true)]
public class ControllerBehaviourEditor : Editor
{
    private ControllerBehaviour controllerBehaviour;

    /// <summary>
    /// Initialize the custom editor.
    /// </summary>
    protected virtual void OnEnable()
    {
        controllerBehaviour = (ControllerBehaviour)target;
        EditorApplication.update += Update;
    }

    /// <summary>
    /// Called every editor frame.
    /// </summary>
    protected virtual void Update()
    {
        if (!controllerBehaviour.ShouldDestroy) return;

        EditorUtility.DisplayDialog("Can't add controller!", "Can't add controller because it already exists in the world.", "Okay");
        controllerBehaviour.ShouldDestroy = false;

        DestroyImmediate(controllerBehaviour);
    }

    /// <summary>
    /// Handle any destruction-related logic.
    /// Called when this <see cref="ControllerBehaviourEditor"/> is destroyed.
    /// </summary>
    protected virtual void OnDestroy() => EditorApplication.update -= Update;
}
