/*
 * Author: Shon Verch
 * File Name: EncounterController.cs
 * Project Name: TheDungeonMaster
 * Creation Date: 01/02/2018
 * Modified Date: 01/05/2018
 * Description: The top-level manager for a combat session (battle). 
 */

using System;
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
    private Action onEncounterComplete;

    public void BeginEncounter(Action encounterCompleteAction)
    {
        ControllerDatabase.Get<CardHandController>().Clear();
        for (int i = 0; i < 5; i++)
        {
            ControllerDatabase.Get<CardHandController>().AddCard(new Card("Power Punch", "Deals <color=#D5AB5CFF><i>6</i></color> damage to an enemy. " +
                                                                                         "<color=#D5AB5CFF>Overcharge:</color> 3 cards.", 6));
        }

        ClearEnemies();
        AddEnemyToEncounter(new Enemy("Orc Grunt", "ME ORC, ME SMASH!", 12));
        AddEnemyToEncounter(new Enemy("Elessar", "The fantastic shetland sheepdog.", 18));
        ControllerDatabase.Get<PlayerController>().CanMove = false;

        onEncounterComplete = encounterCompleteAction;
    }

    private void Update()
    {
        if (enemies == null) return;
        for (int i = enemies.Count - 1; i >= 0; i--)
        {
            if (enemies[i].Enemy.CurrentHealthPoints <= 0)
            {
                RemoveEnemyFromEncounter(enemies[i]);
            }
        }

        // Encounter is complete
        if (enemies.Count != 0) return;
        ControllerDatabase.Get<PlayerController>().CanMove = true;
        onEncounterComplete();
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

    private void RemoveEnemyFromEncounter(EnemyInstance enemyInstance)
    {
        enemies.Remove(enemyInstance);
        UpdateEnemyPositions();

        Destroy(enemyInstance.gameObject);
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

    public void ExecuteCard(CardInstance card, EnemyInstance target)
    {
        if (!enemies.Contains(target)) return;

        // If our cards attacks then we execute damage!
        if (card.Card.AttackPoints <= 0) return;

        int damage = Mathf.Max(0, target.Enemy.CurrentHealthPoints - card.Card.AttackPoints);
        target.Enemy.CurrentHealthPoints = damage;
    }
}
