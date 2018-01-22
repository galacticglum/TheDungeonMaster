/*
 * Author: Shon Verch
 * File Name: EncounterRoom.cs
 * Project Name: TheDungeonMaster
 * Creation Date: 12/28/2017
 * Modified Date: 1/19/2018
 * Description: A room for an encounter.
 */

using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A room for an encounter.
/// </summary>
public class EncounterRoom : Room
{
    [SerializeField]
    private List<Enemy> enemies;
    [SerializeField]
    private List<Card> lootCards;

    /// <summary>
    /// Called when the <see cref="Room"/> is created and is placed into the world.
    /// </summary>
    protected override void Start()
    {
        base.Start();
        IsComplete = false;
    }

    /// <summary>
    /// Handle the logic for when the player enters this <see cref="Room"/>.
    /// </summary>
    protected override void OnPlayerEntered()
    {
        if (IsComplete) return;
        ControllerDatabase.Get<EncounterController>().BeginEncounter(enemies, OnEncounterComplete);
    }

    /// <summary>
    /// Called when the encounter began by this <see cref="Room"/> has been completed.
    /// </summary>
    private void OnEncounterComplete()
    {
        IsComplete = true;
        LootInstance.Create(transform, lootCards);
    }
}