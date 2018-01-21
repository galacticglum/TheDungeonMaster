/*
 * Author: Shon Verch
 * File Name: OpenDoorGate.cs
 * Project Name: TheDungeonMaster
 * Creation Date: 01/20/2018
 * Modified Date: 01/20/2018
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
    private GameObject doorGameObject;

    private void Start()
    {
        doorGameObject.SetActive(true);
    }

    private void Update()
    {
        if (!rooms.All(room => room.IsComplete)) return;
        doorGameObject.SetActive(false);
    }
}
