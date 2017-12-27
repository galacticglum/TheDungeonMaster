/*
 * Author: Shon Verch
 * File Name: CardInteractionController.cs
 * Project Name: TheDungeonMaster
 * Creation Date: 12/26/2017
 * Modified Date: 12/27/2017
 * Description: Manages all the interaction between cards and the user.
 */

using UnityEngine;

/// <summary>
/// Manages all the interaction between cards and the user.
/// </summary>
public class CardInteractionController : MonoBehaviour
{
    /// <summary>
    /// The active instance of the <see cref="CardInteractionController"/>.
    /// </summary>
    public static CardInteractionController Instance { get; private set; }

    [SerializeField]
    private CardInstance cardPopupInstance;
    [SerializeField]
    private Transform dragCardParent;

    private CardInstance currentDraggingCardInstance;

    /// <summary>
    /// Called when the component is created and placed into the world.
    /// </summary>
    private void Start()
    {
        Instance = this;
        cardPopupInstance.CanvasGroup.SetVisibility(false);
    }

    /// <summary>
    /// Called every frame.
    /// </summary>
    private void Update()
    {
        if (currentDraggingCardInstance == null) return;

        // Make the card follow the mouse position.
        Canvas canvas = UIManager.Instance.Canvas;
        Vector2 mousePosition;

        // Convert from screen-coordinates to UI-space coordinates.
        RectTransformUtility.ScreenPointToLocalPointInRectangle((RectTransform)canvas.transform, Input.mousePosition, canvas.worldCamera, out mousePosition);

        // Centre the card position on the Y-axis.
        mousePosition.y -= currentDraggingCardInstance.RectTransform.rect.height / 2 * currentDraggingCardInstance.RectTransform.localScale.y;
        currentDraggingCardInstance.transform.position = canvas.transform.TransformPoint(mousePosition);
    }

    /// <summary>
    /// Magnifies the hovered card.
    /// </summary>
    /// <param name="cardInstance">The <see cref="CardInstance"/> to magnify (popup).</param>
    public void BeginHover(CardInstance cardInstance)
    {
        if (cardInstance == null || currentDraggingCardInstance != null) return;

        cardInstance.CanvasGroup.SetVisibility(false);

        cardPopupInstance.Initialize(cardInstance.Card);
        cardPopupInstance.CanvasGroup.SetVisibility(true);

        Vector2 position = cardInstance.RectTransform.anchoredPosition;
        position.y = 0.05f * cardInstance.RectTransform.rect.height * cardInstance.RectTransform.localScale.y;

        cardPopupInstance.RectTransform.anchoredPosition = position;
    }

    /// <summary>
    /// Returns the hovered card back to it's normal state within the hand.
    /// </summary>
    /// <param name="cardInstance">The <see cref="CardInstance"/> to operate on.</param>
    public void EndHover(CardInstance cardInstance)
    {
        if (cardInstance == null) return;

        cardInstance.CanvasGroup.SetVisibility(true);
        cardPopupInstance.CanvasGroup.SetVisibility(false);
    }

    /// <summary>
    /// Removes the card from the hand and begins dragging it.
    /// </summary>
    /// <param name="cardInstance">The <see cref="CardInstance"/> to drag.</param>
    public void BeginDrag(CardInstance cardInstance)
    {
        if (cardInstance == null || currentDraggingCardInstance != null) return;

        currentDraggingCardInstance = cardInstance;
        currentDraggingCardInstance.RectTransform.rotation = Quaternion.identity;
        currentDraggingCardInstance.transform.SetParent(dragCardParent, false);

        CardController.Instance.RemoveCardFromHand(cardInstance);    
        EndHover(cardInstance);
    }

    /// <summary>
    /// Executes the drop-operation; if invalid, then the card is returned to the hand.
    /// </summary>
    public void EndDrag()
    {
        if (currentDraggingCardInstance == null) return;

        CardController.Instance.AddCardToHand(currentDraggingCardInstance.Card);

        Destroy(currentDraggingCardInstance.gameObject);
        currentDraggingCardInstance = null;
    }
}
