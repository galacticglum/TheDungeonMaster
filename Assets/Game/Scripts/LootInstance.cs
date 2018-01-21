/*
 * Author: Shon Verch
 * File Name: LootInstance.cs
 * Project Name: TheDungeonMaster
 * Creation Date: 01/14/2018
 * Modified Date: 01/21/2018
 * Description: The visual manager for a loot box.
 */

using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// The visual manager for a loot box.
/// </summary>
public class LootInstance : MonoBehaviour
{
    [SerializeField]
    private TextMeshPro keydownIcon;
    [SerializeField]
    private float activationRadius = 2;

    private bool hasOpened;
    private PlayerController playerController;
    private LootWindowController lootWindowController;
    private List<Card> lootCards;

    /// <summary>
    /// Called when the component is created and placed into the world.
    /// </summary>
    private void Start()
    {
        playerController = ControllerDatabase.Get<PlayerController>();
        lootWindowController = ControllerDatabase.Get<LootWindowController>();
        keydownIcon.enabled = false;
    }

    /// <summary>
    /// Called every frame.
    /// </summary>
    private void Update()
    {
        if (hasOpened)
        {
            keydownIcon.enabled = false;
            return;
        }

        float distanceFromPlayer = Vector3.Distance(transform.position, playerController.transform.position);
        bool isPlayerInRange = distanceFromPlayer <= activationRadius;
        keydownIcon.enabled = isPlayerInRange;

        if (!Input.GetKeyDown(KeyCode.E) || !isPlayerInRange) return;
        lootWindowController.Open(lootCards);

        hasOpened = true;
    }

    /// <summary>
    /// Creates a <see cref="LootInstance"/> an assigns it to the specified parent <see cref="Transform"/>.
    /// </summary>
    /// <param name="parent">The <see cref="Transform"/> to parent the <see cref="LootInstance"/> to.</param>
    public static LootInstance Create(Transform parent, List<Card> lootCards)
    {
        GameObject lootPrefab = Resources.Load<GameObject>("Prefabs/Loot_Box");
        Vector3 spawnPosition = parent.position;
        spawnPosition.y += lootPrefab.transform.localScale.z / 2f;

        GameObject lootGameObject = Instantiate(lootPrefab, spawnPosition, Quaternion.identity);
        lootGameObject.transform.SetParent(parent);

        LootInstance lootInstance = lootGameObject.GetComponent<LootInstance>();
        lootInstance.lootCards = lootCards;

        return lootInstance;
    }
}
