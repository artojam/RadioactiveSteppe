using UnityEngine;

[CreateAssetMenu(fileName = "FoodItem", menuName = "Game/Item/Create new Food item")]
public class FoodItem : Item
{
    [Space(15)]
    [Header("Food")]

    [SerializeField]
    private int Hunger = 0;
    [SerializeField]
    private int Thirst = 0;
    [SerializeField]
    private int Energy = 0;
    [SerializeField]
    private int Radiation = 0;
    [SerializeField]
    private int Health = 0;

    public override void Use(float _dur)
    {
        base.Use(_dur);
        GameController.GC.player.ToHunger(Hunger);
        GameController.GC.player.ToThirst(Thirst);
        GameController.GC.player.ToEnergy(Energy);
        GameController.GC.player.ToRadiation(Radiation);
        GameController.GC.player.ToDemage(-Health, TypeAttack.Default);
        GameController.GC.Inventory.ClearAmountItem(1);
    }

    public override string Infor()
    {
        string _info = "";

        if (Hunger > 0)
            _info += $"�����: +{Hunger}\n";
        else if (Hunger < 0)
            _info += $"�����: {Hunger}\n";

        if (Thirst > 0)
            _info += $"�����: +{Thirst}\n";
        else if (Thirst < 0)
            _info += $"�����: {Thirst}\n";

        if (Energy > 0)
            _info += $"�������: +{Energy}\n";
        else if (Energy < 0)
            _info += $"�������: {Energy}\n";

        if (Radiation > 0)
            _info += $"��������: +{Radiation}\n";
        else if (Radiation < 0)
            _info += $"��������: {Radiation}\n";

        if (Health > 0)
            _info += $"�������������: +{Health}\n";
        else if (Health < 0)
            _info += $"�������������: {Health}\n";

        return base.Infor() + _info;
    }                                        
}
