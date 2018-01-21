/*
 * Author: Shon Verch
 * File Name: CardInstance.cs
 * Project Name: TheDungeonMaster
 * Creation Date: 12/26/2017
 * Modified Date: 12/27/2017
 * Description: The interface between the card data and the card visuals.
 */

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
        if (card == null)
        {
            Debug.Log("CARD IS NULL!");
            return;
        }

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

        // If we haven't targeted a specific enemy yet we require that we target an, enemy 
        // then we bail as we haven't met the critera for this card to be played.
        if (targetEnemyInstance == null && Card.RequiresEnemyTarget) return false;

        // If we have targeted the container gameobject for all enemies (but not a specific enemy)
        // yet we require that we target an enemy, then we bail as we haven't met the critera for 
        // this card to be played.
        if (targetGameObject.name != "Enemy_Spawn_Root" && !Card.RequiresEnemyTarget) return false;

        // If we have targeted an enemy, then let's execute any target-specific logic.
        if (targetEnemyInstance)
        {
            // Attack the targeted enemy.
            targetEnemyInstance.TakeDamage(Card.AttackPoints);
        }

        CardHandController cardHandController = ControllerDatabase.Get<CardHandController>();

        Debug.Log($"Rez. {Card.ResurrectionAmount}");
        for (int i = 0; i < Card.ResurrectionAmount; i++)
        {
            Card card = encounterController.GetLastDiscardedCard();
            cardHandController.AddCard(card);
        }

        Debug.Log($"Overcharge: {Card.OverchargeAmount}");

        // Let's make sure that we don't overcharge more than the amount of cards that we have in our hand.
        int actualOverchargeAmount = Mathf.Min(Card.OverchargeAmount, cardHandController.HandCount);
        for (int i = 0; i < actualOverchargeAmount; i++)
        {
            CardInstance removedCardInstance = cardHandController.RemoveRandomCard();
            encounterController.AddCardToDiscardPile(removedCardInstance.Card);
            Destroy(removedCardInstance.gameObject);
        }

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
