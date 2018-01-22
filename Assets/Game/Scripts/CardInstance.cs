/*
 * Author: Shon Verch
 * File Name: CardInstance.cs
 * Project Name: TheDungeonMaster
 * Creation Date: 12/26/2017
 * Modified Date: 12/27/2017
 * Description: The interface between the card data and the card visuals.
 */

using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <inheritdoc cref="MonoBehaviour" />
/// <summary>
/// The interface between the card data and the card visuals.
/// </summary>
[RequireComponent(typeof(CanvasGroup))]
[RequireComponent(typeof(RectTransform))]
public class CardInstance : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    /// <summary>
    /// The card data which this <see cref="CardInstance"/> is initialized from.
    /// </summary>
    public Card Card { get; private set; }

    /// <summary>
    /// The <see cref="CanvasGroup"/> belonging to this <see cref="CardInstance"/>.
    /// </summary>
    public CanvasGroup CanvasGroup => canvasGroup ?? (canvasGroup = GetComponent<CanvasGroup>());

    /// <summary>
    /// The <see cref="RectTransform"/> of this <see cref="CardInstance"/>.
    /// </summary>
    public RectTransform RectTransform => rectTransform ?? (rectTransform = GetComponent<RectTransform>());

    [Tooltip("Determines whether this card instance responds to even if it can block raycasts.")]
    [SerializeField]
    private bool respondToEvents = true;

    [Tooltip("The text component which renders the card name.")]
    [SerializeField]
    private Text cardNameText;

    [Tooltip("The text component which renders the card description.")]
    [SerializeField]
    private Text cardDescriptionText;

    [Tooltip("The text component which renders the card attack points.")]
    [SerializeField]
    private Text cardAttackPointsText;

    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;

    /// <summary>
    /// Initializes this <see cref="CardInstance"/> from a <see cref="Card"/>.
    /// </summary>
    /// <param name="card">The <see cref="Card"/> data to initialize from.</param>
    public void Initialize(Card card)
    {
        // Let's bail if our card is null
        if (card == null) return;

        Card = card;
        gameObject.name = $"{card.Name}_instance";
        cardNameText.text = card.Name;
        cardDescriptionText.text = card.Description;
        cardAttackPointsText.text = card.AttackPoints.ToString();
    }

    /// <summary>
    /// Handles logic for when this <see cref="Card"/> is played.
    /// </summary>
    /// <param name="targetGameObject">The <see cref="GameObject"/> this card was played on. This doesn't have to be an enemy.</param>
    /// <returns>A boolean indicating whether this card was successfully played.</returns>
    public bool HandleCardPlayed(GameObject targetGameObject)
    {
        EncounterController encounterController = ControllerDatabase.Get<EncounterController>();

        // Make sure that it is the player's turn. If it is not, then we cannot play any cards at this time!
        if (!encounterController.IsPlayerTurn) return false;

        EnemyInstance targetEnemyInstance = targetGameObject.GetComponent<EnemyInstance>();

        // If we haven't targeted a specific enemy yet we require that we target an enemy 
        // then we bail as we haven't met the critera for this card to be played.
        if (targetEnemyInstance == null && Card.RequiresEnemyTarget) return false;

        // If we have targeted the container gameobject for all enemies (but not a specific enemy)
        // yet we require that we target an enemy, then we bail as we haven't met the critera for 
        // this card to be played.
        if (targetGameObject.name != "Enemy_Spawn_Root" && !Card.RequiresEnemyTarget) return false;

        switch (Card.DamageType)
        {
            case CardDamageType.EnemyTarget:
                // At this point, our target enemy instance should never be null as we should
                // have checked for that case above but it's still good to check anyways.
                if (targetEnemyInstance == null) break;

                // Attack the targeted enemy.
                targetEnemyInstance.TakeDamage(Card.AttackPoints);
                break;
            case CardDamageType.AreaOfEffect:
                foreach (EnemyInstance enemyInstance in encounterController.EnemiesOnBoard)
                {
                    enemyInstance.TakeDamage(Card.AttackPoints);
                }
                break;
        }

        if (targetEnemyInstance != null)
        {
            targetEnemyInstance.PoisonDamage = Card.AttackPoints;
            targetEnemyInstance.Effects.AddEffect(EffectType.Poison, Card.PoisonPoints);
            targetEnemyInstance.Effects.AddEffect(EffectType.Stun, Card.StunPoints);
        }

        encounterController.PlayerEffects.AddEffect(EffectType.Shield, Card.ShieldPoints);
        CardHandController cardHandController = ControllerDatabase.Get<CardHandController>();

        // Handle the resurrection.
        // For each card to resurrect, we get the last discarded card and add it
        // back to the hand.
        for (int i = 0; i < Card.ResurrectionAmount; i++)
        {
            // GetLastDiscardCard() can return null but this is fine since
            // AddCard() makes sure that the card passed to it is NOT null.
            Card card = encounterController.GetLastDiscardedCard();
            cardHandController.AddCard(card);
        }

        // Let's make sure that we don't overcharge more than the amount of cards that we have in our hand.
        int actualOverchargeAmount = Mathf.Min(Card.OverchargeAmount, cardHandController.HandCount);
        for (int i = 0; i < actualOverchargeAmount; i++)
        {
            CardInstance removedCardInstance = cardHandController.RemoveRandomCard();
            encounterController.AddCardToDiscardPile(removedCardInstance.Card);
            Destroy(removedCardInstance.gameObject);
        }
        
        encounterController.DamagePlayer(Card.HealthCost, null);
        encounterController.HealPlayer(Card.HealPoints);

        encounterController.AddCardToDiscardPile(Card);
        encounterController.EndPlayerTurn();
        return true;
    }

    /// <summary>
    /// Creates a <see cref="CardInstance"/> from a <see cref="global::Card"/>.
    /// </summary>
    /// <param name="card">The <see cref="global::Card"/> to create the <see cref="CardInstance"/> from.</param>
    /// <param name="parent">The parent of this <see cref="CardInstance"/>.</param>
    /// <returns>The card instance's <see cref="GameObject"/>.</returns>
    public static CardInstance Create(Card card, RectTransform parent)
    {
        GameObject cardGameObject = Instantiate(Resources.Load<GameObject>("Prefabs/Card_Front"));
        cardGameObject.transform.SetParent(parent, false);

        CardInstance cardInstance = cardGameObject.GetComponent<CardInstance>();
        cardInstance.Initialize(card);

        return cardInstance;
    }

    /// <summary>
    /// Executed when the pointer is over the <see cref="T:UnityEngine.GameObject" /> pertaining to this <see cref="T:UnityEngine.MonoBehaviour" />.
    /// </summary>
    /// <param name="eventData">The data pertaining to this event.</param>
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!respondToEvents) return;
        ControllerDatabase.Get<CardInteractionController>().BeginHover(this);
    }

    /// <summary>
    /// Executed when the pointer leaves the <see cref="T:UnityEngine.GameObject" /> pertaining to this <see cref="T:UnityEngine.MonoBehaviour" />.
    /// </summary>
    /// <param name="eventData">The data pertaining to this event.</param>
    public void OnPointerExit(PointerEventData eventData)
    {
        if (!respondToEvents) return;
        ControllerDatabase.Get<CardInteractionController>().EndHover(this);
    }

    /// <summary>
    /// Executed when the pointer is down on the <see cref="T:UnityEngine.GameObject" /> pertaining to this <see cref="T:UnityEngine.MonoBehaviour" />.
    /// </summary>
    /// <param name="eventData">The data pertaining to this event.</param>
    public void OnPointerDown(PointerEventData eventData)
    {
        if (!respondToEvents) return;
        ControllerDatabase.Get<CardInteractionController>().BeginDrag(this);
    }
}
