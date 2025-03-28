using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public EnemyData data;
    public EnemyAnimEvent SkinEnemy;

    [SerializeField]
    private Image HPBar;
    [SerializeField]
    TMP_Text TextHealth;
    [SerializeField]
    TMP_Text TextName;

    private int HealthMax;
    private int Health;

    public bool isDead;

    public void Creat(EnemyData _data)
    {
        data = _data;
        HealthMax = data.GetHealthMax();
        SkinEnemy = Instantiate(data.GetSkin(), transform);
        SkinEnemy.Craete(this);

        Health = HealthMax;
        isDead = false;

        TextName.text = data.GetName();
        UpdateHPBar();

        GameController.IsTurnPlayer = true;
    }

    private void UpdateHPBar()
    {
        HPBar.fillAmount = ((float)Health/HealthMax);
        TextHealth.text = $"{Health}/{HealthMax}";
    }

    public void ToDamage(int damage)
    {
        Health -= damage;
        if(Health <= 0)
        {
            ToDead(); 
        }
        UpdateHPBar();
    }

    public void ToDead()
    {
        isDead = true;
        GameController.GC.EnemyInventory.Clear();
        GameController.GC.ActiveitPanel((int)IdPanel.VinEnemy);
        List<ItemLoot> loots = new List<ItemLoot>();

        foreach(ItemLoot item in data.GetLoot())
        {
            float rnd = Random.Range(0, 100f);

            if (rnd <= item.probability)
            {
                loots.Add(item);
            }
        }

        GameController.GC.EnemyInventory.AddItems(loots);
        SkinEnemy.Dead();
    }

    public int GetHealth() => Health;
    public float GetPercentageOfHealth() => (Health / HealthMax);

}
