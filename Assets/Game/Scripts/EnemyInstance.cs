/*
 * Author: Shon Verch
 * File Name: EnemyInstance.cs
 * Project Name: TheDungeonMaster
 * Creation Date: 12/27/2017
 * Modified Date: 1/18/2018
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
    /// The <see cref="RectTransform"/> of this <see cref="EnemyInstance"/>.
    /// </summary>
    public RectTransform RectTransform => rectTransform ?? (rectTransform = GetComponent<RectTransform>());

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

    private Animator animator;
    private RectTransform rectTransform;
    private EncounterController encounterController;

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
    }

    /// <summary>
    /// Handle the turn logic for this enemy.
    /// </summary>
    public void ExecuteTurn()
    {
        // If we don't have any health then let's skip this turn.
        if (Enemy.CurrentHealthPoints <= 0) return;

        // Can we even attack?
        // If we can't attack then let's bail.
        if (Enemy.AttackPoints <= 0) return;
        
        if (!(Random.value < Enemy.AttackChance)) return;

        encounterController.DamagePlayer(Enemy.AttackPoints, Enemy);
        PlayAnimation("Attack");
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
