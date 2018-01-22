/*
 * Author: Shon Verch
 * File Name: EffectsCollection.cs
 * Project Name: TheDungeonMaster
 * Creation Date: 01/21/2018
 * Modified Date: 01/21/2018
 * Description: Stores all effects and updates visuals.
 */

using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Stores all effects and updates visuals.
/// </summary>
public class EffectsCollection
{
    private readonly Dictionary<EffectType, EffectInstance> effects;
    private readonly RectTransform effectParent;

    /// <summary>
    /// Initializes a new <see cref="EffectsCollection"/>.
    /// </summary>
    public EffectsCollection(RectTransform effectParent)
    {
        effects = new Dictionary<EffectType, EffectInstance>();
        this.effectParent = effectParent;
    }

    /// <summary>
    /// Handles any update logic.
    /// </summary>
    public void Update()
    {
        List<EffectType> effectsToRemove = new List<EffectType>();
        foreach (KeyValuePair<EffectType, EffectInstance> pair in effects)
        {
            if (pair.Value.Count > 0) continue;
            effectsToRemove.Add(pair.Key);
            Object.Destroy(pair.Value.gameObject);
        }

        foreach (EffectType effectType in effectsToRemove)
        {
            effects.Remove(effectType);
        }
    }

    /// <summary>
    /// Adds an effect to this collection.
    /// </summary>
    public void AddEffect(EffectType type, int amount = 1)
    {
        if (amount == 0) return;
        if (!effects.ContainsKey(type))
        {
            effects[type] = EffectInstance.Create(effectParent, type, amount);
        }

        effects[type].Count += amount;
    }

    /// <summary>
    /// Removes one effect of type from the collection.
    /// </summary>
    /// <returns>A boolean indicating whether an effect was used.</returns>
    public bool RemoveEffect(EffectType type)
    {
        if (!effects.ContainsKey(type) || effects[type].Count == 0) return false;

        effects[type].Count -= 1;
        return true;
    }

}