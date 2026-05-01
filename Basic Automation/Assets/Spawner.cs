using System.Collections.Generic;
using UnityEngine;

public class Spawner : Placeable, ICarryable
{
    [SerializeField] private SourceLocalManager[] spawnContainer;
    [SerializeField] private float spawnTimer;
    private Dictionary<GameObject, SourceLocalManager> spawnObject = new Dictionary<GameObject, SourceLocalManager>();
    private Collider c;

    public override void Start()
    {
        base.Start();
        c = GetComponent<Collider>();
        FindPlaceToSources();
        StartSpawning();
    }
    public void Carry()
    {
        StopSpawning();
        c.enabled = false;
        DeleteOnGrid();
        CarryableEvents.Invoke(this);
        OnCarryDrop();
    }

    public void Drop(out bool sc)
    {
        sc = true;
        Transform parent = transform.parent;
        GridManager.Instance.AddOnGrid(parent.position + parent.forward, Size, this, out sc);
        if (sc)
        {
            c.enabled = true;
            FindPlaceToSources();
            StartSpawning();
        }
        else
            OnCarryDrop();
    }

    public Transform GetTransform()
    {
        return transform;
    }
    private void OnCarryDrop()
    {
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.Euler(Vector3.zero);
    }
    private void StartSpawning()
    {
        InvokeRepeating("SpawnSource", 1, spawnTimer);
    }
    private void StopSpawning()
    {
        CancelInvoke("SpawnSource");
        spawnObject.Clear();

    }

    private void SpawnSource()
    {
        foreach (var a in spawnObject)
        {
            if (GridManager.Instance.GetGridData(a.Key.transform.position).Placeable == null)
                Instantiate(a.Value, a.Key.transform.position, Quaternion.identity);
        }
    }

    private void FindPlaceToSources()
    {
        List<GridDictData> gridsArounds = GridManager.Instance.GetGridsInRange(transform.position, Size);
        foreach (var a in gridsArounds)
        {
            if (a.Placeable == null)
            {
                int randomIndex = Random.Range(0, spawnContainer.Length);
                spawnObject.Add(a.Grid, spawnContainer[randomIndex]);
            }
        }
    }

    [ContextMenu("Show Dict")]
    public void ShowDict()
    {
        foreach (var a in spawnObject)
        {
            Debug.Log(a.Key + "" + a.Value);
        }
    }


}
