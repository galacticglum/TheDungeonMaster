/*
 * Author: Shon Verch
 * File Name: EncounterController.cs
 * Project Name: TheDungeonMaster
 * Creation Date: 01/02/2018
 * Modified Date: 01/21/2018
 * Description: The top-level manager for a combat session (battle). 
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

/// <summary>
/// The top-level manager for a combat session (battle). 
/// </summary>
public class EncounterController : ControllerBehaviour
{
    /// <summary>
    /// Indicates whether the current turn belongs to the player.
    /// </summary>
    public bool IsPlayerTurn { get; private set; }

    /// <summary>
    /// Indicates whether the player is currently in an encounter.
    /// </summary>
    public bool IsPlayerInsideEncounter => gameObject.activeInHierarchy;

    /// <summary>
    /// All the enemies on the board.
    /// </summary>
    public ReadOnlyCollection<EnemyInstance> EnemiesOnBoard => new ReadOnlyCollection<EnemyInstance>(enemies);

    /// <summary>
    /// The maximum (and starting) health of the player.
    /// </summary>
    private const int MaximumPlayerHealth = 15;

    [SerializeField]
    private RectTransform enemyInstanceParent;

    private Queue<Func<bool>> animationQueue;
    private Func<bool> currentAnimation;
    private bool isAnimating;

    private List<EnemyInstance> enemies;
    private Stack<Card> deck;
    private Stack<Card> discardPile;

    private Action onEncounterComplete;
    private int playerHealth;

    private CardHandController cardHandController;

    /// <summary>
    /// Called when the component is created and placed into the world.
    /// </summary>
    private void Start()
    {
        cardHandController = ControllerDatabase.Get<CardHandController>();
        gameObject.SetActive(false);
    }

    /// <summary>
    /// Starts an encounter.
    /// </summary>
    public void BeginEncounter(Action encounterCompleteAction)
    {
        // Reset any gameplay data.
        playerHealth = MaximumPlayerHealth;
        IsPlayerTurn = true;
        discardPile = new Stack<Card>();
        animationQueue = new Queue<Func<bool>>();

        cardHandController.Clear();
        cardHandController.CanvasGroup.SetVisibility(true);

        deck = new Stack<Card>(ControllerDatabase.Get<PlayerController>().Deck.CloneShuffled());
        for (int i = 0; i < CardHandController.HandLimit; i++)
        {
            cardHandController.AddCard(deck.Pop());
        }

        enemyInstanceParent.gameObject.SetActive(true);

        ClearEnemies();

        AddEnemyToEncounter(new Enemy("Orc Grunt", "ME ORC, ME SMASH!", 6, 3));
        AddEnemyToEncounter(new Enemy("Elessar", "The fantastic shetland sheepdog.", 12, 5));

        // We don't want to allow our player to move whilst they are in an encounter, therefore we lock them!
        ControllerDatabase.Get<PlayerController>().CanMove = false;

        onEncounterComplete = encounterCompleteAction;
        gameObject.SetActive(true);
    }

    /// <summary>
    /// Called every frame.
    /// </summary>
    private void Update()
    {
        // We want to stop any update logic from running when we are animating. 
        // Effectively, we halt the encounter. If we are still animating, let's
        // make sure to update the isAnimating variable and bail from here.
        if (HandleAnimation())
        {
            isAnimating = true;
            return;
        }

        // Reset the isAnimating variable as we are no longer animating.
        isAnimating = false;
        
        // If we have NO (zero) enemies in the encounter, that means that we are done!
        // Let's check too see if our encounter is complete, if it is we will run any
        // completion logic; otherwise, let's bail!
        if (enemies.Count != 0) return;
        HandleEncounterCompleted();
    }

    /// <summary>
    /// Handle animation logic.
    /// </summary>
    private bool HandleAnimation()
    {
        // If we are currently not animating anything but there are
        // still elements in the queue, then let's take an element out
        // of the queue and animate it.
        if (currentAnimation == null && animationQueue.Count > 0)
        {
            currentAnimation = animationQueue.Dequeue();
        }

        // If we aren't animating then return false.
        if (currentAnimation == null) return false;

        // If our animation callback is still animating; that is, it returns true. 
        // Then let's return true as well so we can continue animating the next frame.
        if (currentAnimation()) return true;

        currentAnimation = null;

        // At this point, we should continue animating if we still have anything left in the queue.
        // So let's return whether the count is greater than 0. If it is, then the next frame
        // this method will be called again; otherwise, the update logic will execute.
        return animationQueue.Count > 0;
    }

    /// <summary>
    /// Handle the logic for an enemy turn.
    /// </summary>
    private IEnumerator HandleEnemyTurn()
    {
        if (enemies != null)
        {
            // Wait until all enemies which are dead have actually been destroyed.
            bool waitForEnemy = true;
            while (waitForEnemy)
            {
                waitForEnemy = false;
                foreach (EnemyInstance enemy in enemies)
                {
                    if (enemy.Enemy.CurrentHealthPoints <= 0)
                    {
                        waitForEnemy = true;
                    }
                }

                yield return null;
            }

            // Iterate through the enemy list and execute each turn.
            // During the loop, the enemies may die and remove themselves from the list. 
            // Therefore, we loop backwards so accessing the collection is not modified
            // (as items are removed behind us—not infront).
            for (int i = enemies.Count - 1; i >= 0; i--)
            {
                // Wait half-a-second before executing a new enemies turn.
                yield return new WaitForSeconds(0.5f);
                enemies[i].ExecuteTurn();

                // We want to wait until animation is done so that
                // enemies animation one-by-one instead of simultaneously.
                while (isAnimating)
                {
                    yield return null;
                }
            }
        }

        // At this point, our enemy turn is finished and we can start the player turn.
        StartPlayerTurn();
    }

    /// <summary>
    /// Start a new player turn.
    /// </summary>
    private void StartPlayerTurn()
    {
        IsPlayerTurn = true;

        // If we have available space in the hand, then let's draw a new card.
        if (cardHandController.HandCount < CardHandController.HandLimit && deck.Count > 0)
        {
            cardHandController.AddCard(deck.Pop());
        }

        Debug.Log("enemy turn ended!");
    }

    /// <summary>
    /// Handle the logic for when an encounter is complete.
    /// </summary>
    private void HandleEncounterCompleted()
    {
        enemyInstanceParent.gameObject.SetActive(false);
        ControllerDatabase.Get<CardHandController>().CanvasGroup.SetVisibility(false);
        ControllerDatabase.Get<PlayerController>().CanMove = true;
        onEncounterComplete();

        gameObject.SetActive(false);
    }

    /// <summary>
    /// End the player turn.
    /// </summary>
    public void EndPlayerTurn()
    {
        IsPlayerTurn = false;
        StartCoroutine(nameof(HandleEnemyTurn));
    }

    /// <summary>
    /// Updates the spacing between enemies.
    /// </summary>
    private void UpdateEnemyPositions()
    {
        // The width-ratio representing the gap between each enemy.
        const float gapRatio = 1.2f;

        // The rect transform of the prefab. This is used for size calculation.
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

    /// <summary>
    /// Adds an enemy to the current encounter.
    /// </summary>
    private void AddEnemyToEncounter(Enemy enemy)
    {
        EnemyInstance enemyInstance = EnemyInstance.Create(enemy, enemyInstanceParent);
        enemies.Add(enemyInstance);
        UpdateEnemyPositions();
    }

    /// <summary>
    /// Removes an enemy from the current encounter and destroys it's visual representation.
    /// </summary>
    public void RemoveEnemyFromEncounter(EnemyInstance enemyInstance)
    {
        enemies.Remove(enemyInstance);
        UpdateEnemyPositions();

        Destroy(enemyInstance.gameObject);
    }

    /// <summary>
    /// Removes all enemies in the current encounter.
    /// </summary>
    private void ClearEnemies()
    {
        if (enemies == null)
        {
            enemies = new List<EnemyInstance>();
            return;
        }

        for (int i = enemies.Count - 1; i >= 0; i--)
        {
            EnemyInstance enemyInstance = enemies[i];

            Destroy(enemyInstance.gameObject);
            enemies.Remove(enemyInstance);
        }

        UpdateEnemyPositions();
    }

    /// <summary>
    /// Damage the player.
    /// </summary>
    public void DamagePlayer(int amount, Enemy enemy)
    {
        playerHealth -= amount;
        Debug.Log($"{enemy.Name} attacked the player for {amount} attack points! Current health: {playerHealth}");
    }

    /// <summary>
    /// Add an animation to the animation queue.
    /// </summary>
    /// <param name="animationAction"></param>
    public void EnqueueAnimation(Func<bool> animationAction)
    {
        isAnimating = true;
        animationQueue.Enqueue(animationAction);
    }

    /// <summary>
    /// Adds a card to the discard pile.
    /// </summary>
    public void AddCardToDiscardPile(Card card) => discardPile.Push(card);

    /// <summary>
    /// Gets the last card which was discarded. 
    /// This can return null if the discard pile is empty.
    /// </summary>
    public Card GetLastDiscardedCard() => discardPile.Count > 0 ? discardPile.Pop() : null;
}
