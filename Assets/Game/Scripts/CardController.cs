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

         UpdateCardPositions();
    }

    private void UpdateCardPositions()
    {
        // The width-ratio representing the gap between each card.
        const float gapBetweenCardRatio = 1 / 2f;
        // The height-ratio representing the distance which the card may go below the screen.
        const float cardRatioBelowScreen = 0.15f;
        // The angle between the left-most and right-most cards.
        const float angleBetweenEdgeCards = 30f;
        const float rotationFromCentreCard = -angleBetweenEdgeCards / 2f;

        RectTransform cardPrefabRectTransform = Resources.Load<GameObject>("Prefabs/Card_Front").GetComponent<RectTransform>();
        float gapBetweenCards = cardPrefabRectTransform.rect.width * cardPrefabRectTransform.localScale.x * gapBetweenCardRatio;
        float positionFromCentreCard = (hand.Count - 1) * gapBetweenCards / 2f;
        float belowScreenOffset = cardPrefabRectTransform.rect.height * cardPrefabRectTransform.localScale.y * cardRatioBelowScreen;

        float rotationPerCard = angleBetweenEdgeCards / hand.Count;
        float cardVerticalOffset = rotationFromCentreCard + (hand.Count - 1) * rotationPerCard / 2f;

        for (int i = 0; i < hand.Count; i++)
        {
            RectTransform rectTransform = hand[i].GetComponent<RectTransform>();

            rectTransform.anchorMin = new Vector2(0.5f, 0);
            rectTransform.anchorMax = new Vector2(0.5f, 0);
            rectTransform.pivot = new Vector2(0.5f, 0);

            float rotation = -(rotationFromCentreCard + i * rotationPerCard - cardVerticalOffset);

            Vector2 position = new Vector2(i * gapBetweenCards - positionFromCentreCard, -belowScreenOffset - Mathf.Abs(rotation) * 0.01f);
            rectTransform.anchoredPosition = position;
            rectTransform.rotation = Quaternion.Euler(0, 0, rotation);
        }
    }
}
