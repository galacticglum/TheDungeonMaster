/*
 * Author: Shon Verch
 * File Name: EnemyInstance.cs
 * Project Name: TheDungeonMaster
 * Creation Date: 12/27/2017
 * Modified Date: 1/21/2018
 * Description: The interface between the enemy data and the enemy visuals.
 */

using System;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

/// <summary>
/// The interface between the enemy data and the enemy visuals.
/// </summary>
[RequireComponent(typeof(RectTransform))]
[RequireComponent(typeof(Animator))]
public class EnemyInstance : MonoBehaviour
{
    /// <summary>
    /// The enemy data which this <see cref="EnemyInstance"/> is initialized from.
    /// </summary>
    public Enemy Enemy { get; private set; }

    /// <summary>
    /// All the effects on this <see cref="global::Enemy"/>.
    /// </summary>
    public EffectsCollection Effects { get; private set; }

    /// <summary>
    /// The <see cref="RectTransform"/> of this <see cref="EnemyInstance"/>.
    /// </summary>
    public RectTransform RectTransform => rectTransform ?? (rectTransform = GetComponent<RectTransform>());

    /// <summary>
    /// The amount of damage this enemy takes in poison.
    /// </summary>
    public int PoisonDamage{ get; set; }

    [SerializeField]
    private Text enemyNameText;
    [SerializeField]
    private Text enemyDescriptionText;
    [SerializeField]
    private RawImage enemyCrystalImage;
    [SerializeField]
    private Text enemyHealthPointsText;
    [SerializeField]
    private Text attackPointsText;
    [SerializeField]
    private RectTransform effectsParent;

    private Animator animator;
    private RectTransform rectTransform;
    private EncounterController encounterController;
    private bool didHealLastTurn = true;
    private int spawnCount;

    private Action onDead;
    private string currentAnimation;

    /// <summary>
    /// Initializes this <see cref="EnemyInstance"/> from an <see cref="Enemy"/>.
    /// </summary>
    /// <param name="enemy"></param>
    public void Initialize(Enemy enemy)
    {
        Enemy = enemy;
        gameObject.name = $"{enemy.Name}_instance";

        enemyNameText.text = enemy.Name;
        enemyDescriptionText.text = enemy.Description;
        enemyCrystalImage.texture = GetEnemyCrystalFromType(enemy.Type);
        attackPointsText.text = enemy.AttackPoints.ToString();

        Effects = new EffectsCollection(effectsParent);
    }

    /// <summary>
    /// Called when the component is created and placed into the world.
    /// </summary>
    private void Start()
    {
        animator = GetComponent<Animator>();
        encounterController = ControllerDatabase.Get<EncounterController>();
    }

    /// <summary>
    /// Called every frame.
    /// </summary>
    private void Update()
    {
        enemyHealthPointsText.text = Enemy.CurrentHealthPoints.ToString();

        // Check if death animation is done playing
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Dead"))
        {
            encounterController.RemoveEnemyFromEncounter(this);
        }

        Effects.Update();
    }

    /// <summary>
    /// Handle the turn logic for this enemy.
    /// </summary>
    public void ExecuteTurn()
    {
        // If we don't have any health then let's skip this turn.
        if (Enemy.CurrentHealthPoints <= 0) return;
        if (Effects.RemoveEffect(EffectType.Stun)) return;

        bool didAttack = false;

        // If our enemy can attack and heal, we must
        // execute slightly different logic.
        if (Enemy.AttackPoints > 0 && Enemy.HealingPoints > 0)
        {
            // If our enemy can both attack and heal on the same turn,
            // let's execute the actions. 
            // Otherwise, if we can't attack and heal on the same turn,
            // we will alternate between attacking and healing.
            if (Enemy.CanSimultaneousHealAttack)
            {
                didAttack = ExecuteAttack();
                ExecuteHealing();
            }
            else 
            {
                // Alternate between attacking and healing.
                if (didHealLastTurn)
                {
                    didAttack = ExecuteAttack();
                }
                else
                {
                    ExecuteHealing();
                }

                didHealLastTurn = !didHealLastTurn;
            }
        }
        else if(Enemy.AttackPoints > 0)
        {
            didAttack = ExecuteAttack();
        }
        else if (Enemy.HealingPoints > 0)
        {
            ExecuteHealing();
        }

        ExecutePoison();

        if (!didAttack)
        {
            ExecuteSpawn();
        }
    }

    /// <summary>
    /// Handle the logic for attacking the player.
    /// </summary>
    private bool ExecuteAttack()
    {
        if (encounterController.PlayerEffects.RemoveEffect(EffectType.Shield)) return false;
        if (!(Random.value < Enemy.AttackChance)) return false;

        encounterController.DamagePlayer(Enemy.AttackPoints, Enemy);
        PlayAnimation("Attack");

        return true;
    }

