/*
 * Author: Shon Verch
 * File Name: Enemy.cs
 * Project Name: TheDungeonMaster
 * Creation Date: 12/27/2017
 * Modified Date: 1/11/2017
 * Description: An entity which the user fights against.
 */

/// <summary>
/// An entity which the user fights against.
/// </summary>
public class Enemy
{
    /// <summary>
    /// The name of this <see cref="Enemy"/>.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// A description of the behaviour(s) of this <see cref="Enemy"/>.
    /// </summary>
    public string Description { get; }

    /// <summary>
    /// The type of this <see cref="Enemy"/>.
    /// </summary>
    public EnemyType Type { get; }

    /// <summary>
    /// The maximum health points (HP) of this <see cref="Enemy"/>; this is the starting HP as well.
    /// </summary>
    public int MaximumHealthPoints { get; }

    /// <summary>
    /// The current health points of this <see cref="Enemy"/>; defaults to the maximum HP.
    /// </summary>
    public int CurrentHealthPoints { get; set; }

    /// <summary>
    /// The amount of damage that this <see cref="Enemy"/> does.
    /// </summary>
    public int AttackPoints { get; }

    /// <summary>
    /// The chance that this <see cref="Enemy"/> will attack the player. 
    /// This values is from 0 to 1.
    /// </summary>
    public float AttackChance { get; }

    /// <summary>
    /// Initialize a <see cref="Enemy"/>.
    /// </summary>
    /// <param name="name">The name of this <see cref="Enemy"/>.</param>
    /// <param name="description">A description of the behaviour(s) of this <see cref="Enemy"/>.</param>
    /// <param name="maximumHealthPoints">The maximum health points (HP) of this <see cref="Enemy"/>; this is the starting HP as well.</param>
    /// <param name="type">The type of this <see cref="Enemy"/>. Defaults to <see cref="EnemyType"/>.Normal.</param>
    /// <param name="attackPoints">The amount of damage that this <see cref="Enemy"/> does.</param>
    /// <param name="attackChance">The chance that this <see cref="Enemy"/> will attack the player.</param>
    public Enemy(string name, string description, int maximumHealthPoints, int attackPoints = 0, float attackChance = 1, EnemyType type = EnemyType.Normal)
    {
        Name = name;
        Description = description;
        Type = type;
        MaximumHealthPoints = maximumHealthPoints;
        CurrentHealthPoints = MaximumHealthPoints;
        AttackPoints = attackPoints;
        AttackChance = attackChance;
    }
}