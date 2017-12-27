/*
 * Author: Shon Verch
 * File Name: CardInstance.cs
 * Project Name: TheDungeonMaster
 * Creation Date: 12/26/2017
 * Modified Date: 12/26/2017
 * Description: The interface between the card data and the card visuals.
 */

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// The interface between the card data and the card visuals.
/// </summary>
[RequireComponent(typeof(CanvasGroup))]
public class CardInstance : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    /// <summary>
    /// The card data which this <see cref="CardInstance"/> is initialized from.
    /// </summary>
    public Card Card { get; private set; }

    [SerializeField]
    private Text cardNameText;
    [SerializeField]
    private Text cardDescriptionText;

    private CanvasGroup canvasGroup;

    /// <summary>
    /// Initializes this <see cref="CardInstance"/> from a <see cref="Card"/>.
    /// </summary>
    /// <param name="card">The <see cref="Card"/> data to initialize from.</param>
    private void Initialize(Card card)
    {
        Card = card;

        gameObject.name = $"{card.Name}_instance";
        cardNameText.text = card.Name;
        cardDescriptionText.text = card.Description;

        canvasGroup = GetComponent<CanvasGroup>();
    }

    /// <summary>
    /// Creates a <see cref="CardInstance"/> from a <see cref="Card"/>.
    /// </summary>
    /// <param name="card">The <see cref="Card"/> to create the <see cref="CardInstance"/> from.</param>
    /// <returns>The card instance's <see cref="GameObject"/>.</returns>
    public static CardInstance Create(Card card)
    {
        GameObject cardGameObject = Instantiate(Resources.Load<GameObject>("Prefabs/Card_Front"));
        cardGameObject.transform.SetParent(UIManager.Instance.Canvas.transform, false);

        CardInstance cardInstance = cardGameObject.GetComponent<CardInstance>();
        cardInstance.Initialize(card);

        return cardInstance;
    }

    /// <summary>
    /// Executed when the pointer is over the <see cref="GameObject"/> pertaining to this <see cref="MonoBehaviour"/>.
    /// </summary>
    /// <param name="eventData">The data pertaining to this event.</param>
    public void OnPointerEnter(PointerEventData eventData)
    {
        canvasGroup.SetVisibility(false);
    }

    /// <summary>
    /// Executed when the pointer leaves the <see cref="GameObject"/> pertaining to this <see cref="MonoBehaviour"/>.
    /// </summary>
    /// <param name="eventData">The data pertaining to this event.</param>
    public void OnPointerExit(PointerEventData eventData)
    {
        canvasGroup.SetVisibility(true);
    }
}
