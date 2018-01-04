/*
 * Author: Shon Verch
 * File Name: EncounterController.cs
 * Project Name: TheDungeonMaster
 * Creation Date: 01/02/2018
 * Modified Date: 01/02/2018
 * Description: The top-level manager for a combat session (battle). 
 */

using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The top-level manager for a combat session (battle). 
/// </summary>
public class EncounterController : ControllerBehaviour
{
    [SerializeField]
    private RectTransform enemyInstanceParent;

    private List<EnemyInstance> enemies;

    public void BeginEncounter()
    {
        ControllerDatabase.Get<CardHandController>().Clear();
        for (int i = 0; i < 5; i++)
        {
            ControllerDatabase.Get<CardHandController>().AddCard(new Card("Power Punch", "Deals <color=#D5AB5CFF><i>6</i></color> damage to an enemy. <color=#D5AB5CFF>Overcharge:</color> 3 cards."));
        }

        ClearEnemies();
        AddEnemyToEncounter(new Enemy("Orc Grunt", "ME ORC, ME SMASH!", 10));
        AddEnemyToEncounter(new Enemy("Elessar", "The fantastic shetland sheepdog.", 20));
    }

    private void UpdateEnemyPositions()
    {
        const float gapRatio = 1.2f;
        RectTransform enemyPrefabRectTransform = Resources.Load<GameObject>("Prefabs/Enemy_Front").GetComponent<RectTransform>();

        float gapBetweenCards = enemyPrefabRectTransform.rect.width * enemyPrefabRectTransform.localScale.x * gapRatio;
        float edgeCardOffset = (enemies.Count - 1) * gapBetweenCards / 2f;

        for (int i = 0; i < enemies.Count; i++)
        {
            EnemyInstance enemyInstance = enemies[i];

            // Set the anchor and pivot of the card to the centre.
            enemyInstance.RectTransform.anchorMin = new Vector2(0.5f, 0.5f);
            enemyInstance.RectTransform.anchorMax = new Vector2(0.5f, 0.5f);
            enemyInstance.RectTransform.pivot = new Vector2(0.5f, 0.5f);

            enemyInstance.RectTransform.anchoredPosition = new Vector2(i * gapBetweenCards - edgeCardOffset, 0);
        }
    }

    private void AddEnemyToEncounter(Enemy enemy)
    {
        EnemyInstance enemyInstance = EnemyInstance.Create(enemy, enemyInstanceParent);
        enemies.Add(enemyInstance);
        UpdateEnemyPositions();
    }

    private void ClearEnemies()
    {
        if (enemies == null)
        {
            enemies = new List<EnemyInstance>();
            return;
        }

        foreach (EnemyInstance enemyInstance in enemies)
        {
            Destroy(enemyInstance.gameObject);
        }

        enemies.Clear();
        UpdateEnemyPositions();
    }
}
