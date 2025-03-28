using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTurnController : MonoBehaviour
{
    [SerializeField]
    private List<GameEvent> Events = new List<GameEvent>();

    private Player player;


    private void Start()
    {
        player = GetComponent<Player>();
    }

    public void OnDownToForth()
    {
        GameController.CountTurn++;
        int ReHunger = Random.Range(-5, -10);
        int ReThirst = Random.Range(-5, -15);

        player.ToHunger(ReHunger);
        player.ToThirst(ReThirst);

        GameController.GC.player.OnWalking();

        GameController.GC.ActiveitPanel((int)IdPanel.SalvageOperation);
        
        OnEvent();
    }

    private void OnEvent()
    {
        for (int i = 0; i <= 10; i++)
        {
            int id_event = Random.Range(0, Events.Count);
            float rnd = Random.Range(0, 100f);

            if (rnd <= Events[id_event].GetProbability())
            {
                Events[id_event].Execute(id_event);
                return;
            }
        }
        Events[0].Execute(0);
    }

}

