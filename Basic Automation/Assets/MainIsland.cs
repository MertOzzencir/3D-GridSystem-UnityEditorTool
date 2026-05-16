using System;
using System.Collections.Generic;
using UnityEngine;

public class MainIsland : MonoBehaviour
{
    private List<GridDatasFromIsland> localData = new List<GridDatasFromIsland>();

    void Awake()
    {
        foreach (Transform child in transform)
        {
            Vector3Int snappedPos = new Vector3Int(
                Mathf.FloorToInt(child.position.x),
                Mathf.FloorToInt(child.position.y),
                Mathf.FloorToInt(child.position.z)
            );
            localData.Add(new GridDatasFromIsland(child.gameObject, snappedPos));
        }
        GridManager.Instance.RegisterGridsFromIsland(localData);
    }
}
[Serializable]
public class GridDatasFromIsland
{
    public Vector3Int Position;
    public GameObject GridInstance;

    public GridDatasFromIsland(GameObject gridData, Vector3Int position)
    {
        Position = position;
        GridInstance = gridData;
    }
}