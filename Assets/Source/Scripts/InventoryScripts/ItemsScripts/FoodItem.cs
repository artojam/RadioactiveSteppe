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
            _info += $"Голод: +{Hunger}\n";
        else if (Hunger < 0)
            _info += $"Голод: {Hunger}\n";

        if (Thirst > 0)
            _info += $"Жажда: +{Thirst}\n";
        else if (Thirst < 0)
            _info += $"Жажда: {Thirst}\n";

        if (Energy > 0)
            _info += $"Энергия: +{Energy}\n";
        else if (Energy < 0)
            _info += $"Энергия: {Energy}\n";

        if (Radiation > 0)
            _info += $"Радифция: +{Radiation}\n";
        else if (Radiation < 0)
            _info += $"Радифция: {Radiation}\n";

        if (Health > 0)
            _info += $"Востановление: +{Health}\n";
        else if (Health < 0)
            _info += $"Востановление: {Health}\n";

        return base.Infor() + _info;
    }                                        
}