    /// <summary>
    /// Handle the logic for healing the <see cref="global::Enemy"/>.
    /// </summary>
    private void ExecuteHealing()
    {
        if (!(Random.value < Enemy.HealingChance)) return;

        int newHealth = Mathf.Clamp(Enemy.CurrentHealthPoints + Enemy.HealingPoints, 0, Enemy.MaximumHealthPoints);
        Enemy.CurrentHealthPoints = newHealth;
        //PlayAnimation("Cast");
    }

    /// <summary>
    /// Handle the logic for poison.
    /// </summary>
    private void ExecutePoison()
    {
        if (Effects.RemoveEffect(EffectType.Poison))
        {
            TakeDamage(PoisonDamage);
        }

        if (encounterController.PlayerEffects.RemoveEffect(EffectType.Poison))
        {
            encounterController.DamagePlayer(Enemy.PoisonAttackPoints, Enemy);
            return;
        }

        if (!(Random.value < Enemy.PoisonChance)) return;
        encounterController.PlayerEffects.AddEffect(EffectType.Poison, Enemy.PoisonTurnDuration);
    }

    /// <summary>
    /// Handle the logic for spawning.
    /// </summary>
    private void ExecuteSpawn()
    {
        // We can only spawn when all the spawned enemies have been killed.
        if (spawnCount > 0 || !(Random.value < Enemy.SpawnChance)) return;

        foreach (Enemy enemy in Enemy.EnemiesToSpawn)
        {
            PlayAnimation("Cast");
            EnemyInstance spawnedEnemyInstance = encounterController.AddEnemyToEncounter(enemy);

            spawnCount += 1;
            // We need to decrement the spawn count when the spawned enemy dies so we may spawn another one of it.
            spawnedEnemyInstance.onDead += () => spawnCount -= 1;
        }
    }

    /// <summary>
    /// Inflict damage upon this enemy.
    /// </summary>
    /// <param name="amount">The amount of damage to inflict on this enemy.</param>
    public void TakeDamage(int amount)
    {
        // Make sure that our HP does not go below 0.
        int newHealthPoints = Mathf.Max(0, Enemy.CurrentHealthPoints - amount);
        Enemy.CurrentHealthPoints = newHealthPoints;

        if (Enemy.CurrentHealthPoints <= 0)
        {
            Die();
        }
    }

    /// <summary>
    /// Plays an animation.
    /// </summary>
    private void PlayAnimation(string animation)
    {
        currentAnimation = animation;
        animator.SetBool(currentAnimation, true);

        encounterController.EnqueueAnimation(HandleAnimation);
    }

    /// <summary>
    /// Handle the logic for animation this <see cref="EnemyInstance"/>.
    /// </summary>
    private bool HandleAnimation()
    {
        if (animator == null) return false;
        if (!animator.GetCurrentAnimatorStateInfo(0).IsTag("SinglePlay"))
        {
            return animator.GetBool(currentAnimation);
        }

        animator.SetBool(currentAnimation, false);
        return true;

    }

    /// <summary>
    /// Handle the death logic.
    /// </summary>
    private void Die()
    {
        animator.SetTrigger("Die");
        onDead?.Invoke();
    }

    /// <summary>
    /// Retrieves the appropriate enemy crystal from a specified type.
    /// </summary>
    /// <param name="type">Rhe type of enemy.</param>
    /// <returns></returns>
    private static Texture2D GetEnemyCrystalFromType(EnemyType type)
    {
        string typeName = Enum.GetName(typeof(EnemyType), type);
        string loadPath = $"Images/enemy_crystal_{typeName}";
        return Resources.Load<Texture2D>(loadPath);
    }

    /// <summary>
    /// Creates a <see cref="EnemyInstance"/> from an <see cref="global::Enemy"/>.
    /// </summary>
    /// <param name="enemy">The <see cref="global::Enemy"/> to create the <see cref="EnemyInstance"/>.</param>
    /// <param name="parent">The parent of this <see cref="EnemyInstance"/>.</param>
    /// <returns>The enemy's instance <see cref="GameObject"/>.</returns>
    public static EnemyInstance Create(Enemy enemy, RectTransform parent)
    {
        GameObject enemyGameObject = Instantiate(Resources.Load<GameObject>("Prefabs/Enemy_Front"));
        enemyGameObject.transform.SetParent(parent, false);

        EnemyInstance enemyInstance = enemyGameObject.GetComponent<EnemyInstance>();
        enemyInstance.Initialize(enemy);    

        return enemyInstance;
    }
}
