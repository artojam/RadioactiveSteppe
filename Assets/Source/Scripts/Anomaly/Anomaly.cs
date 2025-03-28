using UnityEngine;

public class Anomaly : MonoBehaviour
{
    public AnomalyData data;
    
    private Transform _transform => transform;

    private ArtifactItem artifact = null;
    private GameObject _anomaly;

    public void Create()
    {
        _anomaly = Instantiate(data.GetSkinAnomaly(), _transform);

        int id_event = Random.Range(0, data.GetArtefacts().Count);

        ScannerItem _scanner = 
            ((ScannerItem)GameController.GC.inventoryEquipment
           .Cells[InventoryEquipment.DetectorCell].item.data);
        float _prob = 0;
        if (_scanner != null) 
            _prob = _scanner.GetProbabilityPathArtifact();

        float rnd = Random.Range(0, 100f - _prob);
        Debug.Log($"{ rnd }");

        if (rnd <= data.GetArtefacts()[id_event].probability)
        {
            artifact = data.GetArtefacts()[id_event].item;
        }


    }

    public void Create(AnomalyData _data)
    {
        data = _data;
        Create();
    }

    public void Clear()
    {
        data = null;
        Destroy(_anomaly);
        _anomaly = null;
        artifact = null;
    }

    public ArtifactItem GetArtifact() => artifact;

}
