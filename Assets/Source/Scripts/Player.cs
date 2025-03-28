using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;


public class Player : MonoBehaviour
{
    private const int space = 15;

    [Header("Data")]
    public int HealthMax;
    private int DefaultHealthMax => HealthMax;
    [SerializeField]
    private TMP_Text HealthText;
    private int Health;

    [Space(space)]

    public int EnergyMax;
    private int DefaultEnergyMax => EnergyMax;
    [SerializeField]
    private TMP_Text EnergyText;
    private int Energy;

    [Space(space)]

    public int HungerMax;
    private int DefaultHungerMax => HungerMax;
    [SerializeField]
    private TMP_Text HungerText;
    private int Hunger;

    [Space(space)]


    public int ThirstMax;
    private int DefaultThirstMax => ThirstMax;
    [SerializeField]
    private TMP_Text ThirstText;
    private int Thirst;

    [Space(space)]


    private int RadiationMax;
    [SerializeField]
    private TMP_Text RadiationText;
    private int Radiation;

    [Space(space)]

    [SerializeField]
    private int Money = 10000;
    [SerializeField]
    private TMP_Text MoneyText;

    [Space(space)]
    
    [SerializeField]
    private int EnergyAttack;

    [Space(space)]

    [SerializeField]
    private Gradient GradientText;

    [Space(space)]

    public Weapon weapon;
    public Armor armor;
    public Protection protection;

    private Animator anim;

    public void Create()
    {
        weapon = GetComponent<Weapon>();
        armor = GetComponent<Armor>();
        anim = GetComponent<Animator>();

        Health = HealthMax;
        Energy = EnergyMax;
        Hunger = HungerMax;
        Thirst = ThirstMax;
        RadiationMax = HealthMax;

        UpdatetText();
        MoneyText.text = Money.ToString();
    }

    private void UpdatetText()
    {
        Health = Mathf.Clamp(Health, 0, HealthMax);
        Energy = Mathf.Clamp(Energy, 0, EnergyMax);
        Hunger = Mathf.Clamp(Hunger, 0, HungerMax);
        Thirst = Mathf.Clamp(Thirst, 0, ThirstMax);

        HealthText.text = $"{Health}/{HealthMax}";
        EnergyText.text = $"{Energy}/{EnergyMax}";
        HungerText.text = $"{Hunger}/{HungerMax}";
        ThirstText.text = $"{Thirst}/{ThirstMax}";
        RadiationText.text = $"{Radiation}";
    }

    public void ApplyEffect(EffectArtifact _effect)
    {
        ModifyPlayerVar(_effect.VarName, _effect.Value);
        UpdatetText();
    }

    public void RemoveEffect(EffectArtifact _effect)
    {
        ModifyPlayerVar(_effect.VarName, -_effect.Value);
        UpdatetText();
    }

    public void ModifyPlayerVar(string VarName, int Value)
    {
        string[] path = VarName.Split('.');
        object currentObject = this;
        Type currentType = this.GetType();

        // Переход по пути к нужной переменной
        for (int i = 0; i < path.Length - 1; i++)
        {
            FieldInfo field = currentType.GetField(path[i]);
            if (field != null)
            {
                currentObject = field.GetValue(currentObject);
                currentType = field.FieldType;
            }
            else
            {
                Debug.LogError($"Переменная с именем {path[i]} не найдена в классе {currentType}");
                return;
            }
        }

        // Доступ к последнему полю и его изменение
        FieldInfo finalField = currentType.GetField(path[path.Length - 1]);
        if (finalField != null && finalField.FieldType == typeof(int))
        {
            try
            {
                int convertedValue = (int)finalField.GetValue(currentObject);
                int addValue = Value;

                finalField.SetValue(currentObject, convertedValue + addValue);
            }
            catch (Exception e)
            {
                Debug.LogError($"Ошибка при преобразовании значения: {e.Message}");
            }
        }
        else
        {
            Debug.LogError($"Переменная с именем {VarName} не найдена в классе {currentType}");
        }
    }

    public void OnWalking()
    {
        Transform tr = transform;
        anim.SetBool("Walking", true);
        tr.position = new Vector3(0f, tr.position.y, tr.position.z);
    }

