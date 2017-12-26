/*
 * Author: Shon Verch
 * File Name: CardInstance.cs
 * Project Name: The Dungeon Master
 * Creation Date: 12/26/2017
 * Modified Date: 12/26/2017
 * Description: The interface between the card data and the card visuals.
 */

using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// The interface between the card data and the card visuals.
/// </summary>
public class CardInstance : MonoBehaviour
{
    [SerializeField]
    private Text cardNameText;
    [SerializeField]
    private Text cardDescriptionText;

    /// <summary>
    /// Initializes this <see cref="CardInstance"/> from a <see cref="Card"/>.
    /// </summary>
    /// <param name="card">The <see cref="Card"/> data to initialize from.</param>
    private void Initialize(Card card)
    {
        gameObject.name = $"{card.Name}_instance";
        cardNameText.text = card.Name;
        cardDescriptionText.text = card.Description;
    }

    /// <summary>
    /// Creates a <see cref="CardInstance"/> from a <see cref="Card"/>.
    /// </summary>
    /// <param name="card">The <see cref="Card"/> to create the <see cref="CardInstance"/> from.</param>
    /// <returns>The card instance's <see cref="GameObject"/>.</returns>
    public static GameObject Create(Card card)
    {
        GameObject cardGameObject = Instantiate(Resources.Load<GameObject>("Prefabs/Card_Front"));
        cardGameObject.transform.SetParent(UIManager.Instance.Canvas.transform, false);

        CardInstance cardInstance = cardGameObject.GetComponent<CardInstance>();
        cardInstance.Initialize(card);

        return cardGameObject;
    }
}
