using System.Collections.Generic;
using UnityEngine;

public class Spawner : CarryablePlaceable
{
    [SerializeField] private SourceLocalManager[] spawnContainer;
    [SerializeField] private float spawnTimer;
    private Dictionary<GridDictData, SourceLocalManager> spawnObject = new Dictionary<GridDictData, SourceLocalManager>();
    private GlobalAnimationManager animManager;
    private GlobalAnimationTrigger animTrigger;
    public override void Start()
    {
        base.Start();
        animTrigger = GetComponentInChildren<GlobalAnimationTrigger>();
        animManager = GetComponent<GlobalAnimationManager>();
        SpawnPackage();
        animTrigger.OnAnimationTrigger += Spawn;
    }

    public override void Carry()
    {
        StopSpawning();
        base.Carry();
    }
    public void SpawnPackage()
    {
        StopSpawning();
        FindPlaceToSources();
        StartSpawning();
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
        InvokeRepeating("TryToSpawn", 1, spawnTimer);
    }

    private void StopSpawning()
    {
        CancelInvoke("TryToSpawn");
        spawnObject.Clear();
    }

    private void TryToSpawn()
    {
        foreach (var a in spawnObject)
        {
            if (GridManager.Instance.GetGridData(a.Key.Grid.transform.position).Placeable == null)
            {
                animManager.PlayTrigger("hit");
                break;
            }
        }
    }

    private void Spawn(string animname)
    {
        foreach (var a in spawnObject)
        {
            if (GridManager.Instance.GetGridData(a.Key.Grid.transform.position).Placeable == null)
                Instantiate(a.Value, a.Key.Grid.transform.position, Quaternion.identity);
        }
    }

    private void FindPlaceToSources()
    {
        GridDictData gridsArounds = GridManager.Instance.GetOneGridWithPosition(GridPosition + RotationManager.GetDirection());
        if (gridsArounds != null)
        {

            if (gridsArounds.Placeable == null)
            {
                int randomIndex = Random.Range(0, spawnContainer.Length);
                spawnObject.Add(gridsArounds, spawnContainer[randomIndex]);
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