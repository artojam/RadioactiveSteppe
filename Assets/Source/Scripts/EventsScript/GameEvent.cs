using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum TypeEvent
{
    BASE,
    FOUND,
    ENEMY,
    ANOMALY,
    CACHE,
    PEACEFUL_MAN,
    FOUND_THE_LOCATION
}

[CreateAssetMenu(fileName = "NewEvent", menuName = "Game/Event/New Event")]
public class GameEvent : ScriptableObject
{
    public TypeEvent e_Type;
    [SerializeField, Range(0f, 100f)]
    private float Probability;

    public virtual void Execute(int id)
    {
        GameController.GC.InfoText.text = $"Ёвент произашол {id}";
    }

    public float GetProbability() => Probability;
}
