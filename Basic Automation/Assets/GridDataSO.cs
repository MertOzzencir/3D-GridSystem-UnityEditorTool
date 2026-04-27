// GridData.cs - Assets/ altında, Editor klasöründe değil
using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "GridData", menuName = "Grid/GridData")]
public class GridDataSO : ScriptableObject
{
    public List<GridRawData> entries = new List<GridRawData>();
}

[System.Serializable]
public class GridRawData
{
    public Vector3Int position;
    public bool IsFull;
    public string OccupiedBy;
}
