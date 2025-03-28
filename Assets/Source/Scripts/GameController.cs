using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum IdPanel
{
    Home = 0,
    SalvageOperation,
    Inventory,
    EnemyAttack,
    VinEnemy,
    Trader,
    Anomaly
}

public class GameController : MonoBehaviour
{
    public static GameController GC;
    public bool isDebug = false;

    [Space(10f)]

    public Player player;

    [Space(10f)]

    public InventoryController Inventory;
    public InventoryEnemy EnemyInventory;
    public InventoryEquipment inventoryEquipment;
    public InventoryEquipment ArtifactEquipment;
    public TradeController Trade;

    [Space(10f)]

    public InfoPanelController InfoPanel;
    public GameObject EnemyTurnPanel;

    [Space(10f)]

    public TMP_Text InfoText;
    public TMP_Text InfoEnemyAttackText;
    public TMP_Text InfoAnomalyText;

    [SerializeField, Space(5f)]
    private Enemy enemy;
    [SerializeField]
    private Anomaly anomaly;

    [SerializeField, Space(5f)]
    Animator anim;

    [Space(10f)]

    [SerializeField]
    private Transform Graund;
    [SerializeField]
    private Transform Becks;

    [Space(10f)]

    [SerializeField]
    private float TimeToEnergy;

    [Space(10f)]

    public Button ScanButton;

    [Space(25f)]
    [SerializeField, TextArea(25, 10)]
    private string InfoGame;

    private List<GraundRepeat> Graunds = new List<GraundRepeat>();

    private int IDHome = 0;
    private int IdActivePanel = 0;
    private float time;

    public static int CountTurn = 0;
    public static bool IsTurnPlayer = true;

    private void Start()
    {
        enemy.gameObject.SetActive(false);
        
        GC = GetComponent<GameController>();
        
        Inventory.Craet();
        
        player.Create();
        
        inventoryEquipment.Creat();
        ArtifactEquipment.Creat();

        
        
        player.weapon.UpdataWeapon();
        player.armor.Create();

        
        EnemyInventory.Craet();
        
        Trade.Craet();
        

        ActiveitPanel(0);

        InfoPanel.Active( "Что Нового?", InfoGame );

        for(int i = 0; i < Graund.childCount; i++)
        {
            Graunds.Add(Graund.GetChild(i).GetComponent<GraundRepeat>());
        }
        for (int i = 0; i < Becks.childCount; i++)
        {
            Graunds.Add(Becks.GetChild(i).GetComponent<GraundRepeat>());
        }
        time = TimeToEnergy;
    }

    private void Update()
    {
        if(IDHome == 0)
        {
            if (time < 0)
            {
                player.ToEnergy(5);
                time = TimeToEnergy;
            }
            time -= Time.deltaTime;
        }
    }

    public void ActiveitPanel(int id)
    {
        if (id == 0)
        {
            StopWalking();
        }
        if (id == 1)
        {
            StartWalking();
        }

        if (id != 2)
        {
            IDHome = id;
        }
        IdActivePanel = id;
        anim.SetInteger("IDPanel", id);
    }

    public void OnHome()
    {
        ActiveitPanel(IDHome);
    }

    public void DeadPlayer()
    {
        Scene id_s = SceneManager.GetActiveScene();
        SceneManager.LoadScene(id_s.name);
    }

    public void OnEscapeEnemy()
    {
        for (int i = 0; i <= 10; i++)
        {
            float rnd = Random.Range(0, 100f);

            if (rnd <= (enemy.data.GetEscapeChance() / enemy.GetPercentageOfHealth()))
            {
                ActiveitPanel(1); 
                enemy.SkinEnemy.Escape();
                enemy.gameObject.SetActive(false);
                InfoText.text = "Мне удалось сбежать";
                
                return;
            }
        }
        InfoEnemyAttackText.text = "Я не смог сбежать";

        TurnEnemy();
    }

    public Enemy GetEnemy() => enemy;
    public Anomaly GetAnomaly() => anomaly;

    public void TurnEnemy() 
    {
        enemy.SkinEnemy.anim.SetTrigger("Attack");
        IsTurnPlayer = false;
        ActiveEnemyTurnPanel();
    }

    public int GetIDHome() => IDHome;

    public void ActiveEnemyTurnPanel()
    {
        EnemyTurnPanel.SetActive(!IsTurnPlayer);
    }

    public void StartGraundRepeat(bool isStart)
    {
        foreach(GraundRepeat graund in Graunds)
        {
            graund.OnStartRepeat(isStart);
        }
    }

    public void StartWalking()
    {
        player.OnWalking();
        StartGraundRepeat(true);
    }

    public void StopWalking()
    {
        player.OnStop();
        StartGraundRepeat(false);
    }

}
