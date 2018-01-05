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
    private Stack<Card> deck;
    private bool isPlayerTurn = true;

    public void BeginEncounter(Action encounterCompleteAction)
    {
        CardHandController cardHandController = ControllerDatabase.Get<CardHandController>();
        deck = new Stack<Card>(ControllerDatabase.Get<PlayerController>().Deck.CloneShuffled());
        cardHandController.Clear();
        for (int i = 0; i < 5; i++)
        {
            cardHandController.AddCard(deck.Pop());
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

        // TODO: Actual logic for enemy turn
        if (Input.GetKeyDown(KeyCode.Space) && !isPlayerTurn)
        {
            isPlayerTurn = true;
            Debug.Log("enemy turn ended");
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

    public bool ExecuteCard(CardInstance card, EnemyInstance target)
    {
        if (!enemies.Contains(target) || !isPlayerTurn) return false;

        // If our cards attacks then we execute damage!
        if (card.Card.AttackPoints <= 0) return false;

        int damage = Mathf.Max(0, target.Enemy.CurrentHealthPoints - card.Card.AttackPoints);
        target.Enemy.CurrentHealthPoints = damage;

        isPlayerTurn = !isPlayerTurn;
        Debug.Log("player turn ended");
        return true;
    }
}
