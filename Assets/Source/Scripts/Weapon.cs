using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public ItemInstance data;

    [Space(10f)]

    [SerializeField]
    private SpriteRenderer WeaponSprite;

    [Space(15f)]

    [SerializeField]
    private TMP_Text TextCountBullet;
    [SerializeField]
    private TMP_Text DamageText;

    [Space(15f)]

    [SerializeField]
    private GameObject ShotEffect;
    [SerializeField]
    private Transform ShotPoint;

    private int CountBullet;
    private Enemy enemy;
    private int Damage = 0;
    private string InfoText = "";

    [SerializeField]
    private float[] time_offset = { 0.3f, 0f, 0.25f };

    public void UpdataWeapon()
    {
        if (!data.data)
        {
            WeaponSprite.sprite = null;
            return;
        }
        WeaponSprite.sprite = ((WeaponItem)data.data).GetSprite();
        CountBullet = ((WeaponItem)data.data).GetCountBulletMax();
        TextCountBullet.text = $"{CountBullet}/{((WeaponItem)data.data).GetCountBulletMax()}";
        DamageText.text = $"{ ((WeaponItem)data.data).GetDamage().x } - { ((WeaponItem)data.data).GetDamage().y }";
        GameController.GC.player.GetAnim().SetInteger("WeaponType", (int)((WeaponItem)data.data).GetWeaponType());
    }

    public IEnumerator Shot( Enemy _enemy )
    {

        GameController.GC.InfoEnemyAttackText.text = "";

        if (CountBullet > 0)
        {
            int NumOfShots = ((WeaponItem)data.data).GetNumberOfShots();
            if (CountBullet < NumOfShots)
            {
                NumOfShots = NumOfShots - CountBullet;
            }    
            
            for (int i = 0; i < NumOfShots; i++)
            {
                float rnd = Random.Range(0, 100f);
                int damage = 0;
                if (rnd <= ((WeaponItem)data.data).GetProbabilityCriticalDamage())
                {
                    damage = ((WeaponItem)data.data).GetCriticalDamage();
                    InfoText = $"Вы нанесли крит: <color=#CDAE01>{damage}</color>\n";
                }
                else
                {
                    damage = Random.Range(((WeaponItem)data.data).GetDamage().x, ((WeaponItem)data.data).GetDamage().y);
                    InfoText = $"Вы нанесли: <color=#CDAE01>{damage}</color>\n";
                }

                enemy = _enemy;
                Damage = damage;

                GameController.GC.player.GetAnim().SetBool("Attack", true);
                
                yield return new WaitForSeconds(time_offset[(int)((WeaponItem)data.data).GetWeaponType()]);
                               
                CountBullet--;
                TextCountBullet.text = $"{CountBullet}/{((WeaponItem)data.data).GetCountBulletMax()}";
            }                                                   
        }
        else
        {
            Reload();
        }

        GameController.GC.player.GetAnim().SetBool("Attack", false);
        
        yield return new WaitForSeconds(0.1f);

        if (!GameController.GC.GetEnemy().isDead)
            GameController.GC.TurnEnemy();

        //yield return new WaitForSeconds(0.5f);
    } 

    public void OnShot()
    {
        enemy.ToDamage(Damage);
        data.OnDurability(1);
        GameController.GC.inventoryEquipment.Cells[InventoryEquipment.WeaponCell].item.OnDurability(1);
        GameController.GC.InfoEnemyAttackText.text += InfoText;
        Destroy(Instantiate(ShotEffect, ShotPoint.position, Quaternion.identity), 1.5f);
    }

    public void Reload()
    {
        InventoryCell cell = GameController.GC.Inventory.PathItem(((WeaponItem)data.data).GetBullet().ID);
        Debug.Log("cell");
        if (cell != null)
        {
            int _max = ((WeaponItem)data.data).GetCountBulletMax();
            CountBullet = _max;
            if (cell.amount < _max)
                CountBullet = _max - (_max - cell.amount);

            GameController.GC.Inventory.IDUseCell = cell.GetID();
            Debug.Log($"{cell.GetID()}, { CountBullet }");
            GameController.GC.Inventory.ClearAmountItem(CountBullet);
            TextCountBullet.text = $"{CountBullet}/{((WeaponItem)data.data).GetCountBulletMax()}";

            GameController.GC.InfoEnemyAttackText.text = $"Перезарядка";
        }
        else
            GameController.GC.InfoEnemyAttackText.text = $"У тебя нет патрон";
    }

    public void SetData(ItemInstance item)
    {
        data.data = item.data;
        data.CurrentDurability = item.CurrentDurability;
        if (item.data == null)
        { 
            WeaponSprite.sprite = null;

            TextCountBullet.text = $"NULL";
            return;
        }
        UpdataWeapon();
    }

    public bool GetIsData() => data != null;
}
