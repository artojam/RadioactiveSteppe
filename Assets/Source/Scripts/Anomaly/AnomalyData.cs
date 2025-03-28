using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AnomalyType
{
    Electric = 0,
    Fire,
    Chemical,
    Gravitational
}

[CreateAssetMenu(fileName = "NewAnomaly", menuName = "Game/Anomaly")]
public class AnomalyData : ScriptableObject
{
    [SerializeField]
    private string NameAnomaly;
    [SerializeField]
    private GameObject SkinAnomaly;
    [SerializeField]
    private AnomalyType AnomalyType;

    [SerializeField]
    private List<ArtifactItemLoot> Artifacts = new List<ArtifactItemLoot>();

    public string GetName() => NameAnomaly;
    public GameObject GetSkinAnomaly() => SkinAnomaly;
    public AnomalyType GetTypeAnomaly() => AnomalyType;
    public List<ArtifactItemLoot> GetArtefacts() => Artifacts;

    
}

[Serializable]
public class ArtifactItemLoot
{
    public ArtifactItem item;
    public float probability;

    public ArtifactItemLoot(ArtifactItem _item, float _probability)
    {
        item = _item;
        probability = _probability;
    }
}
