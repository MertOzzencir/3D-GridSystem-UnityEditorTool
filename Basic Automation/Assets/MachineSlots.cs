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
        currentTool.GetVisual().localPosition = new Vector3(0, currentTool.GetVisual().localPosition.y, 0);
        currentTool.Setup(m);
    }
    public void UseTool()
    {
        currentTool.UseTool();
    }
    public MachineTools GetTool()
    {
        return currentTool;
    }
    public float ToolTimer()
    {
        return currentTool.CooldownTimer();
    }
    public void RemoveTool()
    {
        currentTool = null;
    }
    public void ToolUpdateGridPosition(Vector3Int gPosition)
    {
        currentTool.GridPosition = gPosition;
    }
}
