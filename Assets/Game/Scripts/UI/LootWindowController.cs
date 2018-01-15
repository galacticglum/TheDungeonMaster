/*
 * Author: Shon Verch
 * File Name: LootWindowController.cs
 * Project Name: TheDungeonMaster
 * Creation Date: 01/14/2018
 * Modified Date: 01/14/2018
 * Description: The top-level UI manager for the loot window.
 */

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// The top-level UI manager for the loot window.
/// </summary>
public class LootWindowController : ControllerBehaviour
{
    public bool IsOpen => gameObject.activeInHierarchy;

    [SerializeField]
    private RectTransform lootCardListContentParent;
    [SerializeField]
    private GameObject lootCardListItemPrefab;

    [SerializeField]
    private RectTransform deckListBoxContentParent;
    [SerializeField]
    private GameObject deckListItemPrefab;

    private void Start()
    {
        gameObject.SetActive(false);
    }

    public void Open(List<Card> loot)  
    {
        deckListBoxContentParent.DestroyChildren();
        foreach (Card card in ControllerDatabase.Get<PlayerController>().Deck)
        {
            GameObject cardItemGameObject = Instantiate(deckListItemPrefab);
            cardItemGameObject.transform.Find("Text").GetComponent<Text>().text = card.Name;
            cardItemGameObject.transform.SetParent(deckListBoxContentParent, false);
        }

        lootCardListContentParent.DestroyChildren();
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

    private void OnLootSelected(Card card)
    {
        gameObject.SetActive(false);
        ControllerDatabase.Get<PlayerController>().Deck.Add(card);
        Debug.Log($"selected {card.Name} as loot");
    }
}