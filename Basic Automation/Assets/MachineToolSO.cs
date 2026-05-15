using UnityEngine;

[CreateAssetMenu(fileName = "NewMachineTool", menuName = "Crafting/Machine Tool")]
public class MachineToolSO : ScriptableObject
{
    public string ToolName;
    public Sprite Icon;
    public MachineTools Prefab;
}