    public void OnStop()
    {
        Transform tr = transform;
        anim.SetBool("Walking", false);
        tr.position = new Vector3(-1.84f, tr.position.y, tr.position.z);
    }


    public void Attack()
    {
        if(weapon.data.data == null)
        {
            GameController.GC.InfoPanel.Active("Внимание", "У тебя нет оружия");
            return;
        }
        if (!GameController.IsTurnPlayer) 
            return;
        if(Energy <= 0)
        {
            GameController.GC.InfoPanel.Active("Внимание", "Я сильно устал, мне надо востановить энергию");
            return;
        }

        StartCoroutine(weapon.Shot(GameController.GC.GetEnemy()));
        ToEnergy(EnergyAttack);
        
    }

    private int GetProtection(TypeAttack _type)
    {
        int Protection = 0;
        if (_type == TypeAttack.Default)
        {
            Protection = 0;
        }
        else if (_type == TypeAttack.Bullet)
        {
            Protection = protection.BulletProtection;
        }
        else if (_type == TypeAttack.Chemical)
        {
            Protection = protection.ChemicalProtection;
        }
        else if (_type == TypeAttack.Rad)
        {
            Protection = protection.RadProtection;
        }
        else if (_type == TypeAttack.Physical)
        {
            Protection = protection.PhysicalProtection;
        }
        else if (_type == TypeAttack.Electro)
        {
            Protection = protection.ElectroProtection;
        }


        return Protection;
    }

    public int ToDemage(int demage, TypeAttack _type)
    {
        int _damage = demage + GetProtection(_type);
        Health = Mathf.Clamp(Health - _damage, 0, HealthMax);
        anim.SetTrigger("Damege");

        if (Health < 0)
        {
            GameController.GC.DeadPlayer();
            return 0;
        }


        string hex = GetHexColorText(Energy, EnergyMax + Radiation).Remove(5, 2);
        HealthText.text = $"<color=#{hex}>{Health}</color>/{HealthMax}";
        return _damage;
    }

    public void ToEnergy(int value)
    {
        Energy = Mathf.Clamp(Energy + value, 0, EnergyMax);
        if(Energy <= 0)
            GameController.GC.InfoPanel
                .Active
                (
                "Вы очень устали",
                "Я очень сильно устал мне надо одохнуть"
                );

        string hex = GetHexColorText(Energy, EnergyMax).Remove(5, 2);
        EnergyText.text = $"<color=#{hex}>{Energy}</color>/{EnergyMax}";
    }

    public void ToHunger(int value)
    {
        Hunger = Mathf.Clamp(Hunger + value, 0, HungerMax);
        if (Hunger <= 0)
        {
            GameController.GC.InfoPanel
                .Active
                (
                "Вы очень голодны",
                "Съеште что нибуть перед тем как ваше здоровье опустится до нуля"
                );

            ToDemage(value, TypeAttack.Default);
        }

        string hex = GetHexColorText(Hunger, HungerMax).Remove(5, 2);
        HungerText.text = $"<color=#{hex}>{Hunger}</color>/{HungerMax}";
    }

    public void ToThirst(int value)
    {
        Thirst = Mathf.Clamp(Thirst + value, 0, ThirstMax);
        if (Thirst <= 0)
        {
            GameController.GC.InfoPanel
                .Active
                (
                "Вы очень хотите пить",
                "Быстрее выпийте что нибуть перед тем как ваше здоровье опустится до нуля"
                );

            ToDemage(value, TypeAttack.Default);
        }

        string hex = GetHexColorText(Thirst, ThirstMax).Remove(5, 2);
        ThirstText.text = $"<color=#{hex}>{Thirst}</color>/{ThirstMax}";
    }

    public void ToRadiation(int value)
    {
        int remainder = Mathf.Clamp(Radiation + value, 0, RadiationMax);
        
        Radiation += remainder;
        RadiationText.text = $"{Radiation}";

        HealthMax -= remainder;
        ToDemage(-remainder, TypeAttack.Default);

    }

    public int GetMoney() => Money;
    public void UpdateMoney(int money)
    {
        Money += money;
        MoneyText.text = Money.ToString();
    }

    public Animator GetAnim() => anim;

    private string GetHexColorText(float value, float max_value) => GradientText.Evaluate (
        (max_value - value) / max_value
         ).ToHexString(); 
}
