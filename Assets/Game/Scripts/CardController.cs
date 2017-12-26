/*
 * Author: Shon Verch
 * File Name: CardController.cs
 * Project Name: The Dungeon Master
 * Creation Date: 12/25/2017
 * Modified Date: 12/26/2017
 * Description: Handles all the cards in the game.
 */

using UnityEngine;

/// <summary>
/// Handles all the cards in the game.
/// </summary>
public class CardController : MonoBehaviour
{
    private void Start()
    {
        Card cardPrototype = new Card("Ole Smasher", "Pleb description; lorem ipsum, is this Latin? NO IT'S NOT!");
        CardInstance.Create(cardPrototype);
    }
}
