/*
 * Author: Shon Verch
 * File Name: CardDamageType.cs
 * Project Name: TheDungeonMaster
 * Creation Date: 01/21/2018
 * Modified Date: 01/21/2018
 * Description: The type of damage a card does.
 */

/// <summary>
/// The type of damage a card does.
/// </summary>
public enum CardDamageType
{
    /// <summary>
    /// Affects an <see cref="Enemy"/> target only.
    /// </summary>
    EnemyTarget,

    /// <summary>
    /// Affects all enemies on the board.
    /// </summary>
    AreaOfEffect
}
