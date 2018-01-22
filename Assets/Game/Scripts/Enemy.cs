/*
 * Author: Shon Verch
 * File Name: Enemy.cs
 * Project Name: TheDungeonMaster
 * Creation Date: 12/27/2017
 * Modified Date: 1/21/2017
 * Description: An entity which the user fights against.
 */

using UnityEngine;

/// <summary>
/// An entity which the user fights against.
/// </summary>
[CreateAssetMenu]
public class Enemy : ScriptableObject
{
    /// <summary>
    /// The name of this <see cref="Enemy"/>.
    /// </summary>
    public string Name => name;

    /// <summary>
    /// A description of the behaviour(s) of this <see cref="Enemy"/>.
    /// </summary>
    public string Description => description;

    /// <summary>
    /// The type of this <see cref="Enemy"/>.
    /// </summary>
    public EnemyType Type => type;

    /// <summary>
    /// The maximum health points (HP) of this <see cref="Enemy"/>; this is the starting HP as well.
    /// </summary>
    public int MaximumHealthPoints => maximumHealthPoints;

    /// <summary>
    /// The current health points of this <see cref="Enemy"/>; defaults to the maximum HP.
    /// </summary>
    public int CurrentHealthPoints { get; set; }

    /// <summary>
    /// The amount of damage that this <see cref="Enemy"/> does.
    /// </summary>
    public int AttackPoints => attackPoints;

    /// <summary>
    /// The chance that this <see cref="Enemy"/> will attack the player. 
    /// This values is from 0 to 1.
    /// </summary>
    public float AttackChance => attackChance;

    public int PoisonAttackPoints => poisonAttackPoints;
    public int PoisonTurnDuration => poisonTurnDuration;
    public float PoisonChance => poisonChance;

    /// <summary>
    /// The amount of health this <see cref="Enemy"/> heals.
    /// </summary>
    public int HealingPoints => healingPoints;

    /// <summary>
    /// The chance that this <see cref="Enemy"/> will heal.
    /// This value is from 0 to 1.
    /// </summary>
    public float HealingChance => healingChance;

    /// <summary>
    /// Determines whether this <see cref="Enemy"/> can heal and attack on the same turn.
    /// </summary>
    public bool CanSimultaneousHealAttack => canSimultaneousHealAttack;

    [Header("General")]
    [SerializeField]
    private new string name;
    [TextArea(5, 25)]
    [SerializeField]
    private string description;
    [SerializeField]
    private EnemyType type;
    
    [Header("Combat")]
    [SerializeField]
    private int maximumHealthPoints;
    [SerializeField]
    private int attackPoints;
    [Range(0, 1)]
    [SerializeField]
    private float attackChance;
    [SerializeField]
    private bool canSimultaneousHealAttack;

    [Header("Poison")]
    [SerializeField]
    private int poisonAttackPoints;
    [SerializeField]
    private int poisonTurnDuration;
    [Range(0, 1)]
    [SerializeField]
    private float poisonChance;

    [Header("Heal")]
    [SerializeField]
    private int healingPoints;
    [Range(0, 1)]
    [SerializeField]
    private float healingChance;

    /// <summary>
    /// Called when this object is enabled.
    /// </summary>
    private void OnEnable()
    {
        CurrentHealthPoints = maximumHealthPoints;
    }

    /// <summary>
    /// Creates a copy of this <see cref="Enemy"/>.
    /// </summary>
    public virtual Enemy Clone()
    {
        Enemy copy = CreateInstance<Enemy>();
        copy.name = Name;
        copy.description = Description;
        copy.type = Type;

        copy.maximumHealthPoints = MaximumHealthPoints;
        copy.CurrentHealthPoints = MaximumHealthPoints;

        copy.attackPoints = AttackPoints;
        copy.attackChance = AttackChance;

        copy.poisonAttackPoints = PoisonAttackPoints;
        copy.poisonTurnDuration = PoisonTurnDuration;
        copy.poisonChance = PoisonChance;

        copy.healingPoints = HealingPoints;
        copy.healingChance = HealingChance;

        copy.canSimultaneousHealAttack = CanSimultaneousHealAttack;

        return copy;
    }
}