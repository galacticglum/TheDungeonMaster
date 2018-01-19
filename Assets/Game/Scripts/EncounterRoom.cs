/*
 * Author: Shon Verch
 * File Name: EncounterRoom.cs
 * Project Name: TheDungeonMaster
 * Creation Date: 12/28/2017
 * Modified Date: 1/18/2018
 * Description: A room for an encounter.
 */

/// <summary>
/// A room for an encounter.
/// </summary>
public class EncounterRoom : Room
{
    /// <inheritdoc />
    public override bool IsComplete { get; protected set; }

    /// <summary>
    /// Handle the logic for when the player enters this <see cref="Room"/>.
    /// </summary>
    protected override void OnPlayerEntered()
    {
        if (IsComplete) return;
        ControllerDatabase.Get<EncounterController>().BeginEncounter(OnEncounterComplete);
    }

    /// <summary>
    /// Called when the encounter began by this <see cref="Room"/> has been completed.
    /// </summary>
    private void OnEncounterComplete()
    {
        IsComplete = true;
        LootInstance.Create(transform);
    }
}