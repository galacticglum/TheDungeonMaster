/*
 * Author: Shon Verch
 * File Name: EncounterRoom.cs
 * Project Name: TheDungeonMaster
 * Creation Date: 12/28/2017
 * Modified Date: 12/28/2017
 * Description: A room for an encounter.
 */

using UnityEngine;

/// <summary>
/// A room for an encounter.
/// </summary>
public class EncounterRoom : Room
{
    private bool isComplete;

    private void OnEnable()
    {
        PlayerEntered += OnPlayerEntered;
    }

    private void OnPlayerEntered(object sender, RoomEventArgs args)
    {
        isComplete = true;
    }

    public override bool IsComplete()
    {
        return isComplete;
    }
}