/*
 * Author: Shon Verch
 * File Name: Card.cs
 * Project Name: TheDungeonMaster
 * Creation Date: 12/25/2017
 * Modified Date: 1/21/2018
 * Description: An action that the player can execute.
 */

using UnityEngine;

/// <summary>
/// An action that the player can execute.
/// </summary>
[CreateAssetMenu]
public class Card : ScriptableObject
{
    /// <summary>
    /// The name of this <see cref="Card"/>.
    /// </summary>
    public string Name => name;

    /// <summary>
    /// A description of the behaviour(s) of this <see cref="Card"/>.
    /// </summary>
    public string Description => description;

    /// <summary>
    /// The amount of damage this <see cref="Card"/> does to an <see cref="Enemy"/>.
    /// </summary>
    public int AttackPoints => attackPoints;

    /// <summary>
    /// The method which damage is delivered.
    /// </summary>
    public CardDamageType DamageType => damageType;

    /// <summary>
    /// The amount of cards this <see cref="Card"/> will resurrect when it is played.
    /// </summary>
    public int ResurrectionAmount => resurrectionAmount;

    /// <summary>
    /// The amount of cards that this <see cref="Card"/> will discard when it is played.
    /// </summary>
    public int OverchargeAmount => overchargeAmount;

    /// <summary>
    /// The amount of health points that this <see cref="Card"/> can heal the player.
    /// </summary>
    public int HealPoints => healPoints;

    /// <summary>
    /// The rarity of this <see cref="Card"/>.
    /// </summary>
    public CardRarity Rarity => rarity;

    /// <summary>
    /// The amount of turns this <see cref="Card"/> will stun an enemy for.
    /// </summary>
    public int StunPoints => stunPoints;

    /// <summary>
    /// The amount of attacks the player will shield from.
    /// </summary>
    public int ShieldPoints => shieldPoints;

    /// <summary>
    /// Determines whether this <see cref="Card"/> requires an <see cref="Enemy"/> target to execute.
    /// </summary>
    /// We only require a target if we need to attack as we need to know what <see cref="Enemy"/> to attack.
    public bool RequiresEnemyTarget => AttackPoints > 0 && damageType == CardDamageType.EnemyTarget || StunPoints > 0;

    [SerializeField]
    private new string name;
    [TextArea(5, 25)]
    [SerializeField]
    private string description;
    [SerializeField]
    private int attackPoints;
    [SerializeField]
    private CardDamageType damageType;
    [SerializeField]
    private int resurrectionAmount;
    [SerializeField]
    private int overchargeAmount;
    [SerializeField]
    private int healPoints;
    [SerializeField]
    private int stunPoints;
    [SerializeField]
    private int shieldPoints;
    [SerializeField]
    private CardRarity rarity;

    /// <summary>
    /// Creates a copy of this <see cref="Card"/>.
    /// </summary>
    public virtual Card Clone()
    {
        Card copy = CreateInstance<Card>();
        copy.name = Name;
        copy.description = Description;
        copy.attackPoints = AttackPoints;
        copy.damageType = DamageType;
        copy.resurrectionAmount = ResurrectionAmount;
        copy.overchargeAmount = OverchargeAmount;
        copy.healPoints = HealPoints;
        copy.shieldPoints = ShieldPoints;
        copy.rarity = Rarity;

        return copy;
    }
}
