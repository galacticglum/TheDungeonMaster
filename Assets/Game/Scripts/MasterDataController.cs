/*
 * Author: Shon Verch
 * File Name: MasterDataController.cs
 * Project Name: TheDungeonMaster
 * Creation Date: 12/27/2017
 * Modified Date: 12/27/2017
 * Description: Global data controller which stores any game-wide variables.
 */

using UnityEngine;

/// <summary>
/// Global data controller which stores any game-wide variables.
/// </summary>
[ExecuteInEditMode]
public class MasterDataController : MonoBehaviour
{
    public static MasterDataController Current { get; private set; }

    public Transform CardSpawnRoot => cardSpawnRoot;
    public Transform EnemySpawnRoot => enemySpawnRoot;

    [Header("Spawners")]
    [SerializeField]
    private Transform cardSpawnRoot;
    [SerializeField]
    private Transform enemySpawnRoot;

    /// <summary>
    /// Called when the component is created and placed into the world.
    /// </summary>
    private void OnEnable()
    {
        Current = this;
    }
}
