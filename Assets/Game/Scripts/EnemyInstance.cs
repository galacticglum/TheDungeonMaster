/*
 * Author: Shon Verch
 * File Name: EnemyInstance.cs
 * Project Name: TheDungeonMaster
 * Creation Date: 12/27/2017
 * Modified Date: 1/11/2018
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

    private RectTransform rectTransform;

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

    private void Update()
    {
        enemyHealthPointsText.text = Enemy.CurrentHealthPoints.ToString();
    }

    public void ExecuteTurn()
    {
        // Can we even attack?
        if (Enemy.AttackPoints > 0)
        {
            if (Random.value < Enemy.AttackChance)
            {
                ControllerDatabase.Get<EncounterController>().DamagePlayer(Enemy.AttackPoints, Enemy);
            }
        }
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
