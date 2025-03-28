using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

[Serializable]
public class Protection
{
    public int BulletProtection;
    public int RadProtection;
    public int ChemicalProtection;
    public int PhysicalProtection;
    public int ElectroProtection;
    public int FireProtection;

    public void PlusProtection(Protection _protection)
    {
        BulletProtection += _protection.BulletProtection;
        RadProtection += _protection.RadProtection;
        ChemicalProtection += _protection.ChemicalProtection;
        PhysicalProtection += _protection.PhysicalProtection;
        ElectroProtection += _protection.ElectroProtection;
        FireProtection += _protection.FireProtection;
    }

    public void MinusProtection(Protection _protection)
    {
        BulletProtection -= _protection.BulletProtection;
        RadProtection -= _protection.RadProtection;
        ChemicalProtection -= _protection.ChemicalProtection;
        PhysicalProtection -= _protection.PhysicalProtection;
        ElectroProtection -= _protection.ElectroProtection;
        FireProtection -= _protection.FireProtection;
    }
}

[Serializable]
public class SpritsStorageArmor
{
    public List<SpriteRenderer> Sprits = new List<SpriteRenderer>();
    public List<Sprite> DefuldSprites = new List<Sprite>();
    public Protection Data;

    public void SetSprite(List<Sprite> sprites)
    {
        for (int i = 0; i < Sprits.Count; ++i)
        {
            Sprits[i].sprite = sprites[i];
            
        }
    }

    public void SetDefuld(string name)
    {
        Debug.Log(name);
        foreach(SpriteRenderer renderer in Sprits)
        {
            DefuldSprites.Add(renderer.sprite);
        }
        Debug.Log(DefuldSprites.Count);
    }

    public void OnDefuld()
    {
        SetSprite(DefuldSprites);
    }

    public List<Sprite> GetSprite()
    {
        List<Sprite> list = new List<Sprite>();
        for (int i = 0; i < Sprits.Count; ++i)
        {
            list.Add(Sprits[i].sprite);
        }
        return list;
    }
}

public class Armor : MonoBehaviour
{
    [Header("Head")]
    [SerializeField]
    private SpriteRenderer Head;

    [Header("Fase")]
    [SerializeField]
    private SpriteRenderer Fase;

    [Header("Body")]
    [SerializeField]
    private SpriteRenderer Body;
    
    [Header("LeftHand")]
    [SerializeField]
    private SpriteRenderer LeftHand_1;
    [SerializeField]
    private SpriteRenderer LeftHand_2;
    [SerializeField]
    private SpriteRenderer LeftHand_3;

    [Header("RightHand")]
    [SerializeField]
    private SpriteRenderer RightHand_1;
    [SerializeField]
    private SpriteRenderer RightHand_2;
    [SerializeField]
    private SpriteRenderer RightHand_3;

    [Header("LeftLeg")]
    [SerializeField]
    private SpriteRenderer LeftLeg_1;
    [SerializeField]
    private SpriteRenderer LeftLeg_2;
    [SerializeField]
    private SpriteRenderer LeftLeg_3;
    [SerializeField]
    private SpriteRenderer LeftLeg_4;

    [Header("RightLeg")]
    [SerializeField]
    private SpriteRenderer RightLeg_1;
    [SerializeField]
    private SpriteRenderer RightLeg_2;
    [SerializeField]
    private SpriteRenderer RightLeg_3;
    [SerializeField]
    private SpriteRenderer RightLeg_4;

    public List<SpritsStorageArmor> Armors = new List<SpritsStorageArmor>();

    [Header("Data Defuld")]
    [SerializeField]
    private List<ArmorItem> ArmorsDefuld = new List<ArmorItem>();

    public void Create()
    {
        SpritsStorageArmor HandArmor = new SpritsStorageArmor();
        HandArmor.Sprits.Add(Head);
        HandArmor.SetDefuld("Head");

        SpritsStorageArmor FaseArmor = new SpritsStorageArmor();
        FaseArmor.Sprits.Add(Fase);
        FaseArmor.SetDefuld("Fase");

        SpritsStorageArmor BodyArmor = new SpritsStorageArmor();
        BodyArmor.Sprits.Add(Body);
        BodyArmor.Sprits.Add(LeftHand_1);
        BodyArmor.Sprits.Add(LeftHand_2);
        BodyArmor.Sprits.Add(LeftHand_3);
        BodyArmor.Sprits.Add(RightHand_1);
        BodyArmor.Sprits.Add(RightHand_2);
        BodyArmor.Sprits.Add(RightHand_3);
        BodyArmor.SetDefuld("Body");

        SpritsStorageArmor LegArmor = new SpritsStorageArmor();
        LegArmor.Sprits.Add(LeftLeg_1);
        LegArmor.Sprits.Add(LeftLeg_2);
        LegArmor.Sprits.Add(LeftLeg_3);
        LegArmor.Sprits.Add(RightLeg_1);
        LegArmor.Sprits.Add(RightLeg_2);
        LegArmor.Sprits.Add(RightLeg_3);
        LegArmor.SetDefuld("Leg");

        Armors.Add(HandArmor);
        Armors.Add(FaseArmor);
        Armors.Add(BodyArmor);
        Armors.Add(LegArmor);

        foreach (ArmorItem armor in ArmorsDefuld) 
        {
            if (armor != null)
            {
                SetArmor(armor.GetArmorType(), new ItemInstance(armor));
            }
        }

    }

    public void SetArmor(ArmorType type, ItemInstance item)
    {
        ArmorItem _armor = (ArmorItem)item.data;
        Armors[(int)type].SetSprite(_armor.GetSprite());
        Armors[(int)type].Data = _armor.GetData();
        GameController.GC.player.protection.PlusProtection(_armor.GetData());
    }

    public void ClearArmor(int id)
    {

        GameController.GC.player.protection.MinusProtection(Armors[id].Data);
        Armors[id].OnDefuld();
        Protection data = new Protection();
        Armors[id].Data = data;
        
    }
}