using UnityEngine;

[CreateAssetMenu(fileName = "New Drop", menuName = "Create Drop/New Drop")]
public class DropSO : ScriptableObject
{
    public DropType DropType;
}
public enum DropType
{
    Rock,
    Wood

}
