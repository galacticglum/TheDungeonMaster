/*
 * Author: Shon Verch
 * File Name: CardHandController.cs
 * Project Name: TheDungeonMaster
 * Creation Date: 12/25/2017
 * Modified Date: 1/15/2018
 * Description: Manages the players hand.
 */

using System.Collections.Generic;
using UnityEngine;

/// <inheritdoc />
/// <summary>
/// Manages the players hand.
/// </summary>
public class CardHandController : ControllerBehaviour
{
    public const int HandLimit = 5;

    public CanvasGroup CanvasGroup => handCanvasGroup;

    /// <summary>
    /// The amount of cards in the hand.
    /// </summary>
    public int HandCount => hand.Count;

    /// <summary>
    /// The parent transform for all cards in the hand. When a card is added to the hand, it's <see cref="GameObject"/> is parented to this <see cref="Transform"/>.
    /// </summary>
    [SerializeField]
    private RectTransform handParentTransform;
    [SerializeField]
    private CanvasGroup handCanvasGroup;

    private List<CardInstance> hand;

    /// <summary>
    /// Called when the component is created and placed into the world.
    /// </summary>
    private void Start()
    {
        hand = new List<CardInstance>();
    }

    /// <summary>
    /// Re-organize the visual represenation of the hand.
    /// </summary>
    private void UpdateCardPositions()
    {
        // The highest amount of cards we account for when arranging for display.
        const int handSizeCutoff = 10;
        // Radius of the circle arc upon which the cards are placed, if we have 1 card.
        const float minimumCircleRadius = 300f;
        // Radius of the circle arc upon which the cards are placed, if we have reached the maximum size of a hand (the cutoff).
        const float maximumCircleRadius = 1250f;
        // The width-ratio representing the gap between each card, if we have 1 card.
        const float minimumGapBetweenCardRatio = 1 / 2f;
        // The width-ratio representing the gap between each card, if we have reached the maximum size of a hand (the cutoff).
        const float maximumGapBetweenCardRatio = 1 / 3f;
        // The height-ratio representing the distance which the card may go below the screen, if we have 1 card.
        const float minimumCardRatioBelowScreen = 0.15f;
        // The height-ratio representing the distance which the card may go below the screen, if we have reached the maximum size of a hand (the cutoff).
        const float maximumCardRatioBelowScreen = 0.04f;

        // The ratio of current cards to maximum cards based on how many cards currently are in the hand, 
        // compared to our max hand size (offset by 1 since we only care about starting at 1).
        float cardRatio = (float) (hand.Count - 1) / (handSizeCutoff - 1);
        // Radius of the circle arc upon which the cards are placed.
        float circleRadius = Mathf.Lerp(minimumCircleRadius, maximumCircleRadius, cardRatio);
        // The width-ratio representing the gap between each card.
        float gapBetweenCardRatio = Mathf.Lerp(minimumGapBetweenCardRatio, maximumGapBetweenCardRatio, cardRatio);
        // The height-ratio representing the distance which the card may go below the screen.
        float cardRatioBelowScreen = Mathf.Lerp(minimumCardRatioBelowScreen, maximumCardRatioBelowScreen, cardRatio);

        RectTransform cardPrefabRectTransform = Resources.Load<GameObject>("Prefabs/Card_Front")
            .GetComponent<RectTransform>();

        // Retrieve the pixel-size of the various offsets via their respective ratio, multiplied by the card scale.
        float gapBetweenCards = cardPrefabRectTransform.rect.width * cardPrefabRectTransform.localScale.x *
                                gapBetweenCardRatio;
        float belowScreenOffset = cardPrefabRectTransform.rect.height * cardPrefabRectTransform.localScale.y *
                                  cardRatioBelowScreen;

        Vector2 centreOfCircle = new Vector2(0, -(circleRadius + belowScreenOffset));
        float distanceBetweenEdgeCards = gapBetweenCards * (hand.Count - 1);
        float arcAngle = 2 * Mathf.Asin(distanceBetweenEdgeCards / (2 * circleRadius));

        for (int i = 0; i < hand.Count; i++)
        {
            CardInstance cardInstance = hand[i];

            // Set the anchor and pivot of the card to the bottom centre.
            cardInstance.RectTransform.anchorMin = new Vector2(0.5f, 0);
            cardInstance.RectTransform.anchorMax = new Vector2(0.5f, 0);
            cardInstance.RectTransform.pivot = new Vector2(0.5f, 0);

            float rotation = hand.Count > 1 ? arcAngle / 2 - i * (arcAngle / (hand.Count - 1)) : 0f;

            Vector2 position = new Vector2(Mathf.Sin(-rotation), Mathf.Cos(rotation)) * circleRadius + centreOfCircle;
            cardInstance.RectTransform.anchoredPosition = position;
            cardInstance.RectTransform.rotation = Quaternion.Euler(0, 0, rotation * Mathf.Rad2Deg);
        }

        OrderCardsInHierarchy();
    }

    /// <summary>
    /// Orders the cards in the hierarchy based on their position in the hand.
    /// </summary>
    private void OrderCardsInHierarchy()
    {
        for (int i = 0; i < hand.Count; i++)
        {
            hand[i].transform.SetSiblingIndex(i);
        }
    }

    /// <summary>
    /// Adds a card into the hand.
    /// </summary>
    /// <param name="card">The <see cref="Card"/> to add to the hand.</param>
    public CardInstance AddCard(Card card) => card == null ? null : AddCard(card, hand.Count);

    /// <summary>
    /// Adds a card into the hand at a specified index.
    /// </summary>
    /// <param name="card">The <see cref="Card"/> to add to the hand.</param>
    /// <param name="index">The index at which to insert the card</param>
    public CardInstance AddCard(Card card, int index)
    {
        if (HandCount >= HandLimit || card == null) return null;
        CardInstance cardInstance = CardInstance.Create(card, handParentTransform);

        hand.Insert(index, cardInstance);
        UpdateCardPositions();

        return cardInstance;
    }

    /// <summary>
    /// Removes a <see cref="Card"/> from the hand.
    /// </summary>
    /// <param name="cardInstance">The <see cref="CardInstance"/> to remove from the hand.</param>
    public bool RemoveCard(CardInstance cardInstance)
    {
        bool result = hand.Remove(cardInstance);
        UpdateCardPositions();

        return result;
    }

    /// <summary>
    /// Removes a random <see cref="Card"/> from thhe hand.
    /// </summary>
    public CardInstance RemoveRandomCard()
    {
        int index = Random.Range(0, hand.Count);
        CardInstance cardInstance = hand[index];
        RemoveCard(cardInstance);

        return cardInstance;
    }

    /// <summary>
    /// Retrieves the position of a card in the hand.
    /// </summary>
    /// <param name="cardInstance">The card to get the index of.</param>
    public int GetIndexOfCard(CardInstance cardInstance) => hand.IndexOf(cardInstance);

    /// <summary>
    /// Clears the hand.
    /// </summary>
    public void Clear()
    {
        foreach (CardInstance cardInstance in hand)
        {
            Destroy(cardInstance.gameObject);
        }

        hand.Clear();
        UpdateCardPositions();
    }
}
