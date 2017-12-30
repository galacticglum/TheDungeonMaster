/*
 * Author: Shon Verch
 * File Name: DungeonInstance.cs
 * Project Name: TheDungeonMaster
 * Creation Date: 12/28/2017
 * Modified Date: 12/28/2017
 * Description: The top-level manager for a dungeon.
 */

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

    private void Update()
    {
        if (IsReadyForBoss())
        {
            Debug.Log("boss is ready!");
        }
    }

    private bool IsReadyForBoss()
    {
        return encounters.All(encounterRoom => encounterRoom.IsComplete());
    }
}
