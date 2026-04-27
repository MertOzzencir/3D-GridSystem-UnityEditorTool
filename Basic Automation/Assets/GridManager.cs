using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance;
    [SerializeField] private Transform player;
    public GridDataSO gridData;

    private Dictionary<Vector3Int, GridDictData> gridDict = new Dictionary<Vector3Int, GridDictData>();
    private GameObject currentGrid;
    private Vector3Int currentPos;
    private Coroutine gridDownAnim;
    private Coroutine gridUpAnim;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
        foreach (var entry in gridData.entries)
        {


            string gridName = $"Grid_{entry.position.x}_{entry.position.y}_{entry.position.z}";
            GameObject grid = GameObject.Find(gridName);
            if (grid == null) continue;

            GridDictData data = new GridDictData();
            data.Grid = grid;
            gridDict[entry.position] = data;
            // if (entry.IsFull)
            // {
            //     string placeableName = $"Placeable_{entry.position.x}_{entry.position.z}";
            //     GameObject placeableObj = GameObject.Find(placeableName);
            //     Placeable placeableRef = placeableObj.GetComponent<Placeable>();
            //     data.Placeable = placeableRef;
            //     if (placeableObj != null)
            //     {
            //         for (int z = 0; z < data.Placeable.Size.y; z++)
            //         {
            //             for (int x = 0; x < data.Placeable.Size.x; x++)
            //             {
            //                 if (x == 0 && z == 0) continue;
            //                 GridDictData nData = new GridDictData();
            //                 string nName = $"Grid_{entry.position.x + x}_{entry.position.y}_{entry.position.z + z}";
            //                 GameObject ngrid = GameObject.Find(nName);
            //                 nData.Grid = ngrid;
            //                 nData.Placeable = placeableRef;
            //                 nData.HasChecked = true;
            //                 gridDict[entry.position + new Vector3Int(x, 0, z)] = nData;
            //             }
            //         }
            //     }
            // }
        }
    }

    void Update()
    {
        Vector3 playerPos = player.transform.position;
        Vector3Int snappedPos = SnappedPosition(playerPos);
        GridDictData selectedData = GetGridData(snappedPos);
        if (selectedData == null) return;
        if (selectedData.Placeable != null) return;
        GameObject selectedGrid = GetGridGameObject(selectedData);

        if (selectedGrid == null)
        {
            if (currentGrid != null)
            {
                if (gridDownAnim != null) StopCoroutine(gridDownAnim);
                if (gridUpAnim != null) StopCoroutine(gridUpAnim);
                gridUpAnim = StartCoroutine(GridVerticalOffSetAnimation(currentGrid.transform, currentPos));
                currentGrid = null;
            }
            return;
        }

        if (selectedGrid != currentGrid)
        {
            if (currentGrid != null)
            {
                if (gridUpAnim != null) StopCoroutine(gridUpAnim);
                gridUpAnim = StartCoroutine(GridVerticalOffSetAnimation(currentGrid.transform, currentPos));
            }

            currentGrid = selectedGrid;
            currentPos = snappedPos;

            if (gridDownAnim != null) StopCoroutine(gridDownAnim);
            gridDownAnim = StartCoroutine(GridVerticalOffSetAnimation(
                currentGrid.transform,
                snappedPos + Vector3.down * 0.2f)
            );
        }
    }
    public GridDictData GetFreeGrid(Vector3 origin, Vector3 size)
    {
        Vector3Int snapped = SnappedPosition(origin);
        for (int z = 0; z < size.y + 2; z++)
        {
            for (int x = 0; x < size.x + 2; x++)
            {
                Vector3Int currentPos = snapped + new Vector3Int(-1 + x, 0, -1 + z);
                GridDictData currentGrid = GetGridData(currentPos);
                if (currentGrid != null && currentGrid.Placeable == null)
                {
                    return currentGrid;
                }
            }
        }
        return default;
    }
    public void AddOnGrid(Vector3 gridPos, Vector2 size, Placeable placeableObject)
    {
        for (int z = 0; z < size.y; z++)
        {
            for (int x = 0; x < size.x; x++)
            {
                GridDictData grid = GetGridData(gridPos + new Vector3(x, 0, z));
                if (grid != null && grid.Placeable == null)
                {
                    grid.Placeable = placeableObject;
                }
            }
        }

    }
    public void DeleteOnGrid(Vector3 pos, Vector3 size)
    {
        for (int z = 0; z < size.y; z++)
        {
            for (int x = 0; x < size.x; x++)
            {
                GridDictData currentGrid = GetGridData(pos + new Vector3(x, 0, z));
                if (currentGrid != null)
                {
                    currentGrid.Placeable = default;
                }
            }
        }
    }

    public GridDictData GetGridData(Vector3 worldPos)
    {
        Vector3Int pos = SnappedPosition(worldPos);
        gridDict.TryGetValue(pos, out GridDictData data);

        return data;
    }
    public GameObject GetGridGameObject(GridDictData data)
    {
        return data?.Grid;
    }
    public Vector3Int SnappedPosition(Vector3 pos)
    {
        pos.y = 0;
        return new Vector3Int(Mathf.FloorToInt(pos.x), Mathf.FloorToInt(pos.y), Mathf.FloorToInt(pos.z));
    }

    IEnumerator GridVerticalOffSetAnimation(Transform grid, Vector3 aimPosition)
    {
        while (Vector3.Distance(grid.position, aimPosition) > 0.01f)
        {
            grid.position = Vector3.Lerp(grid.position, aimPosition, 15f * Time.deltaTime);
            yield return null;
        }
        grid.position = aimPosition;
    }
    [ContextMenu("GridDebug")]
    public void GridDataDebug()
    {
        foreach (var a in gridDict)
        {
            Debug.Log("Position: " + a.Key + " Grid Name: " + a.Value.Grid + "Placeable Name: " + a.Value.Placeable);
        }
    }

}

[Serializable]

public class GridDictData
{
    public GameObject Grid;
    public Placeable Placeable;
}