/*
 * Author: Shon Verch
 * File Name: Card.cs
 * Project Name: TheDungeonMaster
 * Creation Date: 12/25/2017
 * Modified Date: 1/5/2018
 * Description: An action that the player can execute.
 */

/// <summary>
/// An action that the player can execute.
/// </summary>
public class Card
{
    /// <summary>
    /// The name of this <see cref="Card"/>.
    /// </summary>
    public string Name { get; }
    
    /// <summary>
    /// A description of the behaviour(s) of this <see cref="Card"/>.
    /// </summary>
    public string Description { get; }

    /// <summary>
    /// The amount of health points this <see cref="Card"/> can subtract from an <see cref="Enemy"/>.
    /// </summary>
    public int AttackPoints { get; }

    /// <summary>
    /// Initialize a <see cref="Card"/>.
    /// </summary>
    /// <param name="name">The name of this <see cref="Card"/>.</param>
    /// <param name="description">A description of the behaviour(s) of this <see cref="Card"/>.</param>
    /// <param name="attackPoints">The amount of health points this <see cref="Card"/> can subtract from an <see cref="Enemy"/>.</param>
    public Card(string name, string description, int attackPoints = 0)
    {
        Name = name;
        Description = description;
        AttackPoints = attackPoints;
    }

    /// <summary>
    /// Initialize a <see cref="Card"/> from another <see cref="Card"/> value.
    /// </summary>
    protected Card(Card other)
    {
        Name = other.Name;
        Description = other.Description;
        AttackPoints = other.AttackPoints;
    }

    /// <summary>
    /// Creates a copy of this <see cref="Card"/>.
    /// </summary>
    public virtual Card Clone() => new Card(this);
}
