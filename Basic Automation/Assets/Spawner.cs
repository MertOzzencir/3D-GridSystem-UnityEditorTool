using System.Collections.Generic;
using UnityEngine;

public class Spawner : CarryablePlaceable
{
    [SerializeField] private SourceLocalManager[] spawnContainer;
    [SerializeField] private float spawnTimer;
    private Dictionary<GridDictData, SourceLocalManager> spawnObject = new Dictionary<GridDictData, SourceLocalManager>();

    public override void Start()
    {
        base.Start();
        FindPlaceToSources();
        StartSpawning();
    }

    public override void Carry()
    {
        StopSpawning();
        base.Carry();
    }

    public override void Drop(out GridDictData sc)
    {
        base.Drop(out sc);
        if (sc != null)
        {
            FindPlaceToSources();
            StartSpawning();
        }
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
            if (GridManager.Instance.GetGridData(a.Key.Grid.transform.position).Placeable == null)
                Instantiate(a.Value, a.Key.Grid.transform.position, Quaternion.identity);
        }
    }

    private void FindPlaceToSources()
    {
        List<GridDictData> gridsArounds = GridManager.Instance.GetGridsInRange(GridPosition, Size);
        foreach (var a in gridsArounds)
        {
            if (a.Placeable == null)
            {
                int randomIndex = Random.Range(0, spawnContainer.Length);
                spawnObject.Add(a, spawnContainer[randomIndex]);
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