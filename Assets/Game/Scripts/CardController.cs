/*
 * Author: Shon Verch
 * File Name: CardController.cs
 * Project Name: The Dungeon Master
 * Creation Date: 12/25/2017
 * Modified Date: 12/26/2017
 * Description: Handles all the cards in the game.
 */

using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles all the cards in the game.
/// </summary>
public class CardController : MonoBehaviour
{
    private List<CardInstance> hand;

    private void Start()
    {
        hand = new List<CardInstance>();

        Card cardPrototype = new Card("Brutish Sheltie", "Bork bork RURU. Does 15 sound damage to all nearby targets.");

        hand.Add(CardInstance.Create(cardPrototype));
        hand.Add(CardInstance.Create(cardPrototype));
        hand.Add(CardInstance.Create(cardPrototype));
        hand.Add(CardInstance.Create(cardPrototype));
        hand.Add(CardInstance.Create(cardPrototype));
        hand.Add(CardInstance.Create(cardPrototype));
        hand.Add(CardInstance.Create(cardPrototype));
        hand.Add(CardInstance.Create(cardPrototype));
        hand.Add(CardInstance.Create(cardPrototype));
        hand.Add(CardInstance.Create(cardPrototype));

        UpdateCardPositions();
    }

    private void UpdateCardPositions()
    {
        // The highest amount of cards we account for when arranging for display.
        const int maxHandSize = 10;
        // Radius of the circle arc upon which the cards are placed, if we have 1 card.
        const float minCircleRadius = 300f;
        // Radius of the circle arc upon which the cards are placed, if we have maxHandSize cards.
        const float maxCircleRadius = 1250f;
        // The width-ratio representing the gap between each card, if we have 1 card.
        const float minGapBetweenCardRatio = 1 / 2f;
        // The width-ratio representing the gap between each card, if we have maxHandSize cards.
        const float maxGapBetweenCardRatio = 1 / 3f;
        // The height-ratio representing the distance which the card may go below the screen, if we have 1 card.
        const float minCardRatioBelowScreen = 0.15f;
        // The height-ratio representing the distance which the card may go below the screen, if we have maxHandSize cards.
        const float maxCardRatioBelowScreen = 0.04f;

        // Lerp factor based on how many cards currently are in the hand, compared to our max hand size (offset by 1 since we only care about starting at 1).
        float lerpFactor = (float)(hand.Count - 1) / (maxHandSize - 1);
        // Radius of the circle arc upon which the cards are placed.
        float circleRadius = Mathf.Lerp(minCircleRadius, maxCircleRadius, lerpFactor);
        // The width-ratio representing the gap between each card.
        float gapBetweenCardRatio = Mathf.Lerp(minGapBetweenCardRatio, maxGapBetweenCardRatio, lerpFactor);
        // The height-ratio representing the distance which the card may go below the screen.
        float cardRatioBelowScreen = Mathf.Lerp(minCardRatioBelowScreen, maxCardRatioBelowScreen, lerpFactor);

        RectTransform cardPrefabRectTransform = Resources.Load<GameObject>("Prefabs/Card_Front").GetComponent<RectTransform>();
        float gapBetweenCards = cardPrefabRectTransform.rect.width * cardPrefabRectTransform.localScale.x * gapBetweenCardRatio;
        float belowScreenOffset = cardPrefabRectTransform.rect.height * cardPrefabRectTransform.localScale.y * cardRatioBelowScreen;

        Vector2 circleCentre = new Vector2(0, -(circleRadius + belowScreenOffset));
        float distanceBetweenEdgeCards = gapBetweenCards * (hand.Count - 1);
        float arcAngle = 2 * Mathf.Asin(distanceBetweenEdgeCards / (2 * circleRadius));

        for (int i = 0; i < hand.Count; i++)
        {
            RectTransform rectTransform = hand[i].GetComponent<RectTransform>();

            rectTransform.anchorMin = new Vector2(0.5f, 0);
            rectTransform.anchorMax = new Vector2(0.5f, 0);
            rectTransform.pivot = new Vector2(0.5f, 0);

            float rotation = hand.Count > 1 ? arcAngle / 2 - i * (arcAngle / (hand.Count - 1)) : 0f;

            Vector2 position = new Vector2(Mathf.Sin(-rotation), Mathf.Cos(rotation)) * circleRadius + circleCentre;
            rectTransform.anchoredPosition = position;
            rectTransform.rotation = Quaternion.Euler(0, 0, rotation * Mathf.Rad2Deg);
        }
    }
}
