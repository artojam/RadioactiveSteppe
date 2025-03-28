using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MiniGameArtifact : MonoBehaviour
{
    [SerializeField]
    private Collider2D 
        PlayerPoint,
        DamegeZon,
        Artifact;

    [SerializeField]
    private AnimationCurve CurveDamegeZon;
    [SerializeField]
    private float SpeedTimeToCurve = 3;
    [SerializeField]
    private float SpeedToPlayerPoint = 3;

    [SerializeField]
    private GameObject BasePanel, MiniGamePanel;

    [SerializeField]
    private int MaxNumberLife = 3;
    private int NumberLife;

    [SerializeField]
    private TMP_Text TextNumberLife;
    [SerializeField]
    private TMP_Text TextName;
    [SerializeField]
    private TMP_Text TextInfo;
    [SerializeField]
    private TMP_Text TextData;

    private RectTransform TrDamegeZon => DamegeZon.GetComponent<RectTransform>();
    private RectTransform TrPlayerPoint => PlayerPoint.GetComponent<RectTransform>();

    private Image ImageArtifact => Artifact.GetComponent<Image>();

    private Anomaly _anomaly => GameController.GC.GetAnomaly();
    private ArtifactItem _artifact => GameController.GC.GetAnomaly().GetArtifact();
    private InventoryCell _cell => GameController.GC.inventoryEquipment.Cells[InventoryEquipment.DetectorCell];

    private TMP_Text _infoAnomalyText => GameController.GC.InfoAnomalyText;

    private float MaxTime => CurveDamegeZon.keys[CurveDamegeZon.length-1].time;
    
    private float SizeDamegeZon;
    private float size = 1;

    private bool isMovePlayer = false;

    private void Update()
    {
        size += (Time.deltaTime / SpeedTimeToCurve);   
        
        SizeDamegeZon = CurveDamegeZon.Evaluate(size);

        if (size >= MaxTime) size = 0;

        TrDamegeZon.localScale = new Vector3(SizeDamegeZon, SizeDamegeZon, 1);

        if (CheckCollision())
        {
            if(NumberLife - 1 != 0)
            {
                NumberLife--;
                TextNumberLife.text = $"Жизней: { NumberLife }";
                TrPlayerPoint.anchoredPosition = new Vector3(0, -220, 0);

                int _damege = Random.Range(_artifact.Damege.x, _artifact.Damege.y);
                GameController.GC.player.ToDemage(_damege, _artifact.TypeArtifact);
            }
            else
            {
                ActiveBasePanel();
                _infoAnomalyText.text = "К сожелению ты не смог поймать артефакт";
                GameController.GC.ScanButton.gameObject.SetActive(false);

                int _damege = Random.Range(_artifact.Damege.x, _artifact.Damege.y) * 2;
                GameController.GC.player.ToDemage(_damege, _artifact.TypeArtifact);

            }

            _cell.item.OnDurability(15);

        }

        if (CheckArtifact())
        {
            ActiveBasePanel();
            _infoAnomalyText.text = $"Вы захватили <color=#CDAE01> {_artifact.Name } </color>";
            GameController.GC.ScanButton.gameObject.SetActive(false);
            GameController.GC.Inventory.AddItem(new ItemInstance(_artifact), 1);
        }

        if (TrPlayerPoint.anchoredPosition.y >= -220 && !isMovePlayer)
            TrPlayerPoint.Translate(new Vector3(0, -1, 0) * SpeedToPlayerPoint * Time.deltaTime);

        else if (isMovePlayer)
            TrPlayerPoint.Translate(new Vector3(0, 1, 0) * (SpeedToPlayerPoint * 1.5f) * Time.deltaTime);
    }

    private bool CheckCollision()
    {
        return PlayerPoint.bounds.Intersects(DamegeZon.bounds);
    }

    private bool CheckArtifact()
    {
        return PlayerPoint.bounds.Intersects(Artifact.bounds);
    }

    public void StartMiniGame()
    {
        ItemInstance _scanner = _cell.item;
        if (_scanner.data == null)
        {
            _infoAnomalyText.text = "у тебя нет сканера";
            return;
        }
        else if(_scanner.CurrentDurability <= 0)
        {
            _infoAnomalyText.text = "твой сканер разряжен";
            return;
        }

        _scanner.OnDurability(15);

        if(_artifact == null)
        {
            _infoAnomalyText.text = "Аномалия пуста";
            GameController.GC.ScanButton.gameObject.SetActive(false);
            return;
        }

        ImageArtifact.sprite = _artifact.Icon;
        CurveDamegeZon = _artifact.CurveArtifact;

        NumberLife = MaxNumberLife;
        TextNumberLife.text = $"Жизней: {NumberLife}";
        TextName.text = _artifact.Name;
        TextInfo.text = _artifact.Info;
        TextData.text = _artifact.GetEffectInfo();
        
        TrPlayerPoint.anchoredPosition = new Vector3(0, -220, 0);
        
        isMovePlayer = false;

        ActiveMiniGamePanel();
    }

    public void ActiveBasePanel()
    {
        MiniGamePanel.SetActive(false);
        BasePanel.SetActive(true);
    }

    public void ActiveMiniGamePanel()
    {
        MiniGamePanel.SetActive(true);
        BasePanel.SetActive(false);
    }

    public void OnMove( bool isMove ) => isMovePlayer = isMove;


}
