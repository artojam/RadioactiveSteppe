using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemyAnimEvent : MonoBehaviour
{
    private Enemy data;
    public Animator anim;

    public void Craete(Enemy _data)
    {
        data = _data;
        anim = GetComponent<Animator>();
    }

    public void Attack()
    {
        Vector2Int vec_damege = data.data.GetDamage();
        int damage = Random.Range(vec_damege.x, vec_damege.y);

        GameController.GC.player.ToDemage(damage, data.data.AttackType);
        GameController.IsTurnPlayer = true;
        GameController.GC.ActiveEnemyTurnPanel();
        GameController.GC.InfoEnemyAttackText.text = $"Вам нанесли <color=#EB1010>-{ damage }</color> едениц урона";
    }

    public void Dead()
    {
        anim.SetTrigger("Dead");
        Destroy(gameObject, 1.999f);
    }

    public void Escape()
    {
        Destroy(gameObject);
    } 
}
