using Unity.VisualScripting;
using UnityEngine;

public class MachineSlots : MonoBehaviour
{

    private MachineTools currentTool;
    public void SetupTool(MachineTools tool, MachineBlueprintBase m)
    {
        currentTool = tool;
        currentTool.transform.parent = transform;
        currentTool.transform.localPosition = Vector3.zero;
        currentTool.transform.forward = this.transform.forward;
        currentTool.Setup(m);
    }
    public void UseTool()
    {
        currentTool.ToolLogic();
    }
    public MachineTools GetTool()
    {
        return currentTool;
    }
    public void ToolUpdateGridPosition(Vector3Int gPosition)
    {
        currentTool.GridPosition = gPosition;
    }
}
