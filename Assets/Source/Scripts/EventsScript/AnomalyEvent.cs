using UnityEngine;

[CreateAssetMenu(fileName = "NewAnomalyEvent", menuName = "Game/Event/New Anomaly Event")]
public class GameAnomalyEvent : GameEvent
{
    [SerializeField]
    private AnomalyData anomaly;

    private GameController _controller => GameController.GC; 

    private void Awake()
    {
        e_Type = TypeEvent.ANOMALY;
    }

    public override void Execute(int id)
    {
        _controller.InfoAnomalyText.text = $"вы нашли <color=#CDAE01>{anomaly.GetName()}</color>";

        _controller.ActiveitPanel((int)IdPanel.Anomaly);

        _controller.GetAnomaly().gameObject.SetActive(true);
        _controller.GetAnomaly().Create(anomaly);

        _controller.StopWalking();
        _controller.ScanButton.gameObject.SetActive(true);

    }
}
