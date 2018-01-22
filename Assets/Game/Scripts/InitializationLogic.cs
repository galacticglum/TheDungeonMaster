/*
 * Author: Shon Verch
 * File Name: InitializationLogic.cs
 * Project Name: TheDungeonMaster
 * Creation Date: 01/21/2018
 * Modified Date: 01/21/2018
 * Description: A component which exposes a initialization event to the editor.
 */

using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// A component which exposes a initialization event to the editor.
/// </summary>
public class InitializationLogic : MonoBehaviour
{
    [SerializeField]
    private UnityEvent onStart;

    /// <summary>
    /// Called when this component is created in the world.
    /// </summary>
    private void Start() => onStart.Invoke();
}
