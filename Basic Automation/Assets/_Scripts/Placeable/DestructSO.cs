using UnityEngine;

[CreateAssetMenu(fileName = "New Desctructable", menuName = "Create Destructable Data/New DestructData")]
public class DestructSO : ScriptableObject
{
    public int Health;
    public int XPAmount;
    public SourceBase DropGameObject;


}
