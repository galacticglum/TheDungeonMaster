/*
 * Author: Shon Verch
 * File Name: EffectInstance.cs
 * Project Name: TheDungeonMaster
 * Creation Date: 01/21/2018
 * Modified Date: 01/21/2018
 * Description: The instance of a single effect in the world.
 */

using System;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// The instance of a single effect in the world.
/// </summary>
public class EffectInstance : MonoBehaviour
{
    /// <summary>
    /// The amount of times this effect has been applied.
    /// </summary>
    public int Count { get; set; }

    [SerializeField]
    private Image icon;
    [SerializeField]
    private Text counterText;

    /// <summary>
    /// Called every frame.
    /// </summary>
    private void Update()
    {
        counterText.text = Count.ToString();
    }

    /// <summary>
    /// Creates an <see cref="EffectInstance"/> from an <see cref="EffectType"/> and count.
    /// </summary>
    public static EffectInstance Create(RectTransform parent, EffectType type, int count)
    {
        GameObject effectGameObject = Instantiate(Resources.Load<GameObject>("Prefabs/Effect"));
        effectGameObject.transform.SetParent(parent, false);

        EffectInstance effectInstance = effectGameObject.GetComponent<EffectInstance>();
        effectInstance.icon.sprite = Resources.Load<Sprite>($"Images/Effect Icons/effect_icon_{Enum.GetName(typeof(EffectType), type)}");

        return effectInstance;
    }
}
