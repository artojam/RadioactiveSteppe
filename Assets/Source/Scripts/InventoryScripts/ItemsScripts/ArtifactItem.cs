using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

[CreateAssetMenu(fileName = "ArtifactItem", menuName = "Game/Item/Create new Artifact item")]
public class ArtifactItem : Item
{
    [field: Header("Artifact")]

    [field: SerializeField]
    public AnimationCurve CurveArtifact { get; private set; }
    [field: SerializeField]
    public TypeAttack TypeArtifact { get; private set; }
    [field: SerializeField]
    public Vector2Int Damege { get; private set; }

    [Space(10f)]
    
    [HideInInspector]
    public List<EffectArtifact> Effects = new List<EffectArtifact>();



    public override void Use(float _dur)
    {
        base.Use(_dur);

        GameController.GC.ArtifactEquipment.AddCell( new ItemInstance(this));
        GameController.GC.Inventory.ClearItem();

        foreach (EffectArtifact effect in Effects)
            GameController.GC.player.ApplyEffect(effect);
    }

    public string GetEffectInfo()
    {
        string info = "\n";
        foreach (EffectArtifact effect in Effects)
        {
            if (effect.Value < 0)
                info += $"{effect.VarName}: <color=#EB1010>-{effect.Value}</color>\n";
            else
                info += $"{effect.VarName}: <color=#2DD431>+{effect.Value}</color>\n";
        }
        return info;
    }

    public override string Infor()
    {
        return base.Infor() + GetEffectInfo();
    }

}

[Serializable]
public class EffectArtifact
{
    public string VarName;
    public int Value;
}