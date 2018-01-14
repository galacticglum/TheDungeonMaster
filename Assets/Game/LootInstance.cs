/*
 * Author: Shon Verch
 * File Name: LootInstance.cs
 * Project Name: TheDungeonMaster
 * Creation Date: 01/14/2018
 * Modified Date: 01/14/2018
 * Description: The visual manager for a loot box.
 */

using TMPro;
using UnityEngine;

/// <summary>
/// The visual manager for a loot box.
/// </summary>
public class LootInstance : MonoBehaviour
{
    [SerializeField]
    private TextMeshPro keydownIcon;
    private PlayerController playerController;

    private void Start()
    {
        playerController = ControllerDatabase.Get<PlayerController>();
    }

    private void Update()
    {
        float distanceFromPlayer = Vector3.Distance(transform.position, playerController.transform.position);
        keydownIcon.enabled = distanceFromPlayer <= 2;
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
