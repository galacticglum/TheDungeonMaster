/*
 * Author: Shon Verch
 * File Name: CardInteractionController.cs
 * Project Name: TheDungeonMaster
 * Creation Date: 12/26/2017
 * Modified Date: 1/19/2018
 * Description: Manages all the interaction between cards and the user.
 */

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// Manages all the interaction between cards and the user.
/// </summary>
[RequireComponent(typeof(GraphicRaycaster))]
public class CardInteractionController : ControllerBehaviour
{
    [SerializeField]
    private CardInstance cardPopupInstance;

    [Tooltip("The object which contains the card which is currently being dragged.")]
    [SerializeField]
    private Transform dragCardParent;

    private CardInstance currentDraggingCardInstance;
    /// <summary>
    /// The position of the <see cref="CardInstance"/> we are currently dragging in the hand.
    /// We use this to return the card back to it's position before dragging if needed.
    /// </summary>  
    private int indexOfDraggedCardInHand;

    private GraphicRaycaster graphicRaycaster;
    private CardHandController cardHandController;

    /// <summary>
    /// Called when the component is created and placed into the world.
    /// </summary>
    private void Start()
    {
        cardPopupInstance.CanvasGroup.SetVisibility(false);
        graphicRaycaster = GetComponent<GraphicRaycaster>();
        cardHandController = ControllerDatabase.Get<CardHandController>();
    }

    /// <summary>
    /// Called every frame.
    /// </summary>
    private void Update()
    {
        // Let's bail if we aren't dragging anything.
        if (currentDraggingCardInstance == null) return;

        // Make the card follow the mouse position.
        Canvas canvas = ControllerDatabase.Get<UIController>().Canvas;
        Vector2 mousePosition;

        // Convert from screen-coordinates to UI-space coordinates.
        RectTransformUtility.ScreenPointToLocalPointInRectangle((RectTransform)canvas.transform, Input.mousePosition, canvas.worldCamera, out mousePosition);

        // Centre the card position on the Y-axis.
        mousePosition.y -= currentDraggingCardInstance.RectTransform.rect.height / 2 * currentDraggingCardInstance.RectTransform.localScale.y;
        currentDraggingCardInstance.transform.position = canvas.transform.TransformPoint(mousePosition);

        // Right mouse cancels the drag.
        if (Input.GetMouseButtonUp(1))
        {
            EndDrag(true);
        }
        else if (Input.GetMouseButtonUp(0))
        {
            HandleDrop();
        }
    }

    /// <summary>
    /// Magnifies the hovered card.
    /// </summary>
    /// <param name="cardInstance">The <see cref="CardInstance"/> to magnify (popup).</param>
    public void BeginHover(CardInstance cardInstance)
    {
        if (cardInstance == null || currentDraggingCardInstance != null) return;

        // We hide the original hand-card as we use a clone of the hovered card for the popup.
        cardInstance.CanvasGroup.SetVisibility(false);

        cardPopupInstance.Initialize(cardInstance.Card);
        cardPopupInstance.CanvasGroup.SetVisibility(true);

        Vector2 position = cardInstance.RectTransform.anchoredPosition;

        // Nudge the card "up-a-bit" so that it is not COMPLETELY aligned with the bottom of the screen.
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
        // We can drag if it is our players turn; we have been given a 
        // valid card instance; and we aren't currently dragging.
        bool canDrag = ControllerDatabase.Get<EncounterController>().IsPlayerTurn && cardInstance != null &&
                       currentDraggingCardInstance == null;

        if (!canDrag) return;

        indexOfDraggedCardInHand = cardHandController.GetIndexOfCard(cardInstance);

        // Let's make sure that the transformational properties
        // of our card is all "kosher." Basically, we reset
        // our card instance to the default transform properties
        // and parent it to our card drag container.
        currentDraggingCardInstance = cardInstance;
        currentDraggingCardInstance.RectTransform.rotation = Quaternion.identity;
        currentDraggingCardInstance.transform.SetParent(dragCardParent, false);
        currentDraggingCardInstance.CanvasGroup.blocksRaycasts = false;

        cardHandController.RemoveCard(cardInstance);    
        EndHover(cardInstance);
    }

    /// <summary>
    /// Executes the drop-operation; if invalid, then the card is returned to the hand.
    /// <param name="cancelled">Has this drop-operation been cancelled?</param>
    /// </summary>
    public void EndDrag(bool cancelled)
    {
        if (currentDraggingCardInstance == null) return;
        if (cancelled)
        {
            // Return the card back to hand at it's original position.
            cardHandController.AddCard(currentDraggingCardInstance.Card, indexOfDraggedCardInHand);
        }

        // We destroy our original hand-card as we create a clone of the card if it is returned to the hand.
        Destroy(currentDraggingCardInstance.gameObject);
        currentDraggingCardInstance = null;
    }

    /// <summary>
    /// Handle a card being dropped in the scene.
    /// </summary>
    private void HandleDrop()
    {
        // Perform a raycast from the mouse cursor and find the first valid target gameobject.
        List<RaycastResult> raycastResults = new List<RaycastResult>();
        PointerEventData eventData = new PointerEventData(EventSystem.current) { position = Input.mousePosition, pointerId = -1 };
        graphicRaycaster.Raycast(eventData, raycastResults);

        bool cardExecuteSuccess = false;
        foreach (RaycastResult result in raycastResults)
        {
            cardExecuteSuccess = currentDraggingCardInstance.HandleCardPlayed(result.gameObject);
            if (cardExecuteSuccess) break;
        }

        EndDrag(!cardExecuteSuccess);
    }
}
