/*
 * Author: Shon Verch
 * File Name: CardDeck.cs
 * Project Name: TheDungeonMaster
 * Creation Date: 01/05/2018
 * Modified Date: 01/05/2018
 * Description: A collection of cards.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A collection of cards.
/// </summary>
public class CardDeck : IEnumerable<Card>
{
    private readonly List<Card> cards;

    /// <summary>
    /// Initializes a <see cref="CardDeck"/> with a collection of cards.
    /// </summary>
    public CardDeck(params Card[] cards)
    {
        this.cards = new List<Card>(cards);
    }

    public void Add(Card card) => cards.Add(card);
    public void Remove(Card card) => cards.Remove(card);

    /// <summary>
    /// Creates a copy of this <see cref="CardDeck"/> and shuffles it.
    /// </summary>
    /// <returns></returns>
    public IEnumerable<Card> CloneShuffled()
    {
        List<Card> deck = new List<Card>(this);
        for (int i = deck.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            Card temp = deck[j];
            deck[j] = cards[i];
            cards[i] = temp;
        }

        return deck;
    }

    /// <summary>
    /// Retrieve the <see cref="IEnumerator{T}"/> for this <see cref="CardDeck"/> which iterates over the stored <see cref="Card"/> collection.
    /// </summary>
    public IEnumerator<Card> GetEnumerator() => cards.GetEnumerator();

    /// <summary>
    /// Retrieve the <see cref="IEnumerator{T}"/> for this <see cref="CardDeck"/> which iterates over the stored <see cref="Card"/> collection.
    /// </summary>
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}