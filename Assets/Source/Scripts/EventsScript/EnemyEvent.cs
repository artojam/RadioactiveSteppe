using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemyEvent", menuName = "Game/Event/New Enemy Event")]
public class GameEnemyEvent : GameEvent
{
    [SerializeField]
    private EnemyData enemy;

    private void Awake()
    {
        e_Type = TypeEvent.ENEMY;
    }

    public override void Execute(int id)
    {
        GameController.GC.InfoEnemyAttackText.text = $"ты встретил <color=#EB1010>{ enemy.GetName() }</color>";

        Enemy newEnemy = GameController.GC.GetEnemy();

        newEnemy.gameObject.SetActive(true);
        newEnemy.Creat(enemy);

        GameController.GC.ActiveitPanel((int)IdPanel.EnemyAttack);

        GameController.GC.EnemyTurnPanel.SetActive(false);

        GameController.GC.StopWalking();
    }
}