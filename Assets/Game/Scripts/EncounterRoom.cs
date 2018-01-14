/*
 * Author: Shon Verch
 * File Name: EncounterRoom.cs
 * Project Name: TheDungeonMaster
 * Creation Date: 12/28/2017
 * Modified Date: 1/14/2018
 * Description: A room for an encounter.
 */

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
        if (isComplete) return;
        ControllerDatabase.Get<EncounterController>().BeginEncounter(OnEncounterComplete);
    }

    private void OnEncounterComplete()
    {
        isComplete = true;
        LootInstance.Create(transform);
    }

    public override bool IsComplete()
    {
        return isComplete;
    }
}