/*
 * Author: Shon Verch
 * File Name: Card.cs
 * Project Name: TheDungeonMaster
 * Creation Date: 12/25/2017
 * Modified Date: 12/25/2017
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
    /// Initialize a <see cref="Card"/>.
    /// </summary>
    /// <param name="name">The name of this <see cref="Card"/>.</param>
    /// <param name="description">A description of the behaviour(s) of this <see cref="Card"/>.</param>
    public Card(string name, string description)
    {
        Name = name;
        Description = description;
    }
}
