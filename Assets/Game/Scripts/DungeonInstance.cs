/*
 * Author: Shon Verch
 * File Name: DungeonInstance.cs
 * Project Name: TheDungeonMaster
 * Creation Date: 12/28/2017
 * Modified Date: 12/28/2017
 * Description: The top-level manager for a dungeon.
 */

using System;
using System.Linq;
using UnityEngine;

/// <summary>
/// The top-level manager for a dungeon.
/// </summary>
public class DungeonInstance : MonoBehaviour
{
    [SerializeField]
    private Room entryRoom;
    [SerializeField]
    private EncounterRoom[] encounters;
    [SerializeField]
    private EncounterRoom bossRoom;
    [SerializeField]
    private GameObject dungeonDoor;
    [SerializeField]
    private GameObject bossDoor;

    private bool isBossDoorOpen;

    private void Start()
    {
        dungeonDoor.SetActive(false);
        entryRoom.PlayerEntered += OnPlayerEnterEntryRoom;
    }

    private void OnPlayerEnterEntryRoom(object sender, RoomEventArgs args)
    {
        dungeonDoor.SetActive(true);
    }

    private void Update()
    {
        if (IsReadyForBoss() && !isBossDoorOpen)
        {
            isBossDoorOpen = false;
            bossDoor.SetActive(false);
        }

        if (bossRoom.IsComplete() && dungeonDoor.activeInHierarchy)
        {
            dungeonDoor.SetActive(false);
            entryRoom.PlayerEntered -= OnPlayerEnterEntryRoom;
        }
    }

    private bool IsReadyForBoss()
    {
        return encounters.All(encounterRoom => encounterRoom.IsComplete());
    }
}
