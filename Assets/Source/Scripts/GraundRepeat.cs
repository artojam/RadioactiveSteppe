using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraundRepeat : MonoBehaviour
{
    [SerializeField]
    private Vector2 PointsRepeat;
    [SerializeField]
    private float Speed;

    Transform tr;

    private void Start()
    {
        tr = transform;
    }

    private void Update()
    {
        Vector3 pos = tr.position;
        if(tr.position.x <= PointsRepeat.x)
            tr.position = new Vector3(PointsRepeat.y, pos.y, pos.z);

        
        tr.position = Vector2.MoveTowards(tr.position, new Vector2(PointsRepeat.x, pos.y), Time.deltaTime * Speed);
    }

    public void OnStartRepeat(bool isStart)
    {
        this.enabled = isStart;
    }
}
