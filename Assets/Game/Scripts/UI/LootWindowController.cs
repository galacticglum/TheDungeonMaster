/*
 * Author: Shon Verch
 * File Name: LootWindowController.cs
 * Project Name: TheDungeonMaster
 * Creation Date: 01/14/2018
 * Modified Date: 01/14/2018
 * Description: The top-level UI manager for the loot window.
 */

using UnityEngine;

/// <summary>
/// The top-level UI manager for the loot window.
/// </summary>
public class LootWindowController : ControllerBehaviour
{
    public bool IsOpen => gameObject.activeInHierarchy;

    [SerializeField]
    private ListBox deckListBox;

    private void Start()
    {
        gameObject.SetActive(false);
    }

    public void Open()
    {
        deckListBox.Clear();
        foreach (Card card in ControllerDatabase.Get<PlayerController>().Deck)
        {
            deckListBox.AddItem(card.Name);
        }

        gameObject.SetActive(true);
    }
}