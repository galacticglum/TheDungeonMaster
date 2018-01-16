/*
 * Author: Shon Verch
 * File Name: LootInstance.cs
 * Project Name: TheDungeonMaster
 * Creation Date: 01/14/2018
 * Modified Date: 01/14/2018
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

    private void Start()
    {
        playerController = ControllerDatabase.Get<PlayerController>();
        lootWindowController = ControllerDatabase.Get<LootWindowController>();
        keydownIcon.enabled = false;
    }

    private void Update()
    {
        float distanceFromPlayer = Vector3.Distance(transform.position, playerController.transform.position);
        bool isPlayerInRange = distanceFromPlayer <= activationRadius;
        keydownIcon.enabled = isPlayerInRange;

        if (!Input.GetKeyDown(KeyCode.E) || !isPlayerInRange) return;
        lootWindowController.Open(new List<Card>
        {
            new Card("Norwegian Brute", "A tall, handsome, brute who comes from an arbitrary Norwegian province.", 10),
            new Card("German Brute", "A tall but smaller, handsome, brute who comes from an arbitrary German province.", 8),
            new Card("Dwarven Brute", "A short, hairy, brute who comes from an arbitrary European province.", 8)
        });
        hasOpened = true;
    }

    public static LootInstance Create(Transform parent)
    {
        GameObject lootPrefab = Resources.Load<GameObject>("Prefabs/Loot_Box");
        Vector3 spawnPosition = parent.position;
        spawnPosition.y += lootPrefab.transform.localScale.z / 2f;

        GameObject lootGameObject = Instantiate(lootPrefab, spawnPosition, Quaternion.identity);
        lootGameObject.transform.SetParent(parent);

        return lootGameObject.GetComponent<LootInstance>();
    }
}
