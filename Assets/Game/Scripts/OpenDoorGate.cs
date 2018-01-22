/*
 * Author: Shon Verch
 * File Name: OpenDoorGate.cs
 * Project Name: TheDungeonMaster
 * Creation Date: 01/20/2018
 * Modified Date: 01/21/2018
 * Description: A logic-gate which opens a door if all referenced rooms are complete.
 */

using System.Linq;
using UnityEngine;

/// <summary>
/// A logic-gate which opens a door if all referenced rooms are complete.
/// </summary>
public class OpenDoorGate : MonoBehaviour
{
    [SerializeField]
    private Room[] rooms;
    [SerializeField]
    private GameObject door;

    /// <summary>
    /// Called when this component is created in the world.
    /// </summary>
    private void Start()
    {
        door.SetActive(true);
    }

    /// <summary>
    /// Called every frame.
    /// </summary>
    private void Update()
    {
        // If the door is already disabled, there is no reason to continue.
        if (!door.activeInHierarchy) return;

        // If some of the rooms are still incomplete then we bail!
        // Otherwise, we enable the door.
        if (!rooms.All(room => room.IsComplete)) return;
        door.SetActive(false);
    }
}
