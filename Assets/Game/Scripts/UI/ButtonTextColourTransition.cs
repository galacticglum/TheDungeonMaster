/*
 * Author: Shon Verch
 * File Name: ButtonTextColourTransition.cs
 * Project Name: TheDungeonMaster
 * Creation Date: 01/21/2018
 * Modified Date: 01/21/2018
 * Description: A colour transition for button text.
 */

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// A colour transition for button text.
/// </summary>
[RequireComponent(typeof(Button))]
public class ButtonTextColourTransition : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField]
    private Text target;
    [SerializeField]
    private Color normalColour = Color.white;
    [SerializeField]
    private Color highlightedColour = Color.white;
    [SerializeField]
    private Color pressedColour = new Color(0.78f, 0.78f, 0.78f);
    [SerializeField]
    private Color disabledColour = new Color(0.78f, 0.78f, 0.78f, 0.5f);
    [SerializeField]
    private float fadeDuration = 0.1f;

    [SerializeField]
    private UnityEvent onPressed;
    [SerializeField]
    private UnityEvent onNormalState;

    private LerpInformation<Color> colourTransitionLerpInformation;
    private Button button;

    private void Start()
    {
        button = GetComponent<Button>();
        target.color = normalColour;
    }

    private void Update()
    {
        if (!button.IsInteractable() && target.color != disabledColour)
        {
            Transition(disabledColour);
        }

        if (colourTransitionLerpInformation != null)
        {
            target.color = colourTransitionLerpInformation.Step(Time.deltaTime);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!button.IsInteractable()) return;
        Transition(highlightedColour);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!button.IsInteractable()) return;
        Transition(normalColour);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!button.IsInteractable()) return;
        Transition(pressedColour);
        onPressed.Invoke();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!button.IsInteractable()) return;
        Transition(normalColour);
        onNormalState.Invoke();
    }

    private void Transition(Color destination)
    {
        colourTransitionLerpInformation = new LerpInformation<Color>(target.color, destination, fadeDuration, Color.Lerp);
        colourTransitionLerpInformation.Finished += (sender, args) => colourTransitionLerpInformation = null;
    }
}