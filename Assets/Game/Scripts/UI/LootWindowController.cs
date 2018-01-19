/*
 * Author: Shon Verch
 * File Name: LootWindowController.cs
 * Project Name: TheDungeonMaster
 * Creation Date: 01/14/2018
 * Modified Date: 01/18/2018
 * Description: The top-level UI manager for the loot window.
 */

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// The top-level UI manager for the loot window.
/// </summary>
public class LootWindowController : ControllerBehaviour
{
    /// <summary>
    /// Indicates whether this <see cref="LootWindowController"/> is currently open.
    /// </summary>
    public bool IsOpen => gameObject.activeInHierarchy;

    [SerializeField]
    private RectTransform lootCardListContentParent;
    [SerializeField]
    private GameObject lootCardListItemPrefab;

    [SerializeField]
    private RectTransform deckListBoxContentParent;
    [SerializeField]
    private GameObject deckListItemPrefab;

    /// <summary>
    /// Called when the component is created and placed into the world.
    /// </summary>
    private void Start()
    {
        gameObject.SetActive(false);
    }

    /// <summary>
    /// Shows this <see cref="LootWindowController"/> with the specified loot.
    /// </summary>
    public void Open(List<Card> loot)  
    {
        // Reset the deck list box so that there are no leftovers from the last time this window was opened.
        deckListBoxContentParent.DestroyChildren();

        // Add each card from our deck into the deck list box.
        foreach (Card card in ControllerDatabase.Get<PlayerController>().Deck)
        {
            GameObject cardItemGameObject = Instantiate(deckListItemPrefab);
            cardItemGameObject.transform.Find("Text").GetComponent<Text>().text = card.Name;
            cardItemGameObject.transform.SetParent(deckListBoxContentParent, false);
        }

        // Reset the loot card list so that there are no leftovers fom the last time this window was opened.
        lootCardListContentParent.DestroyChildren();

        // Add our loot to the loot list.
        foreach (Card card in loot)
        {
            GameObject lootCardItemGameObject = Instantiate(lootCardListItemPrefab);
            CardInstance lootCardInstance = lootCardItemGameObject.GetComponent<CardInstance>();
            lootCardInstance.Initialize(card);

            lootCardItemGameObject.GetComponent<Button>().onClick.AddListener(() => OnLootSelected(card));
            lootCardItemGameObject.transform.SetParent(lootCardListContentParent, false);
        }

        gameObject.SetActive(true);
    }

    /// <summary>
    /// Handle the logic for when a certain loot item is selected by the user.
    /// </summary>
    private void OnLootSelected(Card card)
    {
        gameObject.SetActive(false);
        ControllerDatabase.Get<PlayerController>().Deck.Add(card.Clone());

        Debug.Log($"selected {card.Name} as loot");
    }
}