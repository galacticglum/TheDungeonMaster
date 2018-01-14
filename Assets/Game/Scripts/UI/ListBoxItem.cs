/*
 * Author: Shon Verch
 * File Name: ListBoxItem.cs
 * Project Name: TheDungeonMaster
 * Creation Date: 01/14/2018
 * Modified Date: 01/14/2018
 * Description: An individual item of a ListBox.
 */

using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// An individual item of a <see cref="ListBox"/>.
/// </summary>
public class ListBoxItem : MonoBehaviour
{
    [SerializeField]
    private Text valueText;
    [SerializeField]
    private Image iconImage;

    [SerializeField]
    private Sprite defaultIconSprite;

    public void Initialize(string value, Sprite icon = null)
    {
        valueText.text = value;
        iconImage.sprite = icon == null ? defaultIconSprite : icon;
    }
}